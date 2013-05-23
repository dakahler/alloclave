using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenTK;
using System.Threading;
using System.Windows;

namespace Alloclave
{
	public partial class AddressSpace : UserControl
	{
		bool IsLeftMouseDown;
		bool IsMiddleMouseDown;
		Vector LastMouseLocation;
		Vector MouseDownLocation;
		Vector CurrentMouseLocation;
		const int WheelDelta = 120;

		// TODO: This should be exposed in the UI
		const UInt64 AddressWidth = 0xFF;

		History LastHistory = History.Instance;

		RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		AddressSpaceRenderer Renderer;

		private Object RebuildDataLock = new Object();
		private Object RebuildGfxLock = new Object();
		private AutoResetEvent RecalculateSelectedBlock = new AutoResetEvent(false);
		private AutoResetEvent RecalculateHoverBlock = new AutoResetEvent(false);

		// Represents all packets from the beginning of the profile to the current shifted time
		SortedList<UInt64, IPacket> AggregatePacketData = new SortedList<UInt64, IPacket>();

		// TODO: May want to make the idea of multiple heaps more pervasive
		// throughout the data flow
		Dictionary<uint, RectangleF> HeapBounds = new Dictionary<uint, RectangleF>();

		TimeStamp LastTimestamp = new TimeStamp();

		CancellationTokenSource TokenSource;

		// TODO: Probably shouldn't static
		public static event SelectionChangedEventHandler SelectionChanged;

		public event EventHandler Rebuilt;

		public bool IsPaused;

		public void History_Updated(object sender, EventArgs e)
		{
			LastHistory = sender as History;
			Rebuild(LastHistory);
		}

		public void Rebuild(History history, bool forceFullRebuild = false)
		{
			if (Parent == null)
			{
				return;
			}

			new Task(() => 
			{
				NBug.Exceptions.Handle(false, () =>
				{
					lock (RebuildDataLock)
					{
						lock (history.AddLock)
						{

							IEnumerable<KeyValuePair<TimeStamp, IPacket>> packets = null;
							bool isBackward = false;
							if (forceFullRebuild)
							{
								AggregatePacketData.Clear();
								MemoryBlockManager.Instance.Reset();

								packets = history.Get();
							}
							else
							{
								// Determine what entries we need to get based off scrubber position
								double position = (double)Scrubber.Position;
								UInt64 timeRange = history.TimeRange.Max - history.TimeRange.Min;
								UInt64 currentTime = history.TimeRange.Min + (UInt64)((double)timeRange * position);

								if (currentTime > LastTimestamp.Time)
								{
									packets = history.GetForward(new TimeStamp(currentTime));
								}
								else if (currentTime < LastTimestamp.Time)
								{
									packets = history.GetBackward(new TimeStamp(currentTime));
									isBackward = true;
								}
								else
								{
									return;
								}

								LastTimestamp = new TimeStamp(currentTime);
							}

							if (history.AddressRange.Max < history.AddressRange.Min)
							{
								return;
							}

							// Create final list, removing allocations as frees are encountered
							// TODO: STILL needs performance improvements for large datasets
							foreach (var pair in packets)
							//Parallel.ForEach(newList, pair =>
							{
								// TODO: Can allocation and free processing be combined?
								// They should be exact opposites of each other
								if (pair.Value is Allocation)
								{
									Allocation allocation = pair.Value as Allocation;

									lock (AggregatePacketData)
									{
										if (!isBackward)
										{
											if (AggregatePacketData.ContainsKey(allocation.Address))
											{
												AggregatePacketData.Remove(allocation.Address);
												MessagesForm.Add(MessagesForm.MessageType.Error, allocation, "Duplicate allocation!");
											}

											AggregatePacketData.Add(allocation.Address, allocation);

											VisualMemoryBlock newBlock = MemoryBlockManager.Instance.Add(
												allocation, history.AddressRange.Min, AddressWidth, Width);
										}
										else
										{
											AggregatePacketData.Remove(allocation.Address);
											MemoryBlockManager.Instance.Remove(allocation.Address);
										}
									}
								}
								else
								{
									Free free = pair.Value as Free;

									lock (AggregatePacketData)
									{
										if (AggregatePacketData.ContainsKey(free.Address))
										{
											AggregatePacketData.Remove(free.Address);

											VisualMemoryBlock removedBlock = MemoryBlockManager.Instance.Remove(free.Address);
											if (removedBlock != null)
											{
												removedBlock.Allocation.AssociatedFree = free;
												free.AssociatedAllocation = removedBlock.Allocation;
											}
											else
											{
												throw new DataException();
											}
										}
										else
										{
											if (isBackward)
											{
												AggregatePacketData.Add(free.Address, free.AssociatedAllocation);
												VisualMemoryBlock newBlock = MemoryBlockManager.Instance.Add(
												free.AssociatedAllocation, history.AddressRange.Min, AddressWidth, Width);
											}
											else
											{
												MessagesForm.Add(MessagesForm.MessageType.Error, free.AssociatedAllocation, "Duplicate free!");
											}
										}
									}
								}
							} //);
						}

						if (AggregatePacketData.Count == 0)
						{
							return;
						}

						if (!MemoryBlockManager.Instance.Contains(Renderer.SelectedBlock))
						{
							Renderer.SelectedBlock = null;
						}

						Renderer.HoverBlock = null;

						if (Rebuilt != null)
						{
							EventArgs e = new EventArgs();
							Rebuilt.Invoke(this, e);
						}

						// TODO: This should hook into the callback above instead
						RenderManager_OGL.Instance.Rebuild(MemoryBlockManager.Instance.HeapOffsets);
					}
				});
			}).Start();
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			Renderer = new AddressSpaceRenderer_OGL(this);

			Tooltip.RtbPCtrl = printCtrl;

			this.MouseWheel += AddressSpace_MouseWheel;

			ColorPickerDialog.ColorChanged += ColorPickerDialog_ColorChanged;

			TokenSource = new CancellationTokenSource();
			var cancellationToken = TokenSource.Token;

			Task.Factory.StartNew(() => HoverTask(cancellationToken), cancellationToken);
			Task.Factory.StartNew(() => SelectTask(cancellationToken), cancellationToken);
		}

		~AddressSpace()
		{
			TokenSource.Cancel();
			RecalculateHoverBlock.Set();
			RecalculateSelectedBlock.Set();
		}

		void ColorPickerDialog_ColorChanged(object sender, EventArgs e)
		{
			Rebuild(LastHistory, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			return;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			return;
		}

		public void AddressSpace_MouseMove(object sender, MouseEventArgs e)
		{
			Renderer.CurrentMouseLocation = e.Location.ToVector();
			CurrentMouseLocation = e.Location.ToVector();

			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				Vector mouseDelta = e.Location.ToVector() - LastMouseLocation;
				LastMouseLocation = e.Location.ToVector();

				Renderer.Offset = Renderer.Offset + mouseDelta;
			}

			RecalculateHoverBlock.Set();
		}

		public void AddressSpace_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = true;
				LastMouseLocation = e.Location.ToVector();
				MouseDownLocation = e.Location.ToVector();
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = true;
				LastMouseLocation = e.Location.ToVector();
				MouseDownLocation = e.Location.ToVector();
			}
		}

		public void AddressSpace_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				if (MouseDownLocation == e.Location.ToVector())
				{
					SelectAt();
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				if (MouseDownLocation == e.Location.ToVector())
				{
					SelectAt();
				}
			}
		}

		public void AddressSpace_MouseWheel(object sender, MouseEventArgs e)
		{
			Matrix currentViewMatrix = new Matrix();
			currentViewMatrix.Translate((float)Renderer.Offset.X, (float)Renderer.Offset.Y);
			currentViewMatrix.Scale(Renderer.Scale, Renderer.Scale);

			Vector pointBefore = CurrentMouseLocation;
			Matrix InvertedTransform = currentViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformVector(ref pointBefore);

			int amountToMove = e.Delta / WheelDelta;
			amountToMove = amountToMove.Clamp(-4, 4);

			float finalScale = 1.0f + (float)amountToMove / 5.0f;
			Renderer.Scale *= finalScale;

			// TODO: Better correction
			if (Renderer.Scale < 10.0)
			{
				currentViewMatrix.Scale(finalScale, finalScale);
			}
			else
			{
				Renderer.Scale = 10.0f;
			}

			// Scale is not allowed to be < 1.0
			if (Renderer.Scale < 1.0f)
			{
				float correctionFactor = 1.0f / Renderer.Scale;
				Renderer.Scale = 1.0f;
				currentViewMatrix.Scale(correctionFactor, correctionFactor);
			}

			Vector pointAfter = CurrentMouseLocation;
			InvertedTransform = currentViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformVector(ref pointAfter);

			Vector delta = pointAfter - pointBefore;

			currentViewMatrix.Translate((float)delta.X, (float)delta.Y);

			Renderer.Offset = new Vector(currentViewMatrix.OffsetX, currentViewMatrix.OffsetY);
		}

		void SelectAt()
		{
			RecalculateSelectedBlock.Set();
		}

		public void SelectAt(Allocation targetAllocation)
		{
			if (targetAllocation == null)
			{
				return;
			}

			VisualMemoryBlock block = MemoryBlockManager.Instance.Find(targetAllocation.Address);
			if (block != null)
			{
				Renderer.SelectedBlock = block;

				SelectionChangedEventArgs e = new SelectionChangedEventArgs();
				e.SelectedBlock = Renderer.SelectedBlock;
				this.Invoke((MethodInvoker)(() => SelectionChanged(this, e)));
			}
			else
			{
				// TODO: What should be done here? Scroll the timeline to a point where it exists?
			}

			// TODO: Automatically bring the new selection into view
		}

		// TODO: HoverTask and SelectTask are very similar. Find a way to consolidate them.
		void HoverTask(CancellationToken ct)
		{
			while (true)
			{
				RecalculateHoverBlock.WaitOne();

				if (ct.IsCancellationRequested)
				{
					return;
				}

				Renderer.HoverBlock = null;
				lock (RebuildDataLock)
				{
					Renderer.HoverBlock = MemoryBlockManager.Instance.Find(Renderer.GetLocalMouseLocation());
				}
			}
		}

		void SelectTask(CancellationToken ct)
		{
			while (true)
			{
				RecalculateSelectedBlock.WaitOne();

				if (ct.IsCancellationRequested)
				{
					return;
				}

				Renderer.SelectedBlock = null;
				lock (RebuildDataLock)
				{
					Renderer.SelectedBlock = MemoryBlockManager.Instance.Find(Renderer.GetLocalMouseLocation());
					if (Renderer.SelectedBlock != null)
					{
						SelectionChangedEventArgs e = new SelectionChangedEventArgs();
						e.SelectedBlock = Renderer.SelectedBlock;
						this.Invoke((MethodInvoker)(() => SelectionChanged(this, e)));
					}
				}
			}
		}

		void HoverAt(Vector location)
		{
			// TODO: Tooltip?
		}

		private void AddressSpace_SizeChanged(object sender, EventArgs e)
		{
			Rebuild(LastHistory);
		}

		private void AddressSpace_MouseHover(object sender, EventArgs e)
		{
			HoverAt(CurrentMouseLocation);
		}

		public void CenterAt(Vector location)
		{
			Vector newOffset = location - new Vector(Width / 2, Height / 2);
			newOffset.Negate();
			Renderer.Offset = newOffset;
			Renderer.Scale = 1.0f;
		}

		public void AddressSpace_MouseLeave(object sender, EventArgs e)
		{
			Renderer.CurrentMouseLocation = new Vector(-1, -1);
		}
	}

	public class SelectionChangedEventArgs : EventArgs
	{
		public VisualMemoryBlock SelectedBlock;
	}

	public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}

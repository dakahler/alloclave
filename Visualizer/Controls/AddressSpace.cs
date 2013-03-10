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

		// TODO: Too inefficient?
		History LastHistory = History.Instance;

		RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		AddressSpaceRenderer Renderer;

		private Object RebuildDataLock = new Object();
		private Object RebuildGfxLock = new Object();
		private AutoResetEvent RecalculateSelectedBlock = new AutoResetEvent(false);
		private AutoResetEvent RecalculateHoverBlock = new AutoResetEvent(false);

		// TODO: Better name
		SortedList<UInt64, IPacket> CombinedList = new SortedList<UInt64, IPacket>();

		// TODO: May want to make the idea of multiple heaps more pervasive
		// throughout the data flow
		Dictionary<uint, RectangleF> HeapBounds = new Dictionary<uint, RectangleF>();

		TimeStamp LastTimestamp = new TimeStamp();

		public event SelectionChangedEventHandler SelectionChanged;

		public event EventHandler Rebuilt;

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

			var task3 = new Task(() => 
			{
				lock (RebuildDataLock)
				{
					IEnumerable<KeyValuePair<TimeStamp, IPacket>> packets = null;
					bool isBackward = false;
					if (forceFullRebuild)
					{
						CombinedList.Clear();
						MemoryBlockManager.Instance.Reset();

						packets = history.Get();
					}
					else
					{
						// Determine what entries we need to get based off scrubber position
						double position = (double)AllocationForm.MainScrubber.Position;
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

					// Align to the beginning of the row
					UInt64 startAddress = history.AddressRange.Min & ~AddressWidth;
					UInt64 numRows = (history.AddressRange.Max - startAddress) / AddressWidth;

					// Create final list, removing allocations as frees are encountered
				
					// TODO: STILL needs performance improvements for large datasets
					VisualMemoryBlock lastBlock = null;
					foreach (var pair in packets)
					//Parallel.ForEach(newList, pair =>
					{
						if (pair.Value is Allocation)
						{
							Allocation allocation = pair.Value as Allocation;

							try
							{
								// HACK:
								// There is no good way (that I can find) of making sure an allocation
								// is new when that allocation is exactly the same address/size as
								// the old one. When this happens, this block will get hit.
								// Treat it as a free/allocation combo here
								// This makes it so genuine double allocations cannot be caught/reported,
								// so it must be fixed in the future!
								bool remove = false;
								lock (CombinedList)
								{
									if (CombinedList.ContainsKey(allocation.Address))
									{
										CombinedList.Remove(allocation.Address);
										remove = true;
									}

									if (!isBackward)
									{
										CombinedList.Add(allocation.Address, allocation);
									}
								}

								if (remove)
								{
									MemoryBlockManager.Instance.Remove(allocation.Address);
								}

								if (!isBackward)
								{
									VisualMemoryBlock newBlock = MemoryBlockManager.Instance.Add(
										allocation, history.AddressRange.Min, AddressWidth, Width);
								}

								// Update heap bounds
								// Currently doesn't bother tracking X
								//if (!HeapBounds.ContainsKey(allocation.HeapId))
								//{
								//	HeapBounds.Add(allocation.HeapId, new RectangleF(0.0f, float.MaxValue, 0.0f, 0.0f));
								//}

								//RectangleF bounds = newBlock.Bounds;
								//float oldHeight = bounds.Height;
								//bounds.Y = Math.Min(HeapBounds[allocation.HeapId].Y, bounds.Y);
								//bounds.Height = Math.Max(HeapBounds[allocation.HeapId].Height,
								//	newBlock.Bounds.Bottom - bounds.Y);

								//HeapBounds[allocation.HeapId] = bounds;
								//lastBlock = newBlock;
							}
							catch (ArgumentException)
							{
								// TODO: User-facing error reporting
								//Console.WriteLine("Duplicate allocation!");
								//throw new InvalidConstraintException();
							}
						}
						else
						{
							Free free = pair.Value as Free;

							lock (CombinedList)
							{
								if (CombinedList.ContainsKey(free.Address))
								{
									CombinedList.Remove(free.Address);
								}
								else
								{
									if (isBackward)
									{
										CombinedList.Add(free.Address, free.AssociatedAllocation);
										VisualMemoryBlock newBlock = MemoryBlockManager.Instance.Add(
										free.AssociatedAllocation, history.AddressRange.Min, AddressWidth, Width);
									}

									// This indicates a memory problem on the target side
									// TODO: User-facing error reporting
									// This is going to hit naturally due to the hack above
									//Console.WriteLine("Duplicate free!");
									//throw new InvalidConstraintException();
								}
							}

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
					} //);

					if (CombinedList.Count == 0)
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

					// TODO: Sort spatially
					var boundsSet = HeapBounds.AsEnumerable().OrderBy(p => p.Value.Bottom);

					// Figure out heap offsets for free space compression
					float currentOffset = 0;
					for (int i = 0; i < boundsSet.Count() - 1; i++)
					{
						RectangleF topBounds = boundsSet.ElementAt(i).Value;
						RectangleF bottomBounds = boundsSet.ElementAt(i + 1).Value;
						float diff = bottomBounds.Top - topBounds.Bottom;
						currentOffset += (diff - 100);

						MemoryBlockManager.Instance.HeapOffsets[boundsSet.ElementAt(i + 1).Key] = 0; //currentOffset;
					}

					// TODO: This should hook into the callback above instead
					RenderManager_OGL.Instance.Rebuild(MemoryBlockManager.Instance.HeapOffsets);
				}
			});

			task3.Start();
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			Renderer = new AddressSpaceRenderer_OGL(this);

			Tooltip.RtbPCtrl = printCtrl;

			this.MouseWheel += AddressSpace_MouseWheel;

			ColorPickerDialog.ColorChanged += ColorPickerDialog_ColorChanged;

			Task.Factory.StartNew(() => HoverTask());
			Task.Factory.StartNew(() => SelectTask());
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

		// TODO: HoverTask and SelectTask are very similar. Find a way to consolidate them.
		void HoverTask()
		{
			while (true)
			{
				RecalculateHoverBlock.WaitOne();

				Renderer.HoverBlock = null;
				lock (RebuildDataLock)
				{
					Renderer.HoverBlock = MemoryBlockManager.Instance.Find(Renderer.GetLocalMouseLocation());
				}
			}
		}

		void SelectTask()
		{
			while (true)
			{
				RecalculateSelectedBlock.WaitOne();

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

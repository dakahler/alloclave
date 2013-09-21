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

		// TODO: May want to make the idea of multiple heaps more pervasive
		// throughout the data flow
		Dictionary<uint, RectangleF> HeapBounds = new Dictionary<uint, RectangleF>();

		TimeStamp LastTimestamp = new TimeStamp();
		UInt64 LastRange = 0;

		CancellationTokenSource TokenSource;

		// TODO: Probably shouldn't be static
		public static event SelectionChangedEventHandler SelectionChanged;

		public event EventHandler Rebuilt;

		UInt64 ArtificialMaxTime = 0;

		private bool _IsPaused = false;
		public bool IsPaused
		{
			get
			{
				return _IsPaused;
			}
			set
			{
				_IsPaused = value;

				if (PauseChanged != null)
				{
					PauseChanged(this, new EventArgs());
				}
			}
		}

		public event EventHandler PauseChanged;

		System.Timers.Timer FrameTimer;
		private const double FrameInterval = 30.0;

		public void History_Updated(object sender, EventArgs e)
		{
			LastHistory = sender as History;
			Rebuild(LastHistory);
		}

		public void Rebuild(History history, bool forceFullRebuild = false, bool synchronous = false)
		{
			if (Parent == null)
			{
				return;
			}

			Task task = new Task(() => 
			{
				NBug.Exceptions.Handle(false, () =>
				{
					lock (RebuildDataLock)
					{
						lock (history.AddLock)
						{
							// Start by rebasing if necessary
							bool forceUpdate = false;
							if (history.RebaseBlocks)
							{
								Snapshot.Instance.Rebase(history.AddressRange.Min, AddressWidth, Width);
								history.RebaseBlocks = false;
								forceUpdate = true;
							}

							UInt64 maxTime = ArtificialMaxTime;
							if (maxTime == 0)
							{
								maxTime = history.TimeRange.Max;
							}

							IEnumerable<KeyValuePair<TimeStamp, IPacket>> packets = null;
							bool isBackward = false;
							if (forceFullRebuild)
							{
								Snapshot.Instance.Reset();
								packets = history.Get();
							}
							else
							{
								// Determine what entries we need to get based off scrubber position
								UInt64 timeRange = maxTime - history.TimeRange.Min;

								// Readjust the position if needed
								if (ArtificialMaxTime == 0 && timeRange > 0)
								{
									double rangeScale = (double)LastRange / (double)timeRange;

									if (rangeScale > 0 && Scrubber._Position < 1.0)
									{
										// Hacky
										Scrubber._Position *= rangeScale;
										Scrubber._Position = Scrubber._Position.Clamp(0.0, 1.0);
										Scrubber.Instance.FlagRedraw();
									}
								}

								LastRange = timeRange;

								double position = Scrubber.Position;
								UInt64 currentTime = history.TimeRange.Min + (UInt64)((double)timeRange * position);

								bool nothingToProcess = false;
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
									nothingToProcess = true;
								}

								LastTimestamp = new TimeStamp(currentTime);

								if (nothingToProcess && !forceUpdate)
								{
									return;
								}
							}

							if (history.AddressRange.Max < history.AddressRange.Min)
							{
								return;
							}

							// Create final list, removing allocations as frees are encountered
							if (packets != null)
							{
								foreach (var pair in packets)
								//Parallel.ForEach(newList, pair =>
								{
									// TODO: Can allocation and free processing be combined?
									// They should be exact opposites of each other
									if (pair.Value is Allocation)
									{
										Allocation allocation = pair.Value as Allocation;

										if (!isBackward)
										{
											VisualMemoryBlock newBlock = Snapshot.Instance.Add(
												allocation, history.AddressRange.Min, AddressWidth, Width);

											if (newBlock == null)
											{
												//MessagesForm.Add(MessagesForm.MessageType.Error, allocation, "Duplicate allocation!");
											}
										}
										else
										{
											Snapshot.Instance.Remove(allocation.Address);
										}
									}
									else
									{
										Free free = pair.Value as Free;

										if (Snapshot.Instance.Find(free.Address) != null)
										{
											VisualMemoryBlock removedBlock = Snapshot.Instance.Remove(free.Address);
											if (removedBlock != null)
											{
												removedBlock.Allocation.AssociatedFree = free;
												free.AssociatedAllocation = removedBlock.Allocation;
											}
											else
											{
												//throw new DataException();
											}
										}
										else
										{
											if (isBackward)
											{
												VisualMemoryBlock newBlock = Snapshot.Instance.Add(
													free.AssociatedAllocation, history.AddressRange.Min, AddressWidth, Width);
											}
											else
											{
												//MessagesForm.Add(MessagesForm.MessageType.Error, free.AssociatedAllocation, "Duplicate free!");
											}
										}
									}
								} //);
							}
						}

						if (!Snapshot.Instance.Contains(Renderer.SelectedBlock))
						{
							Renderer.SelectedBlock = null;
						}

						if (!Snapshot.Instance.Contains(Renderer.HoverBlock))
						{
							Renderer.HoverBlock = null;
						}

						if (Rebuilt != null)
						{
							EventArgs e = new EventArgs();
							Rebuilt.Invoke(this, e);
						}

						// TODO: This should hook into the callback above instead
						RenderManager_OGL.Instance.Rebuild(Snapshot.Instance.HeapOffsets);
					}
				});
			});

			task.Start();

			if (synchronous)
			{
				task.Wait();
			}
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			Renderer = new AddressSpaceRenderer_OGL(this);

			//Tooltip.RtbPCtrl = printCtrl;

			this.MouseWheel += AddressSpace_MouseWheel;

			ColorPickerDialog.ColorChanged += ColorPickerDialog_ColorChanged;

			TokenSource = new CancellationTokenSource();
			var cancellationToken = TokenSource.Token;

			Task.Factory.StartNew(() => HoverTask(cancellationToken), cancellationToken);
			Task.Factory.StartNew(() => SelectTask(cancellationToken), cancellationToken);

			this.Load += AddressSpace_Load;
		}

		void AddressSpace_Load(object sender, EventArgs e)
		{
			Scrubber.Instance.MousePressed += Scrubber_MouseDown;
			Scrubber.Instance.MouseReleased += Scrubber_MouseUp;

			FrameTimer = new System.Timers.Timer(FrameInterval);
			FrameTimer.Elapsed += TimerElapsed;
			FrameTimer.Start();
		}

		void Scrubber_MouseDown(object sender, MouseEventArgs e)
		{
			ArtificialMaxTime = History.Instance.TimeRange.Max;
			IsPaused = true;
		}

		void Scrubber_MouseUp(object sender, MouseEventArgs e)
		{
			ArtificialMaxTime = 0;
			Rebuild(History.Instance);
		}

		private void TimerElapsed(object sender, EventArgs e)
		{
			System.Timers.Timer timer = (System.Timers.Timer)sender;
			timer.Stop();

			if (ArtificialMaxTime == 0)
			{
				// Recalculate the percentage
				UInt64 timeRange = History.Instance.TimeRange.Max - History.Instance.TimeRange.Min;

				if (LastTimestamp.Time > 0)
				{
					// Slowly move forward
					// TODO: This should be target-time based, but the tool doesn't really
					// know what units the target time is in right now
					if (!IsPaused && Scrubber.Position < 1.0)
					{
						Scrubber.Position += 0.0005;
						Rebuild(History.Instance, false, true);
					}
				}
			}

			timer.Start();
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

			VisualMemoryBlock block = Snapshot.Instance.Find(targetAllocation.Address);
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

				lock (RebuildDataLock)
				{
					Renderer.HoverBlock = Snapshot.Instance.Find(Renderer.GetLocalMouseLocation());
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

				lock (RebuildDataLock)
				{
					Renderer.SelectedBlock = Snapshot.Instance.Find(Renderer.GetLocalMouseLocation());
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
			LastHistory.RebaseBlocks = true;
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

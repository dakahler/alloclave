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

namespace Alloclave
{
	public partial class AddressSpace : UserControl
	{
		bool IsLeftMouseDown;
		bool IsMiddleMouseDown;
		Point LastMouseLocation;
		Point MouseDownLocation;
		Point CurrentMouseLocation;
		const int WheelDelta = 120;

		// TODO: This should be exposed in the UI
		const UInt64 AddressWidth = 0xFF;

		UInt64 AllocationMin = UInt64.MaxValue;
		UInt64 AllocationMax = UInt64.MinValue;

		// TODO: Too inefficient?
		History LastHistory = new History();

		RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		AddressSpaceRenderer Renderer;

		private Object RebuildDataLock = new Object();
		private Object RebuildGfxLock = new Object();
		private AutoResetEvent RecalculateSelectedBlock = new AutoResetEvent(false);
		private AutoResetEvent RecalculateHoverBlock = new AutoResetEvent(false);

		// TODO: Better name
		SortedList<UInt64, IPacket> CombinedList = new SortedList<UInt64, IPacket>();

		public event SelectionChangedEventHandler SelectionChanged;

		public event EventHandler Rebuilt;

		public void History_Updated(object sender, EventArgs e)
		{
			LastHistory = sender as History;
			Rebuild(LastHistory);
		}

		public void Rebuild(History history)
		{
			if (Parent == null)
			{
				return;
			}

			var task3 = new Task(() => 
			{
				List<KeyValuePair<TimeStamp, IPacket>> newAllocations = history.GetNew(typeof(Allocation));
				List<KeyValuePair<TimeStamp, IPacket>> newFrees = history.GetNew(typeof(Free));

				// Combine allocation and free lists
				var newList = newAllocations.Union(newFrees);
				newList.OrderBy(pair => pair.Key);

				// Start by determining the lowest address
				foreach (var pair in newAllocations)
				{
					AllocationMin = Math.Min(AllocationMin, ((Allocation)pair.Value).Address);
					AllocationMax = Math.Max(AllocationMin, ((Allocation)pair.Value).Address);
				}

				if (AllocationMax < AllocationMin)
				{
					return;
				}

				// Align to the beginning of the row
				UInt64 startAddress = AllocationMin & ~AddressWidth;
				UInt64 numRows = (AllocationMax - startAddress) / AddressWidth;

				// Create final list, removing allocations as frees are encountered
				lock (RebuildDataLock)
				{
					foreach (var pair in newList)
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
								if (CombinedList.ContainsKey(allocation.Address))
								{
									CombinedList.Remove(allocation.Address);
									MemoryBlockManager.Instance.Remove(allocation.Address);
								}

								CombinedList.Add(allocation.Address, pair.Value);
								MemoryBlockManager.Instance.Add(allocation, AllocationMin, AddressWidth, Width);
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

							if (CombinedList.ContainsKey(free.Address))
							{
								CombinedList.Remove(free.Address);
							}
							else
							{
								// This indicates a memory problem on the target side
								// TODO: User-facing error reporting
								// This is going to hit naturally due to the hack above
								//Console.WriteLine("Duplicate free!");
								//throw new InvalidConstraintException();
							}

							MemoryBlockManager.Instance.Remove(free.Address);
						}
					}

					if (CombinedList.Count == 0)
					{
						return;
					}

					if (!MemoryBlockManager.Instance.Contains(Renderer.SelectedBlock))
					{
						Renderer.SelectedBlock = null;
					}

					Renderer.HoverBlock = null;

					int finalHeight = (int)numRows * 2;
					finalHeight = Math.Min(finalHeight, 10000);
					Renderer.WorldSize = new Size(this.Width, finalHeight);
					Renderer.Size = this.Size;

					if (Rebuilt != null)
					{
						EventArgs e = new EventArgs();
						Rebuilt.Invoke(this, e);
					}
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
			Rebuild(LastHistory);
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
			Renderer.CurrentMouseLocation = e.Location;
			CurrentMouseLocation = e.Location;

			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				Point mouseDelta = Point.Subtract(e.Location, new Size(LastMouseLocation));
				LastMouseLocation = e.Location;

				Renderer.Offset = Point.Add(Renderer.Offset, new Size(mouseDelta));
			}

			RecalculateHoverBlock.Set();
		}

		public void AddressSpace_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = true;
				LastMouseLocation = e.Location;
				MouseDownLocation = e.Location;
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = true;
				LastMouseLocation = e.Location;
				MouseDownLocation = e.Location;
			}
		}

		public void AddressSpace_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt();
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt();
				}
			}
		}

		public void AddressSpace_MouseWheel(object sender, MouseEventArgs e)
		{
			Matrix currentViewMatrix = new Matrix();
			currentViewMatrix.Translate(Renderer.Offset.X, Renderer.Offset.Y);
			currentViewMatrix.Scale(Renderer.Scale, Renderer.Scale);

			Point[] pointBefore = { CurrentMouseLocation };
			Matrix InvertedTransform = currentViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(pointBefore);

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

			Point[] pointAfter = { CurrentMouseLocation };
			InvertedTransform = currentViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(pointAfter);

			Point delta = Point.Subtract(pointAfter[0], new Size(pointBefore[0]));

			currentViewMatrix.Translate(delta.X, delta.Y);

			Renderer.Offset = new Point((int)currentViewMatrix.OffsetX, (int)currentViewMatrix.OffsetY);
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

		void HoverAt(Point location)
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

		public void CenterAt(Point location)
		{
			Point topLeft = new Point(location.X - (Width / 2), location.Y - (Height / 2));
			Renderer.Scale = 1.0f;
			Renderer.Offset = Point.Subtract(new Point(0, 0), new Size(topLeft));
		}

		public void AddressSpace_MouseLeave(object sender, EventArgs e)
		{
			Renderer.CurrentMouseLocation = new Point(-1, -1);
		}
	}

	public class SelectionChangedEventArgs : EventArgs
	{
		public VisualMemoryBlock SelectedBlock;
	}

	public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}

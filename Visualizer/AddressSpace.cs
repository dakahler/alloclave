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
		UInt64 AddressWidth = 0xFF;

		UInt64 AllocationMin = UInt64.MaxValue;
		UInt64 AllocationMax = UInt64.MinValue;

		// TODO: Too inefficient?
		History LastHistory = new History();

		SortedList<UInt64, VisualMemoryBlock> VisualMemoryBlocks = new SortedList<UInt64, VisualMemoryBlock>();

		RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		AddressSpaceRenderer Renderer;

		VisualMemoryBlock HoverBlock;

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

		public Bitmap GetMainBitmap()
		{
			return Renderer.GetMainBitmap();
		}

		static int CompareKeyValuePair(KeyValuePair<TimeStamp, IPacket> a, KeyValuePair<TimeStamp, IPacket> b)
		{
			return a.Key.CompareTo(b.Key);
		}

		public void Rebuild(History history)
		{
			// TODO: Should probably move this to a separate thread and render to a secondary bitmap
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
									VisualMemoryBlocks.Remove(allocation.Address);
								}

								CombinedList.Add(allocation.Address, pair.Value);

								VisualMemoryBlock block = new VisualMemoryBlock(allocation, AllocationMin, AddressWidth, Width);
								VisualMemoryBlocks.Add(allocation.Address, block);
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

							VisualMemoryBlocks.Remove(free.Address);
						}
					}

					if (CombinedList.Count == 0)
					{
						return;
					}

					if (!VisualMemoryBlocks.ContainsValue(Renderer.SelectedBlock))
					{
						Renderer.SelectedBlock = null;
					}

					HoverBlock = null;
					Renderer.HoverBlock = null;

					int finalHeight = (int)numRows * 2;
					finalHeight = Math.Min(finalHeight, 10000);
					Renderer.WorldSize = new Size(this.Width, finalHeight);
					Renderer.Size = this.Size;
					Renderer.Blocks = VisualMemoryBlocks;

					Renderer.Update();
				}

				this.Invoke((MethodInvoker)( () => Refresh()));

				if (Rebuilt != null)
				{
					EventArgs e = new EventArgs();
					Rebuilt.Invoke(this, e);
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

			// Set the control style to double buffer.
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

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
			// TODO: Still get tearing here, possibly due to no vsync
			// May have to switch to DirectX to fix this
			IntPtr hdc = e.Graphics.GetHdc();
			Renderer.Blit(hdc);
			e.Graphics.ReleaseHdc(hdc);
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

				{
					//Point[] points = { mouseDelta };

					//Matrix InvertedTransform = Renderer.ViewMatrix.Clone();
					//InvertedTransform.Invert();
					//InvertedTransform.TransformVectors(points);

					//Matrix tempViewMatrix = Renderer.ViewMatrix.Clone();
					//tempViewMatrix.Translate(points[0].X, points[0].Y);

					////Rectangle bitmapRectangle = new Rectangle((int)tempViewMatrix.OffsetX, (int)tempViewMatrix.OffsetY, Renderer.GetMainBitmap().Width, Renderer.GetMainBitmap().Height);
					////bitmapRectangle.Width = (int)((float)bitmapRectangle.Width * GlobalScale);
					////bitmapRectangle.Height = (int)((float)bitmapRectangle.Height * GlobalScale);
					////if (bitmapRectangle.Contains(DisplayRectangle))
					//{
					//	Renderer.ViewMatrix = tempViewMatrix.Clone();
					//}

					Renderer.Offset = Point.Add(Renderer.Offset, new Size(mouseDelta));
				}
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

			// TODO: Clamp method?
			if (amountToMove < -4)
			{
				amountToMove = -4;
			}
			else if (amountToMove > 4)
			{
				amountToMove = 4;
			}

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

			Renderer.Update();
			Refresh();
		}

		void SelectAt()
		{
			RecalculateSelectedBlock.Set();
		}

		void HoverTask()
		{
			while (true)
			{
				RecalculateHoverBlock.WaitOne();

				Renderer.HoverBlock = null;
				VisualMemoryBlock tempBlock = new VisualMemoryBlock();
				Point localMouseLocation = Renderer.GetLocalMouseLocation();
				tempBlock.GraphicsPath.AddLine(localMouseLocation, Point.Add(localMouseLocation, new Size(1, 1)));

				lock (RebuildDataLock)
				{
					int index = VisualMemoryBlocks.Values.ToList().BinarySearch(tempBlock, new VisualMemoryBlockComparer());
					if (index >= 0)
					{
						Renderer.HoverBlock = VisualMemoryBlocks.Values[index];
					}
				}

				Renderer.Update();
				this.Invoke((MethodInvoker)(() => Refresh()));
			}
		}

		void SelectTask()
		{
			while (true)
			{
				RecalculateSelectedBlock.WaitOne();

				Renderer.SelectedBlock = null;
				VisualMemoryBlock tempBlock = new VisualMemoryBlock();
				Point localMouseLocation = Renderer.GetLocalMouseLocation();
				tempBlock.GraphicsPath.AddLine(localMouseLocation, Point.Add(localMouseLocation, new Size(1, 1)));

				lock (RebuildDataLock)
				{
					int index = VisualMemoryBlocks.Values.ToList().BinarySearch(tempBlock, new VisualMemoryBlockComparer());
					if (index >= 0)
					{
						Renderer.SelectedBlock = VisualMemoryBlocks.Values[index];
						SelectionChangedEventArgs e = new SelectionChangedEventArgs();
						e.SelectedBlock = VisualMemoryBlocks.Values[index];
						this.Invoke((MethodInvoker)(() => SelectionChanged(this, e)));
					}
				}

				Renderer.Update();
				this.Invoke((MethodInvoker)(() => Refresh()));
			}
		}

		void HoverAt(Point location)
		{
			//printCtrl.Text = "TESSSSST";
			//Tooltip.Show("", this);
		}

		private void AddressSpace_SizeChanged(object sender, EventArgs e)
		{
			// TODO: Might not be viable to do this dynamically for large datasets
			Rebuild(LastHistory);
		}

		private void AddressSpace_MouseHover(object sender, EventArgs e)
		{
			HoverAt(CurrentMouseLocation);
		}

		public void CenterAt(Point location)
		{
			Point topLeft = new Point(location.X - (Width / 2), location.Y - (Height / 2));
			//Renderer.Scale = GlobalScale; // ?
			Renderer.Offset = Point.Subtract(new Point(0, 0), new Size(topLeft));

			Renderer.Update();
			Refresh();
		}

		public void AddressSpace_MouseLeave(object sender, EventArgs e)
		{
			Renderer.CurrentMouseLocation = new Point(-1, -1);
			Renderer.Update();
			Refresh();
		}
	}

	public class SelectionChangedEventArgs : EventArgs
	{
		public VisualMemoryBlock SelectedBlock;
	}

	public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}

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

namespace Alloclave
{
	public partial class AddressSpace : UserControl
	{
		VisualConstraints VisualConstraints = new VisualConstraints();
		Matrix GlobalTransform = new Matrix();
		bool IsLeftMouseDown;
		bool IsMiddleMouseDown;
		Point LastMouseLocation;
		Point MouseDownLocation;
		Point CurrentMouseLocation;
		const int WheelDelta = 120;

		Bitmap MainBitmap;
		float MainBitmapOpacity = 1.0f;

		Bitmap OverlayBitmap;

		// TODO: Too inefficient?
		History LastHistory = new History();
		
		List<VisualMemoryChunk> VisualMemoryChunks = new List<VisualMemoryChunk>();

		RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		BufferedGraphicsContext BackbufferContext;
		BufferedGraphics BackbufferGraphics;
		Graphics DrawingGraphics;
		bool FinishedInitialization;

		public void History_Updated(object sender, EventArgs e)
		{
			LastHistory = sender as History;
			Rebuild(ref LastHistory);
		}

		public void Rebuild(ref History history)
		{
			VisualMemoryChunks.Clear();
			SortedList<TimeStamp, IPacket> allocations = history.Get(typeof(Allocation));
			SortedList<TimeStamp, IPacket> frees = history.Get(typeof(Free));

			// Combine allocation and free lists
			IEnumerable<KeyValuePair<TimeStamp, IPacket>> combinedList = allocations.Union(frees);

			// Create final list, removing allocations as frees are encountered
			SortedList<TimeStamp, IPacket> finalList = new SortedList<TimeStamp, IPacket>();
			foreach (var pair in combinedList)
			{
				if (pair.Value is Allocation)
				{
					finalList.Add(pair.Key, pair.Value);
				}
				else
				{
					int index = finalList.IndexOfValue(pair.Value);
					if (index != -1)
					{
						finalList.RemoveAt(index);
					}
					else
					{
						// This indicates a memory problem on the target side
						// TODO: User-facing error reporting
						throw new InvalidConstraintException();
					}
				}
			}

			// Start by determining the lowest address
			VisualConstraints.StartAddress = UInt64.MaxValue;
			foreach (var pair in combinedList)
			{
				Allocation allocation = pair.Value as Allocation;
				VisualConstraints.StartAddress = Math.Min(VisualConstraints.StartAddress, allocation.Address);
			}
			VisualConstraints.StartAddress = VisualConstraints.StartAddress & ~VisualConstraints.RowAddressWidth;

			// Then actually build the visual data
			foreach (var pair in combinedList)
			{
				Allocation allocation = pair.Value as Allocation;
				VisualMemoryChunk chunk = new VisualMemoryChunk(allocation.Address,
					allocation.Size, VisualConstraints);

				VisualMemoryChunks.Add(chunk);
			}

			MainBitmap = new Bitmap((int)VisualConstraints.RowAddressPixelWidth, 500);
			Graphics gForm = Graphics.FromImage(MainBitmap);
			gForm.Clear(Color.White);
			gForm.SmoothingMode = SmoothingMode.HighSpeed;

			foreach (VisualMemoryChunk chunk in VisualMemoryChunks)
			{
				foreach (VisualMemoryBox box in chunk.Boxes)
				{
					Rectangle rectangle = box.DefaultBox;
					Region region = new Region(rectangle);
					region.Transform(box.Transform);

					SolidBrush brush = new SolidBrush(box.Color);
					gForm.FillRegion(brush, region);
				}
			}

			FinishedInitialization = true;
			Redraw();
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			OverlayBitmap = new Bitmap((int)VisualConstraints.RowAddressPixelWidth, 500);
			Graphics gForm = Graphics.FromImage(OverlayBitmap);
			gForm.Clear(Color.White);
			gForm.SmoothingMode = SmoothingMode.HighSpeed;

			Tooltip.RtbPCtrl = printCtrl;

			this.MouseWheel += AddressSpace_MouseWheel;

			// Set the control style to double buffer.
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			BackbufferContext = BufferedGraphicsManager.Current;

			RecreateBuffers();
		}

		void RecreateBuffers()
		{
			BackbufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

			if (BackbufferGraphics != null)
				BackbufferGraphics.Dispose();

			BackbufferGraphics = BackbufferContext.Allocate(this.CreateGraphics(),
				new Rectangle(0, 0, Math.Max(this.Width, 1), Math.Max(this.Height, 1)));

			// Assign the Graphics object on backbufferGraphics to "drawingGraphics" for easy reference elsewhere.
			DrawingGraphics = BackbufferGraphics.Graphics;

			// This is a good place to assign drawingGraphics.SmoothingMode if you want a better anti-aliasing technique.

			// Invalidate the control so a repaint gets called somewhere down the line.
			this.Invalidate();
		}

		void Redraw()
		{
			if (!FinishedInitialization)
			{
				return;
			}

			DrawingGraphics.Clear(Color.White);
			DrawingGraphics.SmoothingMode = SmoothingMode.HighSpeed;
			DrawingGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			DrawingGraphics.ResetTransform();
			DrawingGraphics.MultiplyTransform(GlobalTransform);

			ColorMatrix cm = new ColorMatrix();
			cm.Matrix33 = MainBitmapOpacity;
			ImageAttributes ia = new ImageAttributes();
			ia.SetColorMatrix(cm);

			DrawingGraphics.DrawImage(MainBitmap,
				new Rectangle(0, 0, MainBitmap.Width, MainBitmap.Height),
				0, 0, MainBitmap.Width, MainBitmap.Height,
				GraphicsUnit.Pixel, ia);

			DrawingGraphics.DrawImage(OverlayBitmap, 0, 0);

			this.Refresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// TODO: Still get tearing here, possibly due to no vsync
			// May have to switch to DirectX to fix this
			if (BackbufferGraphics != null)
			{
				BackbufferGraphics.Render(e.Graphics);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			return;
		}

		private void AddressSpace_MouseMove(object sender, MouseEventArgs e)
		{
			CurrentMouseLocation = e.Location;

			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				Point mouseDelta = Point.Subtract(e.Location, new Size(LastMouseLocation));
				LastMouseLocation = e.Location;

				Point[] points = { mouseDelta };

				Matrix InvertedTransform = GlobalTransform.Clone();
				InvertedTransform.Invert();
				InvertedTransform.TransformVectors(points);

				GlobalTransform.Translate(points[0].X, points[0].Y);
				Redraw();
			}
			else
			{
				UpdateOverlay();
			}
			
		}

		private void AddressSpace_MouseDown(object sender, MouseEventArgs e)
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

		private void AddressSpace_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt(MouseDownLocation);
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt(MouseDownLocation);
				}
			}
		}

		void AddressSpace_MouseWheel(object sender, MouseEventArgs e)
		{
			Point[] pointBefore = { CurrentMouseLocation };
			Matrix InvertedTransform = GlobalTransform.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(pointBefore);

			int amountToMove = e.Delta / WheelDelta;
			float finalScale = 1.0f + (float)amountToMove / 5.0f;
			GlobalTransform.Scale(finalScale, finalScale);

			Point[] pointAfter = { CurrentMouseLocation };
			InvertedTransform = GlobalTransform.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(pointAfter);

			Point delta = Point.Subtract(pointAfter[0], new Size(pointBefore[0]));

			GlobalTransform.Translate(delta.X, delta.Y);

			Redraw();
		}

		void SelectAt(Point location)
		{
			// TODO
		}

		void HoverAt(Point location)
		{
			printCtrl.Text = "TESSSSST";
			Tooltip.Show("", this);
		}

		private void AddressSpace_SizeChanged(object sender, EventArgs e)
		{
			// TODO: Might not be viable to do this dynamically for large datasets
			VisualConstraints.RowAddressPixelWidth = (uint)Width;
			//VisualConstraints.RowAddressPixelHeight = (uint)Height;
			OverlayBitmap = new Bitmap((int)VisualConstraints.RowAddressPixelWidth, 500);
			Rebuild(ref LastHistory);
		}

		private void AddressSpace_MouseHover(object sender, EventArgs e)
		{
			HoverAt(CurrentMouseLocation);
		}

		private void UpdateOverlay()
		{
			Point[] points = { CurrentMouseLocation };
			Point[] invertedPoints = { CurrentMouseLocation };
			GlobalTransform.TransformPoints(points);

			Matrix InvertedTransform = GlobalTransform.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(invertedPoints);

			Point transformedPoint = points[0];
			Point invertedPoint = invertedPoints[0];

			Point[] bitmapBounds = { new Point(0, 0), new Point(MainBitmap.Size.Width, MainBitmap.Size.Height) };

			GlobalTransform.TransformPoints(bitmapBounds);
			Size scaledSize = new Size(bitmapBounds[1].X - bitmapBounds[0].X, bitmapBounds[1].Y - bitmapBounds[0].Y);
			Rectangle bitmapRectangle = new Rectangle(bitmapBounds[0], scaledSize);

			if (bitmapRectangle.Contains(CurrentMouseLocation))
			{
				MainBitmapOpacity = 0.5f;

				// Find the allocation we're hovering over
				foreach (VisualMemoryChunk chunk in VisualMemoryChunks)
				{
					if (chunk.Contains(invertedPoint))
					{
						Graphics gForm = Graphics.FromImage(OverlayBitmap);
						gForm.Clear(Color.White);
						foreach (VisualMemoryBox box in chunk.Boxes)
						{
							Rectangle rectangle = box.DefaultBox;
							Region region = new Region(rectangle);
							region.Transform(box.Transform);

							SolidBrush brush = new SolidBrush(Color.Black);
							gForm.FillRegion(brush, region);
						}

						OverlayBitmap.MakeTransparent(Color.White);

						break;
					}
				}
			}
			else
			{
				MainBitmapOpacity = 1.0f;
			}

			Redraw();
		}
	}
}

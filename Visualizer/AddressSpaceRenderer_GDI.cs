using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	class AddressSpaceRenderer_GDI : AddressSpaceRenderer
	{
		private enum Tasks
		{
			RecreateBuffers,
			Render,
			UpdateOverlay,
			Redraw,

			NumTasks
		};

		private bool[] TaskList = new bool[(int)Tasks.NumTasks];

		private Bitmap ClippedBitmap;
		private Bitmap MainBitmap;
		private Bitmap OverlayBitmap;

		private Graphics ClippedGraphics;
		private Graphics MainGraphics;
		private Graphics OverlayGraphics;

		private BufferedGraphicsContext BackbufferContext;
		private BufferedGraphics BackbufferGraphics;
		private Graphics DrawingGraphics;

		AddressSpace Parent;

		public AddressSpaceRenderer_GDI(AddressSpace parent)
		{
			Parent = parent;
			BackbufferContext = BufferedGraphicsManager.Current;
			RecreateBuffers();
		}

		~AddressSpaceRenderer_GDI()
		{
			if (MainGraphics != null)
			{
				MainGraphics.Dispose();
			}

			if (OverlayGraphics != null)
			{
				OverlayGraphics.Dispose();
			}
		}

		void RecreateBuffers()
		{
			TaskList[(int)Tasks.RecreateBuffers] = false;

			ClippedBitmap = new Bitmap(_Size.Width, _Size.Height, PixelFormat.Format32bppPArgb);
			if (ClippedGraphics != null)
			{
				ClippedGraphics.Dispose();
			}
			ClippedGraphics = Graphics.FromImage(ClippedBitmap);

			BackbufferContext.MaximumBuffer = new Size(_Size.Width + 1, _Size.Height + 1);

			if (BackbufferGraphics != null)
				BackbufferGraphics.Dispose();

			BackbufferGraphics = BackbufferContext.Allocate(ClippedGraphics,
				new Rectangle(0, 0, Math.Max(_Size.Width, 1), Math.Max(_Size.Height, 1)));

			// Assign the Graphics object on backbufferGraphics to "drawingGraphics" for easy reference elsewhere.
			DrawingGraphics = BackbufferGraphics.Graphics;
			DrawingGraphics.Clip = new System.Drawing.Region(new Rectangle(0, 0, _Size.Width, _Size.Height));
			DrawingGraphics.CompositingQuality = CompositingQuality.HighSpeed;
			DrawingGraphics.SmoothingMode = SmoothingMode.HighSpeed;
			DrawingGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

			// This is a good place to assign drawingGraphics.SmoothingMode if you want a better anti-aliasing technique.

			// Invalidate the control so a repaint gets called somewhere down the line.
			Render();
		}

		protected override void Render()
		{
			if (MemoryBlockManager.Instance.Count == 0)
			{
				return;
			}

			TaskList[(int)Tasks.Render] = false;

			if (MainBitmap != null)
			{
				MainBitmap.Dispose();
			}

			if (_WorldSize.Width == 0 || _WorldSize.Height == 0)
			{
				return;
			}

			// Then actually build the visual data
			MainBitmap = new Bitmap(_WorldSize.Width, _WorldSize.Height, PixelFormat.Format32bppPArgb);
			if (MainGraphics != null)
			{
				MainGraphics.Dispose();
			}
			MainGraphics = Graphics.FromImage(MainBitmap);
			MainGraphics.Clear(Color.White);
			MainGraphics.CompositingQuality = CompositingQuality.HighSpeed;
			MainGraphics.SmoothingMode = SmoothingMode.HighSpeed;
			MainGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

			foreach (var block in MemoryBlockManager.Instance)
			{
				SolidBrush brush = new SolidBrush(block._Color);
				MainGraphics.FillPath(brush, block.GraphicsPath);
			}

			if (OverlayBitmap != null)
			{
				OverlayBitmap.Dispose();
			}

			OverlayBitmap = new Bitmap(_WorldSize.Width, _WorldSize.Height, PixelFormat.Format32bppPArgb);
			if (OverlayGraphics != null)
			{
				OverlayGraphics.Dispose();
			}
			OverlayGraphics = Graphics.FromImage(OverlayBitmap);
			OverlayGraphics.CompositingQuality = CompositingQuality.HighSpeed;
			OverlayGraphics.SmoothingMode = SmoothingMode.HighSpeed;
			OverlayGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

			UpdateOverlay();
		}

		private void UpdateOverlay()
		{
			//if (MainBitmap == null || OverlayBitmap == null)
			//{
			//	return;
			//}

			//TaskList[(int)Tasks.UpdateOverlay] = false;

			//Point[] points = { _CurrentMouseLocation };
			//_ViewMatrix.TransformPoints(points);

			//Point transformedPoint = points[0];

			//Point[] bitmapBounds = { new Point(0, 0), new Point(MainBitmap.Size.Width, MainBitmap.Size.Height) };

			//_ViewMatrix.TransformPoints(bitmapBounds);
			//Size scaledSize = new Size(bitmapBounds[1].X - bitmapBounds[0].X, bitmapBounds[1].Y - bitmapBounds[0].Y);
			//Rectangle bitmapRectangle = new Rectangle(bitmapBounds[0], scaledSize);

			//// Find the allocation we're hovering over
			//OverlayGraphics.Clear(Color.Transparent);
			//VisualMemoryBlock tempBlock = new VisualMemoryBlock();
			//Point localMouseLocation = GetLocalMouseLocation();
			//tempBlock.GraphicsPath.AddLine(localMouseLocation, Point.Add(localMouseLocation, new Size(1,1)));

			//int index = _Blocks.Values.ToList().BinarySearch(tempBlock, new VisualMemoryBlockComparer());
			//if (index >= 0)
			//{
			//	VisualMemoryBlock targetBlock = _Blocks.Values[index];

			//	SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
			//	OverlayGraphics.FillPath(brush, targetBlock.GraphicsPath);
			//}

			//if (_SelectedBlock != null)
			//{
			//	SolidBrush selectedBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			//	OverlayGraphics.FillPath(selectedBrush, _SelectedBlock.GraphicsPath);
			//}

			//Redraw();
		}

		//public override SortedList<UInt64, VisualMemoryBlock> Blocks
		//{
		//	set
		//	{
		//		base.Blocks = value;
		//		TaskList[(int)Tasks.Render] = true;
		//	}
		//}

		public override VisualMemoryBlock SelectedBlock
		{
			set
			{
				base.SelectedBlock = value;
				TaskList[(int)Tasks.UpdateOverlay] = true;
			}
		}

		public override Size Size
		{
			set
			{
				base.Size = value;
				TaskList[(int)Tasks.RecreateBuffers] = true;
			}
		}

		//public override Matrix ViewMatrix
		//{
		//	set
		//	{
		//		base.ViewMatrix = value;
		//		TaskList[(int)Tasks.Redraw] = true;
		//	}
		//}

		public override Size WorldSize
		{
			set
			{
				base.WorldSize = value;
				TaskList[(int)Tasks.RecreateBuffers] = true;
			}
		}

		public override Point CurrentMouseLocation
		{
			set
			{
				base.CurrentMouseLocation = value;
				TaskList[(int)Tasks.UpdateOverlay] = true;
			}
		}

		//public override void Update()
		//{
		//	if (TaskList[(int)Tasks.RecreateBuffers])
		//	{
		//		RecreateBuffers();
		//	}
		//	if (TaskList[(int)Tasks.Render])
		//	{
		//		Render();
		//	}
		//	if (TaskList[(int)Tasks.UpdateOverlay])
		//	{
		//		UpdateOverlay();
		//	}
		//	if (TaskList[(int)Tasks.Redraw])
		//	{
		//		Redraw();
		//	}
		//}

		//protected override void Redraw()
		//{
		//	//TaskList[(int)Tasks.Redraw] = false;

		//	//DrawingGraphics.Clear(Color.White);
		//	//DrawingGraphics.SmoothingMode = SmoothingMode.HighSpeed;
		//	//DrawingGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
		//	//DrawingGraphics.ResetTransform();
		//	//DrawingGraphics.MultiplyTransform(_ViewMatrix);

		//	//DrawingGraphics.DrawImage(MainBitmap, 0, 0);
		//	//DrawingGraphics.DrawImage(OverlayBitmap, 0, 0);
		//}

		//public override void Blit(IntPtr deviceContext)
		//{
		//	BackbufferGraphics.Render(deviceContext);
		//}

		//public override Bitmap GetMainBitmap()
		//{
		//	return MainBitmap;
		//}
	}
}

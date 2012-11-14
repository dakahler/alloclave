using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Alloclave
{
	public partial class AddressSpace : UserControl
	{
		Bitmap MainBitmap;
		VisualConstraints VisualConstraints = new VisualConstraints();

		private History _History;
		public History History
		{
			get
			{ 
				return _History;
			}
			set
			{
				_History = value;
				if (_History != null)
				{
					_History.Updated += new EventHandler(History_Updated);
					Rebuild();
				}
			}
		}

		List<VisualMemoryChunk> VisualMemoryChunks = new List<VisualMemoryChunk>();

		void History_Updated(object sender, EventArgs e)
		{
			Rebuild();
		}

		private void Rebuild()
		{
			Stack<TimeSlice> allocations = History.Get(typeof(Allocation));
			foreach (TimeSlice timeSlice in allocations)
			{
				Allocation allocation = timeSlice.Data as Allocation;
				VisualMemoryChunk chunk = new VisualMemoryChunk(allocation.Address,
					allocation.Size, VisualConstraints);

				VisualMemoryChunks.Add(chunk);
			}

			Invalidate();
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			MainBitmap = new Bitmap(this.Width, this.Height);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics gForm = e.Graphics;
			gForm.Clear(Color.White);

			//GraphicsPath gp = new GraphicsPath();

			Graphics g = Graphics.FromImage(MainBitmap);
			g.Clear(Color.White);

			foreach (VisualMemoryChunk chunk in VisualMemoryChunks)
			{
				foreach (VisualMemoryBox box in chunk.Boxes)
				{
					Rectangle rectangle = box.DefaultBox;
					Region region = new Region(rectangle);
					region.Transform(box.Transform);
					
					SolidBrush brush = new SolidBrush(Color.Red);
					g.FillRegion(brush, region);
				}
			}

			//gp.Transform(matrix);

			//PointF[] pts=gp.PathPoints;

			//Pen pen = new Pen(Color.Red);
			//g.DrawPolygon(pen, pts);

			gForm.DrawImage(MainBitmap, 0, 0, MainBitmap.Width, MainBitmap.Height);

			g.Dispose();

			base.OnPaint(e);
		}
	}
}

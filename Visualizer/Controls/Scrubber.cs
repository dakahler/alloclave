using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Alloclave
{
	public partial class Scrubber : UserControl
	{
		Bitmap MainBitmap;
		Graphics MainGraphics;
		bool LeftMouseDown;

		public float Position = 1.0f;

		const float widthPercentage = 0.9f;
		const float heightPercentage = 0.9f;

		public event EventHandler PositionChanged;

		public Scrubber()
		{
			InitializeComponent();

			Rebuild();
		}

		void Rebuild()
		{
			if (Width == 0 || Height == 0)
			{
				return;
			}

			MainBitmap = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
			if (MainGraphics != null)
			{
				MainGraphics.Dispose();
			}
			MainGraphics = Graphics.FromImage(MainBitmap);
			MainGraphics.Clear(Color.White);
			MainGraphics.CompositingQuality = CompositingQuality.HighSpeed;
			MainGraphics.SmoothingMode = SmoothingMode.HighSpeed;
			MainGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

			Redraw();
		}

		void Redraw()
		{
			if (MainGraphics == null)
			{
				return;
			}

			MainGraphics.Clear(Color.White);

			int barWidth = (int)((float)Width * widthPercentage);

			const int barHeight = 2;
			int barX = (Width - barWidth) / 2;
			int barY = (Height / 2) -(barHeight / 2);

			Rectangle barRectangle = new Rectangle(barX, barY, barWidth, barHeight);

			SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			MainGraphics.FillRectangle(brush, barRectangle);

			int circleWidthHeight = (int)(Height * heightPercentage);
			int circleX = barX + (int)((float)barWidth * Position);
			int circleY = barY - (circleWidthHeight / 2);

			if (!LeftMouseDown)
			{
				MainGraphics.DrawImage(Properties.Resources.ScrubberDot, circleX, circleY, circleWidthHeight, circleWidthHeight);
			}
			else
			{
				MainGraphics.DrawImage(Properties.Resources.ScrubberDot_Selected, circleX, circleY, circleWidthHeight, circleWidthHeight);
			}

			Refresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (MainBitmap == null)
			{
				base.OnPaint(e);
			}

			e.Graphics.DrawImage(MainBitmap, new Point(0, 0));
		}

		private void Scrubber_SizeChanged(object sender, EventArgs e)
		{
			Rebuild();
		}

		private void Scrubber_MouseDown(object sender, MouseEventArgs e)
		{
			LeftMouseDown = true;

			SetPosition(e);
			Redraw();

			if (PositionChanged != null)
			{
				PositionChanged(this, new EventArgs());
			}
		}

		private void Scrubber_MouseUp(object sender, MouseEventArgs e)
		{
			LeftMouseDown = false;
			Redraw();
		}

		private void Scrubber_MouseMove(object sender, MouseEventArgs e)
		{
			if (LeftMouseDown)
			{
				SetPosition(e);
				Redraw();

                if (PositionChanged != null)
                {
                    PositionChanged(this, new EventArgs());
                }
			}
		}

		private void SetPosition(MouseEventArgs e)
		{
			int barWidth = (int)((float)Width * widthPercentage);
			int barX = (Width - barWidth) / 2;
			int circleWidthHeight = (int)(Height * heightPercentage);
			int adjustedMouseX = e.X - barX - (circleWidthHeight / 2);

			Position = ((float)adjustedMouseX / (float)barWidth);
			Position = Math.Min(Position, 1.0f);
			Position = Math.Max(Position, 0.0f);
		}
	}
}

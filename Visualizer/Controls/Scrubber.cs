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

		// TODO: Static hack
		public static float _Position = 1.0f;
		public static float Position
		{
			get
			{
				return _Position;
			}
			set
			{
				_Position = value.Clamp(0.0f, 1.0f);

				if (Instance != null)
				{
					Instance.Redraw();

					if (Instance.PositionChanged != null)
					{
						Instance.PositionChanged(Instance, new EventArgs());
					}
				}
			}
		}

		const float WidthPercentage = 0.02f;

		public event EventHandler PositionChanged;

		public static Scrubber Instance;

		bool HandleExists;

		Object LockObject = new Object();

		public Scrubber()
		{
			InitializeComponent();

			Rebuild();

			Instance = this;

			HandleCreated += Scrubber_HandleCreated;
		}

		void Scrubber_HandleCreated(object sender, EventArgs e)
		{
			HandleExists = true;
		}

		void Rebuild()
		{
			if (Width == 0 || Height == 0)
			{
				return;
			}

			lock (LockObject)
			{
				MainBitmap = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
				if (MainGraphics != null)
				{
					MainGraphics.Dispose();
				}

				MainGraphics = Graphics.FromImage(MainBitmap);
				MainGraphics.Clear(Color.FromArgb(100, 100, 100));
				MainGraphics.CompositingQuality = CompositingQuality.HighSpeed;
				MainGraphics.SmoothingMode = SmoothingMode.HighSpeed;
				MainGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			}

			Redraw();
		}

		void Redraw()
		{
			if (MainGraphics == null)
			{
				return;
			}

			lock (LockObject)
			{
				MainGraphics.Clear(Color.FromArgb(100, 100, 100));

				int barWidth = (int)((float)Width * WidthPercentage);
				int barX = (int)((Width - barWidth) * _Position);
				Rectangle barRectangle = new Rectangle(barX, 0, barWidth, Height);

				SolidBrush brush = new SolidBrush(Color.FromArgb(148, 168, 172));
				MainGraphics.FillRectangle(brush, barRectangle);

				if (HandleExists && IsHandleCreated)
				{
					this.Invoke((MethodInvoker)(() =>
						{
							Refresh();
						}
					));
				}
			}
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

			if (Cursor.Current != Cursors.Hand)
			{
				Cursor.Current = Cursors.Hand;
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
			}

			// Setup cursor
			int barWidth = (int)((float)Width * WidthPercentage);
			int barX = (int)((Width - barWidth) * _Position);

			if (e.X >= barX && e.X <= barX + barWidth)
			{
				if (Cursor.Current != Cursors.Hand)
				{
					Cursor.Current = Cursors.Hand;
				}
			}
			else
			{
				if (Cursor.Current != Cursors.Default)
				{
					Cursor.Current = Cursors.Default;
				}
			}
		}

		private void SetPosition(MouseEventArgs e)
		{
			// This all calculates the fudge factor needed to make sure
			// the scrubber is centered horizontally on the mouse location
			_Position = ((float)e.X / (float)Width).Clamp(0.0f, 1.0f);
			float percentage = _Position - 0.5f;
			int barWidth = (int)((float)Width * WidthPercentage);
			int fudgeWidth = (int)((float)barWidth * percentage);
			float fudgePercentage = (float)fudgeWidth / (float)Width;

			Position = _Position + fudgePercentage;
		}
	}
}

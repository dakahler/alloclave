using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	public abstract partial class AddressSpaceScroller : UserControl
	{
		private bool IsLeftMouseDown;
		private bool IsMiddleMouseDown;

		public event MouseEventHandler FocusChanged;

		protected abstract void Render(PaintEventArgs e);

		public AddressSpaceScroller()
		{
			InitializeComponent();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			return;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Render(e);
		}

		private void AddressSpaceScroller_SizeChanged(object sender, EventArgs e)
		{
			Refresh();
		}

		private void AddressSpaceScroller_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = true;
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = true;
			}
		}

		private void AddressSpaceScroller_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				SetFocus(e.Location);
				Refresh();
			}
		}

		private void AddressSpaceScroller_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				SetFocus(e.Location);
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				SetFocus(e.Location);
			}
		}

		private void SetFocus(Point focus)
		{
			// TODO
			//if (FocusChanged != null)
			//{
			//	float scaleX = (float)MainBitmap.Width / (float)Width;
			//	float scaleY = (float)MainBitmap.Height / (float)Height;

			//	Point finalPoint = new Point((int)((float)focus.X * scaleX), (int)((float)focus.Y * scaleY));

			//	MouseEventArgs eventArgs = new MouseEventArgs(MouseButtons.Left, 1, finalPoint.X, finalPoint.Y, 0);
			//	FocusChanged(this, eventArgs);
			//}
		}
	}
}

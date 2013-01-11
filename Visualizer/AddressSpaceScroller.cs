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

		public int ParentWidth;

		public event MouseEventHandler FocusChanged;

		protected abstract void Render(PaintEventArgs e);

		public AddressSpaceScroller(int parentWidth)
		{
			InitializeComponent();

			ParentWidth = parentWidth;
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

		protected void AddressSpaceScroller_MouseDown(object sender, MouseEventArgs e)
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

		protected void AddressSpaceScroller_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				SetFocus(e.Location);
				Refresh();
			}
		}

		protected void AddressSpaceScroller_MouseUp(object sender, MouseEventArgs e)
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

		protected void SetFocus(Point focus)
		{
			if (FocusChanged != null)
			{
				RectangleF lowerBounds = AddressSpace.VisualMemoryBlocks.Values[AddressSpace.VisualMemoryBlocks.Count - 1].GraphicsPath.GetBounds();

				UInt64 maxWidth = (UInt64)ParentWidth;
				UInt64 maxHeight = (UInt64)lowerBounds.Bottom;

				float scaleX = (float)maxWidth / (float)Width;
				float scaleY = (float)maxHeight / (float)Height;

				Point finalPoint = new Point((int)((float)focus.X * scaleX), (int)((float)focus.Y * scaleY));

				MouseEventArgs eventArgs = new MouseEventArgs(MouseButtons.Left, 1, finalPoint.X, finalPoint.Y, 0);
				FocusChanged(this, eventArgs);
			}
		}
	}
}

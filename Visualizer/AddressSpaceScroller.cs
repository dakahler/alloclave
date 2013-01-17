using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

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
				SetFocus(e.Location.ToVector());
				Refresh();
			}
		}

		protected void AddressSpaceScroller_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				SetFocus(e.Location.ToVector());
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				SetFocus(e.Location.ToVector());
			}
		}

		protected void SetFocus(Vector focus)
		{
			if (FocusChanged != null)
			{
				// TODO: Implement GetBounds in MemoryBlockManager
				Rectangle bounds = MemoryBlockManager.Instance.Bounds;

				UInt64 maxWidth = (UInt64)ParentWidth;
				UInt64 maxHeight = (UInt64)bounds.Bottom;

				Vector scale = new Vector((float)maxWidth / (float)Width, (float)maxHeight / (float)Height);

				Vector finalPoint = new Vector(focus.X * scale.X, focus.Y * scale.Y);

				MouseEventArgs eventArgs = new MouseEventArgs(MouseButtons.Left, 1, (int)finalPoint.X, (int)finalPoint.Y, 0);
				FocusChanged(this, eventArgs);
			}
		}
	}
}

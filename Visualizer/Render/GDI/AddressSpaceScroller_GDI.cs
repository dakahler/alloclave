using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	class AddressSpaceScroller_GDI : AddressSpaceScroller
	{
		public Bitmap MainBitmap;

		public AddressSpaceScroller_GDI(int parentWidth)
			: base(parentWidth)
		{

		}

		//protected override void Render(PaintEventArgs e)
		//{
		//	if (MainBitmap != null)
		//	{
		//		e.Graphics.DrawImage(MainBitmap, new Rectangle(0, 0, Width, Height));
		//	}
		//}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	internal class RichToolTip : ToolTip
	{
		Size toolTipSize = new Size();

		/// <summary>
		/// Gets or sets the RichTextBoxPrintCtrl to show as tool tip
		/// </summary>
		public RichTextBoxPrintCtrl RtbPCtrl { get; set; }

		public RichToolTip()
		{
			OwnerDraw = true;
			Popup += new PopupEventHandler(RichToolTip_Popup);
			Draw += new DrawToolTipEventHandler(RichToolTip_Draw);
			RtbPCtrl = new RichTextBoxPrintCtrl();
		}

		void RichToolTip_Draw(object sender, DrawToolTipEventArgs e)
		{
			e.Graphics.Clear(Color.White);

			using (Image image = new Bitmap(toolTipSize.Width, toolTipSize.Height))
			{
				using (Graphics g = Graphics.FromImage(image))
				{
					RtbPCtrl.Print(0, RtbPCtrl.Text.Length, g, new Rectangle(RtbPCtrl.Location, toolTipSize));
				}
				e.Graphics.DrawImageUnscaled(image, e.Bounds);
			}
		}

		//Fires before the draw event. So set the tooltip size here.
		void RichToolTip_Popup(object sender, PopupEventArgs e)
		{
			using (Graphics g = RtbPCtrl.CreateGraphics())
			{
				toolTipSize = g.MeasureString(RtbPCtrl.Rtf.Trim(), RtbPCtrl.Font).ToSize();
				e.ToolTipSize = toolTipSize;
			}
		}
	}
}

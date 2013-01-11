using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Alloclave
{
	public partial class AllocationForm : ToolForm
	{
		public AllocationForm()
		{
			InitializeComponent();

			AddressSpaceControl.Rebuilt += AddressSpaceControl_Rebuilt;
			addressSpaceScroller.FocusChanged += addressSpaceScroller_FocusChanged;
			this.SizeChanged += AllocationForm_SizeChanged;
		}

		void AllocationForm_SizeChanged(object sender, EventArgs e)
		{
			addressSpaceScroller.ParentWidth = AddressSpaceControl.Width;
		}

		void addressSpaceScroller_FocusChanged(object sender, MouseEventArgs e)
		{
			AddressSpaceControl.CenterAt(e.Location);
		}

		void AddressSpaceControl_Rebuilt(object sender, EventArgs e)
		{
			//Bitmap mainBitmap = AddressSpaceControl.GetMainBitmap();
			//addressSpaceScroller.MainBitmap = mainBitmap;
		}
	}
}

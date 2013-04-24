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

			TopLevel = false;
			AddressSpaceControl.Rebuilt += AddressSpaceControl_Rebuilt;
			AddressSpaceScroller.FocusChanged += addressSpaceScroller_FocusChanged;
			this.SizeChanged += AllocationForm_SizeChanged;
			MainScrubber.PositionChanged += MainScrubber_PositionChanged;
		}

		void MainScrubber_PositionChanged(object sender, EventArgs e)
		{
			AddressSpaceControl.Rebuild(History.Instance);
		}

		void AllocationForm_SizeChanged(object sender, EventArgs e)
		{
			AddressSpaceScroller.ParentWidth = AddressSpaceControl.Width;
		}

		void addressSpaceScroller_FocusChanged(object sender, MouseEventArgs e)
		{
			AddressSpaceControl.CenterAt(e.Location.ToVector());
		}

		void AddressSpaceControl_Rebuilt(object sender, EventArgs e)
		{
			//Bitmap mainBitmap = AddressSpaceControl.GetMainBitmap();
			//addressSpaceScroller.MainBitmap = mainBitmap;
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class TransportForm : ToolForm
	{
		Profile Profile;

		// TODO: Should this be spawned immediately?
		AllocationForm AllocationForm = new AllocationForm();
		MessagesForm MessagesForm = new MessagesForm();
		InfoForm InfoForm = new InfoForm();

		public History History
		{
			get
			{
				return Profile.History;
			}
			set
			{
				Profile.History = value;
				if (Profile.History != null)
				{
					History.Updated += new EventHandler(AllocationForm.AddressSpaceControl.History_Updated);
					AllocationForm.AddressSpaceControl.Rebuild(ref Profile.History);
				}
			}
		}

		public TransportForm()
		{
			InitializeComponent();

			AllocationForm.AddressSpaceControl.SelectionChanged += AddressSpaceControl_SelectionChanged;
		}

		void AddressSpaceControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			InfoForm.Update(e.SelectedChunk.Allocation);
		}

		public TransportForm(ref Transport transport)
			: this()
		{
			Profile = new Profile(ref transport);
			History = new History();

			AllocationForm.Show(DockPanel);
			MessagesForm.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide);
			InfoForm.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide);
		}

		private void testToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Allocation testAllocation1 = new Allocation();
			testAllocation1.Address = 0x0;
			testAllocation1.Size = 8;
			Profile.History.Add(testAllocation1, 0);

			Allocation testAllocation2 = new Allocation();
			testAllocation2.Address = 0x1fe;
			testAllocation2.Size = 32;
			Profile.History.Add(testAllocation2, 0);

			Allocation testAllocation3 = new Allocation();
			testAllocation3.Address = 0x404;
			testAllocation3.Size = 15;
			Profile.History.Add(testAllocation3, 0);

			Allocation testAllocation4 = new Allocation();
			testAllocation4.Address = 0x6a0;
			testAllocation4.Size = 4;
			Profile.History.Add(testAllocation4, 0);
		}
	}
}

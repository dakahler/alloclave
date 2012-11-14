using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class Main : Form
	{
		Profile Profile;

		// TODO: This should be set in the UI
		Transport Transport;

		public Main()
		{
			InitializeComponent();

			TargetSystemInfo targetSystemInfo = new TargetSystemInfo();
			Transport = new Win32Transport(targetSystemInfo);

			Profile = new Profile(Transport);
			AddressSpaceControl.History = Profile.History;
		}

		private void TEST_Click(object sender, EventArgs e)
		{
			Allocation testAllocation1 = new Allocation();
			testAllocation1.Address = 0x1;
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

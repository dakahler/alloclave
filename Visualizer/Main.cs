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
	}
}

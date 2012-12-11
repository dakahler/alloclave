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
	public partial class InfoForm : ToolForm
	{
		// TODO: Should this support other types of messages?

		public InfoForm()
		{
			InitializeComponent();
		}

		public void Update(Allocation allocation)
		{
			if (allocation.Architecture == Common.Architecture._32Bit)
			{
				AddressLabel.Text = String.Format("Address: 0x{0:X8}", allocation.Address);
			}
			else
			{
				AddressLabel.Text = String.Format("Size: 0x{0:X16}", allocation.Address);
			}

			SizeLabel.Text = String.Format("Address: {0} bytes", allocation.Size);
		}
	}
}

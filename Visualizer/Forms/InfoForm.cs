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

		PdbParser pdbParser = new PdbParser();

		public InfoForm()
		{
			InitializeComponent();

			// TODO: Temp location, needs to be pulled out to plugin
			pdbParser.Open(@"C:\dev\Alloclave\Debug_Collector\TestCollector.pdb");
		}

		public void Update(Allocation allocation)
		{
			if (allocation.Architecture == Common.Architecture._32Bit)
			{
				AddressLabel.Text = String.Format("Address: 0x{0:X8}", allocation.Address);
			}
			else
			{
				AddressLabel.Text = String.Format("Address: 0x{0:X16}", allocation.Address);
			}

			SizeLabel.Text = String.Format("Size: {0} bytes", allocation.Size);
			HeapLabel.Text = String.Format("Heap ID: {0}", allocation.HeapId);

			StackComboBox.Items.Clear();
			foreach (CallStack.Frame frame in allocation.Stack.Frames)
			{
				// TODO: 64-bit
				String addressText;
				if (allocation.Architecture == Common.Architecture._32Bit)
				{
					addressText = String.Format("0x{0:X8}: ", frame.Address);
				}
				else
				{
					addressText = String.Format("0x{0:X16}: ", frame.Address);
				}

				String functionName = addressText;
				String rawFunctionName = pdbParser.GetFunctionName(frame.Address);
				if (rawFunctionName.Contains("NULL_THUNK_DATA") || rawFunctionName == String.Empty)
				{
					functionName += "Unknown";
				}
				else
				{
					functionName += rawFunctionName;
				}

				StackComboBox.Items.Add(functionName);
			}

			if (StackComboBox.Items.Count > 0)
			{
				StackComboBox.SelectedIndex = 0;
			}
		}
	}
}

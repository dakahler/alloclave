using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	internal partial class InfoForm : ToolForm
	{
		// TODO: Should this support other types of messages?

		private Allocation CurrentAllocation;

		public InfoForm()
		{
			InitializeComponent();
			TopLevel = false;
		}

		public void Update(Allocation allocation)
		{
			CurrentAllocation = allocation;

			if (!File.Exists(SymbolLookup.SymbolsPath))
			{
				symbolsNotFoundLabel.Visible = true;
			}

			if (allocation.Architecture == Common.Architecture._32Bit)
			{
				AddressLabel.Text = String.Format("Address: 0x{0:X8}", allocation.Address);
			}
			else
			{
				AddressLabel.Text = String.Format("Address: 0x{0:X16}", allocation.Address);
			}

			SizeLabel.Text = String.Format("Size: {0} bytes", allocation.Size);
			//HeapLabel.Text = String.Format("Heap ID: {0}", allocation.HeapId);

			StackTable.Rows.Clear();
			foreach (CallStack.Frame frame in allocation.Stack.Frames.Reverse())
			{
				String addressText;
				if (allocation.Architecture == Common.Architecture._32Bit)
				{
					addressText = String.Format("0x{0:X8}", frame.Address);
				}
				else
				{
					addressText = String.Format("0x{0:X16}", frame.Address);
				}

				// TODO: Is this parser-specific?
				String functionName = "";
				if (SymbolLookup.Instance != null)
				{
					String rawFunctionName = SymbolLookup.Instance.Lookup(frame.Address);
					if (String.IsNullOrEmpty(rawFunctionName) || rawFunctionName.Contains("NULL_THUNK_DATA"))
					{
						functionName += "Unknown";
					}
					else
					{
						functionName += rawFunctionName;
					}
				}
				else
				{
					functionName += "Symbol parser not found!";
				}

				StackTable.Rows.Add(addressText, functionName, "", "");
			}
		}

		private void symbolsNotFoundLabel_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				SymbolLookup.SymbolsPath = openFileDialog.FileName;
				symbolsNotFoundLabel.Visible = false;
				Update(CurrentAllocation);
			}
		}
	}
}

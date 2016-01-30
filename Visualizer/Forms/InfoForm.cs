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

        bool AllowSymbolsErrors;

        /// <summary>
        /// Info form constructor
        /// </summary>
        /// <param name="fromXml">If true, data is coming from a profile loaded from a file.</param>
		public InfoForm(bool fromXml)
		{
			InitializeComponent();
			TopLevel = false;
            AllowSymbolsErrors = !fromXml;
		}

		public void Update(Allocation allocation)
		{
			CurrentAllocation = allocation;

			if (AllowSymbolsErrors && !File.Exists(SymbolLookup.SymbolsPath))
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
                    functionName += "Unknown";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Forms;

namespace Alloclave_Plugin
{
	[Export(typeof(Alloclave.Transport))]
	[ExportMetadata("Name", "Process Spy")]
	public class ProcessSpyTransport : Alloclave.Transport
	{
		static Alloclave.TargetSystemInfo temp;
		Process TargetProcess;

		public ProcessSpyTransport()
			: base(temp)
		{
			
		}

		public ProcessSpyTransport(Alloclave.TargetSystemInfo targetSystemInfo)
			: base(targetSystemInfo)
		{
			
		}

		public override void Connect()
		{

		}

		public override void Disconnect()
		{

		}

		public override void SpawnCustomUI()
		{
			ProcessForm processForm = new ProcessForm();
			if (processForm.ShowDialog() == DialogResult.OK)
			{
				String targetProcessName = processForm.ProcessComboBox.SelectedItem.ToString();
				Process[] processes = Process.GetProcesses();
				foreach (Process process in processes)
				{
					if (targetProcessName == process.ProcessName)
					{
						TargetProcess = process;
						break;
					}
				}
			}
		}
	}
}

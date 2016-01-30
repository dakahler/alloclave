using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Alloclave;
using System.Windows.Threading;
using CommandLine;

namespace Alloclave_Plugin
{
	[Export(typeof(Alloclave.Transport))]
	[ExportMetadata("Name", "Process Inject")]
	public class ProcessInjectTransport : Alloclave_Plugin.Win32Transport
	{
		static Alloclave.TargetSystemInfo temp = new Alloclave.TargetSystemInfo(
			"Process Inject", "", 0, Common.Architecture._32Bit, Common.Endianness.LittleEndian);
		Process TargetProcess;

		CommandLineOptions options = new CommandLineOptions();
		String TargetProcessName;

		System.Windows.Threading.Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;

		public ProcessInjectTransport()
			: this(temp)
		{
			
		}

		public ProcessInjectTransport(Alloclave.TargetSystemInfo targetSystemInfo)
			: base(targetSystemInfo)
		{
			ICommandLineParser parser = new CommandLineParser();
			String[] args = Environment.GetCommandLineArgs();
			if (parser.ParseArguments(args, options))
			{
				if (options.ProcessName != null)
				{
					TargetProcessName = options.ProcessName;
				}
			}
		}

		public override void Connect()
		{
			
		}

		public override void Disconnect()
		{

		}

		public override void SpawnCustomUI(IWin32Window owner)
		{
			if (TargetProcessName == null)
			{
				ProcessForm processForm = new ProcessForm();
				if (processForm.ShowDialog(owner) != DialogResult.OK)
				{
					return;
				}

				TargetProcessName = processForm.ProcessComboBox.SelectedItem.ToString();
			}

			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (TargetProcessName == process.ProcessName)
				{
					TargetProcess = process;
					break;
				}
			}

			// TODO: Should probably be in Connect?
			if (TargetProcess != null)
			{
				InjectHelper injectHelper = new InjectHelper();
				injectHelper.Inject((UInt64)TargetProcess.Id);

				var task3 = new Task(() => MonitorHeap(ref TargetProcess), TaskCreationOptions.LongRunning);
				task3.Start();
			}
		}

		private void MonitorHeap(ref Process process)
		{

		}
	}
}

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
	[ExportMetadata("Name", "Process Spy")]
	public class ProcessSpyTransport : Alloclave.Transport
	{
		static Alloclave.TargetSystemInfo temp = new Alloclave.TargetSystemInfo(
			"Process Spy", "", 0, Common.Architecture._64Bit, Common.Endianness.LittleEndian);
		Process TargetProcess;

		CommandLineOptions options = new CommandLineOptions();
		String TargetProcessName;

		System.Windows.Threading.Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;

		public ProcessSpyTransport()
			: this(temp)
		{
			
		}

		public ProcessSpyTransport(Alloclave.TargetSystemInfo targetSystemInfo)
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
				var task3 = new Task(() => MonitorHeap(ref TargetProcess), TaskCreationOptions.LongRunning);
				task3.Start();
			}
		}

		private void MonitorHeap(ref Process process)
		{
			int pid = process.Id;
			HeapWalker heapWalker = new HeapWalker();
			List<AllocationData> oldData = new List<AllocationData>();
			while (true)
			{
				List<AllocationData> newData = heapWalker.GetHeapData((UInt64)pid);
				if (newData != null)
				{
					IEnumerable<AllocationData> newAllocations = newData.Except(oldData, new AllocationDataEqualityComparer());
					IEnumerable<AllocationData> newFrees = oldData.Except(newData, new AllocationDataEqualityComparer());
					oldData = newData;

					if (newAllocations != null && newAllocations.Count() > 0)
					{
						History.SuspendRebuilding = true;

						List<Allocation> allocations = new List<Allocation>();
						foreach (AllocationData allocation in newAllocations)
						{
							Allocation allocationPacket = new Allocation();
							allocationPacket.Address = allocation.Address;
							allocationPacket.Size = allocation.Size;
							allocationPacket.HeapId = (ushort)allocation.HeapId;

							allocations.Add(allocationPacket);
						}

						TargetSystemInfo targetSystemInfo = new TargetSystemInfo();
						targetSystemInfo.Architecture = Common.Architecture._64Bit;
						Dispatcher.Invoke(new Action(() => ProcessPacket(PacketBundle.Instance.Serialize(allocations, targetSystemInfo))));

						History.SuspendRebuilding = false;
						Dispatcher.Invoke(new Action(() => History.ForceRebuild()));
					}

					if (newFrees != null && newFrees.Count() > 0)
					{
						History.SuspendRebuilding = true;

						List<Free> frees = new List<Free>();
						foreach (AllocationData free in newFrees)
						{
							Free freePacket = new Free();
							freePacket.Address = free.Address;
							freePacket.HeapId = (ushort)free.HeapId;

							frees.Add(freePacket);
						}

						TargetSystemInfo targetSystemInfo = new TargetSystemInfo();
						targetSystemInfo.Architecture = Common.Architecture._64Bit;
						Dispatcher.Invoke(new Action(() => ProcessPacket(PacketBundle.Instance.Serialize(frees, targetSystemInfo))));

						History.SuspendRebuilding = false;
						Dispatcher.Invoke(new Action(() => History.ForceRebuild()));
					}
				}

				Thread.Sleep(1000);
			}
		}
	}
}

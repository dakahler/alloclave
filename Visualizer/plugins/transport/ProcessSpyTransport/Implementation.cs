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
				IEnumerable<AllocationData> newAllocations = newData.Except(oldData, new AllocationDataEqualityComparer());
				IEnumerable<AllocationData> newFrees = oldData.Except(newData, new AllocationDataEqualityComparer());
				oldData = newData;

				// TODO: Hacky
				// Need better location for building up a packet from C#
				if (newAllocations != null && newAllocations.Count() > 0)
				{
					History.SuspendRebuilding = true;

					MemoryStream memoryStream = new MemoryStream();
					BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

					binaryWriter.Write(PacketBundle.Version); // version
					binaryWriter.Write((UInt16)newAllocations.Count());
					foreach (AllocationData allocation in newAllocations)
					{
						byte packetType = (byte)PacketTypeRegistrar.PacketTypes.Allocation;
						binaryWriter.Write(packetType);

						UInt64 timeStamp = (UInt64)DateTime.UtcNow.Ticks;
						binaryWriter.Write(timeStamp);

						binaryWriter.Write(allocation.Address);
						binaryWriter.Write(allocation.Size);
						binaryWriter.Write((UInt64)4); // alignment
						binaryWriter.Write((byte)Allocation.AllocationType.Allocation);
						binaryWriter.Write((UInt16)0); // heap id
					}

					Dispatcher.Invoke(new Action(() => ProcessPacket(memoryStream.GetBuffer())));

					History.SuspendRebuilding = false;
					Dispatcher.Invoke(new Action(() => History.ForceRebuild()));
				}

				if (newFrees != null && newFrees.Count() > 0)
				{
					History.SuspendRebuilding = true;

					MemoryStream memoryStream = new MemoryStream();
					BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

					binaryWriter.Write(PacketBundle.Version); // version
					binaryWriter.Write((UInt16)newFrees.Count());
					foreach (AllocationData free in newFrees)
					{
						byte packetType = (byte)PacketTypeRegistrar.PacketTypes.Free;
						binaryWriter.Write(packetType);

						UInt64 timeStamp = (UInt64)DateTime.UtcNow.Ticks;
						binaryWriter.Write(timeStamp);

						binaryWriter.Write(free.Address);
						//binaryWriter.Write(allocation.Size);
						//binaryWriter.Write((UInt64)4); // alignment
						//binaryWriter.Write((byte)Allocation.AllocationType.Allocation);
						binaryWriter.Write((UInt16)0); // heap id
					}

					Dispatcher.Invoke(new Action(() => ProcessPacket(memoryStream.GetBuffer())));

					History.SuspendRebuilding = false;
					Dispatcher.Invoke(new Action(() => History.ForceRebuild()));
				}

				Thread.Sleep(1000);
			}
		}
	}
}

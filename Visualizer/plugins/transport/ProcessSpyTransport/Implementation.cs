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

namespace Alloclave_Plugin
{
	[Export(typeof(Alloclave.Transport))]
	[ExportMetadata("Name", "Process Spy")]
	public class ProcessSpyTransport : Alloclave.Transport
	{
		static Alloclave.TargetSystemInfo temp = new Alloclave.TargetSystemInfo(
			"Process Spy", "", 0, Common.Architecture._64Bit, Common.Endianness.LittleEndian);
		Process TargetProcess;

		System.Windows.Threading.Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;

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

		public override void SpawnCustomUI(IWin32Window owner)
		{
			ProcessForm processForm = new ProcessForm();
			if (processForm.ShowDialog(owner) == DialogResult.OK)
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

				// TODO: Should probably be in Connect?
				var task3 = new Task(() => MonitorHeap(ref TargetProcess), TaskCreationOptions.LongRunning);
				task3.Start();
			}
		}

		private void MonitorHeap(ref Process process)
		{
			int pid = process.Id;
			HeapWalker heapWalker = new HeapWalker();
			History.SuspendRebuilding = true;
			while (true)
			{
				List<AllocationData> allocationData = heapWalker.GetHeapData((UInt64)pid);

				// TODO: Hacky
				// Need better location for building up a packet from C#
				if (allocationData != null)
				{
					MemoryStream memoryStream = new MemoryStream();
					BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

					binaryWriter.Write(PacketBundle.Version); // version
					binaryWriter.Write((UInt16)allocationData.Count);
					foreach (AllocationData allocation in allocationData)
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
				}

				break;
				//Thread.Sleep(500);
			}

			History.SuspendRebuilding = false;
			Dispatcher.Invoke(new Action(() => History.ForceRebuild()));
		}
	}
}

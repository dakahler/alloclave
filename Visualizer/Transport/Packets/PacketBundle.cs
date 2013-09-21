using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Alloclave
{
	public class PacketReceivedEventArgs : EventArgs
	{
		public IPacket Packet;
		public UInt64 TimeStamp;
	}

	public delegate void PacketReceivedEventHandler(object sender, PacketReceivedEventArgs e);
	public sealed class PacketBundle : ICustomSerializable
	{
		public static readonly UInt16 Version = 0;

		public event PacketReceivedEventHandler PacketReceived;

		// Enforces in-order bundle processing
		BlockingCollection<Tuple<BinaryReader, TargetSystemInfo>> BundleQueue =
			new BlockingCollection<Tuple<BinaryReader, TargetSystemInfo>>();

		public static PacketBundle Instance
		{
			get
			{
				return _Instance;
			}
		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public byte[] Serialize<T>(List<T> packets, TargetSystemInfo targetSystemInfo) where T : IPacket
		{
			Debug.Assert(packets != null);
			Debug.Assert(targetSystemInfo != null);

			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			binaryWriter.Write(PacketBundle.Version); // version
			binaryWriter.Write((UInt32)packets.Count());
			foreach (IPacket packet in packets)
			{
				byte packetType = (byte)PacketTypeRegistrar.GetType(packet.GetType());
				binaryWriter.Write(packetType);

				UInt64 timeStamp = (UInt64)Stopwatch.GetTimestamp();
				binaryWriter.Write(timeStamp);

				binaryWriter.Write(packet.Serialize(targetSystemInfo));
			}

			return memoryStream.ToArray();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			Debug.Assert(binaryReader != null);
			Debug.Assert(targetSystemInfo != null);

			UInt16 incomingVersion = binaryReader.ReadUInt16();
			if (incomingVersion != Version)
			{
				MessagesForm.Add(MessagesForm.MessageType.Error, null, "Version error! Update your collector source.");
				return;
			}

			BundleQueue.Add(new Tuple<BinaryReader, TargetSystemInfo>(binaryReader, targetSystemInfo));
		}

		private static readonly PacketBundle _Instance = new PacketBundle();

		private PacketBundle()
		{
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerAsync();
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				Tuple<BinaryReader, TargetSystemInfo> pair = BundleQueue.Take();
				BinaryReader binaryReader = pair.Item1;
				TargetSystemInfo targetSystemInfo = pair.Item2;

				UInt32 numPackets = binaryReader.ReadUInt32();
				for (UInt32 i = 0; i < numPackets; i++)
				{
					if (!Enum.IsDefined(typeof(PacketTypeRegistrar.PacketTypes), binaryReader.PeekChar()))
					{
						int invalidType = binaryReader.PeekChar();
						MessagesForm.Add(MessagesForm.MessageType.Error, null,
							"Serialization error! Please email support@circularshift.com.");
					}

					PacketTypeRegistrar.PacketTypes packetType = (PacketTypeRegistrar.PacketTypes)binaryReader.ReadByte();
					UInt64 timeStamp = binaryReader.ReadUInt64();

					IPacket specificPacket = PacketTypeRegistrar.Generate(packetType);

					// Deserialize everything else
					specificPacket.Deserialize(binaryReader, targetSystemInfo);

					PacketReceivedEventArgs eventArgs = new PacketReceivedEventArgs();
					eventArgs.Packet = specificPacket;
					eventArgs.TimeStamp = timeStamp;

					History.SuspendRebuilding = true;
					PacketReceived.Invoke(this, eventArgs);
					History.SuspendRebuilding = false;
				}

				Dispatcher.CurrentDispatcher.Invoke(new Action(() => History.ForceRebuild()));
			}
		}
	}
}

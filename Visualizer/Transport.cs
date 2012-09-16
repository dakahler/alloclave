using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Meminator
{
	public class PacketReceivedEventArgs : EventArgs
	{
		public IPacket Packet;
	}

	public delegate void PacketReceivedEventHandler(object sender, PacketReceivedEventArgs e);
	public abstract class Transport
	{
		public event PacketReceivedEventHandler PacketReceived;
		private TargetSystemInfo TargetSystemInfo;

		public Transport(TargetSystemInfo targetSystemInfo)
		{
			TargetSystemInfo = targetSystemInfo;
		}

		public abstract void Connect();
		public abstract void Disconnect();

		protected virtual void ProcessPacket(byte[] packet)
		{
			if (packet.Length <= 0)
			{
				return;
			}

			// ASCII encoding is specified so PeekChar only looks at the first byte
			MemoryStream memoryStream = new MemoryStream(packet);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.ASCII);

			if (!Enum.IsDefined(typeof(PacketTypeRegistrar.PacketTypes), binaryReader.PeekChar()))
			{
				throw new NotImplementedException();
			}

			PacketTypeRegistrar.PacketTypes packetType = (PacketTypeRegistrar.PacketTypes)binaryReader.ReadByte();
			IPacket specificPacket = PacketTypeRegistrar.Generate(packetType);

			// Pass in everything after the first byte, since that contained the type ID
			specificPacket.Deserialize(binaryReader, TargetSystemInfo);

			PacketReceivedEventArgs e = new PacketReceivedEventArgs();
			e.Packet = specificPacket;

			PacketReceived.Invoke(this, e);
		}
	}
}

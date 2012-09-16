using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meminator
{
	public class PacketReceivedEventArgs : EventArgs
	{
		public Packet Packet;
	}

	public delegate void PacketReceivedEventHandler(object sender, PacketReceivedEventArgs e);
	public abstract class Transport
	{
		public event PacketReceivedEventHandler PacketReceived;

		public abstract void Connect();
		public abstract void Disconnect();

		protected virtual void ProcessPacket(byte[] packet)
		{
			if (packet.Length <= 0)
			{
				return;
			}
			
			if (!Enum.IsDefined(typeof(PacketTypeRegistrar.PacketTypes), packet[0]))
			{
				throw new NotImplementedException();
			}

			PacketTypeRegistrar.PacketTypes packetType = (PacketTypeRegistrar.PacketTypes)packet[0];
			Packet specificPacket = PacketTypeRegistrar.Generate(packetType);

			// Pass in everything after the first byte, since that contained the type ID
			byte[] subArray = packet.Skip(1).ToArray();
			specificPacket.Parse(subArray);

			PacketReceivedEventArgs e = new PacketReceivedEventArgs();
			e.Packet = specificPacket;

			PacketReceived.Invoke(this, e);
		}
	}
}

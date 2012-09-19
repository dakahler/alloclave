using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public abstract class Transport
	{
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

			MemoryStream memoryStream = new MemoryStream(packet);
			Common.Endianness targetEndianness = TargetSystemInfo.Endianness;
			Common.Endianness visualizerEndianness = Common.Endianness.LittleEndian;
			if (!BitConverter.IsLittleEndian)
			{
				visualizerEndianness = Common.Endianness.BigEndian;
			}
			CustomBinaryReader binaryReader = new CustomBinaryReader(
				memoryStream, visualizerEndianness, targetEndianness);

			PacketBundle.Instance.Deserialize(binaryReader, TargetSystemInfo);
		}
	}
}

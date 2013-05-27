﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Alloclave
{
	public abstract class Transport
	{
		public TargetSystemInfo TargetSystemInfo = new TargetSystemInfo();

		public Transport()
		{
			
		}

		public abstract void Connect();
		public abstract void Disconnect();
		public virtual void SpawnCustomUI(IWin32Window owner)
		{

		}

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

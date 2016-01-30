using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	internal class SetArchitecture : IPacket
	{
		// Data passed in from target system
		// TODO: Better encapsulation
		public Common.Architecture Architecture = Common.Architecture._32Bit;
		public Common.Endianness Endianness = Common.Endianness.LittleEndian;

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			if (Environment.Is64BitProcess)
			{
				binaryWriter.Write(8);
			}
			else
			{
				binaryWriter.Write(4);
			}

			return memoryStream.ToArray();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			// Custom read so endianness can be determined
			byte[] buffer = new byte[2];
			binaryReader.Read(buffer, 0, 2);
			UInt16 pointerSize = BitConverter.ToUInt16(buffer, 0);

			if (pointerSize != 4 && pointerSize != 8)
			{
				Endianness = Common.Endianness.BigEndian;
				Common.EndianSwap(ref pointerSize);
			}

			switch (pointerSize)
			{
				case 4:
					Architecture = Common.Architecture._32Bit; break;
				case 8:
					Architecture = Common.Architecture._64Bit; break;
			}
		}
	}
}

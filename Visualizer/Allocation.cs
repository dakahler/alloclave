using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Meminator
{
	public class Allocation : IPacket
	{
		// Data passed in from target system
		UInt64 Address;
		UInt64 Size;
		UInt64 Alignment;
		CallStack Stack;
		byte[] UserData;

		// Tool-side-only data
		String Notes;

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			// Process the correct number of bytes depending on the target platform
			if (targetSystemInfo.Architecture == Common.Architecture._32Bit)
			{
				Address = binaryReader.ReadUInt32();
				Size = binaryReader.ReadUInt32();
				Alignment = binaryReader.ReadUInt32();
			}
			else if (targetSystemInfo.Architecture == Common.Architecture._64Bit)
			{
				Address = binaryReader.ReadUInt64();
				Size = binaryReader.ReadUInt64();
				Alignment = binaryReader.ReadUInt64();
			}

			Stack.Deserialize(binaryReader, targetSystemInfo);

			// TODO: User data
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public class Free : IPacket
	{
		// Data passed in from target system
		// TODO: Better encapsulation
		public UInt64 Address;
		public UInt32 HeapId;
		public CallStack Stack = new CallStack();
		public byte[] UserData;

		public Allocation AssociatedAllocation;

		// Tool-side-only data
		String Notes;

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			if (targetSystemInfo.Architecture == Common.Architecture._32Bit)
			{
				binaryWriter.Write((UInt32)Address);
			}
			else if (targetSystemInfo.Architecture == Common.Architecture._64Bit)
			{
				binaryWriter.Write(Address);
			}

			binaryWriter.Write(HeapId);

			return memoryStream.ToArray();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			// Process the correct number of bytes depending on the target platform
			if (targetSystemInfo.Architecture == Common.Architecture._32Bit)
			{
				Address = binaryReader.ReadUInt32();
			}
			else if (targetSystemInfo.Architecture == Common.Architecture._64Bit)
			{
				Address = binaryReader.ReadUInt64();
			}

			HeapId = binaryReader.ReadUInt32();

			Stack.Deserialize(binaryReader, targetSystemInfo);

			// TODO: User data
		}
	}
}

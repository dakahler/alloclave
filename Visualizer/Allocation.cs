using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public class Allocation : IPacket
	{
		public enum AllocationType : byte
		{
			Allocation = 0,
			Heap,
		}

		// Data passed in from target system
		// TODO: Better encapsulation
		public UInt64 Address;
		public UInt64 Size;
		public UInt64 Alignment;
		public AllocationType Type;
		public UInt16 HeapId;
		public CallStack Stack = new CallStack();
		public byte[] UserData;

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

			Type = (AllocationType)binaryReader.ReadByte();
			HeapId = binaryReader.ReadUInt16();

			Stack.Deserialize(binaryReader, targetSystemInfo);

			// TODO: User data
		}
	}
}

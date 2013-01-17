using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public class Allocation : IPacket, IComparable
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

		public Common.Architecture Architecture;

		// Tool-side-only data
		String Notes;

		public int CompareTo(object obj)
		{
			if (obj is Allocation)
			{
				Allocation other = obj as Allocation;
				if (this.Address <= other.Address)
					{
						return -1;
					}
					else
					{
						return 1;
					}
			}

			throw new ArgumentException("object is not a Allocation");
		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			if (targetSystemInfo.Architecture == Common.Architecture._32Bit)
			{
				binaryWriter.Write((UInt32)Address);
				binaryWriter.Write((UInt32)Size);
				binaryWriter.Write((UInt32)4); // alignment
			}
			else if (targetSystemInfo.Architecture == Common.Architecture._64Bit)
			{
				binaryWriter.Write(Address);
				binaryWriter.Write(Size);
				binaryWriter.Write((UInt64)4); // alignment
			}

			
			binaryWriter.Write((byte)AllocationType.Allocation);
			binaryWriter.Write(HeapId);

			return memoryStream.ToArray();
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

			Architecture = targetSystemInfo.Architecture;

			Type = (AllocationType)binaryReader.ReadByte();
			HeapId = binaryReader.ReadUInt16();

			Stack.Deserialize(binaryReader, targetSystemInfo);

			// TODO: User data
		}
	}
}

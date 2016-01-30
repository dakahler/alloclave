using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Alloclave
{
	[DataContract()]
	public class Free : IPacket
	{
		// Data passed in from target system
		[DataMember]
		public UInt64 Address { get; set; }

		[DataMember]
		public UInt32 HeapId { get; set; }

		[DataMember]
		internal CallStack Stack = new CallStack();

		[DataMember]
		public byte[] UserData { get; set; }

		public Allocation AssociatedAllocation { get; set; }

		public Free()
		{
			
		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			Debug.Assert(targetSystemInfo != null);

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
			Debug.Assert(binaryReader != null);
			Debug.Assert(targetSystemInfo != null);

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

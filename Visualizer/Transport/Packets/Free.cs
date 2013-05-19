﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;

namespace Alloclave
{
	public class Free : IPacket
	{
		// Data passed in from target system
		// TODO: Better encapsulation
		public UInt64 Address;
		public UInt32 HeapId;
		public CallStack Stack; // = new CallStack();
		public byte[] UserData;

		public Allocation AssociatedAllocation;

		public Free()
		{
			// TODO: Support arbitrary call stack adapters
			foreach (ExportFactory<CallStack, ICallStackParserName> callStackAdapter in Program.CallStackParserAdapters)
			{
				String transportName = callStackAdapter.Metadata.Name;
				if (transportName == "Call Stack PDB")
				{
					Stack = callStackAdapter.CreateExport().Value;
				}
			}
		}

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

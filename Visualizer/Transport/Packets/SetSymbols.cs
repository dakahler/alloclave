﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	internal class SetSymbols : IPacket
	{
		// Data passed in from target system
		public String SymbolsPath { get; set; }

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			binaryWriter.Write(SymbolsPath);

			return memoryStream.ToArray();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			UInt16 stringLength = binaryReader.ReadUInt16();
			byte[] rawBytes = binaryReader.ReadBytes(stringLength);
			SymbolsPath = Encoding.Unicode.GetString(rawBytes);
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Alloclave
{
	public class PacketReceivedEventArgs : EventArgs
	{
		public IPacket Packet;
		public UInt64 TimeStamp;
	}

	public delegate void PacketReceivedEventHandler(object sender, PacketReceivedEventArgs e);
	public sealed class PacketBundle : ICustomSerializable
	{
		public static readonly UInt16 Version = 0;

		public event PacketReceivedEventHandler PacketReceived;

		public static PacketBundle Instance
		{
			get
			{
				return _Instance;
			}
		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public byte[] Serialize<T>(List<T> packets, TargetSystemInfo targetSystemInfo) where T : IPacket
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			binaryWriter.Write(PacketBundle.Version); // version
			binaryWriter.Write((UInt32)packets.Count());
			foreach (IPacket packet in packets)
			{
				byte packetType = (byte)PacketTypeRegistrar.GetType(packet.GetType());
				binaryWriter.Write(packetType);

				UInt64 timeStamp = (UInt64)Stopwatch.GetTimestamp();
				binaryWriter.Write(timeStamp);

				binaryWriter.Write(packet.Serialize(targetSystemInfo));
			}

			return memoryStream.ToArray();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			UInt16 incomingVersion = binaryReader.ReadUInt16();
			if (incomingVersion != Version)
			{
				throw new NotSupportedException();
			}

			UInt32 numPackets = binaryReader.ReadUInt32();
			for (UInt32 i = 0; i < numPackets; i++)
			{
				if (!Enum.IsDefined(typeof(PacketTypeRegistrar.PacketTypes), binaryReader.PeekChar()))
				{
					int invalidType = binaryReader.PeekChar();
					throw new NotSupportedException();
				}

				PacketTypeRegistrar.PacketTypes packetType = (PacketTypeRegistrar.PacketTypes)binaryReader.ReadByte();
				UInt64 timeStamp = binaryReader.ReadUInt64();

				IPacket specificPacket = PacketTypeRegistrar.Generate(packetType);

				// Deserialize everything else
				specificPacket.Deserialize(binaryReader, targetSystemInfo);

				PacketReceivedEventArgs e = new PacketReceivedEventArgs();
				e.Packet = specificPacket;
				e.TimeStamp = timeStamp;

				PacketReceived.Invoke(this, e);
			}
		}

		private static readonly PacketBundle _Instance = new PacketBundle();

		private PacketBundle() { }
	}
}
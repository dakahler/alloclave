﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	internal class Profile
	{
		public History History = new History();
		Transport Transport;

		public Profile()
		{
			
		}

		public Profile(ref Transport transport)
			: this()
		{
			Transport = transport;
			Transport.PacketBundle.PacketReceived += new PacketReceivedEventHandler(HandlePacket);
			Transport.PacketBundle.History = History;
		}

		public void Start()
		{
			Transport.Connect();
		}

		public void Stop()
		{
			Transport.Disconnect();
		}

		void HandlePacket(object sender, PacketReceivedEventArgs e)
		{
			if (e.Packet is SetSymbols)
			{
				SetSymbols setSymbols = (SetSymbols)e.Packet;
				SymbolLookup.SymbolsPath = setSymbols.SymbolsPath;
			}
			else if (e.Packet is SetArchitecture)
			{
				SetArchitecture setArchitecture = (SetArchitecture)e.Packet;
				Transport.TargetSystemInfo.Architecture = setArchitecture.Architecture;
				Transport.TargetSystemInfo.Endianness = setArchitecture.Endianness;
			}
			else
			{
				History.Add(e.Packet, e.TimeStamp);
			}
		}

	}
}

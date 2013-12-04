using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;

namespace Alloclave
{
	[DataContract()]
	public class Profile
	{
		[DataMember]
		public History History = new History();
		Transport Transport;

		// This should only be used by the serializer
		[DataMember]
		SymbolLookup SymbolLookup
		{
			get
			{
				return SymbolLookup.Instance;
			}
			set
			{
				SymbolLookup.Instance = value;
			}
		}

		[DataMember]
		internal List<Diff> Diffs { get; private set; }

		public Profile()
		{
			Diffs = new List<Diff>();
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

		internal void AddDiff(Diff diff)
		{
			Diffs.Add(diff);
		}

		internal void RemoveDiff(Diff diff)
		{
			Diffs.Remove(diff);
		}

		public void ProcessDiffs()
		{
			if (Diffs != null)
			{
				Diffs.ForEach((diff) => diff.ProcessDiff(History));
			}
			else
			{
				Diffs = new List<Diff>();
			}
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

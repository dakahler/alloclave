using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	class Profile
	{
		public History History;
		FileInfo FileInfo;
		bool Dirty;
		Transport Transport;
		TargetSystemInfo TargetSystemInfo;

		public Profile()
		{
			History = History.Instance;
		}

		public Profile(ref Transport transport)
			: this()
		{
			Transport = transport;
			PacketBundle.Instance.PacketReceived += new PacketReceivedEventHandler(HandlePacket);
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
			History.Add(e.Packet, e.TimeStamp);
		}

	}
}

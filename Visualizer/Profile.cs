using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Meminator
{
	class Profile
	{
		public History History;
		FileInfo FileInfo;
		bool Dirty;
		Transport Transport;

		public Profile()
		{
			History = new History();
		}

		public Profile(Transport transport)
			: this()
		{
			Transport = transport;
			Transport.PacketReceived += new PacketReceivedEventHandler(HandlePacket);
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
			History.Add(e.Packet);
		}

	}
}

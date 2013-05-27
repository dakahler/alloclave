using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace Alloclave_Plugin
{
	[Export(typeof(Alloclave.Transport))]
	[ExportMetadata("Name", "Win32 Messaging")]
	public class Win32Transport : Alloclave.Transport
	{
		MessageWindow MessageWindow = new MessageWindow();

		public Win32Transport()
		{
			MessageWindow.MessageReceived += MessageWindow_MessageReceived;
		}

		void MessageWindow_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			ProcessPacket(e.Bytes);
		}

		public override void Connect()
		{

		}

		public override void Disconnect()
		{

		}
	}
}

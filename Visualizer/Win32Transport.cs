using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public class Win32Transport : Transport
	{
        MessageWindow MessageWindow = new MessageWindow();

		public Win32Transport(TargetSystemInfo targetSystemInfo)
			: base(targetSystemInfo)
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

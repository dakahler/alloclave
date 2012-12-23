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
		static Alloclave.TargetSystemInfo temp = new Alloclave.TargetSystemInfo();
		MessageWindow MessageWindow = new MessageWindow();

		public Win32Transport()
			: this(temp)
		{
			
		}

		public Win32Transport(Alloclave.TargetSystemInfo targetSystemInfo)
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave_Plugin
{
	public class MessageReceivedEventArgs : EventArgs
	{
		public byte[] Bytes;
	}

	public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

	[System.ComponentModel.DesignerCategory("")]
	class MessageWindow : Form
	{
		public event MessageReceivedEventHandler MessageReceived;

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public unsafe struct CopyDataStruct
		{
			public IntPtr dwData;
			public int cbData;
			public IntPtr lpData;
		}

		const int WM_COPYDATA = 0x4A;

		[DllImport("user32.dll")]
		static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		public MessageWindow()
		{
			var accessHandle = this.Handle;
			this.Text = "857E3F44-91FB-456B-9D53-03B75C751B28";
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			ChangeToMessageOnlyWindow();
		}

		private void ChangeToMessageOnlyWindow()
		{
			IntPtr HWND_MESSAGE = new IntPtr(-3);
			SetParent(this.Handle, HWND_MESSAGE);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_COPYDATA)
			{
				CopyDataStruct cps = (CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(CopyDataStruct));

				MessageReceivedEventArgs e = new MessageReceivedEventArgs();
				e.Bytes = new byte[cps.cbData];
				Marshal.Copy(cps.lpData, e.Bytes, 0, cps.cbData);
				MessageReceived.Invoke(this, e);
			}

			base.WndProc(ref m);
		}
	}
}

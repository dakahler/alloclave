using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class AboutForm : Form
	{
		[DllImport("user32.dll")]
		static extern bool HideCaret(IntPtr hWnd);

		public AboutForm()
		{
			InitializeComponent();

			dataGrid.Rows.Clear();
			dataGrid.Rows.Add("Alloclave");
			dataGrid.Rows.Add("Version 0.1");
			dataGrid.Rows.Add("© Copyright 2013");
			dataGrid.Rows.Add("Circular Shift, LLC");
			dataGrid.Rows.Add("www.circularshift.com");
			dataGrid.Rows.Add("");
			dataGrid.Rows.Add("Licensed To:");
			dataGrid.Rows.Add("Placeholder Name");
			dataGrid.Rows.Add("placeholder@example.com");
			dataGrid.Rows.Add("Support Ends 1/1/2010");

			ToolTip tt = new ToolTip();
			tt.SetToolTip(logoPictureBox, "Open http://www.alloclave.com/");
		}

		private void purchaseButton_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.alloclave.com/purchase");
		}

		private void checkForUpdatesButton_Click(object sender, EventArgs e)
		{
			// TODO: There should be a more intelligent way of checking for updates
			// than just directing the user to a webpage
			System.Diagnostics.Process.Start("http://www.alloclave.com/update");
		}

		private void logoPictureBox_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.alloclave.com/");
		}
	}
}

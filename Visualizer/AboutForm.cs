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
		}

		private void purchaseButton_Click(object sender, EventArgs e)
		{

		}

		private void checkForUpdatesButton_Click(object sender, EventArgs e)
		{

		}
	}
}

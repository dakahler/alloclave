using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class StartScreen : Form
	{
		public StartScreen()
		{
			InitializeComponent();
		}

		private void linkLabel1_Click(object sender, EventArgs e)
		{
			Main.Instance.StartNewSession();
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Common.ProductWebsiteUrl + "quickstart");
		}
	}
}

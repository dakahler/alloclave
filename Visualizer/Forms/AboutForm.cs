using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;
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
using System.Windows.Threading;

namespace Alloclave
{
	public partial class AboutForm : Form
	{
		[DllImport("user32.dll")]
		static extern bool HideCaret(IntPtr hWnd);

		const String WebsiteUrl = "http://www.alloclave.com/";

		public AboutForm()
		{
			InitializeComponent();

			if (Licensing.IsLicensed)
			{
				purchaseButton.Hide();
			}

			dataGrid.Rows.Clear();
			dataGrid.Rows.Add("Alloclave");
			dataGrid.Rows.Add("Version " + Common.Version);
			dataGrid.Rows.Add("© Copyright 2013");
			dataGrid.Rows.Add("Circular Shift, LLC");
			dataGrid.Rows.Add("www.circularshift.com");
			dataGrid.Rows.Add("");
			dataGrid.Rows.Add("Licensed To:");
			dataGrid.Rows.Add("Placeholder Name");
			dataGrid.Rows.Add("placeholder@example.com");
			dataGrid.Rows.Add("Support Ends 1/1/2010");

			ToolTip tt = new ToolTip();
			tt.SetToolTip(logoPictureBox, "Open " + WebsiteUrl);
		}

		private void purchaseButton_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(WebsiteUrl + "purchase");
		}

		private void checkForUpdatesButton_Click(object sender, EventArgs e)
		{
			UpdateManager.Instance.CleanUp();
			UpdateManager.Instance.BeginCheckForUpdates(asyncResult =>
			{
				Action showUpdateAction = ShowUpdateWindow;

				if (asyncResult.IsCompleted)
				{
					// still need to check for caught exceptions if any and rethrow
					((UpdateProcessAsyncResult)asyncResult).EndInvoke();

					// No updates were found, or an error has occured. We might want to check that...
					if (UpdateManager.Instance.UpdatesAvailable == 0)
					{
						MessageBox.Show("There are no updates available.");
						return;
					}
				}

				if (Dispatcher.CurrentDispatcher.CheckAccess())
				{
					showUpdateAction();
				}
				else
				{
					Dispatcher.CurrentDispatcher.Invoke(showUpdateAction);
				}
			}, null);
		}

		private void ShowUpdateWindow()
		{
			this.BeginInvoke((Action)(() =>
			{
				UpdateForm updateForm = new UpdateForm();
				updateForm.ShowDialog(this);
			}));
		}

		private void logoPictureBox_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(WebsiteUrl);
		}
	}
}

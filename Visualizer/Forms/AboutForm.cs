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
			dataGrid.Rows.Add("Circular Shift");
			dataGrid.Rows.Add("www.circularshift.com");
			dataGrid.Rows.Add("For support, email support@circularshift.com");
			dataGrid.Rows.Add("");
			dataGrid.Rows.Add(Licensing.LicenseName);

			if (Licensing.IsLicensed)
			{
				dataGrid.Rows.Add(Licensing.LicenseEmail);
				dataGrid.Rows.Add("Support Ends " + Licensing.LicenseDate.ToShortDateString());
			}
			else
			{
				dataGrid.Rows.Add("");
				dataGrid.Rows.Add("");
			}

			ToolTip tt1 = new ToolTip();
			tt1.SetToolTip(companyLogoPictureBox, "Open " + Common.CompanyWebsiteUrl);

			ToolTip tt2 = new ToolTip();
			tt2.SetToolTip(logoPictureBox, "Open " + Common.ProductWebsiteUrl);
		}

		private void purchaseButton_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Common.ProductWebsiteUrl + "purchase");
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
			System.Diagnostics.Process.Start(Common.ProductWebsiteUrl);
		}

		private void companyLogoPictureBox_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Common.CompanyWebsiteUrl);
		}
	}
}

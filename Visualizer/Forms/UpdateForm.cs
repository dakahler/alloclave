using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class UpdateForm : Form
	{
		public UpdateForm()
		{
			InitializeComponent();

			if (UpdateManager.Instance.Tasks.Count() == 0)
			{
				// Shouldn't ever happen
				Debug.Assert(false);
				Close();
			}

			CurrentVersionLabel.Text = Application.ProductVersion;
			NewVersionLabel.Text = UpdateManager.Instance.Tasks.First().UpdateConditions.Attributes["version"];

			foreach (var task in UpdateManager.Instance.Tasks)
			{
				
				String version = task.UpdateConditions.Attributes["version"];
				if (version != null)
				{
					ChangesTextBox.Text += "Version " + version + ":" + Environment.NewLine;
				}
				else
				{
					// TODO: What should be done here?
				}

				ChangesTextBox.Text += task.Description + Environment.NewLine + Environment.NewLine;
			}

			ChangesTextBox.Select(0, 0);
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			UpdateManager.Instance.BeginPrepareUpdates(asyncResult =>
			{
				if (asyncResult.IsCompleted)
				{
					// still need to check for caught exceptions if any and rethrow
					((UpdateProcessAsyncResult)asyncResult).EndInvoke();

					UpdateManager.Instance.ApplyUpdates();
				}
			}, null);
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

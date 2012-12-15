using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommandLine;
using System.Runtime.InteropServices;

namespace Alloclave
{
	public partial class Main : Form, IMessageFilter
	{
		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		const uint WM_MOUSEWHEEL = 0x20a;

		CommandLineOptions options = new CommandLineOptions();

		public Main()
		{
			InitializeComponent();

			Application.AddMessageFilter(this);

			ICommandLineParser parser = new CommandLineParser();
			String[] args = Environment.GetCommandLineArgs();
			if (parser.ParseArguments(args, options))
			{
				if (options.TransportType != null)
				{
					foreach (ExportFactory<Transport, ITransportName> transportAdapter in Program.TransportAdapters)
					{
						String transportName = transportAdapter.Metadata.Name;
						if (transportName == options.TransportType)
						{
							SpawnTransport(transportAdapter);
						}
					}
				}
			}

			licenseToolStripMenuItem.Text = Licensing.LicenseName;
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_MOUSEWHEEL)
			{
				// WM_MOUSEWHEEL, find the control at screen position m.LParam
				Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
				IntPtr hWnd = WindowFromPoint(pos);
				if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
				{
					SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
					return true;
				}
			}
			return false;
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewForm newForm = new NewForm();

			// Populate combo box with transport adapter plugins
			foreach (ExportFactory<Transport, ITransportName> transportAdapter in Program.TransportAdapters)
			{
				String transportName = transportAdapter.Metadata.Name;
				newForm.TransportComboBox.Items.Add(transportName);
			}

			if (newForm.TransportComboBox.Items.Count > 0)
			{
				newForm.TransportComboBox.SelectedIndex = 0;
			}

			if (newForm.ShowDialog() == DialogResult.OK)
			{
				foreach (ExportFactory<Transport, ITransportName> transportAdapter in Program.TransportAdapters)
				{
					String transportName = transportAdapter.Metadata.Name;
					if (transportName == newForm.TransportComboBox.SelectedItem.ToString())
					{
						SpawnTransport(transportAdapter);
						break;
					}
				}
			}
		}

		private void SpawnTransport(ExportFactory<Transport, ITransportName> transportAdapter)
		{
			Transport transport = transportAdapter.CreateExport().Value;

			TransportForm transportForm = new TransportForm(ref transport);
			transportForm.Text = transportAdapter.Metadata.Name;

			transport.SpawnCustomUI(this);
			transportForm.Show(DockPanel);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog();
		}
	}
}

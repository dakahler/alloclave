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

namespace Alloclave
{
	public partial class Main : Form
	{
		CommandLineOptions options = new CommandLineOptions();

		public Main()
		{
			InitializeComponent();

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

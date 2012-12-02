using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
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
						Transport transport = transportAdapter.CreateExport().Value;
						transport.SpawnCustomUI();

						TransportForm transportForm = new TransportForm(ref transport);
						transportForm.Text = transportName;
						transportForm.Show(DockPanel);
						break;
					}
				}
			}

			
		}
	}
}

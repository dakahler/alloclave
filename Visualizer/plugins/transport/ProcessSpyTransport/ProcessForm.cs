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

namespace Alloclave_Plugin
{
	public partial class ProcessForm : Form
	{
		public ProcessForm()
		{
			InitializeComponent();

			Process[] processes = Process.GetProcesses();
			List<String> processNames = new List<String>();
			foreach (Process process in processes)
			{
				processNames.Add(process.ProcessName);
			}
			processNames.Sort();

			ProcessComboBox.Items.AddRange(processNames.ToArray());

			ProcessComboBox.SelectedIndex = 0;
		}
	}
}

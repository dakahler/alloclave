﻿using System;
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
		public static Main Instance;

		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		private const uint WM_MOUSEWHEEL = 0x20a;

		CommandLineOptions options = new CommandLineOptions();

		public Main()
		{
			Instance = this;

			InitializeComponent();

			Application.AddMessageFilter(this);

			var settings = new CommandLineParserSettings();
			settings.CaseSensitive = false;
			settings.HelpWriter = Console.Error;

			ICommandLineParser parser = new CommandLineParser(settings);
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
			else
			{
				Close();
				return;
			}

			//licenseToolStripMenuItem.Text = Licensing.LicenseName;

			FormClosing += Main_FormClosing;

			ReturnToStartScreen();
		}

		void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			RenderManager_OGL.Instance.Dispose();
			Properties.Settings.Default.Save();
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

		void ClearDockControls()
		{
			_DockPanel.Controls.Clear();

			//foreach (Control control in _DockPanel.Controls)
			//{
			//	control.Dispose();
			//}
		}

		public void StartNewSession(String autoTransportSelect = null)
		{
			NewForm newForm = new NewForm();

			// Populate combo box with transport adapter plugins
			foreach (ExportFactory<Transport, ITransportName> transportAdapter in Program.TransportAdapters)
			{
				String transportName = transportAdapter.Metadata.Name;
				newForm.TransportComboBox.Items.Add(transportName);

				// If a string was passed in for transport, force it here
				if (transportName == autoTransportSelect)
				{
					newForm.TransportComboBox.Items.Clear();
					newForm.TransportComboBox.Items.Add(transportName);
				}
			}

			if (newForm.TransportComboBox.Items.Count > 0)
			{
				newForm.TransportComboBox.SelectedIndex = 0;
			}

            if (newForm.TransportComboBox.Items.Count == 1 || newForm.ShowDialog() == DialogResult.OK)
			{
				ClearDockControls();

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

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StartNewSession();
		}

		private void SpawnTransport(ExportFactory<Transport, ITransportName> transportAdapter)
		{
			Transport transport = transportAdapter.CreateExport().Value;

			TransportForm transportForm = new TransportForm(ref transport);
			transportForm.Text = transportAdapter.Metadata.Name;

			transport.SpawnCustomUI(this);

			transportForm.TopLevel = false;
			transportForm.FormBorderStyle = FormBorderStyle.None;
			transportForm.Dock = DockStyle.Fill;
			transportForm.Visible = true;
			_DockPanel.Controls.Add(transportForm);
		}

		public void ReturnToStartScreen()
		{
			// Too many issues with disposing controls

			ClearDockControls();
			//History.Instance.Reset();
			//MemoryBlockManager.Instance.Reset();
			//RenderManager_OGL.Instance.Rebuild();
			//Scrubber.Position = 1.0f;

			// Show the start screen
			StartScreen startScreen = new StartScreen();

			startScreen.TopLevel = false;
			startScreen.FormBorderStyle = FormBorderStyle.None;
			startScreen.Dock = DockStyle.Fill;
			startScreen.Visible = true;
			_DockPanel.Controls.Add(startScreen);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog();
		}

		private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Alloclave.ColorPickerDialog dialog = new Alloclave.ColorPickerDialog();
			dialog.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}

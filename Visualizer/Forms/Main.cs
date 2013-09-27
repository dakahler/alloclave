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
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace Alloclave
{
	internal partial class Main : Form, IMessageFilter
	{
		public static Main Instance;

		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		private const uint WM_MOUSEWHEEL = 0x20a;

		CommandLineOptions options = new CommandLineOptions();

		TransportForm TransportForm;
		StartScreen StartScreen;

		public Main()
		{
			Instance = this;

			InitializeComponent();

			menuStrip1.Hide();

			menuStrip1.Renderer = new MyRenderer();

			ToolStripSystemTextBox licenseItem = new ToolStripSystemTextBox();
			licenseItem.TextBox.BackColor = Color.FromArgb(64, 64, 64);
			licenseItem.TextBox.ForeColor = Color.FromArgb(200, 200, 200);
			licenseItem.TextBox.Text = Licensing.LicenseName + "  ";
			licenseItem.TextBox.Font = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
			licenseItem.TextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			licenseItem.TextBox.BorderStyle = BorderStyle.None;
			licenseItem.Width = 300;
			licenseItem.TextBox.ReadOnly = true;
			licenseItem.TextBox.TabStop = false;
			licenseItem.Alignment = ToolStripItemAlignment.Right;
			this.menuStrip1.Items.Add(licenseItem);

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
				//ClearDockControls();
				Controls.Remove(StartScreen);

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

			TransportForm = new TransportForm(ref transport);
			TransportForm.Text = transportAdapter.Metadata.Name;

			transport.SpawnCustomUI(this);

			TransportForm.TopLevel = false;
			TransportForm.FormBorderStyle = FormBorderStyle.None;
			TransportForm.Dock = DockStyle.Fill;
			TransportForm.Visible = true;
			menuStrip1.Show();
			Controls.Add(TransportForm);
		}

		public void ReturnToStartScreen()
		{
			// Too many issues with disposing controls

			//ClearDockControls();
			//History.Instance.Reset();
			//MemoryBlockManager.Instance.Reset();
			//RenderManager_OGL.Instance.Rebuild();
			//Scrubber.Position = 1.0f;

			// Show the start screen
			StartScreen = new StartScreen();

			StartScreen.TopLevel = false;
			StartScreen.FormBorderStyle = FormBorderStyle.None;
			StartScreen.Dock = DockStyle.Fill;
			StartScreen.Visible = true;
			Controls.Add(StartScreen);
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

		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			fileToolStripMenuItem.ForeColor = Color.Black;
		}

		private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
		{
			fileToolStripMenuItem.ForeColor = Color.White;
		}

		private void helpToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			helpToolStripMenuItem.ForeColor = Color.Black;
		}

		private void helpToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
		{
			helpToolStripMenuItem.ForeColor = Color.White;
		}

		private void NewMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void OpenMenuItem_Click(object sender, EventArgs e)
		{
			RenderManager_OGL.Instance.Suspend = true;

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "XML|*.xml";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Load(openFileDialog.FileName);
			}

			RenderManager_OGL.Instance.Suspend = false;
		}

		private void SaveMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "XML|*.xml";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				Save(saveFileDialog.FileName);
			}
		}

		public void Save(String path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			DataContractSerializer serializer = new DataContractSerializer(typeof(Profile));
			var settings = new XmlWriterSettings { Indent = true };
			using (var w = XmlWriter.Create(File.Create(path), settings))
			{
				serializer.WriteObject(w, TransportForm.Profile);
			}
		}

		public void Load(String path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			DataContractSerializer serializer = new DataContractSerializer(typeof(Profile));
			FileStream fileStream = new FileStream(path, FileMode.Open);
			Profile profile = (Profile)serializer.ReadObject(fileStream);

			// TODO: Whole transport form needs to be reinitialized on load
			//TransportForm.Close();
			TransportForm.Dispose();
			TransportForm = new TransportForm(profile);

			TransportForm.TopLevel = false;
			TransportForm.FormBorderStyle = FormBorderStyle.None;
			TransportForm.Dock = DockStyle.Fill;
			TransportForm.Visible = true;
			Controls.Add(TransportForm);

			TransportForm.Profile.History.LastTimestamp = new TimeStamp();
			TransportForm.Profile.History.UpdateRollingSnapshotAsync(true);
		}
	}

	public class ToolStripSystemTextBox : System.Windows.Forms.ToolStripControlHost
	{
		public ToolStripSystemTextBox() : base(new TextBox()) { }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextBox TextBox { get { return Control as TextBox; } }
	}

	public class MyRenderer : ToolStripProfessionalRenderer
	{
		public MyRenderer() : base(new MyColors()) { }
	}

	public class MyColors : ProfessionalColorTable
	{
		public override Color MenuItemSelected
		{
			get { return Color.FromArgb(128, 128, 128); }
		}
		public override Color MenuItemSelectedGradientBegin
		{
			get { return Color.FromArgb(100, 100, 100); }
		}
		public override Color MenuItemSelectedGradientEnd
		{
			get { return Color.FromArgb(100, 100, 100); }
		}
		public override Color MenuItemBorder
		{
			get { return Color.FromArgb(100, 100, 100); }
		}
	}
}

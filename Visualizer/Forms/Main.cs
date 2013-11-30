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
		static extern IntPtr WindowFromPoint(Point pt);
		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		const uint WM_MOUSEWHEEL = 0x20a;

		CommandLineOptions options = new CommandLineOptions();

		TransportForm TransportForm;
		StartScreen StartScreen;

		Diff CurrentDiff;

		static bool SentDiffNag;

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
			RenderManager_OGL.Suspend = true;
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
				panel1.Controls.Remove(StartScreen);

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

		void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StartNewSession();
		}

		void SpawnTransport(ExportFactory<Transport, ITransportName> transportAdapter)
		{
			Transport transport = transportAdapter.CreateExport().Value;

			TransportForm = new TransportForm(ref transport);
			TransportForm.Text = transportAdapter.Metadata.Name;

			transport.SpawnCustomUI(this);

			menuStrip1.Show();

			TransportForm.TopLevel = false;
			TransportForm.FormBorderStyle = FormBorderStyle.None;
			TransportForm.Dock = DockStyle.Fill;
			TransportForm.Visible = true;
			TransportForm.AllocationForm.MainScrubber.DoubleClick += MainScrubber_DoubleClick;
			panel1.Controls.Add(TransportForm);
		}

		void MainScrubber_DoubleClick(object sender, EventArgs e)
		{
			// React to double click based on current diff state
			if (CurrentDiff == null || CurrentDiff.Difference != null)
			{
				// Start a new diff
				CurrentDiff = new Diff();
				CurrentDiff.SetLeft(TransportForm.Profile.History.Snapshot);
				TransportForm.AllocationForm.DiffMarkers.SetDiff1((float)TransportForm.AllocationForm.MainScrubber._Position);
			}
			else if (CurrentDiff.Difference == null)
			{
				// Complete the diff
				CurrentDiff.SetRight(TransportForm.Profile.History.Snapshot);
				TransportForm.AllocationForm.DiffMarkers.SetDiff2((float)TransportForm.AllocationForm.MainScrubber._Position);

				AllocationForm allocationForm = new AllocationForm(CurrentDiff, TransportForm.AllocationForm.AddressSpaceControl.Width);
				allocationForm.Text = "Diff";
				allocationForm.ControllerContainer.Controls.Remove(allocationForm.MainScrubber);
				DiffButtons diffButtons = new DiffButtons();
				diffButtons.StartLabel.Click += (_, __) => allocationForm.SetDiffMode(AllocationForm.DiffMode.Left);
				diffButtons.DifferenceLabel.Click += (_, __) => allocationForm.SetDiffMode(AllocationForm.DiffMode.Middle);
				diffButtons.EndLabel.Click += (_, __) => allocationForm.SetDiffMode(AllocationForm.DiffMode.Right);
				allocationForm.ControllerContainer.Controls.Add(diffButtons);
				allocationForm.AddressSpaceControl.SnapshotOverride = CurrentDiff.Difference;
				TransportForm.AddTab(allocationForm);

				if (Licensing.IsTrial && !SentDiffNag)
				{
					MessagesForm.Add(MessagesForm.MessageType.Info, null,
									"Find your leak? Support Alloclave and purchase a license!");

					SentDiffNag = true;
				}
			}
		}

		public void ReturnToStartScreen()
		{
			// Show the start screen
			StartScreen = new StartScreen();

			StartScreen.TopLevel = false;
			StartScreen.FormBorderStyle = FormBorderStyle.None;
			StartScreen.Dock = DockStyle.Fill;
			StartScreen.Visible = true;
			panel1.Controls.Add(StartScreen);
		}

		void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog();
		}

		void customizeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Alloclave.ColorPickerDialog dialog = new Alloclave.ColorPickerDialog();
			dialog.ShowDialog();
		}

		void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			fileToolStripMenuItem.ForeColor = Color.Black;
		}

		void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
		{
			fileToolStripMenuItem.ForeColor = Color.White;
		}

		void helpToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			helpToolStripMenuItem.ForeColor = Color.Black;
		}

		void helpToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
		{
			helpToolStripMenuItem.ForeColor = Color.White;
		}

		void NewMenuItem_Click(object sender, EventArgs e)
		{

		}

		public void OpenMenuItem_Click(object sender, EventArgs e)
		{
			RenderManager_OGL.Suspend = true;

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "XML|*.xml";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Load(openFileDialog.FileName);
			}

			RenderManager_OGL.Suspend = false;
		}

		void SaveMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "XML|*.xml";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				Save(saveFileDialog.FileName);
			}
		}

		void Save(String path)
		{
			List<Type> knownTypes = new List<Type>();
			if (SymbolLookup.Instance != null)
			{
				knownTypes.Add(SymbolLookup.Instance.GetType());
			}

			DataContractSerializer serializer = new DataContractSerializer(typeof(Profile), knownTypes);
			var settings = new XmlWriterSettings { Indent = true };
			using (var w = XmlWriter.Create(File.Create(path), settings))
			{
				// Force all symbols into the name cache
				TransportForm.Profile.History.ForceFullSymbolLookup();

				serializer.WriteObject(w, TransportForm.Profile);
				w.Close();
			}
		}

		new void Load(String path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			List<Type> knownTypes = new List<Type>();
			foreach (ExportFactory<SymbolLookup, ISymbolLookupExtension> symbolLookupAdapter in Program.SymbolLookupAdapters)
			{
				Type type = symbolLookupAdapter.CreateExport().Value.GetType();
				knownTypes.Add(type);
			}

			DataContractSerializer serializer = new DataContractSerializer(typeof(Profile), knownTypes);
			FileStream fileStream = new FileStream(path, FileMode.Open);
			Profile profile = (Profile)serializer.ReadObject(fileStream);
			fileStream.Close();

			if (TransportForm != null)
			{
				TransportForm.Dispose();
			}

			panel1.Controls.Remove(StartScreen);
			menuStrip1.Show();

			TransportForm = new TransportForm(profile);

			TransportForm.TopLevel = false;
			TransportForm.FormBorderStyle = FormBorderStyle.None;
			TransportForm.Dock = DockStyle.Fill;
			TransportForm.Visible = true;
			TransportForm.AllocationForm.MainScrubber.DoubleClick += MainScrubber_DoubleClick;
			panel1.Controls.Add(TransportForm);

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

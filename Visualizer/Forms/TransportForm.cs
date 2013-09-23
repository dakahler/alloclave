using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace Alloclave
{
	internal partial class TransportForm : ToolForm
	{
		public Profile Profile
		{
			get;
			set;
		}

		AllocationForm AllocationForm;
		MessagesForm MessagesForm = new MessagesForm();
		InfoForm InfoForm = new InfoForm();

		void History_Updated(object sender, EventArgs e)
		{
			this.Invoke((MethodInvoker)(() =>
			{
				AllocationForm.Enabled = true;
				Profile.History.Updated -= History_Updated;
			}));
		}

		public TransportForm()
		{
			InitializeComponent();

			AddressSpace.SelectionChanged += AddressSpaceControl_SelectionChanged;
			MessagesForm.AllocationSelected += MessagesForm_AllocationSelected;
		}

		void AddressSpaceControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			InfoForm.Update(e.SelectedBlock.Allocation);
		}

		void MessagesForm_AllocationSelected(object sender, MessagesForm.AllocationSelectedEventArgs e)
		{
			AllocationForm.AddressSpaceControl.SelectAt(e.SelectedAllocation);
		}

		void MainScrubber_PositionChanged(object sender, EventArgs e)
		{
			Profile.History.UpdateRollingSnapshotAsync();
		}

		public TransportForm(ref Transport transport)
			: this()
		{
			Profile = new Profile(ref transport);
			Init();
		}

		private void Init()
		{
			Profile.History.Updated += History_Updated;

			AllocationForm = new AllocationForm(Profile.History);
			AllocationForm.MainScrubber.PositionChanged += MainScrubber_PositionChanged;

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = true;
			_DockPanel.Theme = new VS2012LightTheme();

			AllocationForm.Show(_DockPanel);
			MessagesForm.Show(_DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom);
			InfoForm.Show(MessagesForm.Pane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.55);

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = false;
		}

		public void Save(String path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			DataContractSerializer serializer = new DataContractSerializer(Profile.GetType());
			var settings = new XmlWriterSettings { Indent = true };
			using (var w = XmlWriter.Create(File.Create(path), settings))
			{
				serializer.WriteObject(w, Profile);
			}
		}

		public void Load(String path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			DataContractSerializer serializer = new DataContractSerializer(Profile.GetType());
			FileStream fileStream = new FileStream(path, FileMode.Open);
			Profile = (Profile)serializer.ReadObject(fileStream);

			// TODO: Whole transport form needs to be reinitialized on load
			InitializeComponent();
			Init();
			Profile.History.LastTimestamp = new TimeStamp();
			Profile.History.UpdateRollingSnapshotAsync(true);
		}
	}
}

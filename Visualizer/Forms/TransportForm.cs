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

        public AllocationForm AllocationForm { get; private set; }
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

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			AddressSpace.SelectionChanged -= AddressSpaceControl_SelectionChanged;
			MessagesForm.AllocationSelected -= MessagesForm_AllocationSelected;

			AllocationForm.MainScrubber.Dispose();

			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
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
			Profile.History.UpdateSnapshotAsync(Profile.History.Snapshot);
		}

		public TransportForm(Profile profile)
			: this()
		{
			Profile = profile;
			Init();
			AllocationForm.Enabled = true;
		}

		public TransportForm(ref Transport transport)
			: this()
		{
			Profile = new Profile(ref transport);
			Init();
		}

		void Init()
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

		public void AddTab(AllocationForm allocationForm)
		{
			allocationForm.TopLevel = false;
			allocationForm.Dock = DockStyle.Fill;
			allocationForm.Show(_DockPanel);
		}
	}
}

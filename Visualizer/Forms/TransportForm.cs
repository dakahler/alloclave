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
	}
}

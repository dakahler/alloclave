using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Alloclave
{
	public partial class TransportForm : ToolForm
	{
		Profile Profile;

		// TODO: Should this be spawned immediately?
		AllocationForm AllocationForm = new AllocationForm();
		MessagesForm MessagesForm = new MessagesForm();
		InfoForm InfoForm = new InfoForm();

		public History History
		{
			get
			{
				return Profile.History;
			}
			set
			{
				Profile.History = value;
				if (Profile.History != null)
				{
					History.Updated += new EventHandler(AllocationForm.AddressSpaceControl.History_Updated);
					History.Updated += History_Updated;
					AllocationForm.AddressSpaceControl.Rebuild(Profile.History);
				}
			}
		}

		void History_Updated(object sender, EventArgs e)
		{
			this.Invoke((MethodInvoker)(() =>
			{
				AllocationForm.Enabled = true;
				History.Updated -= History_Updated;
			}));
		}

		public TransportForm()
		{
			InitializeComponent();

			AddressSpace.SelectionChanged += AddressSpaceControl_SelectionChanged;
			MessagesForm.AllocationSelected += MessagesForm_AllocationSelected;
		}

		~TransportForm()
		{

		}

		void AddressSpaceControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			InfoForm.Update(e.SelectedBlock.Allocation);
		}

		void MessagesForm_AllocationSelected(object sender, MessagesForm.AllocationSelectedEventArgs e)
		{
			AllocationForm.AddressSpaceControl.SelectAt(e.SelectedAllocation);
		}

		public TransportForm(ref Transport transport)
			: this()
		{
			Profile = new Profile(ref transport);
			History = History.Instance;

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = true;
			_DockPanel.Theme = new VS2012LightTheme();

			AllocationForm.Show(_DockPanel);
			MessagesForm.Show(_DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom);
			InfoForm.Show(MessagesForm.Pane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.55);

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = false;
		}
	}
}

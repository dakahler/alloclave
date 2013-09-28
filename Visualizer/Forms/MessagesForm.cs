using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Alloclave
{
	internal partial class MessagesForm : ToolForm
	{
		private static MessagesForm Instance;

		private class StringSource
		{
			public String Description
			{
				get;
				set;
			}

			public Allocation ReferenceAllocation;

			public StringSource(String description)
			{
				Description = description;
			}

			public StringSource(String description, Allocation referenceAllocation)
				: this(description)
			{
				ReferenceAllocation = referenceAllocation;
			}

			public static implicit operator StringSource(String description)
			{
				return new StringSource(description);
			}
		}

		public class AllocationSelectedEventArgs : EventArgs
		{
			public Allocation SelectedAllocation;
		}

		public delegate void AllocationSelectedEventHandler(object sender, AllocationSelectedEventArgs e);
		public event AllocationSelectedEventHandler AllocationSelected;

		private List<StringSource> Errors = new List<StringSource>();
		private List<StringSource> Warnings = new List<StringSource>();
		private List<StringSource> Infos = new List<StringSource>();

		ToolForm ErrorsForm;
		ToolForm WarningsForm;
		ToolForm InfosForm;

		public enum MessageType
		{
			Error,
			Warning,
			Info,
		}

		public MessagesForm()
		{
			Instance = this;
			InitializeComponent();

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = true;

			TopLevel = false;

			dockPanel1.Theme = new VS2012LightTheme();

			ErrorsForm = new ToolForm();
			ErrorsForm.CloseButton = false;
			ErrorsForm.CloseButtonVisible = false;
			ErrorsForm.TopLevel = false;
			ErrorsForm.Text = "Errors";
			ErrorsForm.Controls.Add(ErrorsDataGrid);
			this.Controls.Add(ErrorsForm);
			ErrorsForm.Show(dockPanel1);

			WarningsForm = new ToolForm();
			WarningsForm.CloseButton = false;
			WarningsForm.CloseButtonVisible = false;
			WarningsForm.TopLevel = false;
			WarningsForm.Text = "Warnings";
			WarningsForm.Controls.Add(WarningsDataGrid);
			this.Controls.Add(WarningsForm);
			WarningsForm.Show(dockPanel1);

			InfosForm = new ToolForm();
			InfosForm.CloseButton = false;
			InfosForm.CloseButtonVisible = false;
			InfosForm.TopLevel = false;
			InfosForm.Text = "Info";
			InfosForm.Controls.Add(InfosDataGrid);
			this.Controls.Add(InfosForm);
			InfosForm.Show(dockPanel1);

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = false;

			this.Load += MessagesForm_Load;
		}

		void MessagesForm_Load(object sender, EventArgs e)
		{
			if (Licensing.IsTrial)
			{
				AddInternal(MessageType.Info, null, "You are running a trial version of Alloclave.");
				InfosForm.Activate();
			}

			bool isAdmin = (new WindowsPrincipal(WindowsIdentity.GetCurrent())).
				IsInRole(WindowsBuiltInRole.Administrator);

			if (isAdmin)
			{
				AddInternal(MessageType.Warning, null, "Alloclave has elevated priveleges, which may cause transport errors.");
				WarningsForm.Activate();
			}
		}

		public static void Add(MessageType type, Allocation referenceAllocation, String text)
		{
			Instance.AddInternal(type, referenceAllocation, text);
		}

		private void AddInternal(MessageType type, Allocation referenceAllocation, String text)
		{
			Task task = new Task(() =>
			{
				this.Invoke((MethodInvoker)(() =>
				{
					switch (type)
					{
						case MessageType.Error:
							{
								Errors.Add(new StringSource(text, referenceAllocation));
								var bindingList = new BindingList<StringSource>(Errors);
								ErrorsDataGrid.DataSource = bindingList;
								ErrorsForm.Text = "Errors (" + Errors.Count + ")";
								break;
							}
						case MessageType.Warning:
							{
								Warnings.Add(new StringSource(text, referenceAllocation));
								var bindingList = new BindingList<StringSource>(Warnings);
								WarningsDataGrid.DataSource = bindingList;
								WarningsForm.Text = "Warnings (" + Warnings.Count + ")";
								break;
							}
						case MessageType.Info:
							{
								Infos.Add(new StringSource(text, referenceAllocation));
								var bindingList = new BindingList<StringSource>(Infos);
								InfosDataGrid.DataSource = bindingList;
								InfosForm.Text = "Info (" + Infos.Count + ")";
								break;
							}
					}
				}));
			});

			task.Start();
		}

		private void DataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridView dataGrid = (DataGridView)sender;
			BindingList<StringSource> dataSource = (BindingList<StringSource>)dataGrid.DataSource;
			Allocation allocation = dataSource[e.RowIndex].ReferenceAllocation;
			if (allocation != null && AllocationSelected != null)
			{
				AllocationSelectedEventArgs newEvent = new AllocationSelectedEventArgs();
				newEvent.SelectedAllocation = allocation;
				AllocationSelected(this, newEvent);
			}
		}
	}
}

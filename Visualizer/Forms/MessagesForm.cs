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
	public partial class MessagesForm : ToolForm
	{
		private static MessagesForm Instance;

		private class StringSource
		{
			private String _Description;
			public System.String Description
			{
				get { return _Description; }
				set { _Description = value; }
			}

			public Allocation ReferenceAllocation;

			public StringSource(String description)
			{
				_Description = description;
			}

			public StringSource(String description, Allocation referenceAllocation)
			{
				_Description = description;
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

			ToolForm errorsForm = new ToolForm();
			errorsForm.CloseButton = false;
			errorsForm.CloseButtonVisible = false;
			errorsForm.TopLevel = false;
			errorsForm.Text = "Errors";
			errorsForm.Controls.Add(ErrorsDataGrid);
			this.Controls.Add(errorsForm);
			errorsForm.Show(dockPanel1);

			ToolForm warningsForm = new ToolForm();
			warningsForm.CloseButton = false;
			warningsForm.CloseButtonVisible = false;
			warningsForm.TopLevel = false;
			warningsForm.Text = "Warnings";
			warningsForm.Controls.Add(WarningsDataGrid);
			this.Controls.Add(warningsForm);
			warningsForm.Show(dockPanel1);

			ToolForm infosForm = new ToolForm();
			infosForm.CloseButton = false;
			infosForm.CloseButtonVisible = false;
			infosForm.TopLevel = false;
			infosForm.Text = "Info";
			infosForm.Controls.Add(InfosDataGrid);
			this.Controls.Add(infosForm);
			infosForm.Show(dockPanel1);

			WeifenLuo.WinFormsUI.Docking.DockHelper.PreventActivation = false;

			if (Licensing.IsTrial)
			{
				AddInternal(MessageType.Info, null, "You are running a trial version of alloclave.");
			}
		}

		public static void Add(MessageType type, Allocation referenceAllocation, String text)
		{
			Instance.AddInternal(type, referenceAllocation, text);
		}

		private void AddInternal(MessageType type, Allocation referenceAllocation, String text)
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
							ErrorsTabPage.Text = "Errors (" + Errors.Count + ")";
							break;
						}
					case MessageType.Warning:
						{
							Warnings.Add(new StringSource(text, referenceAllocation));
							var bindingList = new BindingList<StringSource>(Warnings);
							WarningsDataGrid.DataSource = bindingList;
							WarningsTabPage.Text = "Warnings (" + Warnings.Count + ")";
							break;
						}
					case MessageType.Info:
						{
							Infos.Add(new StringSource(text, referenceAllocation));
							var bindingList = new BindingList<StringSource>(Infos);
							InfosDataGrid.DataSource = bindingList;
							InfoTabPage.Text = "Info (" + Infos.Count + ")";
							break;
						}
				}
			}));
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

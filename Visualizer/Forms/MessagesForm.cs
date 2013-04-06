using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

			public StringSource(String description)
			{
				_Description = description;
			}

			public static implicit operator StringSource(String description)
			{
				return new StringSource(description);
			}
		}

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

			if (Licensing.IsTrial)
			{
				AddInternal(MessageType.Warning, "You are running a trial version of alloclave.");
				tabControl1.SelectedTab = tabPage2;
			}
		}

		public static void Add(MessageType type, String text)
		{
			Instance.AddInternal(type, text);
		}

		private void AddInternal(MessageType type, String text)
		{
			switch (type)
			{
				case MessageType.Error:
				{
					Errors.Add(text);
					var bindingList = new BindingList<StringSource>(Errors);
					ErrorsDataGrid.DataSource = bindingList;
					break;
				}
				case MessageType.Warning:
				{
					Warnings.Add(text);
					var bindingList = new BindingList<StringSource>(Warnings);
					WarningsDataGrid.DataSource = bindingList;
					break;
				}
				case MessageType.Info:
				{
					Infos.Add(text);
					var bindingList = new BindingList<StringSource>(Infos);
					InfosDataGrid.DataSource = bindingList;
					break;
				}
			}
		}
	}
}

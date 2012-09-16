using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Meminator
{
	public partial class AddressSpace : UserControl
	{
		private History _History;
		public History History
		{
			get
			{ 
				return _History;
			}
			set
			{
				_History = value;
				_History.Updated += new EventHandler(History_Updated);
				Rebuild();
			}
		}

		void History_Updated(object sender, EventArgs e)
		{
			Rebuild();
		}

		private void Rebuild()
		{
			
		}

		public AddressSpace()
		{
			InitializeComponent();
		}
	}
}

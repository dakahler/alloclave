﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class Main : Form
	{
		Profile Profile = new Profile();

		public Main()
		{
			InitializeComponent();

			AddressSpaceControl.History = Profile.History;
		}
	}
}

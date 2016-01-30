using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
    internal partial class DiffButtons : UserControl
    {
		Color InactiveColor = System.Drawing.Color.FromArgb(237, 208, 177);
		Color ActiveColor = System.Drawing.Color.FromArgb(231, 157, 103);

        public DiffButtons()
        {
            InitializeComponent();
			this.Dock = DockStyle.Fill;
        }

		private void StartLabel_Click(object sender, EventArgs e)
		{
			DifferenceLabel.BackColor = InactiveColor;
			DifferenceLabel.ForeColor = Color.Black;

			EndLabel.BackColor = InactiveColor;
			EndLabel.ForeColor = Color.Black;

			StartLabel.BackColor = ActiveColor;
			StartLabel.ForeColor = Color.White;

		}

		private void DifferenceLabel_Click(object sender, EventArgs e)
		{
			DifferenceLabel.BackColor = ActiveColor;
			DifferenceLabel.ForeColor = Color.White;

			EndLabel.BackColor = InactiveColor;
			EndLabel.ForeColor = Color.Black;

			StartLabel.BackColor = InactiveColor;
			StartLabel.ForeColor = Color.Black;
		}

		private void EndLabel_Click(object sender, EventArgs e)
		{
			DifferenceLabel.BackColor = InactiveColor;
			DifferenceLabel.ForeColor = Color.Black;

			EndLabel.BackColor = ActiveColor;
			EndLabel.ForeColor = Color.White;

			StartLabel.BackColor = InactiveColor;
			StartLabel.ForeColor = Color.Black;
		}
    }
}

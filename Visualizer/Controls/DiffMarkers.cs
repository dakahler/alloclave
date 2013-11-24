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
    public partial class DiffMarkers : UserControl
    {
        public DiffMarkers()
        {
            InitializeComponent();

            Diff1.Visible = false;
            Diff2.Visible = false;
        }

        public void SetDiff1(float t)
        {
            Diff1.Location = new Point((int)((Width - Diff1.Width) * t), 0);
            Diff1.Visible = true;
            Diff2.Visible = false;
        }

        public void SetDiff2(float t)
        {
            Diff2.Location = new Point((int)((Width - Diff2.Width) * t), 0);
            Diff2.Visible = true;
        }
    }
}

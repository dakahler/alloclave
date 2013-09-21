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
	internal partial class AllocationForm : ToolForm
	{
		private AddressSpaceScroller_OGL AddressSpaceScroller;

		public AllocationForm()
		{
			InitializeComponent();

			this.AddressSpaceScroller = new Alloclave.AddressSpaceScroller_OGL(AddressSpaceControl.Width);
			this.AddressSpaceScroller.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddressSpaceScroller.Location = new System.Drawing.Point(679, 6);
			this.AddressSpaceScroller.Name = "AddressSpaceScroller";
			this.AddressSpaceScroller.Size = new System.Drawing.Size(44, 436);
			this.AddressSpaceScroller.TabIndex = 5;
			this.TableLayoutPanel.Controls.Add(this.AddressSpaceScroller, 2, 0);

			TopLevel = false;
			AddressSpaceScroller.FocusChanged += addressSpaceScroller_FocusChanged;
			this.SizeChanged += AllocationForm_SizeChanged;
			MainScrubber.PositionChanged += MainScrubber_PositionChanged;

			// Disabled by default - gets enabled when data comes in
			this.Enabled = false;

			AddressSpaceControl.PauseChanged += AddressSpaceControl_PauseChanged;
		}

		void MainScrubber_PositionChanged(object sender, EventArgs e)
		{
			History.Instance.UpdateRollingSnapshotAsync();
		}

		void AllocationForm_SizeChanged(object sender, EventArgs e)
		{
			AddressSpaceScroller.ParentWidth = AddressSpaceControl.Width;
		}

		void addressSpaceScroller_FocusChanged(object sender, MouseEventArgs e)
		{
			AddressSpaceControl.CenterAt(e.Location.ToVector());
		}

		void PlayPausePictureBox_Click(object sender, System.EventArgs e)
		{
			AddressSpaceControl.IsPaused = !AddressSpaceControl.IsPaused;

			if (AddressSpaceControl.IsPaused)
			{
				PlayPausePictureBox.Image = Properties.Resources.play;
			}
			else
			{
				PlayPausePictureBox.Image = Properties.Resources.pause;
			}
		}

		void AddressSpaceControl_PauseChanged(object sender, EventArgs e)
		{
			if (AddressSpaceControl.IsPaused)
			{
				PlayPausePictureBox.Image = Properties.Resources.play;
			}
			else
			{
				PlayPausePictureBox.Image = Properties.Resources.pause;
			}
		}
	}
}

﻿using System;
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
		AddressSpaceScroller_OGL AddressSpaceScroller;
		RenderManager_OGL RenderManager = new RenderManager_OGL();

		Diff Diff;

		private AllocationForm()
		{
			InitializeComponent();

			AddressSpaceControl.Renderer = new AddressSpaceRenderer_OGL(AddressSpaceControl, RenderManager);
			TopLevel = false;
			this.SizeChanged += AllocationForm_SizeChanged;
		}

		public AllocationForm(History history)
			: this()
		{
			history.Scrubber = MainScrubber;

			MainScrubber.MousePressed += ((object sender, MouseEventArgs e) => Scrubber_MouseDown(history, e));
			MainScrubber.MouseReleased += ((object sender, MouseEventArgs e) => Scrubber_MouseUp(history, e));

			// TODO: Why does this need history?
			this.AddressSpaceScroller =
				new Alloclave.AddressSpaceScroller_OGL(history, RenderManager, AddressSpaceControl.Width);
			this.AddressSpaceScroller.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddressSpaceScroller.Location = new System.Drawing.Point(679, 6);
			this.AddressSpaceScroller.Name = "AddressSpaceScroller";
			this.AddressSpaceScroller.Size = new System.Drawing.Size(44, 436);
			this.AddressSpaceScroller.TabIndex = 5;
			this.AddressSpaceScroller.FocusChanged += addressSpaceScroller_FocusChanged;
			this.TableLayoutPanel.Controls.Add(this.AddressSpaceScroller, 2, 0);

			// Disabled by default - gets enabled when data comes in
			this.Enabled = false;

			AddressSpaceControl.PauseChanged += AddressSpaceControl_PauseChanged;
			AddressSpaceControl.History = history;
		}

		public AllocationForm(Diff diff)
			: this()
		{
			Diff = diff;
			RenderManager.Rebuild(Diff.Difference, AddressSpaceControl.Width);
		}

		void AllocationForm_SizeChanged(object sender, EventArgs e)
		{
			if (AddressSpaceScroller != null)
			{
				AddressSpaceScroller.ParentWidth = AddressSpaceControl.Width;
			}
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

		void Scrubber_MouseDown(object sender, MouseEventArgs e)
		{
			History history = sender as History;

			history.ArtificialMaxTime = history.TimeRange.Max;
			AddressSpaceControl.IsPaused = true;
		}

		void Scrubber_MouseUp(object sender, MouseEventArgs e)
		{
			History history = sender as History;

			history.ArtificialMaxTime = 0;
			history.UpdateRollingSnapshotAsync();
		}
	}
}

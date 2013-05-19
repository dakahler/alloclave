﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Alloclave
{
	public partial class Tour : Form
	{
		enum TourStage
		{
			Start,
			EmptyProfile,
			DemoStarted,
			SelectAllocation,
			AllocationSelected,
			Scroll,
			AutoScrub,
			UserScrub,
			End
		}

		TourStage Stage = TourStage.Start;

		Process TestProcess;

		public Tour()
		{
			InitializeComponent();
		}

		void QuickStartLabel_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Common.ProductWebsiteUrl + "quickstart");
		}

		void AdvanceButton_Click(object sender, EventArgs e)
		{
			switch (Stage)
			{
				case TourStage.Start:
					Stage = TourStage.EmptyProfile;
					SetupEmptyProfilePage();
					break;
				case TourStage.EmptyProfile:
					Stage = TourStage.DemoStarted;
					SetupDemoStartedPage();
					break;
				case TourStage.DemoStarted:
					Stage = TourStage.SelectAllocation;
					SetupSelectAllocationPage();
					break;
				case TourStage.SelectAllocation:
					Stage = TourStage.AllocationSelected;
					SetupAllocationSelectedPage();
					break;
				case TourStage.AllocationSelected:
					Stage = TourStage.End;
					SetupEndPage();
					break;
				case TourStage.End:
					// This has too many issues right now
					//Main.Instance.ReturnToStartScreen();
					Application.Restart();
					this.Close();
					break;
			}
		}

		void SetupEmptyProfilePage()
		{
			Main.Instance.StartNewSession("Win32 Messaging");

			MainTextBox.Text =
				"This is Alloclave's main work area. We haven't started collecting any data yet, " +
				"so it's pretty boring looking. Let's fix that." + Environment.NewLine + Environment.NewLine +
				"Click the button below to launch Alloclave's test collection application " +
				"and begin receiving data.";

			AdvanceButton.Text = "Let's Go!";
		}

		void SetupDemoStartedPage()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.CreateNoWindow = true;
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.FileName = @"collector\bin\TestCollector.exe";
			startInfo.UseShellExecute = true;
			startInfo.WorkingDirectory = Application.StartupPath;

			TestProcess = Process.Start(startInfo);
			TestProcess.EnableRaisingEvents = true;
			TestProcess.Exited += TestProcess_Exited;

			MainTextBox.Text =
				"Now we're talking! The data populating in the main window is a real-time " +
				"representation of the memory allocations created in the test application." + Environment.NewLine +
				Environment.NewLine +
				"For the purposes of this tour, let's wait a moment for the test application " +
				"to exit. Remember, though, that it is possible (and suggested!) to inspect " +
				"the data as the application is running!";

			AdvanceButton.Text = "Please Wait...";
			AdvanceButton.Enabled = false;
		}

		void TestProcess_Exited(object sender, EventArgs e)
		{
			this.Invoke((MethodInvoker)(() =>
				{
					AdvanceButton.Text = "Done! Click Here.";
					AdvanceButton.Enabled = true;
				}
			));
		}

		void SetupSelectAllocationPage()
		{
			MainTextBox.Text =
				"Great! Now there's something to look at. To inspect an allocation, " +
				"just click on it." + Environment.NewLine + Environment.NewLine +
				"Click on an allocation in the main window to continue.";

			AdvanceButton.Text = "Waiting...";
			AdvanceButton.Enabled = false;

			AddressSpace.SelectionChanged += AddressSpace_SelectionChanged;
		}

		void AddressSpace_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			AdvanceButton.Text = "Nice work! Click Here.";
			AdvanceButton.Enabled = true;
			AddressSpace.SelectionChanged -= AddressSpace_SelectionChanged;
		}

		void SetupAllocationSelectedPage()
		{
			MainTextBox.Text =
				"The history of all the allocations generated by the application is " +
				"available at any time. Simply drag the slider that's under the allocation area " +
				"as shown automatically right now.";

			AdvanceButton.Text = "Please Wait...";
			AdvanceButton.Enabled = false;

			System.Timers.Timer startTimer = new System.Timers.Timer();
			startTimer.AutoReset = false;
			startTimer.Interval = 2000.0;
			startTimer.Elapsed += startTimer_Elapsed;
			startTimer.Start();
		}

		void startTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			System.Timers.Timer scrubBackTimer = new System.Timers.Timer();
			scrubBackTimer.AutoReset = true;
			scrubBackTimer.Interval = 10.0;
			scrubBackTimer.Elapsed += scrubBackTimer_Elapsed;
			scrubBackTimer.Start();
		}

		void scrubBackTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Scrubber.Position > 0.0f)
			{
				Scrubber.Position -= 0.005f;
			}
			else
			{
				System.Timers.Timer timer = (System.Timers.Timer)sender;
				timer.Enabled = false;

				System.Timers.Timer scrubForwardTimer = new System.Timers.Timer();
				scrubForwardTimer.AutoReset = true;
				scrubForwardTimer.Interval = 10.0;
				scrubForwardTimer.Elapsed += scrubForwardTimer_Elapsed;
				scrubForwardTimer.Start();
			}
		}

		void scrubForwardTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Scrubber.Position < 1.0f)
			{
				Scrubber.Position += 0.005f;
			}
			else
			{
				System.Timers.Timer timer = (System.Timers.Timer)sender;
				timer.Enabled = false;

				this.Invoke((MethodInvoker)(() =>
				{
					MainTextBox.Text =
						"Try dragging the slider around yourself, then click the button below to continue.";

					AdvanceButton.Text = "Continue";
					AdvanceButton.Enabled = true;
				}
				));
			}
		}

		void SetupEndPage()
		{
			MainTextBox.Text =
				"Pretty neat, huh? To move around the allocation space, simply click and drag " +
				"in the main area or in the zoomed out \"scroller\" to the right. " +
				"You can also zoom in and out with the mouse scroll wheel." + Environment.NewLine +
				Environment.NewLine +
				"That's it for the tour! The button below will take you back to the start screen, " +
				"where you can click \"New Profile\" to start using Alloclave after integrating " +
				"it into your application. Enjoy!";

			AdvanceButton.Text = "Finish";
			AdvanceButton.Enabled = true;

			AlloclaveLabel.Visible = true;
		}

		private void AlloclaveLabel_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Common.ProductWebsiteUrl);
		}
	}
}

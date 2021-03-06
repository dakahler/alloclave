﻿using Alloclave.Properties;
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
	internal partial class StartScreen : Form
	{
		Tour Tour;

		public StartScreen()
		{
			InitializeComponent();
		}

		private void QuickStartPanel_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Common.ProductWebsiteUrl + "quickstart");
		}

		private void QuickStartPictureBox_MouseEnter(object sender, EventArgs e)
		{
			QuickStartPictureBox.Image = Resources.quickstart_hover;
		}

		private void QuickStartPictureBox_MouseLeave(object sender, EventArgs e)
		{
			QuickStartPictureBox.Image = Resources.quickstart;
		}

		private void QuickStartPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			QuickStartPictureBox.Image = Resources.quickstart;
		}

		private void QuickStartPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			QuickStartPictureBox.Image = Resources.quickstart_hover;
		}

		private void NewProfilePictureBox_MouseEnter(object sender, EventArgs e)
		{
			NewProfilePictureBox.Image = Resources.newprofile_hover;
		}

		private void NewProfilePictureBox_MouseLeave(object sender, EventArgs e)
		{
			NewProfilePictureBox.Image = Resources.newprofile;
		}

		private void NewProfilePictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			NewProfilePictureBox.Image = Resources.newprofile;
		}

		private void NewProfilePictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			NewProfilePictureBox.Image = Resources.newprofile_hover;
		}

		private void LogoPictureBox_MouseEnter(object sender, EventArgs e)
		{
			LogoPictureBox.Image = Resources.CircularShiftLogo_hover;
		}

		private void LogoPictureBox_MouseLeave(object sender, EventArgs e)
		{
			LogoPictureBox.Image = Resources.CircularShiftLogo;
		}

		private void LogoPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			LogoPictureBox.Image = Resources.CircularShiftLogo;
		}

		private void LogoPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			LogoPictureBox.Image = Resources.CircularShiftLogo_hover;
		}

		private void LogoPictureBox_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Common.CompanyWebsiteUrl);
		}

		private void NewProfilePictureBox_Click(object sender, EventArgs e)
		{
			if (Tour != null)
			{
				Tour.Close();
			}

			Main.Instance.StartNewSession();
		}

		private void openProfilePictureBox_Click(object sender, EventArgs e)
		{
			if (Tour != null)
			{
				Tour.Close();
			}

			Main.Instance.OpenMenuItem_Click(this, new EventArgs());
		}

		private void TourPictureBox_MouseEnter(object sender, EventArgs e)
		{
			TourPictureBox.Image = Resources.tour_hover;
		}

		private void TourPictureBox_MouseLeave(object sender, EventArgs e)
		{
			TourPictureBox.Image = Resources.tour;
		}

		private void TourPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			TourPictureBox.Image = Resources.tour;
		}

		private void TourPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			TourPictureBox.Image = Resources.tour_hover;
		}

		private void TourPictureBox_Click(object sender, EventArgs e)
		{
			Form existingForm = null;
			foreach (Form form in Application.OpenForms)
			{
				if (form is Tour)
				{
					existingForm = form;
					break;
				}
			}

			if (existingForm == null)
			{
				Tour = new Tour();
				Tour.Height = Main.Instance.Height;
				Tour.Show(Main.Instance);
				Tour.SetDesktopLocation(Main.Instance.Location.X + Main.Instance.Size.Width, Main.Instance.Location.Y);
			}
			else
			{
				existingForm.Activate();
			}
		}

        private void openProfilePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            openProfilePictureBox.Image = Resources.openprofile;
        }

        private void openProfilePictureBox_MouseEnter(object sender, EventArgs e)
        {
            openProfilePictureBox.Image = Resources.openprofile_hover;
        }

        private void openProfilePictureBox_MouseLeave(object sender, EventArgs e)
        {
            openProfilePictureBox.Image = Resources.openprofile;
        }

        private void openProfilePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            openProfilePictureBox.Image = Resources.openprofile_hover;
        }
	}
}

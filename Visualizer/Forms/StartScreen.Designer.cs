namespace Alloclave
{
	partial class StartScreen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.NewPanel = new System.Windows.Forms.Panel();
			this.NewProfilePictureBox = new System.Windows.Forms.PictureBox();
			this.QuickStartPanel = new System.Windows.Forms.Panel();
			this.QuickStartPictureBox = new System.Windows.Forms.PictureBox();
			this.DemoPanel = new System.Windows.Forms.Panel();
			this.TourPictureBox = new System.Windows.Forms.PictureBox();
			this.LogoPanel = new System.Windows.Forms.Panel();
			this.LogoPictureBox = new System.Windows.Forms.PictureBox();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.NewPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NewProfilePictureBox)).BeginInit();
			this.QuickStartPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.QuickStartPictureBox)).BeginInit();
			this.DemoPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TourPictureBox)).BeginInit();
			this.LogoPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Controls.Add(this.linkLabel2);
			this.panel1.Controls.Add(this.linkLabel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(713, 454);
			this.panel1.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.Controls.Add(this.NewPanel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.QuickStartPanel, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.DemoPanel, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.LogoPanel, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(713, 454);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// NewPanel
			// 
			this.NewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.NewPanel.Controls.Add(this.NewProfilePictureBox);
			this.NewPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.NewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NewPanel.Location = new System.Drawing.Point(10, 78);
			this.NewPanel.Margin = new System.Windows.Forms.Padding(10);
			this.NewPanel.Name = "NewPanel";
			this.NewPanel.Size = new System.Drawing.Size(407, 229);
			this.NewPanel.TabIndex = 0;
			this.NewPanel.Click += new System.EventHandler(this.NewPanel_Click);
			// 
			// NewProfilePictureBox
			// 
			this.NewProfilePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NewProfilePictureBox.Image = global::Alloclave.Properties.Resources.newprofile;
			this.NewProfilePictureBox.Location = new System.Drawing.Point(0, 0);
			this.NewProfilePictureBox.Name = "NewProfilePictureBox";
			this.NewProfilePictureBox.Size = new System.Drawing.Size(407, 229);
			this.NewProfilePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.NewProfilePictureBox.TabIndex = 0;
			this.NewProfilePictureBox.TabStop = false;
			this.NewProfilePictureBox.Click += new System.EventHandler(this.NewProfilePictureBox_Click);
			this.NewProfilePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NewProfilePictureBox_MouseDown);
			this.NewProfilePictureBox.MouseEnter += new System.EventHandler(this.NewProfilePictureBox_MouseEnter);
			this.NewProfilePictureBox.MouseLeave += new System.EventHandler(this.NewProfilePictureBox_MouseLeave);
			this.NewProfilePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NewProfilePictureBox_MouseUp);
			// 
			// QuickStartPanel
			// 
			this.QuickStartPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.QuickStartPanel.Controls.Add(this.QuickStartPictureBox);
			this.QuickStartPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.QuickStartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuickStartPanel.Location = new System.Drawing.Point(437, 78);
			this.QuickStartPanel.Margin = new System.Windows.Forms.Padding(10);
			this.QuickStartPanel.Name = "QuickStartPanel";
			this.QuickStartPanel.Size = new System.Drawing.Size(266, 229);
			this.QuickStartPanel.TabIndex = 1;
			this.QuickStartPanel.Click += new System.EventHandler(this.QuickStartPanel_Click);
			// 
			// QuickStartPictureBox
			// 
			this.QuickStartPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuickStartPictureBox.Image = global::Alloclave.Properties.Resources.quickstart;
			this.QuickStartPictureBox.Location = new System.Drawing.Point(0, 0);
			this.QuickStartPictureBox.Name = "QuickStartPictureBox";
			this.QuickStartPictureBox.Size = new System.Drawing.Size(266, 229);
			this.QuickStartPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.QuickStartPictureBox.TabIndex = 0;
			this.QuickStartPictureBox.TabStop = false;
			this.QuickStartPictureBox.Click += new System.EventHandler(this.QuickStartPanel_Click);
			this.QuickStartPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.QuickStartPictureBox_MouseDown);
			this.QuickStartPictureBox.MouseEnter += new System.EventHandler(this.QuickStartPictureBox_MouseEnter);
			this.QuickStartPictureBox.MouseLeave += new System.EventHandler(this.QuickStartPictureBox_MouseLeave);
			this.QuickStartPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.QuickStartPictureBox_MouseUp);
			// 
			// DemoPanel
			// 
			this.DemoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.tableLayoutPanel1.SetColumnSpan(this.DemoPanel, 2);
			this.DemoPanel.Controls.Add(this.TourPictureBox);
			this.DemoPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DemoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DemoPanel.Location = new System.Drawing.Point(10, 327);
			this.DemoPanel.Margin = new System.Windows.Forms.Padding(10);
			this.DemoPanel.Name = "DemoPanel";
			this.DemoPanel.Size = new System.Drawing.Size(693, 117);
			this.DemoPanel.TabIndex = 2;
			this.DemoPanel.Click += new System.EventHandler(this.DemoPanel_Click);
			// 
			// TourPictureBox
			// 
			this.TourPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TourPictureBox.Image = global::Alloclave.Properties.Resources.tour;
			this.TourPictureBox.Location = new System.Drawing.Point(0, 0);
			this.TourPictureBox.Name = "TourPictureBox";
			this.TourPictureBox.Size = new System.Drawing.Size(693, 117);
			this.TourPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.TourPictureBox.TabIndex = 0;
			this.TourPictureBox.TabStop = false;
			this.TourPictureBox.Click += new System.EventHandler(this.TourPictureBox_Click);
			this.TourPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TourPictureBox_MouseDown);
			this.TourPictureBox.MouseEnter += new System.EventHandler(this.TourPictureBox_MouseEnter);
			this.TourPictureBox.MouseLeave += new System.EventHandler(this.TourPictureBox_MouseLeave);
			this.TourPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TourPictureBox_MouseUp);
			// 
			// LogoPanel
			// 
			this.LogoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.tableLayoutPanel1.SetColumnSpan(this.LogoPanel, 2);
			this.LogoPanel.Controls.Add(this.LogoPictureBox);
			this.LogoPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LogoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LogoPanel.Location = new System.Drawing.Point(10, 10);
			this.LogoPanel.Margin = new System.Windows.Forms.Padding(10);
			this.LogoPanel.Name = "LogoPanel";
			this.LogoPanel.Size = new System.Drawing.Size(693, 48);
			this.LogoPanel.TabIndex = 3;
			// 
			// LogoPictureBox
			// 
			this.LogoPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LogoPictureBox.Image = global::Alloclave.Properties.Resources.CircularShiftLogo;
			this.LogoPictureBox.Location = new System.Drawing.Point(0, 0);
			this.LogoPictureBox.Name = "LogoPictureBox";
			this.LogoPictureBox.Size = new System.Drawing.Size(693, 48);
			this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.LogoPictureBox.TabIndex = 0;
			this.LogoPictureBox.TabStop = false;
			this.LogoPictureBox.Click += new System.EventHandler(this.LogoPictureBox_Click);
			this.LogoPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LogoPictureBox_MouseDown);
			this.LogoPictureBox.MouseEnter += new System.EventHandler(this.LogoPictureBox_MouseEnter);
			this.LogoPictureBox.MouseLeave += new System.EventHandler(this.LogoPictureBox_MouseLeave);
			this.LogoPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LogoPictureBox_MouseUp);
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel2.LinkColor = System.Drawing.Color.Black;
			this.linkLabel2.Location = new System.Drawing.Point(41, 211);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(352, 26);
			this.linkLabel2.TabIndex = 2;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Not sure where to start? Click here!";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel1.LinkColor = System.Drawing.Color.Black;
			this.linkLabel1.Location = new System.Drawing.Point(41, 158);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(357, 29);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Click here to start a new session";
			this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
			// 
			// StartScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(713, 454);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StartScreen";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "StartScreen";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.NewPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.NewProfilePictureBox)).EndInit();
			this.QuickStartPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.QuickStartPictureBox)).EndInit();
			this.DemoPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.TourPictureBox)).EndInit();
			this.LogoPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel NewPanel;
		private System.Windows.Forms.Panel QuickStartPanel;
		private System.Windows.Forms.Panel DemoPanel;
		private System.Windows.Forms.PictureBox QuickStartPictureBox;
		private System.Windows.Forms.PictureBox NewProfilePictureBox;
		private System.Windows.Forms.Panel LogoPanel;
		private System.Windows.Forms.PictureBox LogoPictureBox;
		private System.Windows.Forms.PictureBox TourPictureBox;
	}
}
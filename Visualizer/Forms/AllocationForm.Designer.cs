namespace Alloclave
{
	partial class AllocationForm
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
			if (AddressSpaceScroller != null)
			{
				AddressSpaceScroller.Dispose();
			}

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
            this.AddressSpaceControl = new Alloclave.AddressSpace();
            this.ControllerContainer = new ControllerContainer();
            this.MainScrubber = new Alloclave.Scrubber();
            this.DiffMarkers = new Alloclave.DiffMarkers();
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PlayPausePictureBox = new System.Windows.Forms.PictureBox();
            this.TableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AddressSpaceControl
            // 
            this.AddressSpaceControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressSpaceControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressSpaceControl.History = null;
            this.AddressSpaceControl.IsPaused = false;
            this.AddressSpaceControl.Location = new System.Drawing.Point(1, 1);
            this.AddressSpaceControl.Margin = new System.Windows.Forms.Padding(1);
            this.AddressSpaceControl.Name = "AddressSpaceControl";
            this.AddressSpaceControl.Renderer = null;
            this.AddressSpaceControl.Size = new System.Drawing.Size(677, 442);
            this.AddressSpaceControl.TabIndex = 0;
			this.AddressSpaceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // ControllerContainer
            // 
            this.ControllerContainer.AutoSize = true;
            this.ControllerContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ControllerContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControllerContainer.Location = new System.Drawing.Point(1, 445);
            this.ControllerContainer.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.ControllerContainer.Name = "ControllerContainer";
            this.ControllerContainer.Size = new System.Drawing.Size(677, 26);
            this.ControllerContainer.TabIndex = 1;
            // 
            // MainScrubber
            // 
            this.MainScrubber.AutoSize = true;
            this.MainScrubber.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainScrubber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainScrubber.Location = new System.Drawing.Point(1, 445);
            this.MainScrubber.Margin = new System.Windows.Forms.Padding(0);
            this.MainScrubber.Name = "MainScrubber";
            this.MainScrubber.Position = 1D;
            this.MainScrubber.Size = new System.Drawing.Size(677, 26);
            this.MainScrubber.TabIndex = 1;
            // 
            // DiffMarkers
            // 
            this.DiffMarkers.AutoSize = true;
            this.DiffMarkers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DiffMarkers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DiffMarkers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DiffMarkers.Location = new System.Drawing.Point(1, 473);
            this.DiffMarkers.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.DiffMarkers.Name = "DiffMarkers";
            this.DiffMarkers.Size = new System.Drawing.Size(677, 4);
            this.DiffMarkers.TabIndex = 2;
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TableLayoutPanel.ColumnCount = 3;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TableLayoutPanel.Controls.Add(this.AddressSpaceControl, 0, 0);
            this.TableLayoutPanel.Controls.Add(this.ControllerContainer, 0, 1);
            this.TableLayoutPanel.Controls.Add(this.PlayPausePictureBox, 2, 1);
            this.TableLayoutPanel.Controls.Add(this.DiffMarkers, 0, 2);
            this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 3;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel.Size = new System.Drawing.Size(729, 478);
            this.TableLayoutPanel.TabIndex = 2;
            // 
            // PlayPausePictureBox
            // 
            this.PlayPausePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayPausePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayPausePictureBox.Image = global::Alloclave.Properties.Resources.pause;
            this.PlayPausePictureBox.Location = new System.Drawing.Point(680, 445);
            this.PlayPausePictureBox.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.PlayPausePictureBox.Name = "PlayPausePictureBox";
            this.PlayPausePictureBox.Size = new System.Drawing.Size(48, 26);
            this.PlayPausePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PlayPausePictureBox.TabIndex = 6;
            this.PlayPausePictureBox.TabStop = false;
			this.PlayPausePictureBox.Click += PlayPausePictureBox_Click;
            // 
            // AllocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(729, 478);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.ControlBox = false;
            this.Controls.Add(this.TableLayoutPanel);
            this.ControllerContainer.Controls.Add(MainScrubber);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AllocationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Allocations";
            this.TableLayoutPanel.ResumeLayout(false);
            this.TableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		public AddressSpace AddressSpaceControl;
		private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        public ControllerContainer ControllerContainer;
		public Scrubber MainScrubber;
        public DiffMarkers DiffMarkers;
		private System.Windows.Forms.PictureBox PlayPausePictureBox;

	}
}
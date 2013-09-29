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
			this.MainScrubber = new Alloclave.Scrubber();
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
			this.AddressSpaceControl.Location = new System.Drawing.Point(6, 6);
			this.AddressSpaceControl.Name = "AddressSpaceControl";
			this.AddressSpaceControl.Size = new System.Drawing.Size(667, 436);
			this.AddressSpaceControl.TabIndex = 0;
			// 
			// MainScrubber
			// 
			this.MainScrubber.AutoSize = true;
			this.MainScrubber.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainScrubber.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainScrubber.Location = new System.Drawing.Point(6, 448);
			this.MainScrubber.Name = "MainScrubber";
			this.MainScrubber.Size = new System.Drawing.Size(667, 24);
			this.MainScrubber.TabIndex = 1;
			// 
			// TableLayoutPanel
			// 
			this.TableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.TableLayoutPanel.ColumnCount = 3;
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.TableLayoutPanel.Controls.Add(this.AddressSpaceControl, 0, 0);
			this.TableLayoutPanel.Controls.Add(this.MainScrubber, 0, 1);
			this.TableLayoutPanel.Controls.Add(this.PlayPausePictureBox, 2, 1);
			this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.TableLayoutPanel.Name = "TableLayoutPanel";
			this.TableLayoutPanel.Padding = new System.Windows.Forms.Padding(3);
			this.TableLayoutPanel.RowCount = 2;
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.70277F));
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.29723F));
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableLayoutPanel.Size = new System.Drawing.Size(729, 478);
			this.TableLayoutPanel.TabIndex = 2;
			// 
			// pictureBox1
			// 
			this.PlayPausePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PlayPausePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PlayPausePictureBox.Image = global::Alloclave.Properties.Resources.pause;
			this.PlayPausePictureBox.Location = new System.Drawing.Point(679, 448);
			this.PlayPausePictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.PlayPausePictureBox.Name = "pictureBox1";
			this.PlayPausePictureBox.Size = new System.Drawing.Size(47, 27);
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
		public Scrubber MainScrubber;
		private System.Windows.Forms.PictureBox PlayPausePictureBox;

	}
}
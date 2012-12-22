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
			this.scrubber1 = new Alloclave.Scrubber();
			this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.addressSpaceScroller = new Alloclave.AddressSpaceScroller();
			this.TableLayoutPanel.SuspendLayout();
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
			// scrubber1
			// 
			this.scrubber1.AutoSize = true;
			this.scrubber1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.scrubber1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrubber1.Location = new System.Drawing.Point(6, 448);
			this.scrubber1.Name = "scrubber1";
			this.scrubber1.Size = new System.Drawing.Size(667, 24);
			this.scrubber1.TabIndex = 1;
			// 
			// TableLayoutPanel
			// 
			this.TableLayoutPanel.ColumnCount = 3;
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.TableLayoutPanel.Controls.Add(this.AddressSpaceControl, 0, 0);
			this.TableLayoutPanel.Controls.Add(this.scrubber1, 0, 1);
			this.TableLayoutPanel.Controls.Add(this.addressSpaceScroller, 2, 0);
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
			// addressSpaceScroller
			// 
			this.addressSpaceScroller.Dock = System.Windows.Forms.DockStyle.Fill;
			this.addressSpaceScroller.Location = new System.Drawing.Point(679, 6);
			this.addressSpaceScroller.Name = "addressSpaceScroller";
			this.addressSpaceScroller.Size = new System.Drawing.Size(44, 436);
			this.addressSpaceScroller.TabIndex = 5;
			// 
			// AllocationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(729, 478);
			this.Controls.Add(this.TableLayoutPanel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "AllocationForm";
			this.Text = "Allocations";
			this.TableLayoutPanel.ResumeLayout(false);
			this.TableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public AddressSpace AddressSpaceControl;
		private Scrubber scrubber1;
		private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
		private AddressSpaceScroller addressSpaceScroller;

	}
}
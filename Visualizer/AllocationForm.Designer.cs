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
			this.XAxisControl = new Alloclave.AxisControl();
			this.YAxisControl = new Alloclave.AxisControl();
			this.OptionsPanel = new System.Windows.Forms.Panel();
			this.ModeComboBox = new System.Windows.Forms.ComboBox();
			this.ModeLabel = new System.Windows.Forms.Label();
			this.TableLayoutPanel.SuspendLayout();
			this.OptionsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// AddressSpaceControl
			// 
			this.AddressSpaceControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AddressSpaceControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddressSpaceControl.Location = new System.Drawing.Point(6, 26);
			this.AddressSpaceControl.Name = "AddressSpaceControl";
			this.AddressSpaceControl.Size = new System.Drawing.Size(697, 342);
			this.AddressSpaceControl.TabIndex = 0;
			// 
			// scrubber1
			// 
			this.scrubber1.AutoSize = true;
			this.scrubber1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanel.SetColumnSpan(this.scrubber1, 2);
			this.scrubber1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrubber1.Location = new System.Drawing.Point(6, 374);
			this.scrubber1.Name = "scrubber1";
			this.scrubber1.Size = new System.Drawing.Size(717, 17);
			this.scrubber1.TabIndex = 1;
			// 
			// TableLayoutPanel
			// 
			this.TableLayoutPanel.ColumnCount = 2;
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableLayoutPanel.Controls.Add(this.AddressSpaceControl, 0, 1);
			this.TableLayoutPanel.Controls.Add(this.scrubber1, 0, 2);
			this.TableLayoutPanel.Controls.Add(this.XAxisControl, 0, 0);
			this.TableLayoutPanel.Controls.Add(this.YAxisControl, 1, 1);
			this.TableLayoutPanel.Controls.Add(this.OptionsPanel, 0, 3);
			this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.TableLayoutPanel.Name = "TableLayoutPanel";
			this.TableLayoutPanel.Padding = new System.Windows.Forms.Padding(3);
			this.TableLayoutPanel.RowCount = 4;
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.70277F));
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.29723F));
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.TableLayoutPanel.Size = new System.Drawing.Size(729, 478);
			this.TableLayoutPanel.TabIndex = 2;
			// 
			// XAxisControl
			// 
			this.XAxisControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.XAxisControl.Location = new System.Drawing.Point(6, 6);
			this.XAxisControl.Name = "XAxisControl";
			this.XAxisControl.Size = new System.Drawing.Size(697, 14);
			this.XAxisControl.TabIndex = 2;
			// 
			// YAxisControl
			// 
			this.YAxisControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YAxisControl.Location = new System.Drawing.Point(709, 26);
			this.YAxisControl.Name = "YAxisControl";
			this.YAxisControl.Size = new System.Drawing.Size(14, 342);
			this.YAxisControl.TabIndex = 3;
			// 
			// OptionsPanel
			// 
			this.OptionsPanel.Controls.Add(this.ModeComboBox);
			this.OptionsPanel.Controls.Add(this.ModeLabel);
			this.OptionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OptionsPanel.Location = new System.Drawing.Point(6, 397);
			this.OptionsPanel.Name = "OptionsPanel";
			this.OptionsPanel.Size = new System.Drawing.Size(697, 75);
			this.OptionsPanel.TabIndex = 4;
			// 
			// ModeComboBox
			// 
			this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ModeComboBox.FormattingEnabled = true;
			this.ModeComboBox.Items.AddRange(new object[] {
            "Allocations",
            "Frees"});
			this.ModeComboBox.Location = new System.Drawing.Point(63, 8);
			this.ModeComboBox.Name = "ModeComboBox";
			this.ModeComboBox.Size = new System.Drawing.Size(103, 21);
			this.ModeComboBox.TabIndex = 1;
			// 
			// ModeLabel
			// 
			this.ModeLabel.AutoSize = true;
			this.ModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ModeLabel.Location = new System.Drawing.Point(15, 11);
			this.ModeLabel.Name = "ModeLabel";
			this.ModeLabel.Size = new System.Drawing.Size(42, 13);
			this.ModeLabel.TabIndex = 0;
			this.ModeLabel.Text = "Mode:";
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
			this.OptionsPanel.ResumeLayout(false);
			this.OptionsPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public AddressSpace AddressSpaceControl;
		private Scrubber scrubber1;
		private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
		private AxisControl XAxisControl;
		private AxisControl YAxisControl;
		private System.Windows.Forms.Panel OptionsPanel;
		private System.Windows.Forms.ComboBox ModeComboBox;
		private System.Windows.Forms.Label ModeLabel;
	}
}
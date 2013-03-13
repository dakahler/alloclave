namespace Alloclave
{
	partial class InfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
			this.SizeLabel = new System.Windows.Forms.Label();
			this.AddressLabel = new System.Windows.Forms.Label();
			this.HeapLabel = new System.Windows.Forms.Label();
			this.StackComboBox = new System.Windows.Forms.ComboBox();
			this.StackLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// SizeLabel
			// 
			this.SizeLabel.AutoSize = true;
			this.SizeLabel.Location = new System.Drawing.Point(35, 46);
			this.SizeLabel.Name = "SizeLabel";
			this.SizeLabel.Size = new System.Drawing.Size(30, 13);
			this.SizeLabel.TabIndex = 1;
			this.SizeLabel.Text = "Size:";
			// 
			// AddressLabel
			// 
			this.AddressLabel.AutoSize = true;
			this.AddressLabel.Location = new System.Drawing.Point(35, 19);
			this.AddressLabel.Name = "AddressLabel";
			this.AddressLabel.Size = new System.Drawing.Size(48, 13);
			this.AddressLabel.TabIndex = 0;
			this.AddressLabel.Text = "Address:";
			// 
			// HeapLabel
			// 
			this.HeapLabel.AutoSize = true;
			this.HeapLabel.Location = new System.Drawing.Point(35, 75);
			this.HeapLabel.Name = "HeapLabel";
			this.HeapLabel.Size = new System.Drawing.Size(36, 13);
			this.HeapLabel.TabIndex = 2;
			this.HeapLabel.Text = "Heap:";
			// 
			// StackComboBox
			// 
			this.StackComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.StackComboBox.FormattingEnabled = true;
			this.StackComboBox.Location = new System.Drawing.Point(91, 100);
			this.StackComboBox.Name = "StackComboBox";
			this.StackComboBox.Size = new System.Drawing.Size(249, 21);
			this.StackComboBox.TabIndex = 3;
			// 
			// StackLabel
			// 
			this.StackLabel.AutoSize = true;
			this.StackLabel.Location = new System.Drawing.Point(36, 103);
			this.StackLabel.Name = "StackLabel";
			this.StackLabel.Size = new System.Drawing.Size(38, 13);
			this.StackLabel.TabIndex = 4;
			this.StackLabel.Text = "Stack:";
			// 
			// InfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(767, 228);
			this.Controls.Add(this.StackLabel);
			this.Controls.Add(this.StackComboBox);
			this.Controls.Add(this.HeapLabel);
			this.Controls.Add(this.SizeLabel);
			this.Controls.Add(this.AddressLabel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "InfoForm";
			this.ShowInTaskbar = false;
			this.Text = "Info";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Label AddressLabel;
		public System.Windows.Forms.Label SizeLabel;
		private System.Windows.Forms.Label HeapLabel;
		private System.Windows.Forms.ComboBox StackComboBox;
		private System.Windows.Forms.Label StackLabel;

	}
}
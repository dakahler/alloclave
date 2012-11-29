namespace Alloclave
{
	partial class Licensing
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox LicenseTextBox;
		private System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.Button BuyButton;
		private System.Windows.Forms.Button TryButton;
		private System.Windows.Forms.Panel LicensePanel;
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
			this.label1 = new System.Windows.Forms.Label();
			this.LicenseTextBox = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.BuyButton = new System.Windows.Forms.Button();
			this.TryButton = new System.Windows.Forms.Button();
			this.LicensePanel = new System.Windows.Forms.Panel();
			this.LicensePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(595, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Welcome to Alloclave! Please paste in your entire license key to continue:";
			// 
			// LicenseTextBox
			// 
			this.LicenseTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.LicenseTextBox.Location = new System.Drawing.Point(7, 8);
			this.LicenseTextBox.Multiline = true;
			this.LicenseTextBox.Name = "LicenseTextBox";
			this.LicenseTextBox.Size = new System.Drawing.Size(578, 97);
			this.LicenseTextBox.TabIndex = 1;
			this.LicenseTextBox.TextChanged += new System.EventHandler(this.LicenseTextBox_TextChanged);
			// 
			// OkButton
			// 
			this.OkButton.Enabled = false;
			this.OkButton.Location = new System.Drawing.Point(518, 179);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(75, 23);
			this.OkButton.TabIndex = 2;
			this.OkButton.Text = "OK";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// BuyButton
			// 
			this.BuyButton.Location = new System.Drawing.Point(423, 179);
			this.BuyButton.Name = "BuyButton";
			this.BuyButton.Size = new System.Drawing.Size(75, 23);
			this.BuyButton.TabIndex = 3;
			this.BuyButton.Text = "Buy";
			this.BuyButton.UseVisualStyleBackColor = true;
			this.BuyButton.Click += new System.EventHandler(this.BuyButton_Click);
			// 
			// TryButton
			// 
			this.TryButton.Location = new System.Drawing.Point(327, 179);
			this.TryButton.Name = "TryButton";
			this.TryButton.Size = new System.Drawing.Size(75, 23);
			this.TryButton.TabIndex = 4;
			this.TryButton.Text = "Try";
			this.TryButton.UseVisualStyleBackColor = true;
			this.TryButton.Click += new System.EventHandler(this.TryButton_Click);
			// 
			// LicensePanel
			// 
			this.LicensePanel.BackColor = System.Drawing.Color.Salmon;
			this.LicensePanel.Controls.Add(this.LicenseTextBox);
			this.LicensePanel.Location = new System.Drawing.Point(16, 49);
			this.LicensePanel.Name = "LicensePanel";
			this.LicensePanel.Size = new System.Drawing.Size(591, 113);
			this.LicensePanel.TabIndex = 5;
			// 
			// Licensing
			// 
			this.ClientSize = new System.Drawing.Size(642, 216);
			this.ControlBox = false;
			this.Controls.Add(this.LicensePanel);
			this.Controls.Add(this.TryButton);
			this.Controls.Add(this.BuyButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Licensing";
			this.Text = "Alloclave";
			this.LicensePanel.ResumeLayout(false);
			this.LicensePanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}
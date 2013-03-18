namespace Alloclave
{
	partial class UpdateForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.CurrentVersionLabel = new System.Windows.Forms.Label();
			this.NewVersionLabel = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ChangesTextBox = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(37, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(217, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "An update is available!";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(48, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Current Version:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(48, 83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "New Version:";
			// 
			// CurrentVersionLabel
			// 
			this.CurrentVersionLabel.AutoSize = true;
			this.CurrentVersionLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CurrentVersionLabel.Location = new System.Drawing.Point(165, 61);
			this.CurrentVersionLabel.Name = "CurrentVersionLabel";
			this.CurrentVersionLabel.Size = new System.Drawing.Size(48, 16);
			this.CurrentVersionLabel.TabIndex = 3;
			this.CurrentVersionLabel.Text = "1.0.0.0";
			// 
			// NewVersionLabel
			// 
			this.NewVersionLabel.AutoSize = true;
			this.NewVersionLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NewVersionLabel.Location = new System.Drawing.Point(165, 83);
			this.NewVersionLabel.Name = "NewVersionLabel";
			this.NewVersionLabel.Size = new System.Drawing.Size(52, 16);
			this.NewVersionLabel.TabIndex = 4;
			this.NewVersionLabel.Text = "1.1.0.0";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(48, 122);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(75, 18);
			this.label4.TabIndex = 5;
			this.label4.Text = "Changes:";
			// 
			// ChangesTextBox
			// 
			this.ChangesTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.ChangesTextBox.Location = new System.Drawing.Point(41, 143);
			this.ChangesTextBox.Multiline = true;
			this.ChangesTextBox.Name = "ChangesTextBox";
			this.ChangesTextBox.ReadOnly = true;
			this.ChangesTextBox.Size = new System.Drawing.Size(380, 178);
			this.ChangesTextBox.TabIndex = 6;
			// 
			// OkButton
			// 
			this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OkButton.Location = new System.Drawing.Point(217, 336);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(98, 23);
			this.OkButton.TabIndex = 7;
			this.OkButton.Text = "Update Now";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// CancelButton
			// 
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.Location = new System.Drawing.Point(331, 336);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(75, 23);
			this.CancelButton.TabIndex = 8;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// UpdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelButton;
			this.ClientSize = new System.Drawing.Size(454, 371);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.ChangesTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.NewVersionLabel);
			this.Controls.Add(this.CurrentVersionLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Update Alloclave";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label CurrentVersionLabel;
		private System.Windows.Forms.Label NewVersionLabel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox ChangesTextBox;
		private System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.Button CancelButton;
	}
}
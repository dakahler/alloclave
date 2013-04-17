namespace Alloclave_Plugin
{
	partial class ProcessForm
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
            this._CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.ProcessComboBox = new System.Windows.Forms.ComboBox();
            this.ChooseTransportTypeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _CancelButton
            // 
            this._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._CancelButton.Location = new System.Drawing.Point(444, 103);
            this._CancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._CancelButton.Name = "_CancelButton";
            this._CancelButton.Size = new System.Drawing.Size(100, 28);
            this._CancelButton.TabIndex = 7;
            this._CancelButton.Text = "Cancel";
            this._CancelButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(323, 103);
            this.OkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 28);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // ProcessComboBox
            // 
            this.ProcessComboBox.DropDownHeight = 300;
            this.ProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessComboBox.FormattingEnabled = true;
            this.ProcessComboBox.IntegralHeight = false;
            this.ProcessComboBox.Location = new System.Drawing.Point(229, 27);
            this.ProcessComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ProcessComboBox.MaxDropDownItems = 16;
            this.ProcessComboBox.Name = "ProcessComboBox";
            this.ProcessComboBox.Size = new System.Drawing.Size(328, 24);
            this.ProcessComboBox.Sorted = true;
            this.ProcessComboBox.TabIndex = 5;
            // 
            // ChooseTransportTypeLabel
            // 
            this.ChooseTransportTypeLabel.AutoSize = true;
            this.ChooseTransportTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseTransportTypeLabel.Location = new System.Drawing.Point(31, 27);
            this.ChooseTransportTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ChooseTransportTypeLabel.Name = "ChooseTransportTypeLabel";
            this.ChooseTransportTypeLabel.Size = new System.Drawing.Size(151, 20);
            this.ChooseTransportTypeLabel.TabIndex = 4;
            this.ChooseTransportTypeLabel.Text = "Choose process:";
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 166);
            this.ControlBox = false;
            this.Controls.Add(this._CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ProcessComboBox);
            this.Controls.Add(this.ChooseTransportTypeLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Process";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _CancelButton;
		private System.Windows.Forms.Button OkButton;
		public System.Windows.Forms.ComboBox ProcessComboBox;
		private System.Windows.Forms.Label ChooseTransportTypeLabel;
	}
}
namespace Alloclave
{
	partial class NewForm
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
            this.ChooseTransportTypeLabel = new System.Windows.Forms.Label();
            this.TransportComboBox = new System.Windows.Forms.ComboBox();
            this.OkButton = new System.Windows.Forms.Button();
            this._CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChooseTransportTypeLabel
            // 
            this.ChooseTransportTypeLabel.AutoSize = true;
            this.ChooseTransportTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseTransportTypeLabel.Location = new System.Drawing.Point(44, 36);
            this.ChooseTransportTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ChooseTransportTypeLabel.Name = "ChooseTransportTypeLabel";
            this.ChooseTransportTypeLabel.Size = new System.Drawing.Size(201, 20);
            this.ChooseTransportTypeLabel.TabIndex = 0;
            this.ChooseTransportTypeLabel.Text = "Choose transport type:";
            // 
            // TransportComboBox
            // 
            this.TransportComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TransportComboBox.FormattingEnabled = true;
            this.TransportComboBox.Location = new System.Drawing.Point(299, 36);
            this.TransportComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.TransportComboBox.Name = "TransportComboBox";
            this.TransportComboBox.Size = new System.Drawing.Size(328, 24);
            this.TransportComboBox.TabIndex = 1;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(392, 112);
            this.OkButton.Margin = new System.Windows.Forms.Padding(4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 28);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _CancelButton
            // 
            this._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._CancelButton.Location = new System.Drawing.Point(515, 112);
            this._CancelButton.Margin = new System.Windows.Forms.Padding(4);
            this._CancelButton.Name = "_CancelButton";
            this._CancelButton.Size = new System.Drawing.Size(100, 28);
            this._CancelButton.TabIndex = 3;
            this._CancelButton.Text = "Cancel";
            this._CancelButton.UseVisualStyleBackColor = true;
            this._CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this._CancelButton;
            this.ClientSize = new System.Drawing.Size(668, 169);
            this.ControlBox = false;
            this.Controls.Add(this._CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.TransportComboBox);
            this.Controls.Add(this.ChooseTransportTypeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label ChooseTransportTypeLabel;
		public System.Windows.Forms.ComboBox TransportComboBox;
		private System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.Button _CancelButton;
	}
}
namespace Alloclave
{
	partial class Main
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this._DockPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// _DockPanel
			// 
			this._DockPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this._DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._DockPanel.ForeColor = System.Drawing.Color.White;
			this._DockPanel.Location = new System.Drawing.Point(0, 0);
			this._DockPanel.Name = "_DockPanel";
			this._DockPanel.Size = new System.Drawing.Size(1017, 726);
			this._DockPanel.TabIndex = 2;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(1017, 726);
			this.Controls.Add(this._DockPanel);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.White;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Main";
			this.Text = "Alloclave";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel _DockPanel;
	}
}


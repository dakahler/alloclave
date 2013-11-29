namespace Alloclave
{
	partial class AddressSpace
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.NoDataPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// NoDataPanel
			// 
			this.NoDataPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
			this.NoDataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NoDataPanel.Location = new System.Drawing.Point(0, 0);
			this.NoDataPanel.Name = "NoDataPanel";
			this.NoDataPanel.Size = new System.Drawing.Size(1348, 756);
			this.NoDataPanel.TabIndex = 0;
			// 
			// AddressSpace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.NoDataPanel);
			this.Margin = new System.Windows.Forms.Padding(6);
			this.Name = "AddressSpace";
			this.Size = new System.Drawing.Size(1348, 756);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AddressSpace_MouseDown);
			this.MouseLeave += new System.EventHandler(this.AddressSpace_MouseLeave);
			this.MouseHover += new System.EventHandler(this.AddressSpace_MouseHover);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AddressSpace_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AddressSpace_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel NoDataPanel;



		//private RichToolTip Tooltip;


	}
}

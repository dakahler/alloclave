namespace Alloclave
{
	partial class AddressSpace
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			//this.Tooltip = new Alloclave.RichToolTip();
			this.SuspendLayout();
			// 
			// Tooltip
			// 
			//this.Tooltip.OwnerDraw = true;
			//this.Tooltip.ShowAlways = true;
			// 
			// AddressSpace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Name = "AddressSpace";
			this.Size = new System.Drawing.Size(674, 393);
			this.SizeChanged += new System.EventHandler(this.AddressSpace_SizeChanged);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AddressSpace_MouseDown);
			this.MouseLeave += new System.EventHandler(this.AddressSpace_MouseLeave);
			this.MouseHover += new System.EventHandler(this.AddressSpace_MouseHover);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AddressSpace_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AddressSpace_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion

		//private RichToolTip Tooltip;


	}
}

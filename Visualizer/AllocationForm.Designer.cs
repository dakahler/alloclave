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
			this.SuspendLayout();
			// 
			// AddressSpaceControl
			// 
			this.AddressSpaceControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AddressSpaceControl.Location = new System.Drawing.Point(25, 12);
			this.AddressSpaceControl.Name = "AddressSpaceControl";
			this.AddressSpaceControl.Size = new System.Drawing.Size(674, 393);
			this.AddressSpaceControl.TabIndex = 0;
			// 
			// AllocationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(729, 478);
			this.Controls.Add(this.AddressSpaceControl);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "AllocationForm";
			this.Text = "Allocations";
			this.ResumeLayout(false);

		}

		#endregion

		public AddressSpace AddressSpaceControl;
	}
}
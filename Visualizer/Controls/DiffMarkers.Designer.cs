namespace Alloclave
{
    partial class DiffMarkers
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
            this.Diff1 = new System.Windows.Forms.PictureBox();
            this.Diff2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Diff1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Diff2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Diff1
            // 
            this.Diff1.Image = global::Alloclave.Properties.Resources.Diff1;
            this.Diff1.Location = new System.Drawing.Point(71, 15);
            this.Diff1.Name = "Diff1";
            this.Diff1.Size = new System.Drawing.Size(15, 10);
            this.Diff1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Diff1.TabIndex = 0;
            this.Diff1.TabStop = false;
            // 
            // Diff2
            // 
            this.Diff2.Image = global::Alloclave.Properties.Resources.Diff2;
            this.Diff2.Location = new System.Drawing.Point(147, 15);
            this.Diff2.Name = "Diff2";
            this.Diff2.Size = new System.Drawing.Size(15, 10);
            this.Diff2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Diff2.TabIndex = 1;
            this.Diff2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Diff2);
            this.panel1.Controls.Add(this.Diff1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 66);
            this.panel1.TabIndex = 2;
            // 
            // DiffMarkers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.panel1);
            this.Name = "DiffMarkers";
            this.Size = new System.Drawing.Size(556, 66);
            ((System.ComponentModel.ISupportInitialize)(this.Diff1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Diff2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Diff1;
        private System.Windows.Forms.PictureBox Diff2;
        private System.Windows.Forms.Panel panel1;
    }
}

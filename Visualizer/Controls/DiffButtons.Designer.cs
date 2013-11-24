namespace Alloclave
{
    partial class DiffButtons
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.DifferenceLabel = new System.Windows.Forms.Label();
			this.StartLabel = new System.Windows.Forms.Label();
			this.EndLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1335, 64);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.StartLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(333, 64);
			this.panel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.DifferenceLabel);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(333, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(667, 64);
			this.panel2.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.EndLabel);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(1000, 0);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(335, 64);
			this.panel3.TabIndex = 2;
			// 
			// DifferenceLabel
			// 
			this.DifferenceLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(157)))), ((int)(((byte)(103)))));
			this.DifferenceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DifferenceLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DifferenceLabel.Location = new System.Drawing.Point(0, 0);
			this.DifferenceLabel.Margin = new System.Windows.Forms.Padding(0);
			this.DifferenceLabel.Name = "DifferenceLabel";
			this.DifferenceLabel.Size = new System.Drawing.Size(667, 64);
			this.DifferenceLabel.TabIndex = 0;
			this.DifferenceLabel.Text = "Difference";
			this.DifferenceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DifferenceLabel.Click += new System.EventHandler(this.DifferenceLabel_Click);
			// 
			// StartLabel
			// 
			this.StartLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(208)))), ((int)(((byte)(177)))));
			this.StartLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StartLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartLabel.ForeColor = System.Drawing.Color.Black;
			this.StartLabel.Location = new System.Drawing.Point(0, 0);
			this.StartLabel.Margin = new System.Windows.Forms.Padding(0);
			this.StartLabel.Name = "StartLabel";
			this.StartLabel.Size = new System.Drawing.Size(333, 64);
			this.StartLabel.TabIndex = 0;
			this.StartLabel.Text = "Start";
			this.StartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.StartLabel.Click += new System.EventHandler(this.StartLabel_Click);
			// 
			// EndLabel
			// 
			this.EndLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(208)))), ((int)(((byte)(177)))));
			this.EndLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EndLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EndLabel.ForeColor = System.Drawing.Color.Black;
			this.EndLabel.Location = new System.Drawing.Point(0, 0);
			this.EndLabel.Margin = new System.Windows.Forms.Padding(0);
			this.EndLabel.Name = "EndLabel";
			this.EndLabel.Size = new System.Drawing.Size(335, 64);
			this.EndLabel.TabIndex = 1;
			this.EndLabel.Text = "End";
			this.EndLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.EndLabel.Click += new System.EventHandler(this.EndLabel_Click);
			// 
			// DiffButtons
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "DiffButtons";
			this.Size = new System.Drawing.Size(1335, 64);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		public System.Windows.Forms.Label StartLabel;
		public System.Windows.Forms.Label DifferenceLabel;
		public System.Windows.Forms.Label EndLabel;
    }
}

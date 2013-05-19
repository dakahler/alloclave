namespace Alloclave
{
	partial class Tour
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tour));
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.AlloclaveLabel = new System.Windows.Forms.Label();
			this.AdvanceButton = new System.Windows.Forms.Button();
			this.MainTextBox = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.QuickStartLabel = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(378, 726);
			this.panel1.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.43251F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.61433F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(378, 726);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.panel3.Controls.Add(this.AlloclaveLabel);
			this.panel3.Controls.Add(this.AdvanceButton);
			this.panel3.Controls.Add(this.MainTextBox);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(10, 157);
			this.panel3.Margin = new System.Windows.Forms.Padding(10);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(358, 559);
			this.panel3.TabIndex = 1;
			// 
			// AlloclaveLabel
			// 
			this.AlloclaveLabel.AutoSize = true;
			this.AlloclaveLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.AlloclaveLabel.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AlloclaveLabel.ForeColor = System.Drawing.Color.LightSteelBlue;
			this.AlloclaveLabel.Location = new System.Drawing.Point(64, 448);
			this.AlloclaveLabel.Name = "AlloclaveLabel";
			this.AlloclaveLabel.Size = new System.Drawing.Size(231, 24);
			this.AlloclaveLabel.TabIndex = 5;
			this.AlloclaveLabel.Text = "www.alloclave.com";
			this.AlloclaveLabel.Visible = false;
			this.AlloclaveLabel.Click += new System.EventHandler(this.AlloclaveLabel_Click);
			// 
			// AdvanceButton
			// 
			this.AdvanceButton.BackColor = System.Drawing.Color.Lavender;
			this.AdvanceButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.AdvanceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.AdvanceButton.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AdvanceButton.ForeColor = System.Drawing.Color.Black;
			this.AdvanceButton.Location = new System.Drawing.Point(13, 488);
			this.AdvanceButton.Name = "AdvanceButton";
			this.AdvanceButton.Size = new System.Drawing.Size(333, 56);
			this.AdvanceButton.TabIndex = 1;
			this.AdvanceButton.Text = "Start The Tour";
			this.AdvanceButton.UseVisualStyleBackColor = false;
			this.AdvanceButton.Click += new System.EventHandler(this.AdvanceButton_Click);
			// 
			// MainTextBox
			// 
			this.MainTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.MainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MainTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.MainTextBox.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainTextBox.ForeColor = System.Drawing.Color.White;
			this.MainTextBox.Location = new System.Drawing.Point(13, 13);
			this.MainTextBox.Multiline = true;
			this.MainTextBox.Name = "MainTextBox";
			this.MainTextBox.ReadOnly = true;
			this.MainTextBox.Size = new System.Drawing.Size(333, 353);
			this.MainTextBox.TabIndex = 0;
			this.MainTextBox.TabStop = false;
			this.MainTextBox.Text = resources.GetString("MainTextBox.Text");
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(10, 10);
			this.panel2.Margin = new System.Windows.Forms.Padding(10);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(358, 45);
			this.panel2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(9, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(340, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Welcome to the Alloclave Tour!";
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.panel4.Controls.Add(this.QuickStartLabel);
			this.panel4.Controls.Add(this.label4);
			this.panel4.Controls.Add(this.label5);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(10, 75);
			this.panel4.Margin = new System.Windows.Forms.Padding(10);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(358, 62);
			this.panel4.TabIndex = 2;
			// 
			// QuickStartLabel
			// 
			this.QuickStartLabel.AutoSize = true;
			this.QuickStartLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.QuickStartLabel.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.QuickStartLabel.ForeColor = System.Drawing.Color.LightSteelBlue;
			this.QuickStartLabel.Location = new System.Drawing.Point(127, 33);
			this.QuickStartLabel.Name = "QuickStartLabel";
			this.QuickStartLabel.Size = new System.Drawing.Size(144, 17);
			this.QuickStartLabel.TabIndex = 4;
			this.QuickStartLabel.Text = "Quick Start Guide";
			this.QuickStartLabel.Click += new System.EventHandler(this.QuickStartLabel_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(14, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 17);
			this.label4.TabIndex = 3;
			this.label4.Text = "Check out the";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(9, 12);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(252, 21);
			this.label5.TabIndex = 2;
			this.label5.Text = "Need More Information?";
			// 
			// Tour
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 726);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.Name = "Tour";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Alloclave Tour";
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label QuickStartLabel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox MainTextBox;
		private System.Windows.Forms.Button AdvanceButton;
		private System.Windows.Forms.Label AlloclaveLabel;
	}
}
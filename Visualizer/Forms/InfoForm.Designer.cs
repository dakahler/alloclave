namespace Alloclave
{
	partial class InfoForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.symbolsNotFoundLabel = new System.Windows.Forms.LinkLabel();
			this.AddressLabel = new System.Windows.Forms.Label();
			this.HeapLabel = new System.Windows.Forms.Label();
			this.SizeLabel = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.StackTable = new System.Windows.Forms.DataGridView();
			this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.StackTable)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(528, 194);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.symbolsNotFoundLabel);
			this.panel1.Controls.Add(this.AddressLabel);
			this.panel1.Controls.Add(this.HeapLabel);
			this.panel1.Controls.Add(this.SizeLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(522, 24);
			this.panel1.TabIndex = 5;
			// 
			// symbolsNotFoundLabel
			// 
			this.symbolsNotFoundLabel.AutoSize = true;
			this.symbolsNotFoundLabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.symbolsNotFoundLabel.LinkColor = System.Drawing.Color.DarkRed;
			this.symbolsNotFoundLabel.Location = new System.Drawing.Point(396, 4);
			this.symbolsNotFoundLabel.Name = "symbolsNotFoundLabel";
			this.symbolsNotFoundLabel.Size = new System.Drawing.Size(116, 14);
			this.symbolsNotFoundLabel.TabIndex = 3;
			this.symbolsNotFoundLabel.TabStop = true;
			this.symbolsNotFoundLabel.Text = "Symbols Not Found!";
			this.symbolsNotFoundLabel.Visible = false;
			this.symbolsNotFoundLabel.Click += new System.EventHandler(this.symbolsNotFoundLabel_Click);
			// 
			// AddressLabel
			// 
			this.AddressLabel.AutoSize = true;
			this.AddressLabel.Location = new System.Drawing.Point(7, 5);
			this.AddressLabel.Name = "AddressLabel";
			this.AddressLabel.Size = new System.Drawing.Size(48, 13);
			this.AddressLabel.TabIndex = 0;
			this.AddressLabel.Text = "Address:";
			// 
			// HeapLabel
			// 
			this.HeapLabel.AutoSize = true;
			this.HeapLabel.Location = new System.Drawing.Point(280, 5);
			this.HeapLabel.Name = "HeapLabel";
			this.HeapLabel.Size = new System.Drawing.Size(36, 13);
			this.HeapLabel.TabIndex = 2;
			this.HeapLabel.Text = "Heap:";
			// 
			// SizeLabel
			// 
			this.SizeLabel.AutoSize = true;
			this.SizeLabel.Location = new System.Drawing.Point(178, 5);
			this.SizeLabel.Name = "SizeLabel";
			this.SizeLabel.Size = new System.Drawing.Size(30, 13);
			this.SizeLabel.TabIndex = 1;
			this.SizeLabel.Text = "Size:";
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.Controls.Add(this.StackTable);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 33);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(522, 158);
			this.panel2.TabIndex = 6;
			// 
			// StackTable
			// 
			this.StackTable.AllowUserToAddRows = false;
			this.StackTable.AllowUserToDeleteRows = false;
			this.StackTable.AllowUserToResizeRows = false;
			this.StackTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.StackTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.StackTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.StackTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.StackTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Symbol});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.StackTable.DefaultCellStyle = dataGridViewCellStyle1;
			this.StackTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StackTable.Location = new System.Drawing.Point(0, 0);
			this.StackTable.Name = "StackTable";
			this.StackTable.ReadOnly = true;
			this.StackTable.RowHeadersVisible = false;
			this.StackTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.StackTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.StackTable.ShowEditingIcon = false;
			this.StackTable.Size = new System.Drawing.Size(522, 158);
			this.StackTable.TabIndex = 4;
			// 
			// Address
			// 
			this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Address.FillWeight = 25F;
			this.Address.HeaderText = "Address";
			this.Address.Name = "Address";
			this.Address.ReadOnly = true;
			// 
			// Symbol
			// 
			this.Symbol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Symbol.FillWeight = 75F;
			this.Symbol.HeaderText = "Symbol";
			this.Symbol.Name = "Symbol";
			this.Symbol.ReadOnly = true;
			// 
			// InfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(528, 194);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "InfoForm";
			this.ShowInTaskbar = false;
			this.Text = "Info";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.StackTable)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Label AddressLabel;
		public System.Windows.Forms.Label SizeLabel;
		private System.Windows.Forms.Label HeapLabel;
		private System.Windows.Forms.DataGridView StackTable;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Address;
		private System.Windows.Forms.DataGridViewTextBoxColumn Symbol;
		private System.Windows.Forms.LinkLabel symbolsNotFoundLabel;

	}
}
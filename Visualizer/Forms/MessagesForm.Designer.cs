namespace Alloclave
{
	partial class MessagesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessagesForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.ErrorsDataGrid = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.WarningsDataGrid = new System.Windows.Forms.DataGridView();
			this.InfosDataGrid = new System.Windows.Forms.DataGridView();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ErrorsDataGrid)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.WarningsDataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InfosDataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(943, 258);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.ErrorsDataGrid);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(935, 229);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Errors";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// ErrorsDataGrid
			// 
			this.ErrorsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.ErrorsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ErrorsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ErrorsDataGrid.Location = new System.Drawing.Point(3, 3);
			this.ErrorsDataGrid.Name = "ErrorsDataGrid";
			this.ErrorsDataGrid.RowHeadersVisible = false;
			this.ErrorsDataGrid.Size = new System.Drawing.Size(929, 223);
			this.ErrorsDataGrid.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.WarningsDataGrid);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(935, 229);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Warnings";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.InfosDataGrid);
			this.tabPage3.Location = new System.Drawing.Point(4, 25);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(935, 229);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Info";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// WarningsDataGrid
			// 
			this.WarningsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.WarningsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.WarningsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WarningsDataGrid.Location = new System.Drawing.Point(3, 3);
			this.WarningsDataGrid.Name = "WarningsDataGrid";
			this.WarningsDataGrid.RowHeadersVisible = false;
			this.WarningsDataGrid.Size = new System.Drawing.Size(929, 223);
			this.WarningsDataGrid.TabIndex = 1;
			// 
			// InfosDataGrid
			// 
			this.InfosDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.InfosDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.InfosDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfosDataGrid.Location = new System.Drawing.Point(0, 0);
			this.InfosDataGrid.Name = "InfosDataGrid";
			this.InfosDataGrid.RowHeadersVisible = false;
			this.InfosDataGrid.Size = new System.Drawing.Size(935, 229);
			this.InfosDataGrid.TabIndex = 1;
			// 
			// MessagesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(943, 258);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MessagesForm";
			this.ShowInTaskbar = false;
			this.Text = "Error List";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ErrorsDataGrid)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.WarningsDataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InfosDataGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.DataGridView ErrorsDataGrid;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.DataGridView WarningsDataGrid;
		private System.Windows.Forms.DataGridView InfosDataGrid;
	}
}
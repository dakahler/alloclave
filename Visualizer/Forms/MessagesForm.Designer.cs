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
			this.ErrorsTabPage = new System.Windows.Forms.TabPage();
			this.ErrorsDataGrid = new System.Windows.Forms.DataGridView();
			this.WarningsTabPage = new System.Windows.Forms.TabPage();
			this.WarningsDataGrid = new System.Windows.Forms.DataGridView();
			this.InfoTabPage = new System.Windows.Forms.TabPage();
			this.InfosDataGrid = new System.Windows.Forms.DataGridView();
			this.tabControl1.SuspendLayout();
			this.ErrorsTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ErrorsDataGrid)).BeginInit();
			this.WarningsTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.WarningsDataGrid)).BeginInit();
			this.InfoTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.InfosDataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControl1.Controls.Add(this.ErrorsTabPage);
			this.tabControl1.Controls.Add(this.WarningsTabPage);
			this.tabControl1.Controls.Add(this.InfoTabPage);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(943, 258);
			this.tabControl1.TabIndex = 0;
			// 
			// ErrorsTabPage
			// 
			this.ErrorsTabPage.Controls.Add(this.ErrorsDataGrid);
			this.ErrorsTabPage.Location = new System.Drawing.Point(4, 25);
			this.ErrorsTabPage.Name = "ErrorsTabPage";
			this.ErrorsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.ErrorsTabPage.Size = new System.Drawing.Size(935, 229);
			this.ErrorsTabPage.TabIndex = 0;
			this.ErrorsTabPage.Text = "Errors";
			this.ErrorsTabPage.UseVisualStyleBackColor = true;
			// 
			// ErrorsDataGrid
			// 
			this.ErrorsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.ErrorsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ErrorsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ErrorsDataGrid.Location = new System.Drawing.Point(3, 3);
			this.ErrorsDataGrid.Name = "ErrorsDataGrid";
			this.ErrorsDataGrid.ReadOnly = true;
			this.ErrorsDataGrid.RowHeadersVisible = false;
			this.ErrorsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ErrorsDataGrid.Size = new System.Drawing.Size(929, 223);
			this.ErrorsDataGrid.TabIndex = 0;
			this.ErrorsDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
			// 
			// WarningsTabPage
			// 
			this.WarningsTabPage.Controls.Add(this.WarningsDataGrid);
			this.WarningsTabPage.Location = new System.Drawing.Point(4, 25);
			this.WarningsTabPage.Name = "WarningsTabPage";
			this.WarningsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.WarningsTabPage.Size = new System.Drawing.Size(935, 229);
			this.WarningsTabPage.TabIndex = 1;
			this.WarningsTabPage.Text = "Warnings";
			this.WarningsTabPage.UseVisualStyleBackColor = true;
			// 
			// WarningsDataGrid
			// 
			this.WarningsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.WarningsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.WarningsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WarningsDataGrid.Location = new System.Drawing.Point(3, 3);
			this.WarningsDataGrid.Name = "WarningsDataGrid";
			this.WarningsDataGrid.ReadOnly = true;
			this.WarningsDataGrid.RowHeadersVisible = false;
			this.WarningsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.WarningsDataGrid.Size = new System.Drawing.Size(929, 223);
			this.WarningsDataGrid.TabIndex = 1;
			this.WarningsDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
			// 
			// InfoTabPage
			// 
			this.InfoTabPage.Controls.Add(this.InfosDataGrid);
			this.InfoTabPage.Location = new System.Drawing.Point(4, 25);
			this.InfoTabPage.Name = "InfoTabPage";
			this.InfoTabPage.Size = new System.Drawing.Size(935, 229);
			this.InfoTabPage.TabIndex = 2;
			this.InfoTabPage.Text = "Info";
			this.InfoTabPage.UseVisualStyleBackColor = true;
			// 
			// InfosDataGrid
			// 
			this.InfosDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.InfosDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.InfosDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfosDataGrid.Location = new System.Drawing.Point(0, 0);
			this.InfosDataGrid.Name = "InfosDataGrid";
			this.InfosDataGrid.ReadOnly = true;
			this.InfosDataGrid.RowHeadersVisible = false;
			this.InfosDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.InfosDataGrid.Size = new System.Drawing.Size(935, 229);
			this.InfosDataGrid.TabIndex = 1;
			this.InfosDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
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
			this.ErrorsTabPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ErrorsDataGrid)).EndInit();
			this.WarningsTabPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.WarningsDataGrid)).EndInit();
			this.InfoTabPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.InfosDataGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage ErrorsTabPage;
		private System.Windows.Forms.DataGridView ErrorsDataGrid;
		private System.Windows.Forms.TabPage WarningsTabPage;
		private System.Windows.Forms.TabPage InfoTabPage;
		private System.Windows.Forms.DataGridView WarningsDataGrid;
		private System.Windows.Forms.DataGridView InfosDataGrid;
	}
}
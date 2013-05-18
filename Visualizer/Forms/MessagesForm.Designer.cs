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
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessagesForm));
			this.ErrorsTabPage = new System.Windows.Forms.TabPage();
			this.WarningsTabPage = new System.Windows.Forms.TabPage();
			this.WarningsDataGrid = new System.Windows.Forms.DataGridView();
			this.InfoTabPage = new System.Windows.Forms.TabPage();
			this.InfosDataGrid = new System.Windows.Forms.DataGridView();
			this.ErrorsDataGrid = new System.Windows.Forms.DataGridView();
			this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.WarningsTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.WarningsDataGrid)).BeginInit();
			this.InfoTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.InfosDataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ErrorsDataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// ErrorsTabPage
			// 
			this.ErrorsTabPage.Location = new System.Drawing.Point(4, 22);
			this.ErrorsTabPage.Name = "ErrorsTabPage";
			this.ErrorsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.ErrorsTabPage.Size = new System.Drawing.Size(935, 232);
			this.ErrorsTabPage.TabIndex = 0;
			this.ErrorsTabPage.Text = "Errors";
			this.ErrorsTabPage.UseVisualStyleBackColor = true;
			// 
			// WarningsTabPage
			// 
			this.WarningsTabPage.Controls.Add(this.WarningsDataGrid);
			this.WarningsTabPage.Location = new System.Drawing.Point(4, 22);
			this.WarningsTabPage.Name = "WarningsTabPage";
			this.WarningsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.WarningsTabPage.Size = new System.Drawing.Size(935, 232);
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
			this.WarningsDataGrid.Size = new System.Drawing.Size(929, 226);
			this.WarningsDataGrid.TabIndex = 1;
			this.WarningsDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
			// 
			// InfoTabPage
			// 
			this.InfoTabPage.Controls.Add(this.InfosDataGrid);
			this.InfoTabPage.Location = new System.Drawing.Point(4, 22);
			this.InfoTabPage.Name = "InfoTabPage";
			this.InfoTabPage.Size = new System.Drawing.Size(935, 232);
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
			this.InfosDataGrid.Size = new System.Drawing.Size(935, 232);
			this.InfosDataGrid.TabIndex = 1;
			this.InfosDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
			// 
			// ErrorsDataGrid
			// 
			this.ErrorsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.ErrorsDataGrid.BackgroundColor = System.Drawing.Color.DimGray;
			this.ErrorsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ErrorsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ErrorsDataGrid.Location = new System.Drawing.Point(0, 0);
			this.ErrorsDataGrid.Name = "ErrorsDataGrid";
			this.ErrorsDataGrid.ReadOnly = true;
			this.ErrorsDataGrid.RowHeadersVisible = false;
			this.ErrorsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ErrorsDataGrid.Size = new System.Drawing.Size(943, 258);
			this.ErrorsDataGrid.TabIndex = 0;
			this.ErrorsDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
			// 
			// dockPanel1
			// 
			this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.dockPanel1.Location = new System.Drawing.Point(0, 0);
			this.dockPanel1.Name = "dockPanel1";
			this.dockPanel1.Size = new System.Drawing.Size(943, 258);
			dockPanelGradient1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			dockPanelGradient1.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
			tabGradient1.EndColor = System.Drawing.SystemColors.Control;
			tabGradient1.StartColor = System.Drawing.SystemColors.Control;
			tabGradient1.TextColor = System.Drawing.Color.White;
			autoHideStripSkin1.TabGradient = tabGradient1;
			autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
			tabGradient2.EndColor = System.Drawing.Color.White;
			tabGradient2.StartColor = System.Drawing.Color.White;
			tabGradient2.TextColor = System.Drawing.Color.Black;
			dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			dockPanelGradient2.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			tabGradient3.StartColor = System.Drawing.SystemColors.Control;
			tabGradient3.TextColor = System.Drawing.Color.Black;
			dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
			dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			tabGradient4.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient4.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			tabGradient4.TextColor = System.Drawing.Color.White;
			dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient5.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient5.TextColor = System.Drawing.Color.White;
			dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient3.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = System.Drawing.SystemColors.ControlDark;
			tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient6.StartColor = System.Drawing.SystemColors.Control;
			tabGradient6.TextColor = System.Drawing.SystemColors.GrayText;
			dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = System.Drawing.SystemColors.Control;
			tabGradient7.StartColor = System.Drawing.SystemColors.Control;
			tabGradient7.TextColor = System.Drawing.SystemColors.GrayText;
			dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
			dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
			this.dockPanel1.Skin = dockPanelSkin1;
			this.dockPanel1.SkinStyle = WeifenLuo.WinFormsUI.Docking.Skins.Style.VisualStudio2012Light;
			this.dockPanel1.TabIndex = 1;
			// 
			// MessagesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(943, 258);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.ControlBox = false;
			this.Controls.Add(this.dockPanel1);
			this.Controls.Add(this.ErrorsDataGrid);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MessagesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Message Area";
			this.WarningsTabPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.WarningsDataGrid)).EndInit();
			this.InfoTabPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.InfosDataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ErrorsDataGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		//private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage ErrorsTabPage;
		private System.Windows.Forms.DataGridView ErrorsDataGrid;
		private System.Windows.Forms.TabPage WarningsTabPage;
		private System.Windows.Forms.TabPage InfoTabPage;
		private System.Windows.Forms.DataGridView WarningsDataGrid;
		private System.Windows.Forms.DataGridView InfosDataGrid;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
	}
}
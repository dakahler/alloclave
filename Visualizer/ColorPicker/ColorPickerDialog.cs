using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Alloclave
{
	public partial class ColorPickerDialog : Form
	{
		public static event EventHandler ColorChanged;

		ColorPickerCtrl m_colorPicker;
		public ColorPickerDialog()
		{
			InitializeComponent();

			m_colorPicker = new ColorPickerCtrl();
			m_colorPicker.Dock = DockStyle.Fill;
			m_colorTabPage.Controls.Add(m_colorPicker);

			// Init combo box
			var dataSource = new List<ColorSetting>();
			dataSource.Add(new ColorSetting() { name = "Allocation1", color = Properties.Settings.Default.Allocation1 });
			dataSource.Add(new ColorSetting() { name = "Allocation2", color = Properties.Settings.Default.Allocation2 });
			colorComboBox.Items.Add(dataSource);

			colorComboBox.DataSource = dataSource;
			colorComboBox.DisplayMember = "name";
			colorComboBox.ValueMember = "color";

			colorComboBox.SelectedIndexChanged += colorComboBox_SelectedIndexChanged;

			this.m_colorPicker.ColorChanged += m_colorPicker_ColorChanged;
		}

		void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			List<ColorSetting> colorSettings = (List<ColorSetting>)colorComboBox.DataSource;
			m_colorPicker.SelectedColor = (Color)colorSettings[colorComboBox.SelectedIndex].color;
		}

		void m_colorPicker_ColorChanged(object sender, EventArgs e)
		{
			List<ColorSetting> colorSettings = (List<ColorSetting>)colorComboBox.DataSource;
			colorSettings[colorComboBox.SelectedIndex].color = m_colorPicker.SelectedColor;

			// HACK
			if (colorComboBox.SelectedIndex == 0)
			{
				Properties.Settings.Default.Allocation1 = m_colorPicker.SelectedColor;
			}
			else
			{
				Properties.Settings.Default.Allocation2 = m_colorPicker.SelectedColor;
			}

			if (ColorChanged != null)
			{
				ColorChanged(this, e);
			}
		}

		private void OnSelected(object sender, TabControlEventArgs e)
		{
			if (e.TabPage == m_knownColorsTabPage)
				m_colorList.SelectColor(m_colorPicker.SelectedColor);
			if (e.TabPage == m_colorTabPage)
				m_colorPicker.SelectedColor = (Color)m_colorList.SelectedItem;
		}
	}

	public class ColorSetting
	{
		public String name { get; set; }
		public Color? color { get; set; }
	}
}
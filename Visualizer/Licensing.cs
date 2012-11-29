using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Alloclave
{
	public partial class Licensing : Form
	{
		// Sample license string format:
		/*
			Licensed to: David Kahler (dave@davekahler.com)
			Support expires: 10/20/2010
			KGXE-WUFN-OEXI-MAIN-YJRD-XUYU-SNZM-YYMO
		*/

		public enum LicenseStatus
		{
			Invalid,
			Trial,
			Valid,
		}

		static LicenseStatus _LicenseStatus = LicenseStatus.Invalid;
		public static LicenseStatus CurrentLicenseStatus
		{
			get
			{
				return _LicenseStatus;
			}
		}

		static String _License;
		public static String License
		{
			set
			{
				_License = value.Trim(' ', '\t').Replace("\t", "");
				ValidateLicense();
			}
		}

		public Licensing()
		{
			#if !_INSTALLER
				_LicenseStatus = LicenseStatus.Valid;
				Close();
				return;
			#else
				InitializeComponent();
			#endif
		}

		public static void ValidateLicense()
		{
			StringReader reader = new StringReader(_License);
			String parsedLicense = reader.ReadLine() + " " + reader.ReadLine();
			String enteredLicenseKey = reader.ReadLine();

			String salt = "pgbnqn547oiymsvlad243";
			String finalString = salt + parsedLicense;
			Byte[] bytes = Encoding.ASCII.GetBytes(finalString);

			System.Security.Cryptography.SHA256Managed hashAlgorithm = new SHA256Managed();
			byte[] hash = hashAlgorithm.ComputeHash(bytes);

			// Transform the bytes so they're only upper case ASCII alphabet characters
			for (int i = 0; i < hash.Length; i++)
			{
				char startChar = 'A';
				int startInt = (int)startChar;
				hash[i] = (byte)(((int)hash[i] % 26) + startInt);
			}

			String calculatedLicenseKey = Encoding.ASCII.GetString(hash);
			
			// Add dashes
			String finalLicenseKey = "";
			for (int i = 0, counter = 0; i < calculatedLicenseKey.Length; i++)
			{
				if (counter == 4)
				{
					finalLicenseKey += "-";
					counter = 0;
				}

				finalLicenseKey += calculatedLicenseKey[i];
				counter++;
			}

			if (finalLicenseKey == enteredLicenseKey)
			{
				_LicenseStatus = LicenseStatus.Valid;
			}
			else
			{
				_LicenseStatus = LicenseStatus.Invalid;
			}
		}

		private void LicenseTextBox_TextChanged(object sender, EventArgs e)
		{
			License = LicenseTextBox.Text;
			if (CurrentLicenseStatus == LicenseStatus.Valid)
			{
				LicensePanel.BackColor = Color.LightGreen;
				OkButton.Enabled = true;
			}
			else
			{
				LicensePanel.BackColor = Color.Salmon;
				OkButton.Enabled = false;
			}
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			if (CurrentLicenseStatus != LicenseStatus.Invalid)
			{
				Close();
			}
		}

		private void BuyButton_Click(object sender, EventArgs e)
		{
			// TODO: Launch website
		}

		private void TryButton_Click(object sender, EventArgs e)
		{
			_LicenseStatus = LicenseStatus.Trial;
			Close();
		}
	}
}

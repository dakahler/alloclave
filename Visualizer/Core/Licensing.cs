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
using Alloclave.Properties;

namespace Alloclave
{
	internal partial class Licensing
	{
		public static String LicenseName
		{
			get
			{
				return "Licensed to: Non-Commercial User";
			}
		}

		public static DateTime LicenseDate
		{
			get
			{
				return DateTime.Now.AddYears(50);
			}
		}

		public static String LicenseEmail
		{
			get
			{
				return String.Empty;
			}
		}

		public static bool IsTrial
		{
			get
			{
				return false;
			}
		}

		public static bool IsLicensed
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Retrieves the build timestamp
		/// </summary>
		public static DateTime LinkerTimestamp
		{
			get
			{
				string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
				const int c_PeHeaderOffset = 60;
				const int c_LinkerTimestampOffset = 8;
				byte[] b = new byte[2048];
				System.IO.Stream s = null;

				try
				{
					s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					s.Read(b, 0, 2048);
				}
				finally
				{
					if (s != null)
					{
						s.Close();
					}
				}

				int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
				int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
				DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
				dt = dt.AddSeconds(secondsSince1970);
				dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
				return dt;
			}
		}
	}
}

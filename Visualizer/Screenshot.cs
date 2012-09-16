using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Meminator
{
	public class Screenshot : Packet
	{
		Image Image;

		public Screenshot(Image image)
		{
			Image = image;
		}

		public void Parse(byte[] data)
		{

		}
	}
}

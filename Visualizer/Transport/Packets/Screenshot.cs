using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace Alloclave
{
	[DataContract()]
	internal class Screenshot : IPacket
	{
		Image Image;

		public Screenshot(Image image)
		{
			Image = image;
		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}
	}
}

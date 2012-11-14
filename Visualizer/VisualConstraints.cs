using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	class VisualConstraints
	{
		public UInt64 StartAddress;
		public UInt64 RowAddressWidth;
		public uint RowPixelHeight;
		public uint RowAddressPixelWidth;

		public VisualConstraints()
		{
			StartAddress = 0;
			RowAddressWidth = 0xFF;
			RowPixelHeight = 20;
			RowAddressPixelWidth = 500;
		}
	}
}

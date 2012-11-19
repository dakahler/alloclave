using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	public class VisualConstraints
	{
		public UInt64 StartAddress;
		public UInt64 RowAddressWidth;

		// TODO: Should this just be based on control size?
		public uint RowAddressPixelHeight;
		public uint RowAddressPixelWidth;

		public VisualConstraints()
		{
			StartAddress = 0;
			RowAddressWidth = 0xF;
			RowAddressPixelHeight = 2;
			RowAddressPixelWidth = 500;
		}
	}
}

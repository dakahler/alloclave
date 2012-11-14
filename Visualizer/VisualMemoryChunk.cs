using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	class VisualMemoryChunk
	{
		// TODO: Better encapsulation
		public List<VisualMemoryBox> Boxes = new List<VisualMemoryBox>();

		public VisualMemoryChunk(UInt64 startAddress, UInt64 size, VisualConstraints constraints)
		{
			if (startAddress < constraints.StartAddress)
			{
				throw new ArgumentOutOfRangeException();
			}

			UInt64 compensatedWidth = constraints.RowAddressWidth + 1;

			UInt64 offset = startAddress - constraints.StartAddress;
			UInt64 rowNum = offset / compensatedWidth;
			UInt64 rowStartAddress = rowNum * compensatedWidth;
			UInt64 offsetFromRowStart = offset - rowStartAddress;
			while (rowStartAddress < offset + size)
			{
				// TODO: Find way to make this flow nicer and be easier to understand
				// TODO: Using the default box width to scale along x here
				// is not correct. Change it to take the row width into account.

				UInt64 rowEnd = (rowNum * compensatedWidth) + compensatedWidth;

				// Clamp box width to current row
				UInt64 currentRowBoxWidth = rowEnd - offset;
				currentRowBoxWidth = Math.Min(currentRowBoxWidth, size);

				// Get percentage for actual width
				float percentage = (float)offsetFromRowStart / (float)compensatedWidth;

				VisualMemoryBox newBox = new VisualMemoryBox();
				newBox.Transform.Translate(percentage * constraints.RowAddressPixelWidth,
					rowNum * constraints.RowPixelHeight);

				newBox.Transform.Scale((float)currentRowBoxWidth, 1.0f);

				Boxes.Add(newBox);

				// Setup for next row
				rowNum++;
				rowStartAddress = rowNum * compensatedWidth;
				offset = rowStartAddress;
				offsetFromRowStart = 0;

				size -= currentRowBoxWidth;
			}
		}
	}
}

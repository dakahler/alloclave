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

			Create(startAddress, size, constraints);
		}

		private void Create(UInt64 startAddress, UInt64 size, VisualConstraints constraints)
		{
			// Subtract out where logical memory begins
			UInt64 workingStartAddress = startAddress - constraints.StartAddress;
			UInt64 endAddress = workingStartAddress + size;

			// Find row start and end addresses
			// Note that this is the same operation as memory alignment
			// TODO: Have an alignment helper function?
			UInt64 rowStartAddress = workingStartAddress & ~constraints.RowAddressWidth;
			UInt64 rowEndAddress = rowStartAddress + constraints.RowAddressWidth;

			// The box should go to either the end of the row or the end of the allocation
			endAddress = Math.Min(endAddress, rowEndAddress);

			// Box creation
			// Transform address space range to pixel space range
			// X
			UInt64 offset = workingStartAddress - rowStartAddress;
			float scaleFactor = ((float)(offset) / (float)constraints.RowAddressWidth);
			UInt64 pixelX = (UInt64)(constraints.RowAddressPixelWidth * scaleFactor);

			// Y
			UInt64 rowNum = rowStartAddress / constraints.RowAddressWidth;
			UInt64 pixelY = rowNum * constraints.RowAddressPixelHeight;

			// ScaleX
			float scaleX = (float)(endAddress - workingStartAddress) / (float)constraints.RowAddressWidth;
			scaleX *= constraints.RowAddressPixelWidth;

			// TODO: ScaleY
			float scaleY = 1.0f;
			scaleY *= constraints.RowAddressPixelHeight;

			AddBox(pixelX, pixelY, scaleX, scaleY);

			// Build the box for the next row of the allocation, if necessary
			UInt64 sizeCovered = endAddress - workingStartAddress + 1;
			if (sizeCovered < size)
			{
				// TODO: Will this cause stack overflows for large allocations?
				// May need to do this iteratively, after all
				Create(constraints.StartAddress + endAddress + 1, size - sizeCovered, constraints);
			}
		}

		private void AddBox(UInt64 x, UInt64 y, float scaleX, float scaleY)
		{
			VisualMemoryBox box = new VisualMemoryBox();
			box.Transform.Translate(x, y);
			box.Transform.Scale(scaleX, scaleY);
			Boxes.Add(box);
		}
	}
}

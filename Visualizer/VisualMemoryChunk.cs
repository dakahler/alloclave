using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	class VisualMemoryChunk
	{
		// TODO: Better encapsulation
		public List<VisualMemoryBox> Boxes = new List<VisualMemoryBox>();

		static List<Color> colors = new List<Color>();
		static int colorIndex = 0;

		public VisualMemoryChunk(UInt64 startAddress, UInt64 size, VisualConstraints constraints)
		{
			// TODO: Put this somewhere else? Maybe user definable.
			if (colors.Count == 0)
			{
				colors.Add(Color.FromArgb(230, 0, 0));
				colors.Add(Color.FromArgb(200, 0, 0));
				colorIndex = 0;
			}

			if (startAddress < constraints.StartAddress)
			{
				throw new ArgumentOutOfRangeException();
			}

			Create(startAddress, size, constraints);
		}

		
		private void Create(UInt64 startAddress, UInt64 size, VisualConstraints constraints)
		{
			UInt64 currentAddress = startAddress;
			UInt64 currentSize = size;
			int tempCounter = 0;

			while (true)
			{
				// Subtract out where logical memory begins
				UInt64 workingStartAddress = currentAddress - constraints.StartAddress;
				UInt64 endAddress = workingStartAddress + currentSize;

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

				AddBox(pixelX, pixelY, scaleX, scaleY, colors[colorIndex]);

				tempCounter++;

				// Build the box for the next row of the allocation, if necessary
				UInt64 sizeCovered = endAddress - workingStartAddress + 1;
				if (sizeCovered < currentSize)
				{
					// TODO: Will this cause stack overflows for large allocations?
					// May need to do this iteratively, after all
					currentAddress = constraints.StartAddress + endAddress + 1;
					currentSize = currentSize - sizeCovered;
				}
				else
				{
					break;
				}
			}

			colorIndex = (colorIndex + 1) % colors.Count;
		}

		private void AddBox(UInt64 x, UInt64 y, float scaleX, float scaleY, Color color)
		{
			VisualMemoryBox box = new VisualMemoryBox();
			box.Transform.Translate(x, y);
			box.Transform.Scale(scaleX, scaleY);
			box.Color = color;
			Boxes.Add(box);
		}

		public bool Contains(Point point)
		{
			foreach (VisualMemoryBox box in Boxes)
			{
				Point[] points = { new Point(box.DefaultBox.X,box.DefaultBox.Y),
									 new Point(box.DefaultBox.Width, box.DefaultBox.Height) };

				box.Transform.TransformPoints(points);

				Rectangle rectangle = new Rectangle(points[0].X, points[0].Y,
					points[1].X - points[0].X, points[1].Y - points[0].Y);

				if (rectangle.Contains(point))
				{
					return true;
				}
			}

			return false;
		}
	}
}

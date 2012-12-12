using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	public class VisualMemoryChunk
	{
		// TODO: Better encapsulation
		public List<VisualMemoryBox> Boxes = new List<VisualMemoryBox>();

		static List<Color> colors = new List<Color>();
		static int colorIndex = 0;

		public Color _Color = Color.Red;

		// TODO: Too specific?
		public Allocation Allocation;

		public Region Region = new Region();

		public VisualMemoryChunk(Allocation allocation, VisualConstraints constraints)
		{
			// TODO: Put this somewhere else? Maybe user definable.
			if (colors.Count == 0)
			{
				colors.Add(Color.FromArgb(255, 230, 0, 0));
				colors.Add(Color.FromArgb(255, 100, 0, 0));
				colorIndex = 0;
			}

			if (allocation.Address < constraints.StartAddress)
			{
				throw new ArgumentOutOfRangeException();
			}

			Allocation = allocation;

			Create(allocation, constraints);
		}


		private void Create(Allocation allocation, VisualConstraints constraints)
		{
			UInt64 currentAddress = allocation.Address;
			UInt64 currentSize = allocation.Size;

			Region.MakeEmpty();

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

				AddBox(pixelX, pixelY, scaleX, scaleY);

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

			_Color = colors[colorIndex];
			colorIndex = (colorIndex + 1) % colors.Count;
		}

		private void AddBox(UInt64 x, UInt64 y, float scaleX, float scaleY)
		{
			VisualMemoryBox box = new VisualMemoryBox();
			box.Transform.Translate(x, y);
			box.Transform.Scale(scaleX, scaleY);
			Boxes.Add(box);

			Rectangle rectangle = box.DefaultBox;
			Region region = new Region(rectangle);
			region.Transform(box.Transform);
			Region.Union(region);
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

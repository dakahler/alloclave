using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	public class VisualMemoryChunk
	{
		// TODO: Better encapsulation
		static List<Color> colors = new List<Color>();
		static int colorIndex = 0;

		public Color _Color = Color.Red;

		// TODO: Too specific?
		public Allocation Allocation;

		public Region Region = new Region();

		public GraphicsPath GraphicsPath = new GraphicsPath();

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

		private Point GetPixelPos(UInt64 address, VisualConstraints constraints)
		{
			UInt64 workingStartAddress = address - constraints.StartAddress;

			// Find row start and end addresses
			// Note that this is the same operation as memory alignment
			// TODO: Have an alignment helper function?
			UInt64 rowStartAddress = workingStartAddress & ~constraints.RowAddressWidth;
			UInt64 rowEndAddress = rowStartAddress + constraints.RowAddressWidth;

			// Box creation
			// Transform address space range to pixel space range
			// X
			UInt64 offset = workingStartAddress - rowStartAddress;
			float scaleFactor = ((float)(offset) / (float)constraints.RowAddressWidth);
			UInt64 pixelX = (UInt64)(constraints.RowAddressPixelWidth * scaleFactor);

			// Y
			UInt64 rowNum = rowStartAddress / constraints.RowAddressWidth;
			UInt64 pixelY = rowNum * constraints.RowAddressPixelHeight;

			return new Point((int)pixelX, (int)pixelY);
		}

		private void Create(Allocation allocation, VisualConstraints constraints)
		{
			UInt64 startAddress = allocation.Address;
			UInt64 size = allocation.Size;
			UInt64 endAddress = startAddress + size;
			UInt64 numRows = (endAddress - startAddress) / constraints.RowAddressWidth;

			Region.MakeEmpty();
			GraphicsPath.Reset();

			List<Point> polygonPoints = new List<Point>();
			HashSet<Point> polygonPointSet = new HashSet<Point>();

			Point rowOneUpperLeft = GetPixelPos(startAddress, constraints);
			Point rowOneLowerLeft = Point.Add(rowOneUpperLeft, new Size(0, (int)constraints.RowAddressPixelHeight));
			Point rowOneFarLeftLowerLeft = new Point(0, rowOneUpperLeft.Y);
			Point rowOneFarRightUpperRight = new Point((int)constraints.RowAddressPixelWidth, rowOneUpperLeft.Y);

			Point lastRowUpperRight = GetPixelPos(endAddress, constraints);
			Point lastRowLowerRight = Point.Add(lastRowUpperRight, new Size(0, (int)constraints.RowAddressPixelHeight));
			Point lastRowFarLeftLowerLeft = new Point(0, lastRowLowerRight.Y);
			Point lastRowFarRightUpperRight = new Point((int)constraints.RowAddressPixelWidth, lastRowUpperRight.Y);

			// Create block by specifying polygon points
			// Use the hash set to make sure the list has unique points
			// TODO: Clean up
			if (polygonPointSet.Add(rowOneUpperLeft))
			{
				polygonPoints.Add(rowOneUpperLeft);
			}

			if (polygonPointSet.Add(rowOneLowerLeft))
			{
				polygonPoints.Add(rowOneLowerLeft);
			}

			if (polygonPointSet.Add(rowOneFarLeftLowerLeft))
			{
				polygonPoints.Add(rowOneFarLeftLowerLeft);
			}

			if (polygonPointSet.Add(lastRowFarLeftLowerLeft))
			{
				polygonPoints.Add(lastRowFarLeftLowerLeft);
			}

			if (polygonPointSet.Add(lastRowLowerRight))
			{
				polygonPoints.Add(lastRowLowerRight);
			}

			if (polygonPointSet.Add(lastRowUpperRight))
			{
				polygonPoints.Add(lastRowUpperRight);
			}

			if (polygonPointSet.Add(lastRowFarRightUpperRight))
			{
				polygonPoints.Add(lastRowFarRightUpperRight);
			}

			if (polygonPointSet.Add(rowOneFarRightUpperRight))
			{
				polygonPoints.Add(rowOneFarRightUpperRight);
			}

			GraphicsPath.AddPolygon(polygonPoints.ToArray());

			_Color = colors[colorIndex];
			colorIndex = (colorIndex + 1) % colors.Count;
		}

		public bool Contains(Point point)
		{
			return GraphicsPath.IsVisible(point);
		}
	}
}

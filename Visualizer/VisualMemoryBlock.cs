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
	public class Triangle
	{
		public Point[] Vertices = new Point[3];

		public Triangle()
		{

		}

		public Triangle(Point point1, Point point2, Point point3)
		{
			Vertices[0] = point1;
			Vertices[1] = point2;
			Vertices[2] = point3;
		}
	}

	public class VisualMemoryBlock
	{
		// TODO: Better encapsulation
		static bool isSecondaryColor = false;

		const int RowHeight = 2;

		public Color _Color = Color.Red;

		// TODO: Too specific?
		public Allocation Allocation;

		public GraphicsPath GraphicsPath = new GraphicsPath();

		public List<Triangle> Triangles = new List<Triangle>();

		public bool IsNew = true;

		public int MaxPixelWidth = 0;

		public VisualMemoryBlock()
		{

		}

		public VisualMemoryBlock(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			if (allocation.Address < startAddress)
			{
				throw new ArgumentOutOfRangeException();
			}

			Allocation = allocation;

			MaxPixelWidth = width;

			Create(allocation, startAddress, addressWidth, width);
		}

		private Point GetPixelPos(UInt64 address, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			UInt64 workingStartAddress = address - startAddress;

			// Find row start and end addresses
			// Note that this is the same operation as memory alignment
			// TODO: Have an alignment helper function?
			UInt64 rowStartAddress = workingStartAddress & ~addressWidth;

			// Box creation
			// Transform address space range to pixel space range
			// X
			UInt64 offset = workingStartAddress - rowStartAddress;
			float scaleFactor = ((float)(offset) / (float)addressWidth);
			UInt64 pixelX = (UInt64)(width * scaleFactor);

			// Y
			UInt64 rowNum = rowStartAddress / addressWidth;
			UInt64 pixelY = rowNum * RowHeight;

			return new Point((int)pixelX, (int)pixelY);
		}

		private void Create(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			Triangles.Clear();

			UInt64 currentStartAddress = allocation.Address;
			UInt64 size = allocation.Size;
			UInt64 endAddress = currentStartAddress + size;

			UInt64 rowStartAddress = (currentStartAddress - startAddress) & ~addressWidth;
			UInt64 rowEndAddress = (endAddress - startAddress) & ~addressWidth;

			GraphicsPath.Reset();

			List<Point> polygonPoints = new List<Point>();
			HashSet<Point> polygonPointSet = new HashSet<Point>();

			Point rowOneUpperLeft = GetPixelPos(currentStartAddress, startAddress, addressWidth, width);
			Point rowOneLowerLeft = Point.Add(rowOneUpperLeft, new Size(0, RowHeight));
			Point rowOneFarLeftLowerLeft = new Point(0, rowOneLowerLeft.Y);
			Point rowOneFarRightUpperRight = new Point(width, rowOneUpperLeft.Y);

			Point lastRowUpperRight = GetPixelPos(endAddress, startAddress, addressWidth, width);
			Point lastRowLowerRight = Point.Add(lastRowUpperRight, new Size(0, RowHeight));
			Point lastRowFarLeftLowerLeft = new Point(0, lastRowLowerRight.Y);
			Point lastRowFarRightUpperRight = new Point(width, lastRowUpperRight.Y);

			//if (rowStartAddress != rowEndAddress)
			//{
			//	GraphicsPath.AddLine(new Point(0, 0), new Point(0, 0));
			//	return;
			//}

			// Create block by specifying polygon points
			// Use the hash set to make sure the list has unique points
			// TODO: Clean up
			//if (polygonPointSet.Add(rowOneUpperLeft))
			{
				polygonPoints.Add(rowOneUpperLeft);
			}

			//if (polygonPointSet.Add(rowOneLowerLeft))
			{
				polygonPoints.Add(rowOneLowerLeft);
			}

			if (rowStartAddress != rowEndAddress)
			{
				//if (polygonPointSet.Add(rowOneFarLeftLowerLeft))
				{
					polygonPoints.Add(rowOneFarLeftLowerLeft);
				}

				//if (polygonPointSet.Add(lastRowFarLeftLowerLeft))
				{
					polygonPoints.Add(lastRowFarLeftLowerLeft);
				}
			}

			//if (polygonPointSet.Add(lastRowLowerRight))
			{
				polygonPoints.Add(lastRowLowerRight);
			}

			//if (polygonPointSet.Add(lastRowUpperRight))
			{
				polygonPoints.Add(lastRowUpperRight);
			}

			if (rowStartAddress != rowEndAddress)
			{
				//if (polygonPointSet.Add(lastRowFarRightUpperRight))
				{
					polygonPoints.Add(lastRowFarRightUpperRight);
				}

				//if (polygonPointSet.Add(rowOneFarRightUpperRight))
				{
					polygonPoints.Add(rowOneFarRightUpperRight);
				}
			}

			GraphicsPath.AddPolygon(polygonPoints.ToArray());

			// Create triangles (2 per section)
			// Counter-clockwise

			if (rowStartAddress != rowEndAddress)
			{
				// Upper
				Triangle upper1 = new Triangle(rowOneFarRightUpperRight, rowOneUpperLeft, rowOneLowerLeft);
				Triangle upper2 = new Triangle(rowOneFarRightUpperRight, rowOneLowerLeft, new Point(rowOneFarRightUpperRight.X, rowOneLowerLeft.Y));
				Triangles.Add(upper1);
				Triangles.Add(upper2);

				// Middle
				Triangle middle1 = new Triangle(new Point(lastRowFarLeftLowerLeft.X, lastRowUpperRight.Y), new Point(rowOneFarRightUpperRight.X, rowOneLowerLeft.Y), rowOneFarLeftLowerLeft);
				Triangle middle2 = new Triangle(new Point(lastRowFarLeftLowerLeft.X, lastRowUpperRight.Y), new Point(lastRowFarRightUpperRight.X, lastRowUpperRight.Y), new Point(rowOneFarRightUpperRight.X, rowOneLowerLeft.Y));
				Triangles.Add(middle1);
				Triangles.Add(middle2);

				// Lower
				Triangle lower1 = new Triangle(lastRowFarLeftLowerLeft, lastRowUpperRight, new Point(lastRowFarLeftLowerLeft.X, lastRowUpperRight.Y));
				Triangle lower2 = new Triangle(lastRowFarLeftLowerLeft, lastRowLowerRight, lastRowUpperRight);
				Triangles.Add(lower1);
				Triangles.Add(lower2);
			}
			else
			{
				// Single row special case
				Triangle upper1 = new Triangle(rowOneUpperLeft, rowOneLowerLeft, lastRowUpperRight);
				Triangle upper2 = new Triangle(rowOneLowerLeft, lastRowLowerRight, lastRowUpperRight);
				Triangles.Add(upper1);
				Triangles.Add(upper2);
			}


			if (!isSecondaryColor)
			{
				_Color = Properties.Settings.Default.Allocation1;
			}
			else
			{
				_Color = Properties.Settings.Default.Allocation2;
			}

			isSecondaryColor = !isSecondaryColor;
		}

		public bool Contains(Point point)
		{
			return GraphicsPath.IsVisible(point);
		}
	}
}

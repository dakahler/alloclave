using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alloclave
{
	public class Triangle
	{
		public Vector[] Vertices = new Vector[3];

		public Triangle()
		{

		}

		public Triangle(Vector point1, Vector point2, Vector point3)
		{
			Vertices[0] = point1;
			Vertices[1] = point2;
			Vertices[2] = point3;
		}
	}

	public class VisualMemoryBlock
	{
		// TODO: Better encapsulation
		const int RowHeight = 2;

		public int ColorIndex = 0;

		// TODO: Too specific?
		public Allocation Allocation;

		public GraphicsPath GraphicsPath = new GraphicsPath();

		private RectangleF _Bounds = new RectangleF();
		public RectangleF Bounds
		{
			get
			{
				return _Bounds;
			}
		}

		public List<Triangle> Triangles = new List<Triangle>();

		public bool IsNew = true;

		public int MaxPixelWidth = 0;

		public VisualMemoryBlock()
		{

		}

		public VisualMemoryBlock(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width, int colorIndex)
		{
			if (allocation.Address < startAddress)
			{
				throw new ArgumentOutOfRangeException();
			}

			Allocation = allocation;
			MaxPixelWidth = width;
			ColorIndex = colorIndex;

			Create(allocation, startAddress, addressWidth, width, colorIndex);
		}

		private Vector GetPixelPos(UInt64 address, UInt64 startAddress, UInt64 addressWidth, int width)
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

			return new Vector((int)pixelX, (int)pixelY);
		}

		private void Create(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width, int colorIndex)
		{
			Triangles.Clear();

			UInt64 currentStartAddress = allocation.Address;
			UInt64 size = allocation.Size;
			UInt64 endAddress = currentStartAddress + size;

			UInt64 rowStartAddress = (currentStartAddress - startAddress) & ~addressWidth;
			UInt64 rowEndAddress = (endAddress - startAddress) & ~addressWidth;

			GraphicsPath.Reset();

			List<Vector> polygonPoints = new List<Vector>();

			Vector rowOneUpperLeft = GetPixelPos(currentStartAddress, startAddress, addressWidth, width);
			Vector rowOneLowerLeft = rowOneUpperLeft + new Vector(0, RowHeight);
			Vector rowOneFarLeftLowerLeft = new Vector(0, rowOneLowerLeft.Y);
			Vector rowOneFarRightUpperRight = new Vector(width, rowOneUpperLeft.Y);

			Vector lastRowUpperRight = GetPixelPos(endAddress, startAddress, addressWidth, width);
			Vector lastRowLowerRight = lastRowUpperRight + new Vector(0, RowHeight);
			Vector lastRowFarLeftLowerLeft = new Vector(0, lastRowLowerRight.Y);
			Vector lastRowFarRightUpperRight = new Vector(width, lastRowUpperRight.Y);

			// Create block by specifying polygon points
			polygonPoints.Add(rowOneUpperLeft);
			polygonPoints.Add(rowOneLowerLeft);

			if (rowStartAddress != rowEndAddress)
			{
				polygonPoints.Add(rowOneFarLeftLowerLeft);
				polygonPoints.Add(lastRowFarLeftLowerLeft);
			}

			polygonPoints.Add(lastRowLowerRight);
			polygonPoints.Add(lastRowUpperRight);

			if (rowStartAddress != rowEndAddress)
			{
				polygonPoints.Add(lastRowFarRightUpperRight);
				polygonPoints.Add(rowOneFarRightUpperRight);
			}

			var newArray = Array.ConvertAll(polygonPoints.ToArray(), item => item.ToPoint());
			GraphicsPath.AddPolygon(newArray);

			_Bounds = GraphicsPath.GetBounds();

			// Create triangles (2 per section)
			// Counter-clockwise

			if (rowStartAddress != rowEndAddress)
			{
				// Upper
				Triangle upper1 = new Triangle(rowOneFarRightUpperRight, rowOneUpperLeft, rowOneLowerLeft);
				Triangle upper2 = new Triangle(rowOneFarRightUpperRight, rowOneLowerLeft, new Vector(rowOneFarRightUpperRight.X, rowOneLowerLeft.Y));
				Triangles.Add(upper1);
				Triangles.Add(upper2);

				// Middle
				Triangle middle1 = new Triangle(new Vector(lastRowFarLeftLowerLeft.X, lastRowUpperRight.Y), new Vector(rowOneFarRightUpperRight.X, rowOneLowerLeft.Y), rowOneFarLeftLowerLeft);
				Triangle middle2 = new Triangle(new Vector(lastRowFarLeftLowerLeft.X, lastRowUpperRight.Y), new Vector(lastRowFarRightUpperRight.X, lastRowUpperRight.Y), new Vector(rowOneFarRightUpperRight.X, rowOneLowerLeft.Y));
				Triangles.Add(middle1);
				Triangles.Add(middle2);

				// Lower
				Triangle lower1 = new Triangle(lastRowFarLeftLowerLeft, lastRowUpperRight, new Vector(lastRowFarLeftLowerLeft.X, lastRowUpperRight.Y));
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

			ColorIndex = colorIndex;
		}

		public bool Contains(Vector v)
		{
			return GraphicsPath.IsVisible(v.ToPoint());
		}
	}
}

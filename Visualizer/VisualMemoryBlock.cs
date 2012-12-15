﻿using System;
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
	public class VisualMemoryBlockComparer : IComparer<VisualMemoryBlock>
	{
		public int Compare(VisualMemoryBlock a, VisualMemoryBlock b)
		{
			if (a.GraphicsPath.IsVisible(b.GraphicsPath.PathPoints[0]))
			{
				return 0;
			}

			if (a.GraphicsPath.PathPoints[0].Y < b.GraphicsPath.PathPoints[0].Y)
			{
				return -1;
			}
			else if (a.GraphicsPath.PathPoints[0].Y > b.GraphicsPath.PathPoints[0].Y)
			{
				return 1;
			}
			else
			{
				if (a.GraphicsPath.PathPoints[0].X < b.GraphicsPath.PathPoints[0].X)
				{
					return 1;
				}
				else
				{
					return -1;
				}
			}
		}
	}

	public class VisualMemoryBlock
	{
		// TODO: Better encapsulation
		static List<Color> colors = new List<Color>();
		static int colorIndex = 0;
		const int RowHeight = 2;

		public Color _Color = Color.Red;

		// TODO: Too specific?
		public Allocation Allocation;

		public GraphicsPath GraphicsPath = new GraphicsPath();

		public VisualMemoryBlock()
		{

		}

		public VisualMemoryBlock(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			// TODO: Put this somewhere else? Maybe user definable.
			if (colors.Count == 0)
			{
				colors.Add(Color.FromArgb(255, 230, 0, 0));
				colors.Add(Color.FromArgb(255, 100, 0, 0));
				colorIndex = 0;
			}

			if (allocation.Address < startAddress)
			{
				throw new ArgumentOutOfRangeException();
			}

			Allocation = allocation;

			Create(allocation, startAddress, addressWidth, width);
		}

		private Point GetPixelPos(UInt64 address, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			UInt64 workingStartAddress = address - startAddress;

			// Find row start and end addresses
			// Note that this is the same operation as memory alignment
			// TODO: Have an alignment helper function?
			UInt64 rowStartAddress = workingStartAddress & ~addressWidth;
			UInt64 rowEndAddress = rowStartAddress + addressWidth;

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
			UInt64 currentStartAddress = allocation.Address;
			UInt64 size = allocation.Size;
			UInt64 endAddress = currentStartAddress + size;
			UInt64 numRows = ((endAddress - currentStartAddress) / addressWidth);

			GraphicsPath.Reset();

			List<Point> polygonPoints = new List<Point>();
			HashSet<Point> polygonPointSet = new HashSet<Point>();

			Point rowOneUpperLeft = GetPixelPos(currentStartAddress, startAddress, addressWidth, width);
			Point rowOneLowerLeft = Point.Add(rowOneUpperLeft, new Size(0, RowHeight));
			Point rowOneFarLeftLowerLeft = new Point(0, rowOneUpperLeft.Y);
			Point rowOneFarRightUpperRight = new Point((int)width, rowOneUpperLeft.Y);

			Point lastRowUpperRight = GetPixelPos(endAddress, startAddress, addressWidth, width);
			Point lastRowLowerRight = Point.Add(lastRowUpperRight, new Size(0, RowHeight));
			Point lastRowFarLeftLowerLeft = new Point(0, lastRowLowerRight.Y);
			Point lastRowFarRightUpperRight = new Point((int)width, lastRowUpperRight.Y);

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
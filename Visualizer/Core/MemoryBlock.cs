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
using System.Diagnostics;

namespace Alloclave
{
	internal class Triangle
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

	internal class MemoryBlock
	{
		// TODO: Better encapsulation
		const int RowHeight = 2;

        // Using System.Drawing.Color here is expensive for how often it's accessed
        public byte[] _Color = new byte[3];

		public Allocation Allocation;

		public GraphicsPath GraphicsPath = new GraphicsPath();

		RectangleF _Bounds = new RectangleF();
		public RectangleF Bounds
		{
			get
			{
				return _Bounds;
			}
		}

		public List<Triangle> Triangles = new List<Triangle>();

		public bool IsValid = true;

		public MemoryBlock()
		{

		}

		public MemoryBlock(MemoryBlock other)
		{
			other._Color.CopyTo(_Color, 0);
			Allocation = other.Allocation;
			GraphicsPath = other.GraphicsPath;
			_Bounds = other._Bounds;
			Triangles = other.Triangles;
		}

		public MemoryBlock(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, Color color)
		{
			Debug.Assert(allocation.Address >= startAddress);

			Allocation = allocation;

            Create(allocation, startAddress, addressWidth, ColorToByteArray(color));
		}

        public static byte[] ColorToByteArray(Color color)
        {
            byte[] tempArray = new byte[3];
            tempArray[0] = color.R;
            tempArray[1] = color.G;
            tempArray[2] = color.B;

            return tempArray;
        }

		static Vector GetPixelPos(UInt64 address, UInt64 startAddress, UInt64 addressWidth)
		{
			UInt64 workingStartAddress = address - startAddress;

			// Find row start and end addresses
			UInt64 rowStartAddress = workingStartAddress.Align(addressWidth);

			// Box creation
			// Transform address space range to pixel space range
			// X
			UInt64 offset = workingStartAddress - rowStartAddress;
			float scaleFactor = ((float)(offset) / (float)addressWidth);
			UInt64 pixelX = (UInt64)scaleFactor;

			// Y
			UInt64 rowNum = rowStartAddress / addressWidth;
			UInt64 pixelY = rowNum * RowHeight;

			return new Vector((int)pixelX, (int)pixelY);
		}

		void Create(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, byte[] color)
		{
			Triangles.Clear();

			UInt64 currentStartAddress = allocation.Address;
			UInt64 size = allocation.Size;
			UInt64 endAddress = currentStartAddress + size;

			UInt64 rowStartAddress = (currentStartAddress - startAddress).Align(addressWidth);
			UInt64 rowEndAddress = (endAddress - startAddress).Align(addressWidth);

			GraphicsPath.Reset();

			List<Vector> polygonPoints = new List<Vector>();

			Vector rowOneUpperLeft = GetPixelPos(currentStartAddress, startAddress, addressWidth);
			Vector rowOneLowerLeft = rowOneUpperLeft + new Vector(0, RowHeight);
			Vector rowOneFarLeftLowerLeft = new Vector(0, rowOneLowerLeft.Y);
			Vector rowOneFarRightUpperRight = new Vector(1.0f, rowOneUpperLeft.Y);

			Vector lastRowUpperRight = GetPixelPos(endAddress, startAddress, addressWidth);
			Vector lastRowLowerRight = lastRowUpperRight + new Vector(0, RowHeight);
			Vector lastRowFarLeftLowerLeft = new Vector(0, lastRowLowerRight.Y);
			Vector lastRowFarRightUpperRight = new Vector(1.0f, lastRowUpperRight.Y);

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

			color.CopyTo(_Color, 0);
		}

		public void Rebase(UInt64 startAddress, UInt64 addressWidth)
		{
			Create(Allocation, startAddress, addressWidth, _Color);
		}

		public bool Contains(Vector v)
		{
			return GraphicsPath.IsVisible(v.ToPoint());
		}
	}

	internal class MemoryBlockEqualityComparer : IEqualityComparer<MemoryBlock>
	{
		public bool Equals(MemoryBlock item1, MemoryBlock item2)
		{
			if (object.ReferenceEquals(item1, item2))
			{
				return true;
			}

			if (item1 == null || item2 == null)
			{
				return false;
			}

			if (item1.Allocation == null || item2.Allocation == null)
			{
				Debug.Assert(false);
				return false;
			}

			return (item1.Allocation.Address == item2.Allocation.Address) &&
				(item1.Allocation.Size == item2.Allocation.Size);
		}

		public int GetHashCode(MemoryBlock item)
		{
			if (item.Allocation == null)
			{
				Debug.Assert(false);
				return item.Bounds.GetHashCode();
			}

			return (item.Allocation.Address + item.Allocation.Size).GetHashCode();
		}
	}
}

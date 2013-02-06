using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alloclave
{
	sealed class MemoryBlockManager : IEnumerable<VisualMemoryBlock>
	{
		private static readonly MemoryBlockManager _Instance = new MemoryBlockManager();
		public static MemoryBlockManager Instance
		{
			get
			{
				return _Instance;
			}
		}

		private SortedList<UInt64, VisualMemoryBlock> VisualMemoryBlocks = new SortedList<UInt64, VisualMemoryBlock>();

		public int Count
		{
			get
			{
				return VisualMemoryBlocks.Count;
			}
		}

		public Rectangle Bounds
		{
			get
			{
				if (Count == 0)
				{
					return new Rectangle();
				}

				VisualMemoryBlock block = VisualMemoryBlocks.Values[VisualMemoryBlocks.Count - 1];
				RectangleF lowerBounds = block.Bounds;
				return new Rectangle(0, 0, block.MaxPixelWidth, (int)lowerBounds.Bottom);
			}
		}

		private MemoryBlockManager()
		{

		}

		public void Reset()
		{
			VisualMemoryBlocks.Clear();
		}

		public void Add(VisualMemoryBlock block)
		{
			VisualMemoryBlocks.Add(block.Allocation.Address, block);
		}

		public void Add(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			VisualMemoryBlock block = new VisualMemoryBlock(allocation, startAddress, addressWidth, width);
			VisualMemoryBlocks.Add(allocation.Address, block);
		}

		public void Remove(VisualMemoryBlock block)
		{
			VisualMemoryBlocks.Remove(block.Allocation.Address);
		}

		public void Remove(UInt64 address)
		{
			VisualMemoryBlocks.Remove(address);
		}

		public void RemoveAt(int index)
		{
			VisualMemoryBlocks.RemoveAt(index);
		}

		public bool Contains(VisualMemoryBlock block)
		{
			return VisualMemoryBlocks.ContainsValue(block);
		}

		public VisualMemoryBlock Find(UInt64 address)
		{
			var result = VisualMemoryBlocks.FirstOrDefault(block => block.Value.Allocation.Address <= address);
			return result.Value;
		}

		public VisualMemoryBlock Find(Vector localMouseCoordinates)
		{
			VisualMemoryBlock tempBlock = new VisualMemoryBlock();
			tempBlock.GraphicsPath.AddLine(localMouseCoordinates.ToPoint(), (localMouseCoordinates + new Vector(1, 1)).ToPoint());

			int index = VisualMemoryBlocks.Values.ToList().BinarySearch(tempBlock, new VisualMemoryBlockComparer());
			if (index >= 0)
			{
				return VisualMemoryBlocks.Values[index];
			}

			return null;
		}

		private class VisualMemoryBlockComparer : IComparer<VisualMemoryBlock>
		{
			public int Compare(VisualMemoryBlock a, VisualMemoryBlock b)
			{
				if (a.GraphicsPath.IsVisible(b.GraphicsPath.PathPoints[0]))
				{
					return 0;
				}

				if (a.GraphicsPath.PathPoints[0].Y + 1 < b.GraphicsPath.PathPoints[0].Y)
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
						return -1;
					}
					else
					{
						return 1;
					}
				}
			}
		}

		public IEnumerator<VisualMemoryBlock> GetEnumerator()
		{
			foreach (var block in VisualMemoryBlocks)
			{
				yield return block.Value;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}

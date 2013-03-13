using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

		private Dictionary<uint, int> ColorDictionary = new Dictionary<uint, int>();
		private int ColorIndex;

		// TODO: hack
		private static bool isSecondaryColor = false;

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
				return new Rectangle(0, 0, block.MaxPixelWidth, (int)(lowerBounds.Bottom - TotalCompression));
			}
		}

		public float TotalCompression
		{
			get
			{
				//float totalOffset = 0.0f;
				//foreach (var pair in HeapOffsets)
				//{
				//	totalOffset += pair.Value;
				//}

				//return totalOffset;

				if (HeapOffsets.Count > 0)
				{
					return HeapOffsets.Last().Value;
				}
				else
				{
					return 0;
				}
			}
		}

		public Dictionary<uint, float> HeapOffsets = new Dictionary<uint, float>();

		private MemoryBlockManager()
		{

		}

		public void Reset()
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlocks.Clear();
			}
		}

		public void Add(VisualMemoryBlock block)
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlocks.Add(block.Allocation.Address, block);
			}
		}

		public VisualMemoryBlock Add(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			// Determine color set based on heap id
			// TODO: This can probably be more generic in the future
			// TODO: This static way of tracking which color to use is too unreliable
			int index = 0;
			if (ColorDictionary.ContainsKey(allocation.HeapId))
			{
				index = ColorDictionary[allocation.HeapId];
			}
			else
			{
				index = ColorIndex;
				ColorDictionary.Add(allocation.HeapId, ColorIndex);
				ColorIndex++;
				ColorIndex %= 4;
			}

			VisualMemoryBlock block = new VisualMemoryBlock(allocation, startAddress, addressWidth, width, index);

			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlocks.Add(allocation.Address, block);
			}

			return block;
		}

		public VisualMemoryBlock Remove(VisualMemoryBlock block)
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlocks.Remove(block.Allocation.Address);
				return block;
			}
		}

		public VisualMemoryBlock Remove(UInt64 address)
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlock block;
				if (VisualMemoryBlocks.TryGetValue(address, out block))
				{
					return Remove(block);
				}

				return null;
			}
		}

		public VisualMemoryBlock RemoveAt(int index)
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlock block = VisualMemoryBlocks.ElementAt(index).Value;
				return Remove(block);
			}
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
				GraphicsPath tempPath = (GraphicsPath)a.GraphicsPath.Clone();
				Matrix tempMatrix = new Matrix();

				if (MemoryBlockManager.Instance.HeapOffsets.ContainsKey(a.Allocation.HeapId))
				{
					tempMatrix.Translate(0.0f, -MemoryBlockManager.Instance.HeapOffsets[a.Allocation.HeapId]);
				}
				tempPath.Transform(tempMatrix);

				if (tempPath.IsVisible(b.GraphicsPath.PathPoints[0]))
				{
					return 0;
				}

				if (tempPath.PathPoints[0].Y + 1 < b.GraphicsPath.PathPoints[0].Y)
				{
					return -1;
				}
				else if (tempPath.PathPoints[0].Y > b.GraphicsPath.PathPoints[0].Y)
				{
					return 1;
				}
				else
				{
					if (tempPath.PathPoints[0].X < b.GraphicsPath.PathPoints[0].X)
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

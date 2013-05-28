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
	sealed class ReverseComparer<T> : IComparer<T>
	{
		private readonly IComparer<T> inner;
		public ReverseComparer() : this(null) { }
		public ReverseComparer(IComparer<T> inner)
		{
			this.inner = inner ?? Comparer<T>.Default;
		}
		int IComparer<T>.Compare(T x, T y) { return inner.Compare(y, x); }
	}

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

		// The dictionary is sorted in reverse order so that the Bounds getter
		// below runs in O(log n) rather than O(n)
		private SortedDictionary<UInt64, VisualMemoryBlock> VisualMemoryBlocks =
			new SortedDictionary<UInt64, VisualMemoryBlock>(new ReverseComparer<UInt64>());

		private Dictionary<uint, int> ColorDictionary = new Dictionary<uint, int>();
		private int ColorIndex;

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

				lock (VisualMemoryBlocks)
				{
					VisualMemoryBlock block = VisualMemoryBlocks.First().Value;
					RectangleF lowerBounds = block.Bounds;
					return new Rectangle(0, 0, block.MaxPixelWidth, (int)(lowerBounds.Bottom - TotalCompression));
				}
			}
		}

		public float TotalCompression
		{
			get
			{
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

		private static bool isSecondaryColor = false;

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

		public bool Add(VisualMemoryBlock block)
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlocks.Add(block.Allocation.Address, block);
				return true;
			}
		}

		public VisualMemoryBlock Add(Allocation allocation, UInt64 startAddress, UInt64 addressWidth, int width)
		{
			if (VisualMemoryBlocks.ContainsKey(allocation.Address))
			{
				return null;
			}

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

			Color color = Color.Red;
			if (allocation.HasColor)
			{
				color = allocation.Color;
			}
			else
			{
				switch (index)
				{
					case 0:
						if (!isSecondaryColor)
							color = Properties.Settings.Default.Heap1_Allocation1;
						else
							color = Properties.Settings.Default.Heap1_Allocation2;
						break;
					case 1:
						if (!isSecondaryColor)
							color = Properties.Settings.Default.Heap2_Allocation1;
						else
							color = Properties.Settings.Default.Heap2_Allocation2;
						break;
					case 2:
						if (!isSecondaryColor)
							color = Properties.Settings.Default.Heap3_Allocation1;
						else
							color = Properties.Settings.Default.Heap3_Allocation2;
						break;
					case 3:
						if (!isSecondaryColor)
							color = Properties.Settings.Default.Heap4_Allocation1;
						else
							color = Properties.Settings.Default.Heap4_Allocation2;
						break;
				}
				isSecondaryColor = !isSecondaryColor;
			}

			VisualMemoryBlock block = new VisualMemoryBlock(allocation, startAddress, addressWidth, width, color);
			allocation.Color = color;

			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlocks.Add(block.Allocation.Address, block);
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

		public bool Contains(VisualMemoryBlock block)
		{
			lock (VisualMemoryBlocks)
			{
				return VisualMemoryBlocks.ContainsValue(block);
			}
		}

		public VisualMemoryBlock Find(UInt64 address)
		{
			lock (VisualMemoryBlocks)
			{
				VisualMemoryBlock outBlock;
				if (VisualMemoryBlocks.TryGetValue(address, out outBlock))
				{
					return outBlock;
				}

				return null;
			}
		}

		public VisualMemoryBlock Find(Vector localMouseCoordinates)
		{
			VisualMemoryBlock tempBlock = new VisualMemoryBlock();
			tempBlock.GraphicsPath.AddLine(localMouseCoordinates.ToPoint(), (localMouseCoordinates + new Vector(1, 1)).ToPoint());

			lock (VisualMemoryBlocks)
			{
				// TODO: Investigate performance implications of ToList() on a SortedDictionary
				var values = VisualMemoryBlocks.Values.ToList();
				int index = values.BinarySearch(tempBlock, new VisualMemoryBlockComparer());
				if (index >= 0)
				{
					return values[index];
				}
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
					return 1;
				}
				else if (tempPath.PathPoints[0].Y > b.GraphicsPath.PathPoints[0].Y)
				{
					return -1;
				}
				else
				{
					if (tempPath.PathPoints[0].X < b.GraphicsPath.PathPoints[0].X)
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alloclave
{
	sealed class ReverseComparer<T> : IComparer<T>
	{
		readonly IComparer<T> inner;
		public ReverseComparer() : this(null) { }
		public ReverseComparer(IComparer<T> inner)
		{
			this.inner = inner ?? Comparer<T>.Default;
		}
		int IComparer<T>.Compare(T x, T y) { return inner.Compare(y, x); }
	}

	[DataContract()]
	internal sealed class Snapshot : IEnumerable<MemoryBlock>
	{
		// The dictionary is sorted in reverse order so that the Bounds getter
		// below runs in O(log n) rather than O(n)
		SortedDictionary<UInt64, MemoryBlock> MemoryBlocks =
			new SortedDictionary<UInt64, MemoryBlock>(new ReverseComparer<UInt64>());

		Dictionary<uint, int> ColorDictionary = new Dictionary<uint, int>();
		int ColorIndex;

		public int Count
		{
			get
			{
				return MemoryBlocks.Count;
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

				lock (MemoryBlocks)
				{
					// TODO: This needs to resolve to actual pixel width somehow
					const int maxPixelWidth = 200;

					MemoryBlock block = MemoryBlocks.First().Value;
					RectangleF lowerBounds = block.Bounds;
					return new Rectangle(0, 0, maxPixelWidth, (int)(lowerBounds.Bottom - TotalCompression));
				}
			}
		}

		public float TotalCompression
		{
			get
			{
				return 0;
			}
		}

		[DataMember]
		public int Position { get; set; }

		bool isSecondaryColor = false;

		public Snapshot()
		{

		}

		public Snapshot(Snapshot other)
		{
			MemoryBlocks = new SortedDictionary<UInt64, MemoryBlock>(
				other.MemoryBlocks, new ReverseComparer<UInt64>());
			ColorDictionary = new Dictionary<uint, int>(other.ColorDictionary);
			ColorIndex = other.ColorIndex;
			Position = other.Position;
			isSecondaryColor = other.isSecondaryColor;
		}

		public void Reset()
		{
			lock (MemoryBlocks)
			{
				MemoryBlocks.Clear();
			}
		}

		public bool Add(MemoryBlock block)
		{
			lock (MemoryBlocks)
			{
				MemoryBlocks.Add(block.Allocation.Address, block);
				return true;
			}
		}

		public MemoryBlock Add(Allocation allocation, UInt64 startAddress, UInt64 addressWidth)
		{
			if (MemoryBlocks.ContainsKey(allocation.Address))
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

			MemoryBlock block = new MemoryBlock(allocation, startAddress, addressWidth, color);
			allocation.Color = color;

			lock (MemoryBlocks)
			{
				MemoryBlocks.Add(block.Allocation.Address, block);

#if DEBUG
				// Look for sorting issues
				var values = MemoryBlocks.Values.ToList();
				values.Reverse();
				float lastAddress = 0;
				foreach (var value in values)
				{
					Debug.Assert(value.Bounds.Y >= lastAddress);
					lastAddress = value.Bounds.Y;
				}
#endif
			}

			return block;
		}

		public void Rebase(UInt64 startAddress, UInt64 addressWidth)
		{
			lock (MemoryBlocks)
			{
				foreach (var block in MemoryBlocks)
				{
					block.Value.Rebase(startAddress, addressWidth);
				}
			}
		}

		public MemoryBlock Remove(MemoryBlock block)
		{
			lock (MemoryBlocks)
			{
				MemoryBlocks.Remove(block.Allocation.Address);
				return block;
			}
		}

		public MemoryBlock Remove(UInt64 address)
		{
			lock (MemoryBlocks)
			{
				MemoryBlock block;
				if (MemoryBlocks.TryGetValue(address, out block))
				{
					return Remove(block);
				}

				return null;
			}
		}

		public bool Contains(UInt64 address)
		{
			lock (MemoryBlocks)
			{
				return MemoryBlocks.ContainsKey(address);
			}
		}

		public bool Contains(MemoryBlock block)
		{
			lock (MemoryBlocks)
			{
				return MemoryBlocks.ContainsValue(block);
			}
		}

		public MemoryBlock Find(UInt64 address)
		{
			lock (MemoryBlocks)
			{
				MemoryBlock outBlock;
				if (MemoryBlocks.TryGetValue(address, out outBlock))
				{
					return outBlock;
				}

				return null;
			}
		}

		public MemoryBlock Find(Vector localMouseCoordinates)
		{
			MemoryBlock tempBlock = new MemoryBlock();
			tempBlock.GraphicsPath.AddLine(localMouseCoordinates.ToPoint(), (localMouseCoordinates + new Vector(1, 1)).ToPoint());

			lock (MemoryBlocks)
			{
				// TODO: Investigate performance implications of ToList() on a SortedDictionary
				var values = MemoryBlocks.Values.ToList();
				int index = values.BinarySearch(tempBlock, new SnapshotComparer());
				if (index >= 0)
				{
					return values[index];
				}
			}

			return null;
		}

		public static Snapshot operator-(Snapshot left, Snapshot right)
		{
			HashSet<MemoryBlock> symmetricHash = new HashSet<MemoryBlock>(left.MemoryBlocks.Values);
			symmetricHash.SymmetricExceptWith(right.MemoryBlocks.Values);

			HashSet<MemoryBlock> intersectionHash = new HashSet<MemoryBlock>(left.MemoryBlocks.Values);
			intersectionHash.IntersectWith(right.MemoryBlocks.Values);

			// symmetricHash now contains the symmetric difference of left and right
			Snapshot finalSnapshot = new Snapshot();
			foreach (MemoryBlock block in symmetricHash)
			{
				// TODO: It's possible for different allocations with overlapping address
				// ranges to be in this hash map. How do we handle that?
				if (!finalSnapshot.Contains(block.Allocation.Address))
				{
					finalSnapshot.Add(block);
				}
			}

			// intersectionHash now contains the intersection of left and right
			foreach (MemoryBlock block in intersectionHash)
			{
				MemoryBlock tempBlock = new MemoryBlock(block);
				tempBlock.IsValid = false;
				tempBlock._Color = LerpColor(tempBlock._Color, Color.FromArgb(102, 102, 102), 0.1f);

				if (!finalSnapshot.Contains(tempBlock.Allocation.Address))
				{
					finalSnapshot.Add(tempBlock);
				}
			}

			return finalSnapshot;
		}

		// TODO: This can be a more generic lerp extension method
		private static Color LerpColor(Color start, Color end, float t)
		{
			byte r = (byte)((start.R * t) + end.R * (1 - t));
			byte g = (byte)((start.G * t) + end.G * (1 - t));
			byte b = (byte)((start.B * t) + end.B * (1 - t));

			return Color.FromArgb(r, g, b);
		}

		class SnapshotComparer : IComparer<MemoryBlock>
		{
			public int Compare(MemoryBlock a, MemoryBlock b)
			{
				GraphicsPath tempPath = (GraphicsPath)a.GraphicsPath.Clone();

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

		public IEnumerator<MemoryBlock> GetEnumerator()
		{
			foreach (var block in MemoryBlocks)
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

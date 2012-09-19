using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	/// <summary>
	/// Extends BinaryReader and transparently swaps endianness when necessary
	/// </summary>
	public class CustomBinaryReader : BinaryReader
	{
		Common.Endianness VisualizerEndianness;
		Common.Endianness TargetEndianness;

		public CustomBinaryReader(System.IO.Stream stream,
			Common.Endianness visualizerEndianness, Common.Endianness targetEndianness)
			: base(stream, Encoding.ASCII)
		{
			VisualizerEndianness = visualizerEndianness;
			TargetEndianness = targetEndianness;
		}

		public override int ReadInt32()
		{
			Int32 value = base.ReadInt32();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override Int16 ReadInt16()
		{
			Int16 value = base.ReadInt16();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override Int64 ReadInt64()
		{
			Int64 value = base.ReadInt64();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override UInt16 ReadUInt16()
		{
			UInt16 value = base.ReadUInt16();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override UInt32 ReadUInt32()
		{
			UInt32 value = base.ReadUInt32();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override UInt64 ReadUInt64()
		{
			UInt64 value = base.ReadUInt64();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override double ReadDouble()
		{
			double value = base.ReadDouble();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override float ReadSingle()
		{
			float value = base.ReadSingle();
			if (VisualizerEndianness != TargetEndianness)
			{
				Common.EndianSwap(ref value);
			}

			return value;
		}
		public override decimal ReadDecimal()
		{
			throw new NotSupportedException();
		}
	}
}

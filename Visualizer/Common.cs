using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Meminator
{
	public static class Common
	{
		public enum Architecture
		{
			_32Bit,
			_64Bit,
		}

		public enum Endianness
		{
			LittleEndian,
			BigEndian,
		}

		public static void EndianSwap(ref UInt16 value)
		{
			value = (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
		}

		public static void EndianSwap(ref UInt32 value)
		{
			value = (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
				   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
		}

		public static void EndianSwap(ref UInt64 value)
		{
			value = (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
				   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
				   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
				   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
		}

		public static void EndianSwap(ref Int16 value)
		{
			value = (Int16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
		}

		public static void EndianSwap(ref Int32 value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			bytes.Reverse();
			value = BitConverter.ToInt32(bytes, 0);
		}

		public static void EndianSwap(ref Int64 value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			bytes.Reverse();
			value = BitConverter.ToInt64(bytes, 0);
		}

		public static void EndianSwap(ref float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			bytes.Reverse();
			value = BitConverter.ToSingle(bytes, 0);
		}

		public static void EndianSwap(ref double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			bytes.Reverse();
			value = BitConverter.ToDouble(bytes, 0);
		}
	}
}

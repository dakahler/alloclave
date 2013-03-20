using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Drawing.Drawing2D;

namespace Alloclave
{
	public static class Common
	{
		public const String Version = "0.1";

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

		public static readonly String CompanyWebsiteUrl = "http://www.circularshift.com/";
		public static readonly String ProductWebsiteUrl = "http://www.alloclave.com/";

		#region Endian swap helpers
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
		#endregion

		#region Extension Methods
		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			else if (val.CompareTo(max) > 0)
			{
				return max;
			}
			else
			{
				return val;
			}
		}

		public static Vector ToVector(this System.Drawing.Point point)
		{
			return new Vector(point.X, point.Y);
		}

		public static System.Drawing.Point ToPoint(this Vector vector)
		{
			return new System.Drawing.Point((int)vector.X, (int)vector.Y);
		}

		public static void TransformVector(this Matrix matrix, ref Vector vector)
		{
			System.Drawing.Point[] points = { vector.ToPoint() };
			matrix.TransformPoints(points);
			vector = points[0].ToVector();
		}

		#endregion
	}
}

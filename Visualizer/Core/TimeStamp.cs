using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Alloclave
{
	public class TimeStamp : IComparable
	{
		public TimeStamp()
		{
			Time = (UInt64)Stopwatch.GetTimestamp();
			UID = UniquenessModifier++;
		}

		public TimeStamp(UInt64 time)
		{
			Time = time;
			UID = UniquenessModifier++;
		}

		public int CompareTo(object obj)
		{
			if (obj is TimeStamp)
			{
				TimeStamp other = obj as TimeStamp;
				if (this.Time != other.Time)
				{

					if (this.Time <= other.Time)
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else
				{
					if (this.UID <= other.UID)
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
			}

			throw new ArgumentException("object is not a TimeStamp");
		}

		public static bool operator <(TimeStamp emp1, TimeStamp emp2)
		{
			return Comparison(emp1, emp2) < 0;
		}

		public static bool operator >(TimeStamp emp1, TimeStamp emp2)
		{
			return Comparison(emp1, emp2) > 0;
		}

		public static bool operator ==(TimeStamp emp1, TimeStamp emp2)
		{
			return Comparison(emp1, emp2) == 0;
		}

		public static bool operator !=(TimeStamp emp1, TimeStamp emp2)
		{
			return Comparison(emp1, emp2) != 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TimeStamp)) return false;
			return this == (TimeStamp)obj;
		}

		public override int GetHashCode()
		{
			return Time.GetHashCode() * (int)UID * (int)UniquenessModifier;
		}

		public static bool operator <=(TimeStamp emp1, TimeStamp emp2)
		{
			return Comparison(emp1, emp2) <= 0;
		}

		public static bool operator >=(TimeStamp emp1, TimeStamp emp2)
		{
			return Comparison(emp1, emp2) >= 0;
		}

		public static int Comparison(TimeStamp emp1, TimeStamp emp2)
		{
			if (emp1.Time < emp2.Time)
			{
				return -1;
			}
			else if (emp1.Time == emp2.Time)
			{
				return 0;
			}
			else if (emp1.Time > emp2.Time)
			{
				return 1;
			}

			return 0;
		}

		public readonly UInt64 Time;
		private UInt64 UID;
		private static UInt64 UniquenessModifier = 0;
	}
}

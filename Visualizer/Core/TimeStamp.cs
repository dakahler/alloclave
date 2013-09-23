using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Alloclave
{
	[DataContract()]
	internal class TimeStamp : IComparable
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
			TimeStamp other = obj as TimeStamp;
			if (other != null)
			{
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

			Debug.Assert(false);
			return 0;
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
			if (System.Object.ReferenceEquals(emp1, emp2))
			{
				return 0;
			}

			if ((Object)emp1 == null || (Object)emp2 == null)
			{
				return -1;
			}

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

		[DataMember]
		public readonly UInt64 Time;

		[DataMember]
		private UInt64 UID;

		private static UInt64 UniquenessModifier = 0;
	}
}

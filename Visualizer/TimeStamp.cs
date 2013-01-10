using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	public class TimeStamp : IComparable
	{
		public TimeStamp()
		{
			Time = (UInt64)DateTime.Now.Ticks;
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

		public readonly UInt64 Time;
		private UInt64 UID;
		private static UInt64 UniquenessModifier = 0;
	}
}

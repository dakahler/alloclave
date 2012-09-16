using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meminator
{
	class TimeSlice
	{
		IPacket Data;
		Int64 TimeStamp;

		public TimeSlice(IPacket data)
		{
			Data = data;
			TimeStamp = DateTime.Now.Ticks;
		}

		public TimeSlice(IPacket data, Int64 timeStamp)
		{
			Data = data;
			TimeStamp = timeStamp;
		}
	}
}

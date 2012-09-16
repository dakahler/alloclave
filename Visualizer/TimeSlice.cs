using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meminator
{
	class TimeSlice
	{
		Packet Data;
		Int64 TimeStamp;

		public TimeSlice(Packet data)
		{
			Data = data;
			TimeStamp = DateTime.Now.Ticks;
		}

		public TimeSlice(Packet data, Int64 timeStamp)
		{
			Data = data;
			TimeStamp = timeStamp;
		}
	}
}

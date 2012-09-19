﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public class History
	{
		Dictionary<Type, Stack<TimeSlice>> DataDictionary = new Dictionary<Type, Stack<TimeSlice>>();

		public event EventHandler Updated;

		public History()
		{
			Array valuesArray = Enum.GetValues(typeof(PacketTypeRegistrar.PacketTypes));
			foreach (PacketTypeRegistrar.PacketTypes packetType in valuesArray)
			{
				DataDictionary.Add(PacketTypeRegistrar.GetType(packetType), new Stack<TimeSlice>());
			}
		}

		public void Add(IPacket packet, Int64 timeStamp)
		{
			TimeSlice timeSlice = new TimeSlice(packet, timeStamp);

			Stack<TimeSlice> data;
			if (DataDictionary.TryGetValue(packet.GetType(), out data))
			{
				data.Push(timeSlice);
			}
			else
			{
				throw new NotImplementedException();
			}

			EventArgs e = new EventArgs();
			Updated.Invoke(this, e);
		}
	}
}

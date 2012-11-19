using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public class History
	{
		Dictionary<Type, SortedList<TimeStamp, IPacket>> DataDictionary = new Dictionary<Type, SortedList<TimeStamp, IPacket>>();

		public event EventHandler Updated;

		public History()
		{
			Array valuesArray = Enum.GetValues(typeof(PacketTypeRegistrar.PacketTypes));
			foreach (PacketTypeRegistrar.PacketTypes packetType in valuesArray)
			{
				DataDictionary.Add(PacketTypeRegistrar.GetType(packetType), new SortedList<TimeStamp, IPacket>());
			}
		}

		public void Add(IPacket packet, UInt64 timeStamp)
		{
			SortedList<TimeStamp, IPacket> data;
			if (DataDictionary.TryGetValue(packet.GetType(), out data))
			{
				data.Add(new TimeStamp(timeStamp), packet);
			}
			else
			{
				throw new NotImplementedException();
			}

			if (Updated != null)
			{
				EventArgs e = new EventArgs();
				Updated.Invoke(this, e);
			}
		}

		public SortedList<TimeStamp, IPacket> Get(Type type)
		{
			SortedList<TimeStamp, IPacket> data;
			if (DataDictionary.TryGetValue(type, out data))
			{
				return data;
			}
			else
			{
				return new SortedList<TimeStamp, IPacket>();
			}
		}
	}
}

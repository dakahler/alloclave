using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public class History
	{
		ConcurrentDictionary<Type, SortedList<TimeStamp, IPacket>> DataDictionary = new ConcurrentDictionary<Type, SortedList<TimeStamp, IPacket>>();

		// Position tracker
		// TODO: Should this be integrated into a single dictionary somehow?
		ConcurrentDictionary<Type, int> PositionDictionary = new ConcurrentDictionary<Type, int>();

		// TODO: Might not be able to make these static
		public static event EventHandler Updated;
		public static bool SuspendRebuilding = false;
		private static History Instance = null;

		public History()
		{
			Instance = this;

			Array valuesArray = Enum.GetValues(typeof(PacketTypeRegistrar.PacketTypes));
			foreach (PacketTypeRegistrar.PacketTypes packetType in valuesArray)
			{
				DataDictionary.GetOrAdd(PacketTypeRegistrar.GetType(packetType), new SortedList<TimeStamp, IPacket>());
				PositionDictionary.GetOrAdd(PacketTypeRegistrar.GetType(packetType), 0);
			}
		}

		public void Add(IPacket packet, UInt64 timeStamp)
		{
			SortedList<TimeStamp, IPacket> data;
			if (DataDictionary.TryGetValue(packet.GetType(), out data))
			{
				lock (data)
				{
					data.Add(new TimeStamp(timeStamp), packet);
				}
			}
			else
			{
				throw new NotImplementedException();
			}

			if (Updated != null && !SuspendRebuilding)
			{
				EventArgs e = new EventArgs();
				Updated.Invoke(this, e);
			}
		}

		public List<KeyValuePair<TimeStamp, IPacket>> Get(Type type)
		{
			SortedList<TimeStamp, IPacket> data;
			if (DataDictionary.TryGetValue(type, out data))
			{
				return data.AsEnumerable().ToList();
			}
			else
			{
				return new List<KeyValuePair<TimeStamp, IPacket>>();
			}
		}

		public List<KeyValuePair<TimeStamp, IPacket>> GetNew(Type type)
		{
			lock (this)
			{
				SortedList<TimeStamp, IPacket> data;
				if (DataDictionary.TryGetValue(type, out data))
				{
					var finalList = data.Skip(PositionDictionary[type]).Take(data.Count - PositionDictionary[type]);
					PositionDictionary[type] = data.Count;
					return finalList.ToList();
				}
				else
				{
					return new List<KeyValuePair<TimeStamp, IPacket>>();
				}
			}
		}

		public static void ForceRebuild()
		{
			EventArgs e = new EventArgs();
			Updated.Invoke(Instance, e);
		}
	}
}

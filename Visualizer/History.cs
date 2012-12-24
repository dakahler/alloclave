using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public class History
	{
		Dictionary<Type, SortedList<TimeStamp, IPacket>> DataDictionary = new Dictionary<Type, SortedList<TimeStamp, IPacket>>();

		// Position tracker
		// TODO: Should this be integrated into a single dictionary somehow?
		Dictionary<Type, int> PositionDictionary = new Dictionary<Type, int>();

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
				DataDictionary.Add(PacketTypeRegistrar.GetType(packetType), new SortedList<TimeStamp, IPacket>());
				PositionDictionary.Add(PacketTypeRegistrar.GetType(packetType), 0);
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

			if (Updated != null && !SuspendRebuilding)
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

		public IEnumerable<KeyValuePair<TimeStamp, IPacket>> GetNew(Type type)
		{
			SortedList<TimeStamp, IPacket> data;
			if (DataDictionary.TryGetValue(type, out data))
			{
				var finalList = data.Skip(PositionDictionary[type]);
				PositionDictionary[type] = data.Count;
				return finalList;
			}
			else
			{
				return new SortedList<TimeStamp, IPacket>();
			}
		}

		public static void ForceRebuild()
		{
			EventArgs e = new EventArgs();
			Updated.Invoke(Instance, e);
		}
	}
}

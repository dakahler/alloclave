using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public static class PacketTypeRegistrar
	{
		// NOTE: To register a new packet type, add it to both the enum
		// AND the constructor below

		/// <summary>
		/// Provides cross-platform values for identifying packets to the visualizer
		/// </summary>
		public enum PacketTypes
		{
			Allocation = 0,
			Free,
			Screenshot,
		};

		static PacketTypeRegistrar()
		{
			Types.Add(PacketTypes.Allocation, typeof(Allocation));
			Types.Add(PacketTypes.Free, typeof(Free));
			Types.Add(PacketTypes.Screenshot, typeof(Screenshot));
		}


		public static IPacket Generate(PacketTypes type)
		{
			return (IPacket)Activator.CreateInstance(GetType(type));
		}

		public static Type GetType(PacketTypes type)
		{
			Type outType;
			if (Types.TryGetValue(type, out outType))
			{
				return outType;
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public static PacketTypes GetType(Type type)
		{
			foreach (var pair in Types)
			{
				if (pair.Value == type)
				{
					return pair.Key;
				}
			}

			throw new NotImplementedException();
		}

		static Dictionary<PacketTypes, Type> Types = new Dictionary<PacketTypes, Type>();
	}
}

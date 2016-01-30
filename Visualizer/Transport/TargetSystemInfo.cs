using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	public class TargetSystemInfo
	{
		public TargetSystemInfo()
		{

		}

		public TargetSystemInfo(String name, String host, int port,
			Common.Architecture architecture, Common.Endianness endianness)
		{
			Name = name;
			Host = host;
			Port = port;
			Architecture = architecture;
			Endianness = endianness;
		}

		public String Name { get; set; }
		public String Host { get; set; }
		public int Port { get; set; }
		public Common.Architecture Architecture { get; set; }
		public Common.Endianness Endianness { get; set; }
	}
}

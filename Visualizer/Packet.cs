using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meminator
{
	public interface Packet
	{
		void Parse(byte[] data);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meminator
{
	public class Allocation : Packet
	{
		UInt64 Address;
		UInt64 Size;
		CallStack Stack;
		int Alignment;
		String UserData;
		String Notes;

		public void Parse(byte[] data)
		{

		}
	}
}

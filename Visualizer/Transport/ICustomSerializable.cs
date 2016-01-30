using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public interface ICustomSerializable
	{
		byte[] Serialize(TargetSystemInfo targetSystemInfo);
		void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo);
	}
}

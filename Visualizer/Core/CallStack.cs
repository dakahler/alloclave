using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Alloclave
{
	[DataContract()]
	internal class CallStack : ICustomSerializable
	{
		[DataContract()]
		public class Frame
		{
			//public String FunctionSignature;
			//public String FilePath;
			//public uint LineNumber;

			[DataMember]
			public UInt64 Address;
		}

		[DataMember]
		public Stack<Frame> Frames = new Stack<Frame>();

		public CallStack()
		{

		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
			Debug.Assert(binaryReader != null);
			Debug.Assert(targetSystemInfo != null);

			if (targetSystemInfo.Architecture == Common.Architecture._32Bit)
			{
				uint stackDepth = binaryReader.ReadUInt32();
				for (int i = 0; i < stackDepth; i++)
				{
					Frame newFrame = new Frame();
					newFrame.Address = (UInt64)binaryReader.ReadUInt32();
					Frames.Push(newFrame);
				}
			}
			else
			{
				uint stackDepth = binaryReader.ReadUInt32();
				for (int i = 0; i < stackDepth; i++)
				{
					Frame newFrame = new Frame();
					newFrame.Address = binaryReader.ReadUInt64();
					Frames.Push(newFrame);
				}
			}
		}
	}
}

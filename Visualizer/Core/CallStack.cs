using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public class CallStack : ICustomSerializable
	{
		public class Frame
		{
			public String FunctionSignature;
			public String FilePath;
			public uint LineNumber;
			public UInt64 Address;
		}

		public Stack<Frame> Frames = new Stack<Frame>();

		public CallStack()
		{

		}

		public CallStack(String callStack)
		{
			//Frames = Parser.Parse(callStack);
		}

		// TODO: Call stack parser plugin system
		//static void RegisterParser(ICallStackParser parser)
		//{
		//	Parser = parser;
		//}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{
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
				UInt64 stackDepth = binaryReader.ReadUInt64();
				for (UInt64 i = 0; i < stackDepth; i++)
				{
					Frame newFrame = new Frame();
					newFrame.Address = binaryReader.ReadUInt64();
					Frames.Push(newFrame);
				}
			}
		}

		//static ICallStackParser Parser;
	}
}

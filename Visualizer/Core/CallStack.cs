using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	public abstract class CallStack : ICustomSerializable
	{
		public static String SymbolsPath;

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

		public abstract String TranslateAddress(UInt64 address);

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
					//newFrame.FunctionSignature = TranslateAddress(newFrame.Address);
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
					//newFrame.FunctionSignature = TranslateAddress(newFrame.Address);
					Frames.Push(newFrame);
				}
			}
		}

		//static ICallStackParser Parser;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Alloclave
{
	class CallStack : ICustomSerializable
	{
		public class Frame
		{
			String FunctionSignature;
			String FilePath;
			uint LineNumber;
		}

		Stack<Frame> Frames = new Stack<Frame>();

		public CallStack(String callStack)
		{
			Frames = Parser.Parse(callStack);
		}

		static void RegisterParser(ICallStackParser parser)
		{
			Parser = parser;
		}

		public byte[] Serialize(TargetSystemInfo targetSystemInfo)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(BinaryReader binaryReader, TargetSystemInfo targetSystemInfo)
		{

		}

		static ICallStackParser Parser;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meminator
{
	class CallStack
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

		static ICallStackParser Parser;
	}
}

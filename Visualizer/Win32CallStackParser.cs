using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	class Win32CallStackParser : ICallStackParser
	{
		public Stack<CallStack.Frame> Parse(String callStack)
		{
			Stack<CallStack.Frame> finalStack = new Stack<CallStack.Frame>();
			return finalStack;
		}
	}
}

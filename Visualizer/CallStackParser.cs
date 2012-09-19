using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alloclave
{
	interface ICallStackParser
	{
		Stack<CallStack.Frame> Parse(String callStack);
	}
}

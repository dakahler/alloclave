// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_CALLSTACK_WIN32_H
#define _ALLOCLAVE_CALLSTACK_WIN32_H

#include "CallStack.h"

namespace Alloclave
{

	// Win32-specific specialization of stack walking
	class CallStack_Win32 : public CallStack
	{
	public:
		CallStack_Win32();

		virtual void Rebuild();
	};

};

#endif // _ALLOCLAVE_CALLSTACK_WIN32_H

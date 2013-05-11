// Copyright Circular Shift. For license information, see license.txt.

#include <windows.h>
#include <winnt.h>
#include "CallStack_Win32.h"
#include <dbghelp.h>

namespace Alloclave
{

CallStack_Win32::CallStack_Win32()
{

}

void CallStack_Win32::Rebuild()
{
	CallStack::Rebuild();

	void* stackFrames[MaxStackDepth];
	int stackDepth = 0;

#ifdef _M_IX86
	CONTEXT lContext;
	ZeroMemory(&lContext, sizeof(CONTEXT));
	RtlCaptureContext(&lContext);

	STACKFRAME64 lFrameStack;
	ZeroMemory(&lFrameStack, sizeof(STACKFRAME64));

	DWORD lTypeMachine;

	lTypeMachine                 = IMAGE_FILE_MACHINE_I386;
	lFrameStack.AddrPC.Offset    = lContext.Eip;
	lFrameStack.AddrPC.Mode      = AddrModeFlat;
	lFrameStack.AddrFrame.Offset = lContext.Ebp;
	lFrameStack.AddrFrame.Mode   = AddrModeFlat;
	lFrameStack.AddrStack.Offset = lContext.Esp;
	lFrameStack.AddrStack.Mode   = AddrModeFlat;

	for (; stackDepth < MaxStackDepth; stackDepth++)
	{
		if( !StackWalk64( lTypeMachine, GetCurrentProcess(), GetCurrentThread(), &lFrameStack, &lContext,
			NULL, SymFunctionTableAccess64, SymGetModuleBase64, NULL ) )
		{
			break;
		}
		if (lFrameStack.AddrPC.Offset != 0)
		{
			stackFrames[stackDepth] = (void*)lFrameStack.AddrPC.Offset;
		}
		else
		{
			break;
		}
	}
#elif _M_X64
	// StackWalk64 is finicky on x64, so use CaptureStackBackTrace instead
	ZeroMemory(stackFrames, sizeof(UINT_PTR) * MaxStackDepth);
	stackDepth = CaptureStackBackTrace(0, MaxStackDepth, (PVOID*)stackFrames, NULL);
#endif

	// De-rebase
	for (int i = 0; i < stackDepth; i++)
	{
		DWORD64 currentAddress = (DWORD64)stackFrames[i];

		MEMORY_BASIC_INFORMATION lInfoMemory;
		VirtualQuery((PVOID)currentAddress, &lInfoMemory, sizeof(lInfoMemory));
		DWORD64 lBaseAllocation = reinterpret_cast<DWORD64>(lInfoMemory.AllocationBase);

		DWORD64 finalAddress = currentAddress - lBaseAllocation;
		Push((void*)finalAddress);
	}

	
}

}

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

	// The below is the platform-specific implementation for walking the stack
	// The StackWalk64 method used for x86 seems to be problemic for x64,
	// so the CaptureBackTrace method is used instead. There may be
	// faster alternatives to both of these which deserve more investigation.

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
	ZeroMemory(stackFrames, sizeof(UINT_PTR) * MaxStackDepth);
	stackDepth = CaptureStackBackTrace(0, MaxStackDepth, (PVOID*)stackFrames, NULL);
#endif

	// The raw stack addresses obtained above have probably been modified from what's
	// stored in the symbol database. At the very least, they've probably been
	// rebased so that each logical address space is unique. The below strips out
	// the rebasing, leaving an address that will correctly resolve to a symbol
	// in the visualization tool.
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

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

	ZeroMemory(stackFrames, sizeof(UINT_PTR) * MaxStackDepth);
	stackDepth = CaptureStackBackTrace(0, MaxStackDepth, (PVOID*)stackFrames, NULL);

	// The raw stack addresses obtained above have probably been modified from what's
	// stored in the symbol database. At the very least, they've probably been
	// rebased so that each logical address space is unique. The below strips out
	// the rebasing, leaving an address that will correctly resolve to a symbol
	// in the visualization tool.
	MEMORY_BASIC_INFORMATION lInfoMemory;
	VirtualQuery((PVOID)stackFrames[0], &lInfoMemory, sizeof(lInfoMemory));
	DWORD64 lBaseAllocation = reinterpret_cast<DWORD64>(lInfoMemory.AllocationBase);
	for (int i = 0; i < stackDepth; i++)
	{
		DWORD64 currentAddress = (DWORD64)stackFrames[i];
		DWORD64 finalAddress = currentAddress - lBaseAllocation;
		Push((void*)finalAddress);
	}

	
}

}

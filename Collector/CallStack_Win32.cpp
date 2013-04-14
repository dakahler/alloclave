
// TODO: Wrap windows headers
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

	CONTEXT lContext;
	ZeroMemory(&lContext, sizeof(CONTEXT));
	RtlCaptureContext(&lContext);

	STACKFRAME64 lFrameStack;
	ZeroMemory(&lFrameStack, sizeof(STACKFRAME64));

	DWORD lTypeMachine;

#ifdef _M_IX86
	lTypeMachine                 = IMAGE_FILE_MACHINE_I386;
	lFrameStack.AddrPC.Offset    = lContext.Eip;
	lFrameStack.AddrPC.Mode      = AddrModeFlat;
	lFrameStack.AddrFrame.Offset = lContext.Ebp;
	lFrameStack.AddrFrame.Mode   = AddrModeFlat;
	lFrameStack.AddrStack.Offset = lContext.Esp;
	lFrameStack.AddrStack.Mode   = AddrModeFlat;
#elif _M_X64
	lTypeMachine                 = IMAGE_FILE_MACHINE_AMD64;
	lFrameStack.AddrPC.Offset    = lContext.Rip;
	lFrameStack.AddrPC.Mode      = AddrModeFlat;
	lFrameStack.AddrFrame.Offset = lContext.Rsp;
	lFrameStack.AddrFrame.Mode   = AddrModeFlat;
	lFrameStack.AddrStack.Offset = lContext.Rsp;
	lFrameStack.AddrStack.Mode   = AddrModeFlat;
#endif

	for (int i = DWORD(); i < MaxStackDepth; i++)
	{
		if( !StackWalk64( lTypeMachine, GetCurrentProcess(), GetCurrentThread(), &lFrameStack, lTypeMachine == IMAGE_FILE_MACHINE_I386 ? 0 : &lContext,
			nullptr, &SymFunctionTableAccess64, &SymGetModuleBase64, nullptr ) )
		{
			break;
		}
		if (lFrameStack.AddrPC.Offset != 0)
		{
			MEMORY_BASIC_INFORMATION lInfoMemory;
			VirtualQuery((PVOID)lFrameStack.AddrPC.Offset, &lInfoMemory, sizeof(lInfoMemory));
			DWORD64 lBaseAllocation = reinterpret_cast<DWORD64>(lInfoMemory.AllocationBase);

			DWORD64 finalAddress = lFrameStack.AddrPC.Offset - lBaseAllocation;
			Push((void*)finalAddress);
		}
		else
		{
			break;
		}
	}

	// Alternate implementation
	//StackDepth = CaptureStackBackTrace(0, MaxStackDepth, StackAddresses, NULL);

	//// De-rebase
	//for (int i = 0; i < StackDepth; i++)
	//{
	//	StackAddresses[i] = (void*)((unsigned int)StackAddresses[i] - (unsigned int)&__ImageBase);
	//}
}

}

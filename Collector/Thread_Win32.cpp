#include <windows.h>
#include "Thread_Win32.h"

namespace Alloclave
{

	static CRITICAL_SECTION CriticalSection;

	Thread_Win32::Thread_Win32(unsigned long (__stdcall *func)(void*))
		: Thread(func)
	{
		InitializeCriticalSection(&CriticalSection);
		ThreadHandle = CreateThread(NULL, 0, func, this, 0, 0);
	}

	Thread_Win32::~Thread_Win32()
	{
		
	}

	void Thread_Win32::Start()
	{
		ResumeThread(ThreadHandle);
	}

	void Thread_Win32::Stop()
	{
		TerminateThread(ThreadHandle, 0);
	}

	void Thread_Win32::Suspend()
	{
		SuspendThread(ThreadHandle);
	}

	void Thread_Win32::Resume()
	{
		ResumeThread(ThreadHandle);
	}

	void Thread_Win32::Sleep(int milliseconds)
	{
		SleepEx(milliseconds, false);
	}

	void Thread_Win32::StartCriticalSection()
	{
		EnterCriticalSection(&CriticalSection);
	}

	void Thread_Win32::EndCriticalSection()
	{
		LeaveCriticalSection(&CriticalSection);
	}

}
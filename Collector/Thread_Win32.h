#ifndef _ALLOCLAVE_THREAD_WIN32_H
#define _ALLOCLAVE_THREAD_WIN32_H

#include "Thread.h"

namespace Alloclave
{

	// Win32 specialized threading implementation
	class Thread_Win32 : public Thread
	{
	public:
		Thread_Win32(unsigned long (__stdcall *func)(void*));

		virtual ~Thread_Win32();

		void Start();
		void Stop();

		void Suspend();
		void Resume();

		void Sleep(int milliseconds);

		void StartCriticalSection();
		void EndCriticalSection();

	private:

		void* ThreadHandle;
	};

};

#endif // _ALLOCLAVE_THREAD_WIN32_H

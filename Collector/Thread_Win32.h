// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_THREAD_WIN32_H
#define _ALLOCLAVE_THREAD_WIN32_H

#include "Thread.h"

namespace Alloclave
{
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

	private:

		void* ThreadHandle;
	};

};

#endif // _ALLOCLAVE_THREAD_WIN32_H

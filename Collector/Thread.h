// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_THREAD_H
#define _ALLOCLAVE_THREAD_H

namespace Alloclave
{

	// Base class for platform-specifc threading
	class Thread
	{
	public:
		Thread(unsigned long (__stdcall *func)(void*)) : UserFunction(func)
		{
			
		}

		virtual ~Thread() {}

		virtual void Start() {}
		virtual void Stop() {}

		virtual void Suspend() {}
		virtual void Resume() {}

		virtual void Sleep(int milliseconds) {}

		virtual void StartCriticalSection() {}
		virtual void EndCriticalSection() {}

	protected:

		unsigned long (__stdcall *UserFunction)(void*);
	};

};

#endif // _ALLOCLAVE_THREAD_H

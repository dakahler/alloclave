#ifndef _ALLOCLAVE_THREAD_H
#define _ALLOCLAVE_THREAD_H

namespace Alloclave
{
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

	protected:

		unsigned long (__stdcall *UserFunction)(void*);
	};

};

#endif // _ALLOCLAVE_THREAD_H

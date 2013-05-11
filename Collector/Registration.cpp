// Copyright Circular Shift. For license information, see license.txt.

#include "Registration.h"
#include "Allocation.h"
#include "Free.h"
#include "Screenshot.h"
#include "SetSymbols.h"

// Built-in transports
#include "Win32Transport.h"

// Built-in stack walkers
#include "CallStack_Win32.h"

// Built-in threads
#include "Thread_Win32.h"

namespace Alloclave
{

#if ALLOCLAVE_TRANSPORT_TYPE == TRANSPORT_TYPE_WIN32
	static Win32Transport s_SpecificTransport;
	static Transport* s_Transport = &s_SpecificTransport;
#elif ALLOCLAVE_TRANSPORT_TYPE == TRANSPORT_TYPE_CUSTOM
	static Transport* s_Transport = NULL;
#endif

	static unsigned long __stdcall FlushThread(void* param)
	{
		Thread* thread = (Thread*)param;

		assert(thread);

		while (thread)
		{
			if (s_Transport != NULL)
			{
				s_Transport->Flush();

				thread->Sleep(1000);
			}
		}

		return 1;
	}
	
#ifdef _WIN32
	static CallStack_Win32 s_PlatformCallStack;

	#if ALLOCLAVE_ENABLED && ALLOCLAVE_USE_THREADS
		static Thread_Win32 s_PlatformThread(FlushThread);
	#else
		static Thread s_PlatformThread(FlushThread);
	#endif
#else
	static CallStack s_PlatformCallStack;
	static Thread s_PlatformThread(FlushThread);

	#if ALLOCLAVE_USE_THREADS
		#error Threads not implemented for this platform
	#endif
#endif

	static CallStack* s_CallStack = &s_PlatformCallStack;
	static Thread* s_Thread = &s_PlatformThread;

	void RegisterTransport(Transport* transport)
	{
		s_Transport = transport;
	}

#if ALLOCLAVE_ENABLED
	void RegisterAllocation(void* address, size_t size, unsigned int alignment, unsigned int heapId)
	{
		Allocation allocation(*s_CallStack);
		allocation.Address = address;
		allocation.Size = size;
		allocation.Alignment = alignment;
		allocation.Type = Allocation::AllocationType_Allocation;
		allocation.HeapId = heapId;
		Transport::Send(allocation);
	}

	void RegisterHeap(void* address, unsigned int size, unsigned int alignment, unsigned int heapId)
	{
		Allocation heap;
		heap.Address = address;
		heap.Size = size;
		heap.Alignment = alignment;
		heap.Type = Allocation::AllocationType_Heap;
		heap.HeapId = heapId;
		Transport::Send(heap);
	}

	void RegisterFree(void* address, unsigned int heapId)
	{
		Free _free;
		_free.Address = address;
		_free.HeapId = heapId;
		Transport::Send(_free);
	}

	void RegisterScreenshot()
	{
		// TODO
	}

	void RegisterCallStackParser(CallStack* parser)
	{
		s_CallStack = parser;
	}

	void RegisterSymbolsPath(const char* symbolsPath)
	{
		SetSymbols setSymbols(symbolsPath);
		Transport::Send(setSymbols);
	}
#else
	void RegisterAllocation(void* /*address*/, size_t /*size*/, unsigned int /*alignment*/, unsigned int /*heapId*/) {}
	void RegisterHeap(void* /*address*/, unsigned int /*size*/, unsigned int /*alignment*/, unsigned int /*heapId*/) {}
	void RegisterFree(void* /*address*/, unsigned int /*heapId*/) {}
	void RegisterScreenshot() {}
	void RegisterCallStackParser(CallStack* /*parser*/) {}
	void RegisterSymbolsPath(const char* /*symbolsPath*/) {}
#endif // ALLOCLAVE_ENABLED

};

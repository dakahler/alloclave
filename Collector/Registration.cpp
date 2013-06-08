// Copyright Circular Shift. For license information, see license.txt.

// Needed to find symbols in win32
#ifdef _WIN32
#include <windows.h>
#endif

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

	static Transport& GetTransport()
	{
		static ALLOCLAVE_TRANSPORT_TYPE s_Transport;
		return s_Transport;
	}

	static Thread& GetThreadModel();

	static unsigned long __stdcall FlushThread(void* param)
	{
		Thread* thread = (Thread*)param;

		const int FlushInterval = 1000; // ms

		#ifdef _WIN32
			// Attempt to find the symbols automatically in win32
			char path[MAX_PATH];
			GetModuleFileName(NULL, path, sizeof(path));
			size_t pathLength = strlen(path);
			if (pathLength > 3)
			{
				strcpy(path + (pathLength - 3), "pdb");
			}
			RegisterSymbolsPath(path);
		#endif

		assert(thread);
		while (thread)
		{
			GetTransport().Flush(GetThreadModel());
			thread->Sleep(FlushInterval);
		}

		return 1;
	}

	static CallStack& GetCallstack()
	{
		static ALLOCLAVE_CALLSTACK_TYPE s_PlatformCallStack;
		return s_PlatformCallStack;
	}

	static Thread& GetThreadModel()
	{
		static ALLOCLAVE_THREAD_TYPE s_ThreadModel(FlushThread);
		return s_ThreadModel;
	}

#if ALLOCLAVE_ENABLED
	void RegisterAllocation(void* address, size_t size, unsigned int heapId)
	{
		// Callstack and Transport are not thread safe
		GetThreadModel().StartCriticalSection();

		Allocation allocation(GetCallstack());
		allocation.Address = address;
		allocation.Size = size;
		allocation.Alignment = 4; // Placeholder
		allocation.Type = Allocation::AllocationType_Allocation;
		allocation.HeapId = heapId;
		Transport::Send(allocation);

		GetThreadModel().EndCriticalSection();
	}

	void RegisterHeap(void* address, unsigned int size, unsigned int heapId)
	{
		GetThreadModel().StartCriticalSection();

		Allocation heap;
		heap.Address = address;
		heap.Size = size;
		heap.Alignment = 4; // Placeholder
		heap.Type = Allocation::AllocationType_Heap;
		heap.HeapId = heapId;
		Transport::Send(heap);

		GetThreadModel().EndCriticalSection();
	}

	void RegisterFree(void* address, unsigned int heapId)
	{
		GetThreadModel().StartCriticalSection();

		Free _free(GetCallstack());
		_free.Address = address;
		_free.HeapId = heapId;
		Transport::Send(_free);

		GetThreadModel().EndCriticalSection();
	}

	void RegisterScreenshot()
	{
		// TODO
	}

	void RegisterSymbolsPath(const char* symbolsPath)
	{
		GetThreadModel().StartCriticalSection();

		SetSymbols setSymbols(symbolsPath);
		Transport::Send(setSymbols);

		GetThreadModel().EndCriticalSection();
	}
#else
	void RegisterAllocation(void* /*address*/, size_t /*size*/, unsigned int /*heapId*/) {}
	void RegisterHeap(void* /*address*/, unsigned int /*size*/, unsigned int /*heapId*/) {}
	void RegisterFree(void* /*address*/, unsigned int /*heapId*/) {}
	void RegisterScreenshot() {}
	void RegisterSymbolsPath(const char* /*symbolsPath*/) {}
#endif // ALLOCLAVE_ENABLED

};

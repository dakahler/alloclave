
#include "Registration.h"
#include "Allocation.h"
#include "Free.h"
#include "Screenshot.h"
#include "SetSymbols.h"

// Built-in transports
#include "Win32Transport.h"

// Built-in stack walkers
#include "CallStack_Win32.h"

namespace Alloclave
{
#if ALLOCLAVE_TRANSPORT_TYPE == TRANSPORT_TYPE_WIN32
	static Win32Transport s_SpecificTransport;
	static Transport* s_Transport = &s_SpecificTransport;
#elif ALLOCLAVE_TRANSPORT_TYPE == TRANSPORT_TYPE_CUSTOM
	static Transport* s_Transport = NULL;
#endif
	
#ifdef _WIN32
	static CallStack_Win32 s_PlatformCallStack;
#else
	static CallStack s_PlatformCallStack;
#endif

	static CallStack* s_CallStack = &s_PlatformCallStack;

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

		// TODO: Temporary location
		if (s_Transport != NULL)
		{
			s_Transport->Flush();
		}
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

		// TODO: Temporary location
		if (s_Transport != NULL)
		{
			s_Transport->Flush();
		}
	}

	void RegisterFree(void* address, unsigned int heapId)
	{
		Free _free;
		_free.Address = address;
		_free.HeapId = heapId;
		Transport::Send(_free);

		// TODO: Temporary location
		if (s_Transport != NULL)
		{
			s_Transport->Flush();
		}
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

		// TODO: Temporary location
		if (s_Transport != NULL)
		{
			s_Transport->Flush();
		}
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

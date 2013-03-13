
#include "Registration.h"
#include "Allocation.h"
#include "Screenshot.h"
#include "Transport.h"
#include "CallStack_Win32.h"

namespace Alloclave
{
	static Transport* s_Transport = NULL;
	
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

	void RegisterAllocation(void* address, unsigned int size, unsigned int alignment, unsigned int heapId)
	{
		if (s_Transport == NULL)
		{
			return;
		}

		Allocation allocation(*s_CallStack);
		allocation.Address = address;
		allocation.Size = size;
		allocation.Alignment = alignment;
		allocation.Type = Allocation::AllocationType_Allocation;
		allocation.HeapId = heapId;
		s_Transport->Send(allocation);
	}

	void RegisterHeap(void* address, unsigned int size, unsigned int alignment, unsigned int heapId)
	{
		Allocation heap;
		heap.Address = address;
		heap.Size = size;
		heap.Alignment = alignment;
		heap.Type = Allocation::AllocationType_Heap;
		heap.HeapId = heapId;
		s_Transport->Send(heap);
	}

	void RegisterFree(void* address)
	{
		if (s_Transport == NULL)
		{
			return;
		}

		// TODO
	}

	void RegisterScreenshot()
	{
		if (s_Transport == NULL)
		{
			return;
		}

		// TODO
	}

	void RegisterCallStackParser(CallStack* parser)
	{
		s_CallStack = parser;
	}

};

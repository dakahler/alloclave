
#include "Registration.h"
#include "Allocation.h"
#include "Screenshot.h"
#include "Transport.h"

namespace Alloclave
{
	static Transport* s_Transport = NULL;

	void RegisterTransport(Transport* transport)
	{
		s_Transport = transport;
	}

	void RegisterAllocation(void* address, unsigned int size, unsigned int alignment, unsigned short heapId)
	{
		if (s_Transport == NULL)
		{
			return;
		}

		Allocation allocation;
		allocation.Address = address;
		allocation.Size = size;
		allocation.Alignment = alignment;
		allocation.Type = Allocation::AllocationType_Allocation;
		allocation.HeapId = heapId;
		s_Transport->Send(allocation);
	}

	void RegisterHeap(void* address, unsigned int size, unsigned int alignment, unsigned short heapId)
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

};

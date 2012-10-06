#include "Registration.h"
#include "Allocation.h"
#include "Screenshot.h"

namespace Alloclave
{
	static Transport* s_Transport = NULL;

	void RegisterTransport(Transport* transport)
	{
		s_Transport = transport;
	}

	void RegisterAllocation(void* address, unsigned int size, unsigned int alignment)
	{
		if (s_Transport == NULL)
		{
			return;
		}

		Allocation allocation;
		allocation.Address = address;
		allocation.Size = size;
		allocation.Alignment = alignment;
		s_Transport->Send(allocation);
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

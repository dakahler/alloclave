
#pragma once

#include "HeapWalker_Common.h"

namespace Alloclave_Plugin {

	private ref class HeapWalker_Base abstract
	{
	public:
		virtual System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ GetHeapNodes(long pid) = 0;
	};
}

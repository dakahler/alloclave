
#pragma once

#include "HeapWalker.h"

namespace Alloclave_Plugin {

	private ref class HeapWalker_Fast : public HeapWalker_Base
	{
	public:
		virtual System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ GetHeapNodes(long pid) override;
	};
}

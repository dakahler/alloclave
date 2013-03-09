
#pragma once

#include "HeapWalker.h"

namespace Alloclave_Plugin {

	private ref class HeapWalker_Standard : public HeapWalker_Base
	{
	public:
		virtual System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ GetHeapNodes(long pid) override;

		static System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ allocationList;
	};
}

// HeapWalker.h

#pragma once

namespace Alloclave_Plugin {

	public ref class AllocationData
	{
	public:
		System::UInt64 Address;
		System::UInt64 Size;
	};

	public ref class HeapWalker
	{
	public:
		System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ GetHeapData(System::UInt64 pid);
	private:
		
	};
}

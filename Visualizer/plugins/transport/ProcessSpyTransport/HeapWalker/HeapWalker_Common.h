
#pragma once

namespace Alloclave_Plugin {

	// TODO: This duplicates Alloclave::Allocation
	// Find a way to use that instead
	public ref class AllocationData : public System::IComparable
	{
	public:
		System::UInt64 Address;
		System::UInt64 Size;
		System::UInt32 HeapId;

		virtual int CompareTo(System::Object^ other)
		{
			AllocationData^ allocationData = (AllocationData^)other;

			if (Address < allocationData->Address)
			{
				return -1;
			}
			else if (Address > allocationData->Address)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}
	};

	public ref class AllocationDataEqualityComparer : public System::Collections::Generic::IEqualityComparer<AllocationData^>
	{
	public:
		virtual bool Equals(AllocationData^ first, AllocationData^ second)
		{
			return first->Address == second->Address && first->Size == second->Size;
		}

		virtual int GetHashCode(AllocationData^ allocationData)
		{
			int hCode = (int)(allocationData->Address * allocationData->Size);
			return hCode.GetHashCode();
		}

	};

	public enum class Technique
	{
		Standard,
		Fast,
	};

	public ref class HeapWalker
	{
	public:
		System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ GetHeapData(System::UInt64 pid, Technique technique);
	private:

	};
}


#include "HeapWalker_Common.h"
#include "HeapWalker_Standard.h"
#include "HeapWalker_Fast.h"

namespace Alloclave_Plugin
{

System::Collections::Generic::List<Alloclave_Plugin::AllocationData^>^ Alloclave_Plugin::HeapWalker::GetHeapData(System::UInt64 pid, Technique technique)
{
	switch (technique)
	{
	case Technique::Standard:
		return (gcnew HeapWalker_Standard())->GetHeapNodes((long)pid);
	case Technique::Fast:
		return (gcnew HeapWalker_Fast())->GetHeapNodes((long)pid);
	default:
		throw gcnew System::NotSupportedException();
	};
}

}

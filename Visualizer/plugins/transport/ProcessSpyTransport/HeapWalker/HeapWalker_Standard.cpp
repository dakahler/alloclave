
#include "HeapWalker_Standard.h"

#include <windows.h>
#include <stdio.h>
#include <malloc.h>
#include <tlhelp32.h>
#include <stdlib.h>
#include <avrfsdk.h>

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;

namespace Alloclave_Plugin
{

	


	typedef ULONG (WINAPI * VerifierEnumerateResourceFn)(
		HANDLE Process,
		ULONG Flags,
		ULONG ResourceType,
		AVRF_RESOURCE_ENUMERATE_CALLBACK ResourceCallback,
		PVOID EnumerationContext
		);

	struct HEAP_BYTES {
		ULONG64 hProcessHeap;
		ULONG64 NumBytes;
	};

	ULONG WINAPI HeapAllocCallback(
		PAVRF_HEAP_ALLOCATION HeapAllocationStruct,
		PVOID EnumerationContext,
		PULONG EnumerationLevel
		)
	{
		HEAP_BYTES * pHeapBytes = (HEAP_BYTES *)EnumerationContext;

		//if (HeapAllocationStruct->HeapHandle == pHeapBytes->hProcessHeap &&
		//	(HeapAllocationStruct->UserAllocationState & ~HeapStateMask) ==
		//	AllocationStateBusy)
		if ((HeapAllocationStruct->UserAllocationState & ~HeapStateMask) ==
			AllocationStateBusy)
		{
			//pHeapBytes->NumBytes += HeapAllocationStruct->UserAllocationSize;

			Alloclave_Plugin::AllocationData^ newAllocation = gcnew Alloclave_Plugin::AllocationData();
			newAllocation->Address = HeapAllocationStruct->Allocation;
			newAllocation->Size = HeapAllocationStruct->AllocationSize;
			newAllocation->HeapId = (unsigned int)HeapAllocationStruct->HeapHandle;

			HeapWalker_Standard::allocationList->Add(newAllocation);
		}
		return 0;
	}


	List<Alloclave_Plugin::AllocationData^>^ HeapWalker_Standard::GetHeapNodes(long pid)
	{
		allocationList = gcnew List<Alloclave_Plugin::AllocationData^>();

		HINSTANCE hInst = LoadLibraryExW(L"verifier.dll",0,0);
		if (NULL == hInst){
			return nullptr;
		}
		//OnDelete<HINSTANCE,BOOL(*)(HINSTANCE),FreeLibrary> fl(hInst);

		VerifierEnumerateResourceFn VerifierEnumerateResource_;
		*(FARPROC *)&VerifierEnumerateResource_ =
			GetProcAddress(hInst,"VerifierEnumerateResource");
		if (NULL == VerifierEnumerateResource_){
			return nullptr;
		}

		HEAP_BYTES HeapBytes = {
			(ULONG64)(ULONG_PTR)GetProcessHeap(),
			0
		};

		VerifierEnumerateResource_(OpenProcess(PROCESS_ALL_ACCESS, TRUE, pid),
			0,
			AvrfResourceHeapAllocation,
			(AVRF_RESOURCE_ENUMERATE_CALLBACK)HeapAllocCallback,
			&HeapBytes);


		return allocationList;






		HEAPLIST32 hl;

		HANDLE hHeapSnap = CreateToolhelp32Snapshot(TH32CS_SNAPHEAPLIST, pid);

		hl.dwSize = sizeof(HEAPLIST32);

		if ( hHeapSnap == INVALID_HANDLE_VALUE )
		{
			return nullptr;
		}

		

		if( Heap32ListFirst( hHeapSnap, &hl ) )
		{
			int counter = 0;
			do
			{
				HEAPENTRY32 he;
				ZeroMemory(&he, sizeof(HEAPENTRY32));
				he.dwSize = sizeof(HEAPENTRY32);

				if( Heap32First( &he, pid, hl.th32HeapID ) )
				{
					//printf( "\nHeap ID: %d\n", hl.th32HeapID );
					Alloclave_Plugin::AllocationData^ lastAllocation = nullptr;
					do
					{
						if (he.dwFlags & LF32_FIXED)
						{
							Alloclave_Plugin::AllocationData^ newAllocation = gcnew Alloclave_Plugin::AllocationData();
							newAllocation->Address = he.dwAddress;
							newAllocation->Size = he.dwBlockSize;
							newAllocation->HeapId = counter;

							allocationList->Add(newAllocation);

							if (lastAllocation != nullptr)
							{
								if (Math::Abs((__int64)((UInt64)lastAllocation->Address - (UInt64)newAllocation->Address)) > 100000)
								{
									int x;
									x=0;
								}
							}

							lastAllocation = newAllocation;
						}

						//printf( "Block size: %d\n", he.dwBlockSize );

						he.dwSize = sizeof(HEAPENTRY32);
					} while( Heap32Next(&he) );
				}
				hl.dwSize = sizeof(HEAPLIST32);

				counter++;
			} while (Heap32ListNext( hHeapSnap, &hl ));
		}
		else
		{
			//printf ("Cannot list first heap (%d)\n", GetLastError());
		}

		CloseHandle(hHeapSnap);

		return allocationList;



		//// Create debug buffer
		//PDEBUG_BUFFER db = RtlCreateQueryDebugBuffer(0, FALSE);

		//// Get process heap data
		//NTSTATUS status = RtlQueryProcessDebugInformation( pid, PDI_HEAPS | PDI_HEAP_BLOCKS, db);

		//ULONG heapNodeCount = db->HeapInformation ? *PULONG(db->HeapInformation):0;
		//List<Alloclave_Plugin::AllocationData^>^ allocationList = gcnew List<Alloclave_Plugin::AllocationData^>();
		//if (heapNodeCount > 0)
		//{

		//	PDEBUG_HEAP_INFORMATION heapInfo = PDEBUG_HEAP_INFORMATION(PULONG(db-> HeapInformation) + 1);

		//	// Go through each of the heap nodes and dispaly the information
		//	for (unsigned int i = 0; i < heapNodeCount; i++)
		//	{
		//		List<Alloclave_Plugin::AllocationData^>^ newList = GetHeapBlocks(pid, heapInfo[i].Base);
		//		allocationList->AddRange(newList);
		//	}
		//}

		//// Clean up the buffer
		//RtlDestroyQueryDebugBuffer( db );

		//return allocationList;
	}

}

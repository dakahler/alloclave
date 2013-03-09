
#include "HeapWalker_Fast.h"

#include <windows.h>
#include <stdio.h>
#include <malloc.h>
#include <tlhelp32.h>
#include <stdlib.h>

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;

//Debug Buffer used by RtlCreateQueryDebugBuffer
typedef struct _DEBUG_BUFFER
{
	HANDLE SectionHandle;
	PVOID SectionBase;
	PVOID RemoteSectionBase;
	ULONG SectionBaseDelta;
	HANDLE EventPairHandle;
	ULONG Unknown[2];
	HANDLE RemoteThreadHandle;
	ULONG InfoClassMask;
	ULONG SizeOfInfo;
	ULONG AllocatedSize;
	ULONG SectionSize;
	PVOID ModuleInformation;
	PVOID BackTraceInformation;
	PVOID HeapInformation;
	PVOID LockInformation;
	PVOID Reserved[8];
} DEBUG_BUFFER, *PDEBUG_BUFFER;

//This represents each heap node
typedef struct _DEBUG_HEAP_INFORMATION
{
	ULONG Base; // 0x00
	ULONG Flags; // 0x04
	USHORT Granularity; // 0x08
	USHORT Unknown; // 0x0A
	ULONG Allocated; // 0x0C
	ULONG Committed; // 0x10
	ULONG TagCount; // 0x14
	ULONG BlockCount; // 0x18
	ULONG Reserved[7]; // 0x1C
	PVOID Tags; // 0x38
	PVOID Blocks; // 0x3C
} DEBUG_HEAP_INFORMATION, *PDEBUG_HEAP_INFORMATION;

//Internal structure used to store heap block information.
struct HeapBlock
{
	PVOID dwAddress;
	DWORD dwSize;
	DWORD dwFlags;
	ULONG reserved;
};

// This is the internal structure used by the CRT for each heap node block
struct HeapBlockInternal
{
	DWORD size;
	DWORD flag;
	DWORD unknown;
	DWORD address;
};

#define HEAP_FLAG_NOSERIALIZE	0x1
#define HEAP_FLAG_GROWABLE		0x2
#define HEAP_FLAG_EXCEPTIONS	0x4
#define HEAP_FLAG_NONDEFAULT	0x1000
#define HEAP_FLAG_SHAREABLE		0x8000
#define HEAP_FLAG_EXECUTABLE	0x40000

#define PDI_MODULES                       0x01
#define PDI_BACKTRACE                     0x02
#define PDI_HEAPS                         0x04
#define PDI_HEAP_TAGS                     0x08
#define PDI_HEAP_BLOCKS                   0x10
#define PDI_LOCKS                         0x20

extern "C"
	__declspec(dllimport)
	NTSTATUS
	__stdcall
	RtlQueryProcessDebugInformation(
	IN ULONG  ProcessId,
	IN ULONG  DebugInfoClassMask,
	IN OUT PDEBUG_BUFFER  DebugBuffer);

extern "C"
	__declspec(dllimport)
	PDEBUG_BUFFER
	__stdcall
	RtlCreateQueryDebugBuffer(
	IN ULONG  Size,
	IN BOOLEAN  EventPair);
extern "C"
	__declspec(dllimport)
	NTSTATUS
	__stdcall
	RtlDestroyQueryDebugBuffer(
	IN PDEBUG_BUFFER  DebugBuffer);


namespace
{
	BOOL GetFirstHeapBlock( PDEBUG_HEAP_INFORMATION curHeapNode, HeapBlock *hb)
	{
		HeapBlockInternal* block;

		hb->reserved = 0;
		hb->dwAddress = 0;
		hb->dwFlags = 0;

		block = (HeapBlockInternal*)curHeapNode->Blocks;

		block->unknown = 4;

		while (block->flag & HEAP_FLAG_GROWABLE)
		{
			hb->reserved++;
			hb->dwAddress = (void *) ( (block->address) + curHeapNode->Granularity );
			block++;
			hb->dwSize = block->size;
		}

		// Update the flags...
		USHORT flags = (block->flag);

		if( ( flags & 0xF1 ) != 0 || ( flags & 0x0200 ) != 0 )
			hb->dwFlags = 1;
		else if( (flags & 0x20) != 0 )
			hb->dwFlags = 4;
		else if( (flags & 0x0100) != 0 )
			hb->dwFlags = 2;

		return TRUE;
	}

	BOOL GetNextHeapBlock( PDEBUG_HEAP_INFORMATION curHeapNode, HeapBlock *hb)
	{
		HeapBlockInternal* block;

		hb->reserved++;
		block = (HeapBlockInternal*)curHeapNode->Blocks;

		if (hb->reserved >= curHeapNode->BlockCount - 1)
			return FALSE;

		// Make it point to next block address entry
		block += hb->reserved;

		__try
		{
			if (block->flag & HEAP_FLAG_GROWABLE)
			{
				do
				{
					// new address = curBlockAddress + Granularity ;
					hb->dwAddress = (void *) ( (block->address) + curHeapNode->Granularity );

					// If all the blocks have been enumerated....exit
					if (hb->reserved >= curHeapNode->BlockCount - 1)
						return FALSE;

					hb->reserved++;
					block++; //move to next block
					hb->dwSize = block->size;
				}
				while (block->flag & HEAP_FLAG_GROWABLE);
			}
			else
			{
				// New Address = prev Address + prev block size ;
				hb->dwAddress = (void*) ( (int)hb->dwAddress + hb->dwSize );
				hb->dwSize = block->size;
			}

			// Update the flags...
			USHORT flags = block->flag;

			if( ( flags & 0xF1 ) != 0 || ( flags & 0x0200 ) != 0 )
				hb->dwFlags = 1;
			else if( (flags & 0x20) != 0 )
				hb->dwFlags = 4;
			else if( (flags & 0x0100) != 0 )
				hb->dwFlags = 2;
		}
		__except (EXCEPTION_EXECUTE_HANDLER)
		{
			printf("Exception!\n");
			return FALSE;
		}

		return TRUE;
	}

	List<Alloclave_Plugin::AllocationData^>^ GetHeapBlocks(DWORD pid, DWORD nodeAddress)
	{
		List<Alloclave_Plugin::AllocationData^>^ allocationList = gcnew List<Alloclave_Plugin::AllocationData^>();

		HeapBlock hb = {0,0,0,0};

		// Create debug buffer
		PDEBUG_BUFFER db = RtlCreateQueryDebugBuffer(0, FALSE);

		// Get process heap data
		LONG ret = RtlQueryProcessDebugInformation( pid, PDI_HEAPS | PDI_HEAP_BLOCKS, db);

		HANDLE hCrtHeap = GetProcessHeap();

		ULONG heapNodeCount = db->HeapInformation ? *PULONG(db->HeapInformation) : 0;

		PDEBUG_HEAP_INFORMATION heapInfo = PDEBUG_HEAP_INFORMATION(PULONG(db->HeapInformation) + 1);

		// Go through each of the heap nodes
		for (unsigned int i = 0; i < heapNodeCount; i++)
		{
			if(heapInfo[i].Base == nodeAddress)
			{
				// Now enumerate all blocks within this heap node
				memset(&hb,0,sizeof(hb));

				if( GetFirstHeapBlock(&heapInfo[i] , &hb) )
				{
					do
					{
						if( hb.dwFlags == LF32_FREE )
						{
							// Do nothing for now
						}
						else
						{
							Alloclave_Plugin::AllocationData^ newAllocation = gcnew Alloclave_Plugin::AllocationData();
							newAllocation->Address = (UInt64)hb.dwAddress;
							newAllocation->Size = (UINT64)hb.dwSize;

							// TODO: Might need to be more intelligent about IDs
							newAllocation->HeapId = i;

							allocationList->Add(newAllocation);
						}
					}
					while( GetNextHeapBlock( &heapInfo[i], &hb) );
				}
				break;
			}
		}

		// Clean up the buffer
		RtlDestroyQueryDebugBuffer( db );

		return allocationList;
	}
}

namespace Alloclave_Plugin
{

List<Alloclave_Plugin::AllocationData^>^ HeapWalker_Fast::GetHeapNodes(long pid)
{
	// Create debug buffer
	PDEBUG_BUFFER db = RtlCreateQueryDebugBuffer(0, FALSE);

	// Get process heap data
	NTSTATUS status = RtlQueryProcessDebugInformation( pid, PDI_HEAPS | PDI_HEAP_BLOCKS, db);

	ULONG heapNodeCount = db->HeapInformation ? *PULONG(db->HeapInformation):0;
	List<Alloclave_Plugin::AllocationData^>^ allocationList = gcnew List<Alloclave_Plugin::AllocationData^>();
	if (heapNodeCount > 0)
	{
		PDEBUG_HEAP_INFORMATION heapInfo = PDEBUG_HEAP_INFORMATION(PULONG(db-> HeapInformation) + 1);

		// Go through each of the heap nodes and dispaly the information
		for (unsigned int i = 0; i < heapNodeCount; i++)
		{
			List<Alloclave_Plugin::AllocationData^>^ newList = GetHeapBlocks(pid, heapInfo[i].Base);
			allocationList->AddRange(newList);
		}
	}

	// Clean up the buffer
	RtlDestroyQueryDebugBuffer( db );

	return allocationList;
}

}

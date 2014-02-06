// Copyright Circular Shift.
// For license information, see license.txt.

// This file is an amalgamation of the original source.
// If you need the original source for easier modification,
// email support@circularshift.com.

#include "alloclave.h"

// Begin File: Allocation.cpp



namespace Alloclave
{

Allocation::Allocation()
	: CallStackParser(NullCallStack)
{
	Address = NULL;
	Size = 0;
	Alignment = 0;
	Type = AllocationType_Allocation;
	HeapId = 0;
}

Allocation::Allocation(CallStack& callStackParser)
	: CallStackParser(callStackParser)
{
	Address = NULL;
	Size = 0;
	Alignment = 0;
	Type = AllocationType_Allocation;
	HeapId = 0;

	// Generate the call stack here
	CallStackParser.Rebuild();
}

Buffer& Allocation::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// Push the allocation data into the buffer
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&Address, sizeof(Address));
	buffer.Add((void*)&Size, sizeof(Size));
	buffer.Add((void*)&Alignment, sizeof(Alignment));
	buffer.Add((void*)&Type, sizeof(char));
	buffer.Add((void*)&HeapId, sizeof(HeapId));
	buffer.Add(CallStackParser.Serialize());

	return buffer;
}

void Allocation::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

Packet::PacketType Allocation::GetPacketType() const
{
	return PacketType_Allocation;
}

}



// End File: Allocation.cpp


// Begin File: Buffer.cpp

#include <memory.h>

namespace Alloclave
{

Buffer::Buffer()
{
	Data = NULL;
	Position = 0;
	CurrentSize = 0;
	Resize(DefaultSize);
}

Buffer::Buffer(unsigned int initialSize)
{
	Data = NULL;
	Position = 0;
	CurrentSize = 0;
	Resize(initialSize);
}

Buffer::Buffer(const Buffer& other)
{
	Copy(other, *this);
}

Buffer& Buffer::operator=(const Buffer& rhs)
{
	if (this == &rhs)
	{
		return *this;
	}

	Copy(rhs, *this);

	return *this;
}

void Buffer::Copy(const Buffer& from, Buffer& to)
{
	to.CurrentSize = from.CurrentSize;
	to.Position = from.Position;
	to.Data = NULL;

	assert(to.CurrentSize);
	if (to.CurrentSize > 0)
	{
		to.Data = (char*)real_malloc(to.CurrentSize);
		assert(to.Data);
		if (to.Data)
		{
			memcpy(to.Data, from.Data, to.CurrentSize);
		}
		else
		{
			to.CurrentSize = 0;
			to.Position = 0;
		}
	}
}

Buffer::~Buffer()
{
	real_free(Data);
	Data = NULL;
}

void Buffer::Resize(unsigned int newSize)
{
	if (Data == NULL)
	{
		Data = (char*)real_malloc(newSize);
		assert(Data);
	}
	else
	{
		// Allocate the new size, copy the data, and free the old memory
		char* newData = (char*)real_malloc(newSize);
		assert(newData);
		unsigned int numBytesToCopy = MIN(newSize, CurrentSize);
		memcpy(newData, Data, numBytesToCopy);
		real_free(Data);
		Data = newData;
	}

	CurrentSize = newSize;
}

void Buffer::Add(void* data, unsigned int dataSize)
{
	assert(dataSize > 0);

	while (Position + dataSize >= CurrentSize)
	{
		// Grow exponentially to accomodate new data
		Resize(CurrentSize * 2);
	}

	memcpy(Data + Position, data, dataSize);
	Position += dataSize;
}

void Buffer::Add(const Buffer& buffer)
{
	Add(buffer.Data, buffer.Position);
}

void Buffer::Clear()
{
	Position = 0;
}

const void* Buffer::GetData() const
{
	return Data;
}

unsigned int Buffer::GetSize() const
{
	return Position;
}

}



// End File: Buffer.cpp


// Begin File: CallStack.cpp


namespace Alloclave
{

CallStack::CallStack()
{
	StackDepth = 0;
}

void CallStack::Rebuild()
{
	StackDepth = 0;
}

Buffer& CallStack::Serialize() const
{
	static Buffer buffer;
	buffer.Clear();

	// Run through each stack frame and add its address to the buffer
	buffer.Add((void*)&StackDepth, sizeof(StackDepth));
	for (unsigned int i = 0; i < StackDepth; i++)
	{
		buffer.Add((void*)&StackAddresses[i], sizeof(StackAddresses[i]));
	}

	return buffer;
}

void CallStack::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

void CallStack::Push(void* address)
{
	assert(StackDepth < MaxStackDepth);

	StackAddresses[StackDepth] = address;
	StackDepth++;
}

}



// End File: CallStack.cpp


// Begin File: CallStack_Win32.cpp

#include <windows.h>
#include <winnt.h>
#include <dbghelp.h>

namespace Alloclave
{

CallStack_Win32::CallStack_Win32()
{

}

void CallStack_Win32::Rebuild()
{
	CallStack::Rebuild();

	void* stackFrames[MaxStackDepth];
	int stackDepth = 0;

	ZeroMemory(stackFrames, sizeof(UINT_PTR) * MaxStackDepth);
	stackDepth = CaptureStackBackTrace(0, MaxStackDepth, (PVOID*)stackFrames, NULL);

	// The raw stack addresses obtained above have probably been modified from what's
	// stored in the symbol database. At the very least, they've probably been
	// rebased so that each logical address space is unique. The below strips out
	// the rebasing, leaving an address that will correctly resolve to a symbol
	// in the visualization tool.
	MEMORY_BASIC_INFORMATION lInfoMemory;
	VirtualQuery((PVOID)stackFrames[0], &lInfoMemory, sizeof(lInfoMemory));
	DWORD64 lBaseAllocation = (DWORD64)lInfoMemory.AllocationBase;
	for (int i = 0; i < stackDepth; i++)
	{
		DWORD64 currentAddress = (DWORD64)stackFrames[i];
		DWORD64 finalAddress = currentAddress - lBaseAllocation;
		Push((void*)finalAddress);
	}

	
}

}



// End File: CallStack_Win32.cpp


// Begin File: Free.cpp



namespace Alloclave
{

Free::Free()
	: CallStackParser(NullCallStack)
{
	Address = NULL;
	HeapId = 0;
}

Free::Free(CallStack& callStackParser)
	: CallStackParser(callStackParser)
{
	Address = NULL;
	HeapId = 0;

	CallStackParser.Rebuild();
}

Buffer& Free::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// Push the free data into the buffer
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&Address, sizeof(Address));
	buffer.Add((void*)&HeapId, sizeof(HeapId));
	buffer.Add(CallStackParser.Serialize());

	return buffer;
}

void Free::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

Packet::PacketType Free::GetPacketType() const
{
	return PacketType_Free;
}

}



// End File: Free.cpp


// Begin File: MemoryOverrides.cpp

#include <new>


#if ALLOCLAVE_ENABLED && ALLOCLAVE_OVERRIDE_NEWDELETE

void* operator new(size_t size)
{
	void* p = malloc(size);
	if (p == NULL)
		throw std::bad_alloc();

	Alloclave::RegisterAllocation(p, size);

	return p;
}

void* operator new[](size_t size)
{
	void* p = malloc(size);
	if (p == NULL)
		throw std::bad_alloc();

	Alloclave::RegisterAllocation(p, size);

	return p;
}

void operator delete(void* p)
{
	if (p)
	{
		Alloclave::RegisterFree(p);
		free(p);
	}
}

void operator delete[](void* p)
{
	if (p)
	{
		Alloclave::RegisterFree(p);
		free(p);
	}
}

void operator delete(void* p, const char* /*file*/, int /*line*/)
{
	if (p)
	{
		Alloclave::RegisterFree(p);
		free(p);
	}
}

void operator delete[](void* p, const char* /*file*/, int /*line*/)
{
	if (p)
	{
		Alloclave::RegisterFree(p);
		free(p);
	}
}

#endif // ALLOCLAVE_ENABLED && ALLOCLAVE_OVERRIDE_NEWDELETE

// Since these are above the malloc/free redefines below,
// they call the original system versions of these functions,
// serving as wrappers to intercept the allocation data
// TODO: Better setup to prevent linkage problems if
// a stdlib gets included before this header

namespace Alloclave
{
	void* real_malloc(size_t size)
	{
		void* p = malloc(size);
		return p;
	}

	void* real_realloc(void* p, size_t size)
	{
		void* newP = realloc(p, size);
		return newP;
	}

	void real_free(void* p)
	{
		if (p)
		{
			free(p);
		}
	}

	void* _malloc(size_t size)
	{
		void* p = real_malloc(size);
		RegisterAllocation(p, size);
		return p;
	}

	void* _realloc(void* p, size_t size)
	{
		RegisterFree(p);
		void* newP = real_realloc(p ,size);
		RegisterAllocation(newP, size);
		return newP;
	}

	void _free(void* p)
	{
		if (p)
		{
			RegisterFree(p);
			real_free(p);
		}
	}
};



// End File: MemoryOverrides.cpp


// Begin File: Packet.cpp

#include <time.h>

namespace Alloclave
{

Buffer& Packet::Serialize() const
{
	unsigned char packetType = (unsigned char)GetPacketType();
	__int64 timeStamp = clock();

	// All the various packet buffers are local statics as a way
	// of reusing data and minimizing dynamic allocations
	static Buffer buffer;

	buffer.Clear();

	buffer.Add(&packetType, sizeof(packetType));
	buffer.Add(&timeStamp, sizeof(timeStamp));

	return buffer;
}

}



// End File: Packet.cpp


// Begin File: Registration.cpp

// Needed to find symbols in win32
#ifdef _WIN32
#include <windows.h>
#endif


// Built-in transports

// Built-in stack walkers

// Built-in threads

namespace Alloclave
{

	static Transport& GetTransport()
	{
		static ALLOCLAVE_TRANSPORT_TYPE s_Transport;
		return s_Transport;
	}

	static Thread& GetThreadModel();

	static unsigned long __stdcall FlushThread(void* param)
	{
		Thread* thread = (Thread*)param;

		const int FlushInterval = 1000; // ms

		#ifdef _WIN32
			// Attempt to find the symbols automatically in win32
			char path[MAX_PATH];
			GetModuleFileName(NULL, path, sizeof(path));
			size_t pathLength = strlen(path);
			if (pathLength > 3)
			{
				strcpy(path + (pathLength - 3), "pdb");
			}
			RegisterSymbolsPath(path);
		#endif

		assert(thread);
		while (thread)
		{
			GetTransport().Flush(GetThreadModel());
			thread->Sleep(FlushInterval);
		}

		return 1;
	}

	static CallStack& GetCallstack()
	{
		static ALLOCLAVE_CALLSTACK_TYPE s_PlatformCallStack;
		return s_PlatformCallStack;
	}

	static Thread& GetThreadModel()
	{
		static ALLOCLAVE_THREAD_TYPE s_ThreadModel(FlushThread);
		return s_ThreadModel;
	}

#if ALLOCLAVE_ENABLED
	void RegisterAllocation(void* address, size_t size, unsigned int heapId)
	{
		// Callstack and Transport are not thread safe
		GetThreadModel().StartCriticalSection();

		Allocation allocation(GetCallstack());
		allocation.Address = address;
		allocation.Size = size;
		allocation.Alignment = 4; // Placeholder
		allocation.Type = Allocation::AllocationType_Allocation;
		allocation.HeapId = heapId;
		Transport::Send(allocation);

		GetThreadModel().EndCriticalSection();
	}

	void RegisterHeap(void* address, unsigned int size, unsigned int heapId)
	{
		GetThreadModel().StartCriticalSection();

		Allocation heap;
		heap.Address = address;
		heap.Size = size;
		heap.Alignment = 4; // Placeholder
		heap.Type = Allocation::AllocationType_Heap;
		heap.HeapId = heapId;
		Transport::Send(heap);

		GetThreadModel().EndCriticalSection();
	}

	void RegisterFree(void* address, unsigned int heapId)
	{
		GetThreadModel().StartCriticalSection();

		Free _free(GetCallstack());
		_free.Address = address;
		_free.HeapId = heapId;
		Transport::Send(_free);

		GetThreadModel().EndCriticalSection();
	}

	void RegisterScreenshot()
	{
		// TODO
	}

	void RegisterSymbolsPath(const char* symbolsPath)
	{
		GetThreadModel().StartCriticalSection();

		SetSymbols setSymbols(symbolsPath);
		Transport::Send(setSymbols);

		GetThreadModel().EndCriticalSection();
	}
#else
	void RegisterAllocation(void* /*address*/, size_t /*size*/, unsigned int /*heapId*/) {}
	void RegisterHeap(void* /*address*/, unsigned int /*size*/, unsigned int /*heapId*/) {}
	void RegisterFree(void* /*address*/, unsigned int /*heapId*/) {}
	void RegisterScreenshot() {}
	void RegisterSymbolsPath(const char* /*symbolsPath*/) {}
#endif // ALLOCLAVE_ENABLED

};



// End File: Registration.cpp


// Begin File: Screenshot.cpp


namespace Alloclave
{

Screenshot::Screenshot()
{

}

Buffer& Screenshot::Serialize() const
{
	static Buffer buffer;
	return buffer;
}

void Screenshot::Deserialize(const Buffer&/* buffer*/, unsigned int /*bufferLength*/)
{

}

Packet::PacketType Screenshot::GetPacketType() const
{
	return PacketType_Screenshot;
}

}



// End File: Screenshot.cpp


// Begin File: SetArchitecture.cpp



namespace Alloclave
{

SetArchitecture::SetArchitecture()
{
	
}

Buffer& SetArchitecture::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// This sends along the number of bytes in a pointer
	// so the visualizer can auto-detect 32- or 64-bit
	unsigned short pointerSize = (unsigned short)sizeof(void*);
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&pointerSize, sizeof(pointerSize));

	return buffer;
}

void SetArchitecture::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

Packet::PacketType SetArchitecture::GetPacketType() const
{
	return PacketType_SetArchitecture;
}

}



// End File: SetArchitecture.cpp


// Begin File: SetSymbols.cpp

#include <string.h>


namespace Alloclave
{

SetSymbols::SetSymbols(const char* symbolsPath)
	: SymbolsPath(symbolsPath)
{
	
}

Buffer& SetSymbols::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// This sends the path of the symbols file to the visualizer
	unsigned short stringLength = (unsigned short)strlen(SymbolsPath);
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&stringLength, sizeof(stringLength));
	buffer.Add((void*)SymbolsPath, stringLength);

	return buffer;
}

void SetSymbols::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

Packet::PacketType SetSymbols::GetPacketType() const
{
	return PacketType_SetSymbols;
}

}



// End File: SetSymbols.cpp


// Begin File: Thread_Win32.cpp

#include <windows.h>

namespace Alloclave
{

	static CRITICAL_SECTION CriticalSection;

	Thread_Win32::Thread_Win32(unsigned long (__stdcall *func)(void*))
		: Thread(func)
	{
		InitializeCriticalSection(&CriticalSection);
		ThreadHandle = CreateThread(NULL, 0, func, this, 0, 0);
	}

	Thread_Win32::~Thread_Win32()
	{
		
	}

	void Thread_Win32::Start()
	{
		ResumeThread(ThreadHandle);
	}

	void Thread_Win32::Stop()
	{
		TerminateThread(ThreadHandle, 0);
	}

	void Thread_Win32::Suspend()
	{
		SuspendThread(ThreadHandle);
	}

	void Thread_Win32::Resume()
	{
		ResumeThread(ThreadHandle);
	}

	void Thread_Win32::Sleep(int milliseconds)
	{
		SleepEx(milliseconds, false);
	}

	void Thread_Win32::StartCriticalSection()
	{
		EnterCriticalSection(&CriticalSection);
	}

	void Thread_Win32::EndCriticalSection()
	{
		LeaveCriticalSection(&CriticalSection);
	}

}


// End File: Thread_Win32.cpp


// Begin File: Transport.cpp


namespace Alloclave
{

static bool g_SentArchtecturePacket = false;

Buffer& GetGlobalBuffer()
{
	static Buffer buffer;
	return buffer;
}

unsigned int Transport::NumItems = 0;
//Queue Transport::PacketQueue;

Transport::Transport()
{
	
}

Transport::~Transport()
{

}

void Transport::Send(const Packet& packet)
{
	if (!g_SentArchtecturePacket)
	{
		g_SentArchtecturePacket = true;
		SetArchitecture architecturePacket;
		GetGlobalBuffer().Add(architecturePacket.Serialize());
		NumItems++;
	}

	GetGlobalBuffer().Add(packet.Serialize());
	NumItems++;

	//PacketQueue.Enqueue(buffer);
}

Buffer& Transport::BuildFinalBuffer(Thread& callingThread)
{
	// This builds up the final packet bundle
	static Buffer buffer;
	buffer.Clear();

	buffer.Add((void*)&Version, sizeof(Version));

	callingThread.StartCriticalSection();
	buffer.Add((void*)&NumItems, sizeof(NumItems));
	buffer.Add((void*)GetGlobalBuffer().GetData(), GetGlobalBuffer().GetSize());

	NumItems = 0;
	GetGlobalBuffer().Clear();
	callingThread.EndCriticalSection();

	return buffer;
}

}



// End File: Transport.cpp


// Begin File: Win32Transport.cpp

#include <windows.h>

namespace Alloclave
{

Win32Transport::Win32Transport()
{
	VisualizerHandle = NULL;
}

Win32Transport::~Win32Transport()
{
	VisualizerHandle = NULL;
}

void Win32Transport::Flush(Thread& callingThread)
{
	if (NumItems == 0)
	{
		return;
	}

	FindVisualizer();
	if (VisualizerHandle == NULL)
	{
		return;
	}

	Buffer& buffer = BuildFinalBuffer(callingThread);

	// Send Win32 WM_COPYDATA message to visualizer
	COPYDATASTRUCT cds;
	cds.dwData = ALLOCLAVE_WIN32_ID;
	cds.cbData = buffer.GetSize();
	cds.lpData = (void*)buffer.GetData(); // TODO: constness
	SendMessage((HWND)VisualizerHandle, WM_COPYDATA, (WPARAM)VisualizerHandle, (LPARAM)(LPVOID)&cds);
}

void Win32Transport::FindVisualizer()
{
	if (VisualizerHandle == NULL)
	{
		VisualizerHandle = FindWindow(NULL, ALLOCLAVE_WIN32_GUID);
	}
}

}



// End File: Win32Transport.cpp



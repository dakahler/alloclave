// Copyright Circular Shift.
// For license information, see license.txt.

// This file is an amalgamation of the original source.
// If you need the original source for easier modification,
// email support@circularshift.com.

// Begin File: Alloclave.h

#ifndef _ALLOCLAVE_H
#define _ALLOCLAVE_H

// Set this to 0 to disable Alloclave completely
#define ALLOCLAVE_ENABLED				1

// Set this to 0 if you have your own new/delete overrides
#define ALLOCLAVE_OVERRIDE_NEWDELETE	1

// Set this to 0 if you have your own malloc/free overrides,
// or do not want to override malloc/free
#define ALLOCLAVE_OVERRIDE_MALLOC		0

// Set this to 0 if you want to call Flush() manually
#define ALLOCLAVE_USE_THREADS			1

// The below defines should only be modified if you need to implement custom behavior
#define TRANSPORT_TYPE_WIN32			Win32Transport
#define ALLOCLAVE_TRANSPORT_TYPE		TRANSPORT_TYPE_WIN32

#define CALLSTACK_TYPE_WIN32			CallStack_Win32
#define ALLOCLAVE_CALLSTACK_TYPE		CALLSTACK_TYPE_WIN32

#define THREAD_TYPE_WIN32				Thread_Win32
#define ALLOCLAVE_THREAD_TYPE			THREAD_TYPE_WIN32

#endif // _ALLOCLAVE_H



// End File: Alloclave.h


// Begin File: Constants.h

#ifndef _ALLOCLAVE_CONSTANTS_H
#define _ALLOCLAVE_CONSTANTS_H

#define ALLOCLAVE_WIN32_ID		0x729a04e2
#define ALLOCLAVE_WIN32_GUID	"857E3F44-91FB-456B-9D53-03B75C751B28"


#endif // _ALLOCLAVE_CONSTANTS_H



// End File: Constants.h


// Begin File: MemoryOverrides.h

#ifndef _ALLOCLAVE_MEMORYOVERRIDES_H
#define _ALLOCLAVE_MEMORYOVERRIDES_H


#ifdef __cplusplus

namespace Alloclave
{
	extern void* _malloc(size_t size);
	extern void* _realloc(void* p, size_t size);
	extern void _free(void* p);

	extern void* real_malloc(size_t size);
	extern void* real_realloc(void* p, size_t size);
	extern void real_free(void* p);
};


#if ALLOCLAVE_ENABLED

	#if ALLOCLAVE_OVERRIDE_NEWDELETE
		void* operator new(size_t size);
		void* operator new[](size_t size);
		void operator delete(void* p);
		void operator delete[](void* p);
		void operator delete(void* p, const char *file, int line);
		void operator delete[](void* p, const char *file, int line);

	#endif // ALLOCLAVE_OVERRIDE_NEWDELETE

	// This redefines everyone else's malloc/free calls to point
	// to the custom ones above, which can then track the calls
	#if ALLOCLAVE_OVERRIDE_MALLOC
		#define malloc Alloclave::_malloc
		#define realloc Alloclave::_realloc
		#define free Alloclave::_free
	#endif

#endif // ALLOCLAVE_ENABLED

#endif


#endif // _ALLOCLAVE_MEMORYOVERRIDES_H



// End File: MemoryOverrides.h


// Begin File: Common.h

#ifndef _ALLOCLAVE_COMMON_H
#define _ALLOCLAVE_COMMON_H

#include <assert.h>



#endif // _ALLOCLAVE_COMMON_H



// End File: Common.h


// Begin File: Buffer.h

#ifndef _ALLOCLAVE_BUFFER_H
#define _ALLOCLAVE_BUFFER_H


// Define NULL if necessary
#ifndef NULL
#ifdef __cplusplus
#define NULL    0
#else
#define NULL    ((void *)0)
#endif
#endif

#define MIN(a,b) (((a)<(b))?(a):(b))
#define MAX(a,b) (((a)>(b))?(a):(b))

namespace Alloclave
{

	// A generic buffer class that collects streams of raw binary data
	class Buffer
	{
	public:
		Buffer();
		Buffer(unsigned int initialSize);
		Buffer(const Buffer& other);

		virtual ~Buffer();

		void Resize(unsigned int newSize);

		void Add(void* data, unsigned int dataSize);
		void Add(const Buffer& buffer);

		void Clear();

		const void* GetData() const;
		unsigned int GetSize() const;

		Buffer& operator=(const Buffer& rhs);

	private:

		static void Copy(const Buffer& from, Buffer& to);

		static const unsigned int DefaultSize = 64;
		unsigned int CurrentSize;

		unsigned int Position;

		char* Data;
	};

};

#endif // _ALLOCLAVE_BUFFER_H



// End File: Buffer.h


// Begin File: ICustomSerializable.h

#ifndef _ALLOCLAVE_ICUSTOMSERIALIZABLE_H
#define _ALLOCLAVE_ICUSTOMSERIALIZABLE_H


namespace Alloclave
{

	// Base interface for anything that needs to serialize data
	class ICustomSerializable
	{
	public:
		virtual Buffer& Serialize() const = 0;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength) = 0;
	};

};

#endif // _ALLOCLAVE_ICUSTOMSERIALIZABLE_H



// End File: ICustomSerializable.h


// Begin File: Packet.h

#ifndef _ALLOCLAVE_IPACKET_H
#define _ALLOCLAVE_IPACKET_H


namespace Alloclave
{

	// Base packet implementation that should be specialized
	// to provide specific information
	class Packet : public ICustomSerializable
	{
	public:
		virtual Buffer& Serialize() const;

	protected:
		enum PacketType
		{
			PacketType_Allocation = 0,
			PacketType_Free,
			PacketType_Screenshot,
			PacketType_SetSymbols,
			PacketType_SetArchitecture,
		};

		virtual PacketType GetPacketType() const = 0;
	};

};

#endif // _ALLOCLAVE_IPACKET_H



// End File: Packet.h


// Begin File: Allocation.h

#ifndef _ALLOCLAVE_ALLOCATION_H
#define _ALLOCLAVE_ALLOCATION_H


namespace Alloclave
{

	class CallStack;

	// Represents an allocation-type packet, containing information such as
	// the memory address and the size
	class Allocation : public Packet
	{
	public:
		enum AllocationType
		{
			AllocationType_Allocation,
			AllocationType_Heap,
		};

		void* Address;
		size_t Size;
		unsigned int Alignment;
		AllocationType Type;
		unsigned int HeapId;

		Allocation();
		Allocation(CallStack& callStackParser);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		CallStack& CallStackParser;

		Allocation& operator=(const Allocation&);
	};

};

#endif // _ALLOCLAVE_ALLOCATION_H



// End File: Allocation.h


// Begin File: CallStack.h

#ifndef _ALLOCLAVE_CALLSTACK_H
#define _ALLOCLAVE_CALLSTACK_H


namespace Alloclave
{

	// Base implementation for implementing platform-specific stack walking
	class CallStack : public ICustomSerializable
	{
	public:
		CallStack();

		virtual void Rebuild();
		
		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);
	protected:
		virtual void Push(void* address);

		static const int MaxStackDepth = 62;

	private:
		void* StackAddresses[MaxStackDepth];
		unsigned int StackDepth;
	};

	static CallStack NullCallStack;

};

#endif // _ALLOCLAVE_CALLSTACK_H



// End File: CallStack.h


// Begin File: CallStack_Win32.h

#ifndef _ALLOCLAVE_CALLSTACK_WIN32_H
#define _ALLOCLAVE_CALLSTACK_WIN32_H


namespace Alloclave
{

	// Win32-specific specialization of stack walking
	class CallStack_Win32 : public CallStack
	{
	public:
		CallStack_Win32();

		virtual void Rebuild();
	};

};

#endif // _ALLOCLAVE_CALLSTACK_WIN32_H



// End File: CallStack_Win32.h


// Begin File: Free.h

#ifndef _ALLOCLAVE_FREE_H
#define _ALLOCLAVE_FREE_H


namespace Alloclave
{

	class CallStack;

	// Represents a free-type packet, mostly for recording the address of the free
	class Free : public Packet
	{
	public:

		void* Address;
		unsigned int HeapId;

		Free();
		Free(CallStack& callStackParser);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		CallStack& CallStackParser;

		Free& operator=(const Free&);
	};

};

#endif // _ALLOCLAVE_FREE_H



// End File: Free.h


// Begin File: Registration.h

#ifndef _ALLOCLAVE_REGISTRATION_H
#define _ALLOCLAVE_REGISTRATION_H

namespace Alloclave
{
	class Transport;
	class CallStack;

	// Registers a specific memory allocation
	void RegisterAllocation(void* address, size_t size, unsigned int heapId = 0);

	// Registers a free of a previous memory allocation
	void RegisterFree(void* address, unsigned int heapId = 0);

	// Registers a screenshot (not yet implemented)
	void RegisterScreenshot(); // TODO

	// Registers a platform-specific stack walker
	void RegisterCallStackParser(CallStack* parser);

	// Tells the visualizer where to look for this program's symbols
	void RegisterSymbolsPath(const char* symbolsPath);
};

#endif // _ALLOCLAVE_REGISTRATION_H



// End File: Registration.h


// Begin File: Screenshot.h

#ifndef _ALLOCLAVE_SCREENSHOT_H
#define _ALLOCLAVE_SCREENSHOT_H


namespace Alloclave
{

	// Base class for generating and serializing screenshots
	// Not yet implemented
	class Screenshot : public Packet
	{
	public:

		Screenshot();

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;
	};

};

#endif // _ALLOCLAVE_SCREENSHOT_H



// End File: Screenshot.h


// Begin File: SetSymbols.h

#ifndef _ALLOCLAVE_SETSYMBOLS_H
#define _ALLOCLAVE_SETSYMBOLS_H


namespace Alloclave
{

	// Sends a symbol path
	class SetSymbols : public Packet
	{
	public:

		SetSymbols(const char* symbolsPath);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		const char* SymbolsPath;
	};

};

#endif // _ALLOCLAVE_SETSYMBOLS_H



// End File: SetSymbols.h


// Begin File: Transport.h

#ifndef _ALLOCLAVE_TRANSPORT_H
#define _ALLOCLAVE_TRANSPORT_H


namespace Alloclave
{
	class Packet;
	class Thread;

	// Base class for transport mechanism used to send data to the visualizer
	class Transport
	{
	public:
		Transport();

		virtual ~Transport();

		static void Send(const Packet& packet);

		virtual void Flush(Thread& callingThread) = 0;

	protected:

		virtual Buffer& BuildFinalBuffer(Thread& callingThread);

		static unsigned int NumItems;

		static const unsigned short Version = 0;
	};

};

#endif // _ALLOCLAVE_TRANSPORT_H



// End File: Transport.h


// Begin File: Win32Transport.h

#ifndef _ALLOCLAVE_WIN32TRANSPORT_H
#define _ALLOCLAVE_WIN32TRANSPORT_H


namespace Alloclave
{

	// Win32-specific transport that uses WM_COPYDATA messages
	class Win32Transport : public Transport
	{
	public:
		Win32Transport();

		virtual ~Win32Transport();

		void Flush(Thread& callingThread);

	protected:

	private:

		void FindVisualizer();

		void* VisualizerHandle;
	};

};

#endif // _ALLOCLAVE_WIN32TRANSPORT_H



// End File: Win32Transport.h


// Begin File: Thread.h

#ifndef _ALLOCLAVE_THREAD_H
#define _ALLOCLAVE_THREAD_H

namespace Alloclave
{

	// Base class for platform-specifc threading
	class Thread
	{
	public:
		Thread(unsigned long (__stdcall *func)(void*)) : UserFunction(func)
		{
			
		}

		virtual ~Thread() {}

		virtual void Start() {}
		virtual void Stop() {}

		virtual void Suspend() {}
		virtual void Resume() {}

		virtual void Sleep(int /*milliseconds*/) {}

		virtual void StartCriticalSection() {}
		virtual void EndCriticalSection() {}

	protected:

		unsigned long (__stdcall *UserFunction)(void*);
	};

};

#endif // _ALLOCLAVE_THREAD_H



// End File: Thread.h


// Begin File: Thread_Win32.h

#ifndef _ALLOCLAVE_THREAD_WIN32_H
#define _ALLOCLAVE_THREAD_WIN32_H


namespace Alloclave
{

	// Win32 specialized threading implementation
	class Thread_Win32 : public Thread
	{
	public:
		Thread_Win32(unsigned long (__stdcall *func)(void*));

		virtual ~Thread_Win32();

		void Start();
		void Stop();

		void Suspend();
		void Resume();

		void Sleep(int milliseconds);

		void StartCriticalSection();
		void EndCriticalSection();

	private:

		void* ThreadHandle;
	};

};

#endif // _ALLOCLAVE_THREAD_WIN32_H



// End File: Thread_Win32.h


// Begin File: SetArchitecture.h

#ifndef _ALLOCLAVE_SETARCHITECTURE_H
#define _ALLOCLAVE_SETARCHITECTURE_H


namespace Alloclave
{

	// Sends across information that allows the visualizer
	// to deduce architecture details of this platform
	class SetArchitecture : public Packet
	{
	public:

		SetArchitecture();

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:

	};

};

#endif // _ALLOCLAVE_SETARCHITECTURE_H



// End File: SetArchitecture.h



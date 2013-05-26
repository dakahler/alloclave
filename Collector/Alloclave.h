// Copyright Circular Shift. For license information, see license.txt.

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

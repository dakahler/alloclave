// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_H
#define _ALLOCLAVE_H

// Set this to 0 to disable Alloclave completely
#define ALLOCLAVE_ENABLED				1

// Set this to 0 if you have your own new/delete overrides
#define ALLOCLAVE_OVERRIDE_NEWDELETE	1

// Set this to 0 if you have your own malloc/free overrides,
// or do not want to override malloc/free
#define ALLOCLAVE_OVERRIDE_MALLOC		1

// Set this to 0 if you want to call Flush() manually
#define ALLOCLAVE_USE_THREADS			1

// NOTE: If you set the transport type to "Custom",
// you must register your transport instance via
// Alloclave::RegisterTransport
#define TRANSPORT_TYPE_WIN32			0
#define TRANSPORT_TYPE_CUSTOM			1
#define ALLOCLAVE_TRANSPORT_TYPE		TRANSPORT_TYPE_WIN32

#endif // _ALLOCLAVE_H

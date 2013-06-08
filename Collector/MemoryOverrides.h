// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_MEMORYOVERRIDES_H
#define _ALLOCLAVE_MEMORYOVERRIDES_H

#include "Alloclave.h"

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

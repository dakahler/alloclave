
#ifndef _ALLOCLAVE_MEMORYOVERRIDES_H
#define _ALLOCLAVE_MEMORYOVERRIDES_H

// TODO: new with file, line?
void* operator new(size_t size);
void* operator new[](size_t size);
void operator delete(void* p);
void operator delete[](void* p);
void operator delete(void* p, const char *file, int line);
void operator delete[](void* p, const char *file, int line);

// Since these are above the malloc/free redefines below,
// they call the original system versions of these functions,
// serving as wrappers to intercept the allocation data
namespace Alloclave
{
	extern void* _malloc(size_t size);
	extern void* _realloc(void* p, size_t size);
	extern void _free(void* p);

	extern void* real_malloc(size_t size);
	extern void* real_realloc(void* p, size_t size);
	extern void real_free(void* p);
};

// This redefines everyone else's malloc/free calls to point
// to the custom ones above, which can then track the calls
#define malloc Alloclave::_malloc
#define realloc Alloclave::_realloc
#define free Alloclave::_free


#endif // _ALLOCLAVE_MEMORYOVERRIDES_H

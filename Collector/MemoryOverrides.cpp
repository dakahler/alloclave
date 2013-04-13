
#include <new>

#include "Registration.h"

void* operator new(size_t size)
{
	void* p = malloc(size);
	if (p == NULL)
		throw std::bad_alloc();

	Alloclave::RegisterAllocation(p, size, 4); // TODO: alignment

	return p;
}

void* operator new[](size_t size)
{
	void* p = malloc(size);
	if (p == NULL)
		throw std::bad_alloc();

	Alloclave::RegisterAllocation(p, size, 4); // TODO: alignment

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

void operator delete(void* p, const char *file, int line)
{
	if (p)
	{
		Alloclave::RegisterFree(p);
		free(p);
	}
}

void operator delete[](void* p, const char *file, int line)
{
	if (p)
	{
		Alloclave::RegisterFree(p);
		free(p);
	}
}

// Since these are above the malloc/free redefines below,
// they call the original system versions of these functions,
// serving as wrappers to intercept the allocation data
// TODO: Better setup to prevent linkage problems if
// a stdlib gets included before this header
#define ALLOCLAVE_OVERRIDE_MALLOC
#ifdef ALLOCLAVE_OVERRIDE_MALLOC

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
		RegisterAllocation(p, size, 4); // TODO: alignment
		return p;
	}

	void* _realloc(void* p, size_t size)
	{
		RegisterFree(p);
		void* newP = real_realloc(p ,size);
		RegisterAllocation(newP, size, 4);
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

#endif // ALLOCLAVE_OVERRIDE_MALLOC

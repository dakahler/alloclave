#include <exception> // for std::bad_alloc
#include <new>
#include <cstdlib> // for malloc() and free()

#include "Registration.h"

using namespace Alloclave;

// TODO: VERY basic allocation overrides for testing
// Needs to be much more robust

// Visual C++ fix of operator new

void* operator new (size_t size)
{
	void *p=malloc(size); 
	if (p==0) // did malloc succeed?
		throw std::bad_alloc(); // ANSI/ISO compliant behavior

	RegisterAllocation(p, size, 4); // TODO: alignment

	return p;
}

void operator delete (void *p)
{
	free(p);
	RegisterFree(p);
}
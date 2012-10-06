#ifndef _ALLOCLAVE_REGISTRATION_H
#define _ALLOCLAVE_REGISTRATION_H

#include "Transport.h"

namespace Alloclave
{

	void RegisterTransport(Transport* transport);
	void RegisterAllocation(void* address, unsigned int size, unsigned int alignment);
	void RegisterFree(void* address);
	void RegisterScreenshot(); // TODO

};


#endif // _ALLOCLAVE_REGISTRATION_H
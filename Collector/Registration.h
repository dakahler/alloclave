#ifndef _ALLOCLAVE_REGISTRATION_H
#define _ALLOCLAVE_REGISTRATION_H

namespace Alloclave
{
	class Transport;

	void RegisterTransport(Transport* transport);
	void RegisterAllocation(void* address, unsigned int size, unsigned int alignment);
	void RegisterFree(void* address);
	void RegisterScreenshot(); // TODO

};


#endif // _ALLOCLAVE_REGISTRATION_H
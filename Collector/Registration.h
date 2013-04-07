#ifndef _ALLOCLAVE_REGISTRATION_H
#define _ALLOCLAVE_REGISTRATION_H

namespace Alloclave
{
	class Transport;
	class CallStack;

	void RegisterTransport(Transport* transport);
	void RegisterAllocation(void* address, unsigned int size, unsigned int alignment, unsigned int heapId = 0);
	void RegisterFree(void* address);
	void RegisterScreenshot(); // TODO
	void RegisterCallStackParser(CallStack* parser);

};


#endif // _ALLOCLAVE_REGISTRATION_H

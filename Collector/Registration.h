// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_REGISTRATION_H
#define _ALLOCLAVE_REGISTRATION_H

namespace Alloclave
{
	class Transport;
	class CallStack;

	void RegisterTransport(Transport* transport);
	void RegisterAllocation(void* address, size_t size, unsigned int alignment, unsigned int heapId = 0);
	void RegisterFree(void* address, unsigned int heapId = 0);
	void RegisterScreenshot(); // TODO
	void RegisterCallStackParser(CallStack* parser);
	void RegisterSymbolsPath(const char* symbolsPath);
};

#endif // _ALLOCLAVE_REGISTRATION_H

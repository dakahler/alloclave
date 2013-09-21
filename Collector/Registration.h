#ifndef _ALLOCLAVE_REGISTRATION_H
#define _ALLOCLAVE_REGISTRATION_H

namespace Alloclave
{
	class Transport;
	class CallStack;

	// Registers a specific memory allocation
	void RegisterAllocation(void* address, size_t size, unsigned int heapId = 0);

	// Registers a free of a previous memory allocation
	void RegisterFree(void* address, unsigned int heapId = 0);

	// Registers a screenshot (not yet implemented)
	void RegisterScreenshot(); // TODO

	// Registers a platform-specific stack walker
	void RegisterCallStackParser(CallStack* parser);

	// Tells the visualizer where to look for this program's symbols
	void RegisterSymbolsPath(const char* symbolsPath);
};

#endif // _ALLOCLAVE_REGISTRATION_H

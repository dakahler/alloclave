
#include "Win32Transport.h"
#include "MemoryOverrides.h"
#include "Registration.h"

using namespace Alloclave;

static Win32Transport s_Transport;

void main()
{
	RegisterTransport(&s_Transport);

	char* testMalloc = (char*)malloc(16);
	char* testNew = new char;
	char* testArrayNew = new char[100];

	free(testMalloc);
	delete testNew;
	delete[] testArrayNew;
}
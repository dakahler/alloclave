
#include <windows.h>
#include "Win32Transport.h"
#include "MemoryOverrides.h"
#include "Registration.h"

using namespace Alloclave;

static Win32Transport s_Transport;

void main()
{
	RegisterTransport(&s_Transport);

	//char* testMalloc = (char*)malloc(16);
	//char* testNew = new char;

	while (true)
	{
		char* testArrayNew = new char[1000];
		Sleep(1000);
	}

	//free(testMalloc);
	//delete testNew;
	//delete[] testArrayNew;
}
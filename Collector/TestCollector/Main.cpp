// Copyright Circular Shift. For license information, see license.txt.

#include <windows.h>
#include <time.h>
#include <stdio.h>

#include "Win32Transport.h"
#include "MemoryOverrides.h"
#include "Registration.h"


using namespace Alloclave;

void main(int argc, char* argv[])
{
	//char* testMalloc = (char*)malloc(16);

	//for (int i = 0; i < 30; i++)
	//{
	//	RegisterAllocation((void*)(0x00001020 + (i * 1024)), 1000);
	//	Sleep(1000);
	//}

	//for (int i = 0; i < 5; i++)
	//{
	//	RegisterFree((void*)(0x00001020 + ((i * 3) * 1024)));
	//	Sleep(1000);
	//}

	//for (int i = 32; i < 45; i++)
	//{
	//	RegisterAllocation((void*)(0x00001020 + (i * 1024)), 1000);
	//	Sleep(1000);
	//}

	srand((unsigned int)time(NULL));

	const int numAllocations = 500;
	char* allocationArray[numAllocations];

	for (int i = 0; i < numAllocations; i++)
	{
		int numBytes = 100 + (rand() % 1000);
		allocationArray[i] = new char[numBytes];
		printf("Allocated %d bytes at address 0x%p\n", numBytes, allocationArray[i]);

		// Random chance of freeing something
		if ((rand() % 5) == 0 && i > 0)
		{
			int index = rand() % i;
			printf("Freeing address 0x%p\n", allocationArray[index]);
			delete[] allocationArray[index];
			allocationArray[index] = NULL;
		}

		Sleep(100 + (rand() % 100));
	}

	for (int i = 30; i < 100; i++)
	{
		printf("Freeing address 0x%p\n", allocationArray[i]);
		delete[] allocationArray[i];
		allocationArray[i] = NULL;
	}
}
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
	// Send the symbols path to the tool
	// Assumes the symbols are in a PDB and
	// it's in the same directory as the executable
	if (argc > 0)
	{
		char modifiedPath[512];
		strncpy(modifiedPath, argv[0], sizeof(modifiedPath) - 1);

		size_t pathLength = strlen(modifiedPath);
		modifiedPath[pathLength - 3] = 'p';
		modifiedPath[pathLength - 2] = 'd';
		modifiedPath[pathLength - 1] = 'b';

		RegisterSymbolsPath(modifiedPath);
	}

	//char* testMalloc = (char*)malloc(16);
	//char* testNew = new char;

	srand(time(NULL));

	const int numAllocations = 30;
	char* allocationArray[numAllocations];

	for (int i = 0; i < numAllocations; i++)
	{
		int numBytes = 100 + (rand() % 2500);
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

		Sleep(100 + (rand() % 1000));
	}
}
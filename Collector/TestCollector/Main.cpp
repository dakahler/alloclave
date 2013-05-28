// Copyright Circular Shift. For license information, see license.txt.

#include <windows.h>
#include <time.h>
#include <stdio.h>

// To override new and delete, include this header in
// a common header, or force include it in the project settings
#include "MemoryOverrides.h"

void main(int argc, char* argv[])
{
	// This test program generates some random allocations and frees
	// to demonstrate the functionality of Alloclave. It is what
	// gets executed when you run the tour in the visualizer.

	srand((unsigned int)time(NULL));

	const int numAllocations = 50;
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
}
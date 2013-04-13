
#include <windows.h>
#include "Win32Transport.h"
#include "MemoryOverrides.h"
#include "Registration.h"

using namespace Alloclave;

static Win32Transport s_Transport;

void main(int argc, char* argv[])
{
	RegisterTransport(&s_Transport);

	// Send the symbols path to the tool
	// Assumes the symbols are in a PDB and
	// it's in the same directory as the executable
	if (argc > 0)
	{
		char modifiedPath[512];
		strncpy(modifiedPath, argv[0], sizeof(modifiedPath) - 1);

		int pathLength = strlen(modifiedPath);
		modifiedPath[pathLength - 3] = 'p';
		modifiedPath[pathLength - 2] = 'd';
		modifiedPath[pathLength - 1] = 'b';

		RegisterSymbolsPath(modifiedPath);
	}

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
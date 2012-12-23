// This is the main DLL file.

#include "Inject.h"
#include "InjectHelper.h"
#include <windows.h>

void Alloclave_Plugin::InjectHelper::Inject(System::UInt64 pid)
{
	InjectLib((DWORD)pid, L"C:\\dev\\Alloclave\\Visualizer\\plugins\\transport\\InjectDll.dll");
}



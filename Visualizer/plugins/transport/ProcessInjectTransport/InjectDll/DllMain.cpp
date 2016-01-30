
#include <Windows.h>
#include "mhook-lib/mhook.h"
#include "Registration.h"
#include "Win32Transport.h"

static Alloclave::Win32Transport s_Transport;

//=========================================================================
// Define _HeapAlloc so we can dynamically bind to the function
typedef LPVOID (WINAPI *_HeapAlloc)(HANDLE, DWORD, SIZE_T);
_HeapAlloc TrueHeapAlloc = (_HeapAlloc)GetProcAddress(GetModuleHandle(L"kernel32"), "HeapAlloc");

static bool s_bIgnoreAllocations = false;

LPVOID WINAPI HookHeapAlloc(HANDLE a_Handle, DWORD a_Bla, SIZE_T a_Bla2)
{
	void* address = TrueHeapAlloc(a_Handle, a_Bla, a_Bla2);
	if (address == NULL)
	{
		// TODO: Error
		return NULL;
	}

	if (s_bIgnoreAllocations)
	{
		return address;
	}

	s_bIgnoreAllocations = true;
	Alloclave::RegisterAllocation(address, a_Bla2, 4);
	s_bIgnoreAllocations = false;

	return address;
}

DWORD WaitForever(void* ptr)
{
	Alloclave::RegisterTransport(&s_Transport);

	if (!Mhook_SetHook((PVOID*)&TrueHeapAlloc, HookHeapAlloc))
	{
		int msgboxID = MessageBox(
			NULL,
			(LPCWSTR)L"Error hooking into HeapAlloc",
			(LPCWSTR)L"Error",
			MB_ICONWARNING | MB_CANCELTRYCONTINUE | MB_DEFBUTTON2
			);
	}

	while (true)
	{
		Sleep(10000);
	}

	return 0;
}

BOOL WINAPI DllMain(HINSTANCE hInst, DWORD dwReason, LPVOID reserved)
{
	switch(dwReason)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread( NULL, 0, (LPTHREAD_START_ROUTINE)WaitForever, (void*)NULL, 0, NULL );
		break;
	}

	//while (true)
	//{
	//	Sleep(1000);
	//}

	return TRUE;
}
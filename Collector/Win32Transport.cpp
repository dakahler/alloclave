#include <windows.h>
#include "Win32Transport.h"

namespace Alloclave
{

Win32Transport::Win32Transport()
{
	VisualizerHandle = NULL;
}

Win32Transport::~Win32Transport()
{
	VisualizerHandle = NULL;
}

void Win32Transport::Flush(Thread& callingThread)
{
	if (NumItems == 0)
	{
		return;
	}

	FindVisualizer();
	if (VisualizerHandle == NULL)
	{
		return;
	}

	Buffer& buffer = BuildFinalBuffer(callingThread);

	// Send Win32 WM_COPYDATA message to visualizer
	COPYDATASTRUCT cds;
	cds.dwData = ALLOCLAVE_WIN32_ID;
	cds.cbData = buffer.GetSize();
	cds.lpData = (void*)buffer.GetData(); // TODO: constness
	SendMessage((HWND)VisualizerHandle, WM_COPYDATA, (WPARAM)VisualizerHandle, (LPARAM)(LPVOID)&cds);
}

void Win32Transport::FindVisualizer()
{
	if (VisualizerHandle == NULL)
	{
		VisualizerHandle = FindWindowW(NULL, ALLOCLAVE_WIN32_GUID);
	}
}

}

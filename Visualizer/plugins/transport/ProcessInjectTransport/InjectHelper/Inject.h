
#ifndef _INJECT_H
#define _INJECT_H

#include <Windows.h>

#ifdef UNICODE
#define InjectLib InjectLibW
#else
#define InjectLib InjectLibA
#endif // !UNICODE

BOOL WINAPI InjectLibW(DWORD dwProcessId, PCWSTR pszLibFile);
BOOL WINAPI InjectLibA(DWORD dwProcessId, PCSTR pszLibFile);

#endif // _INJECT_H
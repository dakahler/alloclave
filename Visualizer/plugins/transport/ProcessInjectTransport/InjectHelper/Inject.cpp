//#include "stdafx.h"

#include "Inject.h"



#include "tchar.h"

#include "malloc.h"    // For alloca 

#include "accctrl.h"

#include "Aclapi.h"

//#include "pi.h" 



#ifdef UNICODE

#define InjectLib InjectLibW

#else

#define InjectLib InjectLibA

#endif   // !UNICODE



BOOL AdjustDacl(HANDLE h, DWORD DesiredAccess)

{

	// the WORLD Sid is trivial to form programmatically (S-1-1-0)

	SID world = { SID_REVISION, 1, SECURITY_WORLD_SID_AUTHORITY, 0 };



	EXPLICIT_ACCESS ea =

	{

		DesiredAccess,

		SET_ACCESS,

		NO_INHERITANCE,

		{

			0, NO_MULTIPLE_TRUSTEE,

				TRUSTEE_IS_SID,

				TRUSTEE_IS_USER,

				reinterpret_cast<LPTSTR>(&world)

		}

	};

	ACL* pdacl = 0;

	DWORD err = SetEntriesInAcl(1, &ea, 0, &pdacl);

	if (err == ERROR_SUCCESS)

	{

		err = SetSecurityInfo(h, SE_KERNEL_OBJECT, DACL_SECURITY_INFORMATION, 0, 0, pdacl, 0);

		LocalFree(pdacl);

		return(err == ERROR_SUCCESS);

	}

	else

		return(FALSE);

}



// Useful helper function for enabling a single privilege

BOOL EnableTokenPrivilege(HANDLE htok, LPCTSTR szPrivilege, TOKEN_PRIVILEGES& tpOld)

{

	TOKEN_PRIVILEGES tp;

	tp.PrivilegeCount = 1;

	tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

	if (LookupPrivilegeValue(0, szPrivilege, &tp.Privileges[0].Luid))

	{

		// htok must have been opened with the following permissions:

		// TOKEN_QUERY (to get the old priv setting)

		// TOKEN_ADJUST_PRIVILEGES (to adjust the priv)

		DWORD cbOld = sizeof tpOld;

		if (AdjustTokenPrivileges(htok, FALSE, &tp, cbOld, &tpOld, &cbOld))

			// Note that AdjustTokenPrivileges may succeed, and yet

				// some privileges weren't actually adjusted.

					// You've got to check GetLastError() to be sure!

						return(ERROR_NOT_ALL_ASSIGNED != GetLastError());

		else

			return(FALSE);

	}

	else

		return(FALSE);

}





// Corresponding restoration helper function

BOOL RestoreTokenPrivilege(HANDLE htok, const TOKEN_PRIVILEGES& tpOld)

{

	return(AdjustTokenPrivileges(htok, FALSE, const_cast<TOKEN_PRIVILEGES*>(&tpOld), 0, 0, 0));

}



HANDLE GetProcessHandleWithEnoughRights(DWORD PID, DWORD AccessRights)

{

	HANDLE hProcess = ::OpenProcess(AccessRights, FALSE, PID);

	if (hProcess == NULL)

	{

		HANDLE hpWriteDAC = OpenProcess(WRITE_DAC, FALSE, PID);

		if (hpWriteDAC == NULL)

		{

			// hmm, we don't have permissions to modify the DACL...

			// time to take ownership...

			HANDLE htok;

			if (!OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES, &htok))

				return(FALSE);



			TOKEN_PRIVILEGES tpOld;

			if (EnableTokenPrivilege(htok, SE_TAKE_OWNERSHIP_NAME, tpOld))

			{

				// SeTakeOwnershipPrivilege allows us to open objects with

				// WRITE_OWNER, but that's about it, so we'll update the owner,

				// and dup the handle so we can get WRITE_DAC permissions.

				HANDLE hpWriteOwner = OpenProcess(WRITE_OWNER, FALSE, PID);

				if (hpWriteOwner != NULL)

				{

					BYTE buf[512]; // this should always be big enough

					DWORD cb = sizeof buf;

					if (GetTokenInformation(htok, TokenUser, buf, cb, &cb))

					{

						DWORD err = 

							SetSecurityInfo( 

							hpWriteOwner, 

							SE_KERNEL_OBJECT,

							OWNER_SECURITY_INFORMATION,

							reinterpret_cast<TOKEN_USER*>(buf)->User.Sid,

							0, 0, 0 

							);

						if (err == ERROR_SUCCESS)

						{

							// now that we're the owner, we've implicitly got WRITE_DAC

							// permissions, so ask the system to reevaluate our request,

							// giving us a handle with WRITE_DAC permissions

							if (

								!DuplicateHandle( 

								GetCurrentProcess(), 

								hpWriteOwner,

								GetCurrentProcess(), 

								&hpWriteDAC,

								WRITE_DAC, FALSE, 0 

								)

								)

								hpWriteDAC = NULL;

						}

					}



					// don't forget to close handle

					::CloseHandle(hpWriteOwner);

				}



				// not truly necessary in this app,

				// but included for completeness

				RestoreTokenPrivilege(htok, tpOld);

			}



			// don't forget to close the token handle

			::CloseHandle(htok);

		}



		if (hpWriteDAC)

		{

			// we've now got a handle that allows us WRITE_DAC permission

			AdjustDacl(hpWriteDAC, AccessRights);



			// now that we've granted ourselves permission to access 

			// the process, ask the system to reevaluate our request,

			// giving us a handle with right permissions

			if (

				!DuplicateHandle( 

				GetCurrentProcess(), 

				hpWriteDAC,

				GetCurrentProcess(), 

				&hProcess,

				AccessRights, 

				FALSE, 

				0

				)

				)

				hProcess = NULL;



			CloseHandle(hpWriteDAC);

		}

	}



	return(hProcess);

}



BOOL WINAPI InjectLibW(DWORD dwProcessId, PCWSTR pszLibFile) 

{

	BOOL fOk = FALSE; // Assume that the function fails

	HANDLE hProcess = NULL, hThread = NULL;

	PWSTR pszLibFileRemote = NULL;



	// Get a handle for the target process.

	hProcess = 



		GetProcessHandleWithEnoughRights(

		dwProcessId,

		PROCESS_QUERY_INFORMATION |   // Required by Alpha

		PROCESS_CREATE_THREAD     |   // For CreateRemoteThread

		PROCESS_VM_OPERATION      |   // For VirtualAllocEx/VirtualFreeEx

		PROCESS_VM_WRITE              // For WriteProcessMemory

		);

	if (hProcess == NULL)

		return(FALSE);



	// Calculate the number of bytes needed for the DLL's pathname

	int cch = 1 + lstrlenW(pszLibFile);

	int cb  = cch * sizeof(WCHAR);



	// Allocate space in the remote process for the pathname

	pszLibFileRemote = 

		(PWSTR) VirtualAllocEx(hProcess, NULL, cb, MEM_COMMIT, PAGE_READWRITE);



	if (pszLibFileRemote != NULL)

	{

		// Copy the DLL's pathname to the remote process's address space

		if (WriteProcessMemory(hProcess, pszLibFileRemote, 

			(PVOID) pszLibFile, cb, NULL))

		{

			// Get the real address of LoadLibraryW in Kernel32.dll

			PTHREAD_START_ROUTINE pfnThreadRtn = (PTHREAD_START_ROUTINE)

				GetProcAddress(GetModuleHandle(TEXT("Kernel32")), "LoadLibraryW");

			if (pfnThreadRtn != NULL)

			{

				// Create a remote thread that calls LoadLibraryW(DLLPathname)

				hThread = CreateRemoteThread(hProcess, NULL, 0, 

					pfnThreadRtn, pszLibFileRemote, 0, NULL);

				if (hThread != NULL)

				{

					// Wait for the remote thread to terminate

					//WaitForSingleObject(hThread, INFINITE);



					fOk = TRUE; // Everything executed successfully



					//CloseHandle(hThread);

				}

			}

		}

		// Free the remote memory that contained the DLL's pathname

		//VirtualFreeEx(hProcess, pszLibFileRemote, 0, MEM_RELEASE);

	}



	//CloseHandle(hProcess);



	return(fOk);

}





BOOL WINAPI InjectLibA(DWORD dwProcessId, PCSTR pszLibFile) {



	// Allocate a (stack) buffer for the Unicode version of the pathname

	PWSTR pszLibFileW = (PWSTR) 

		_alloca((lstrlenA(pszLibFile) + 1) * sizeof(WCHAR));



	// Convert the ANSI pathname to its Unicode equivalent

	wsprintfW(pszLibFileW, L"%S", pszLibFile);



	// Call the Unicode version of the function to actually do the work.

	return(InjectLibW(dwProcessId, pszLibFileW));

}


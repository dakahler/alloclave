// This is the main DLL file.

#include "stdafx.h"
#include "PdbParser.h"

#include "dia2dump.h"

#include "Dbghelp.h"

using namespace System::Runtime::InteropServices;

namespace
{
	IDiaDataSource *pDiaDataSource;
	IDiaSession *pDiaSession;
	IDiaSymbol *pGlobalSymbol;
}

namespace Alloclave
{

bool PdbParser::Open(String^ pdbPath)
{
	IntPtr ptrToNativeString = Marshal::StringToBSTR(pdbPath);
	return LoadDataFromPdb((const wchar_t*)ptrToNativeString.ToPointer(), &pDiaDataSource, &pDiaSession, &pGlobalSymbol);
}

String^ PdbParser::GetFunctionName(UInt64 address)
{
	BSTR functionName = DumpSymbolWithRVA(pDiaSession, (DWORD)address, NULL);
	if (functionName != NULL)
	{
		String^ finalString = gcnew String((wchar_t*)functionName);
		//SysFreeString(functionName);
		return finalString;
	}
	else
	{
		return gcnew String("ERROR");
	}
}

}


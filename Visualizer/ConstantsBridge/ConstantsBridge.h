// ConstantsBridge.h

#pragma once

#include "..\..\Collector\Constants.h"

using namespace System;

namespace Alloclave
{
	public ref class ConstantsBridge
	{
	public:
		literal unsigned int Win32Id = ALLOCLAVE_WIN32_ID;
		literal String^ Win32Guid = ALLOCLAVE_WIN32_GUID;
	};
}

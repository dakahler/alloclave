// PdbParser.h

#pragma once

using namespace System;

namespace Alloclave {

	public ref class PdbParser
	{
	public:
		bool Open(String^ pdbPath);
		String^ GetFunctionName(UInt64 address);

	private:
		
	};
}

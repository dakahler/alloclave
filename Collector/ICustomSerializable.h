// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_ICUSTOMSERIALIZABLE_H
#define _ALLOCLAVE_ICUSTOMSERIALIZABLE_H

#include "Buffer.h"

namespace Alloclave
{

	// Base interface for anything that needs to serialize data
	class ICustomSerializable
	{
	public:
		virtual Buffer Serialize() const = 0;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength) = 0;
	};

};

#endif // _ALLOCLAVE_ICUSTOMSERIALIZABLE_H

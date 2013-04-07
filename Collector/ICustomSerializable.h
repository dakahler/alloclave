#ifndef _ALLOCLAVE_ICUSTOMSERIALIZABLE_H
#define _ALLOCLAVE_ICUSTOMSERIALIZABLE_H

#include "Buffer.h"

namespace Alloclave
{

	class ICustomSerializable
	{
	public:
		virtual Buffer Serialize() const = 0;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength) = 0;
	};

};

#endif // _ALLOCLAVE_ICUSTOMSERIALIZABLE_H

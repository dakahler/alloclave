#ifndef _ALLOCLAVE_ALLOCATION_H
#define _ALLOCLAVE_ALLOCATION_H

#include "IPacket.h"

namespace Alloclave
{

	class Allocation : public IPacket
	{
	public:
		void* Address;
		unsigned int Size;
		unsigned int Alignment;
		char* Stack;

		Allocation();

		virtual Buffer Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);
	};

};


#endif // _ALLOCLAVE_ALLOCATION_H
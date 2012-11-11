#ifndef _ALLOCLAVE_ALLOCATION_H
#define _ALLOCLAVE_ALLOCATION_H

#include "IPacket.h"

namespace Alloclave
{

	class Allocation : public Packet
	{
	public:
		void* Address;
		unsigned int Size;
		unsigned int Alignment;
		char* Stack;

		Allocation();

		virtual Buffer Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;
	};

};


#endif // _ALLOCLAVE_ALLOCATION_H
#ifndef _ALLOCLAVE_ALLOCATION_H
#define _ALLOCLAVE_ALLOCATION_H

#include "Packet.h"

namespace Alloclave
{

	class CallStack;

	// Represents an allocation-type packet, containing information such as
	// the memory address and the size
	class Allocation : public Packet
	{
	public:
		enum AllocationType
		{
			AllocationType_Allocation,
			AllocationType_Heap,
		};

		void* Address;
		size_t Size;
		unsigned int Alignment;
		AllocationType Type;
		unsigned int HeapId;

		Allocation();
		Allocation(CallStack& callStackParser);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		CallStack& CallStackParser;

		Allocation& operator=(const Allocation&);
	};

};

#endif // _ALLOCLAVE_ALLOCATION_H

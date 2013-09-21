#ifndef _ALLOCLAVE_FREE_H
#define _ALLOCLAVE_FREE_H

#include "Packet.h"

namespace Alloclave
{

	class CallStack;

	// Represents a free-type packet, mostly for recording the address of the free
	class Free : public Packet
	{
	public:

		void* Address;
		unsigned int HeapId;

		Free();
		Free(CallStack& callStackParser);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		CallStack& CallStackParser;

		Free& operator=(const Free&);
	};

};

#endif // _ALLOCLAVE_FREE_H

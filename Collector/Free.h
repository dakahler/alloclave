#ifndef _ALLOCLAVE_FREE_H
#define _ALLOCLAVE_FREE_H

#include "IPacket.h"

namespace Alloclave
{

	class CallStack;

	class Free : public Packet
	{
	public:

		void* Address;
		unsigned int HeapId;

		Free();
		Free(CallStack& callStackParser);

		virtual Buffer Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		CallStack& CallStackParser;
	};

};

#endif // _ALLOCLAVE_FREE_H

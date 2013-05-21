// Copyright Circular Shift. For license information, see license.txt.

#include "Free.h"
#include "CallStack.h"


namespace Alloclave
{

static CallStack NullCallStack;

Free::Free()
	: CallStackParser(NullCallStack)
{
	Address = NULL;
	HeapId = 0;
}

Free::Free(CallStack& callStackParser)
	: CallStackParser(callStackParser)
{
	Address = NULL;
	HeapId = 0;

	CallStackParser.Rebuild();
}

Buffer Free::Serialize() const
{
	Buffer buffer = Packet::Serialize();

	// Push the free data into the buffer
	buffer.Add((void*)&Address, sizeof(Address));
	buffer.Add((void*)&HeapId, sizeof(HeapId));
	buffer.Add(CallStackParser.Serialize());

	return buffer;
}

void Free::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{
	assert(false);
}

Packet::PacketType Free::GetPacketType() const
{
	return PacketType_Free;
}

}

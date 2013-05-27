// Copyright Circular Shift. For license information, see license.txt.

#include "Allocation.h"
#include "CallStack.h"


namespace Alloclave
{

static CallStack NullCallStack;

Allocation::Allocation()
	: CallStackParser(NullCallStack)
{
	Address = NULL;
	Size = 0;
	Alignment = 0;
	Type = AllocationType_Allocation;
	HeapId = 0;
}

Allocation::Allocation(CallStack& callStackParser)
	: CallStackParser(callStackParser)
{
	Address = NULL;
	Size = 0;
	Alignment = 0;
	Type = AllocationType_Allocation;
	HeapId = 0;

	// Generate the call stack here
	CallStackParser.Rebuild();
}

Buffer& Allocation::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// Push the allocation data into the buffer
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&Address, sizeof(Address));
	buffer.Add((void*)&Size, sizeof(Size));
	buffer.Add((void*)&Alignment, sizeof(Alignment));
	buffer.Add((void*)&Type, sizeof(char));
	buffer.Add((void*)&HeapId, sizeof(HeapId));
	buffer.Add(CallStackParser.Serialize());

	return buffer;
}

void Allocation::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{
	assert(false);
}

Packet::PacketType Allocation::GetPacketType() const
{
	return PacketType_Allocation;
}

}

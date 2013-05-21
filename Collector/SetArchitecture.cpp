// Copyright Circular Shift. For license information, see license.txt.

#include "SetArchitecture.h"


namespace Alloclave
{

SetArchitecture::SetArchitecture()
{
	
}

Buffer SetArchitecture::Serialize() const
{
	Buffer buffer = Packet::Serialize();

	// This sends along the number of bytes in a pointer
	// so the visualizer can auto-detect 32- or 64-bit
	unsigned short pointerSize = (unsigned short)sizeof(void*);
	buffer.Add((void*)&pointerSize, sizeof(pointerSize));

	return buffer;
}

void SetArchitecture::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{
	assert(false);
}

Packet::PacketType SetArchitecture::GetPacketType() const
{
	return PacketType_SetArchitecture;
}

}

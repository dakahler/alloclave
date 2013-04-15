
#include "SetArchitecture.h"


namespace Alloclave
{

SetArchitecture::SetArchitecture()
{
	
}

Buffer SetArchitecture::Serialize() const
{
	Buffer buffer = Packet::Serialize();

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

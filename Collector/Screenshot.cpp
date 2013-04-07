
#include "Screenshot.h"

namespace Alloclave
{

Screenshot::Screenshot()
{

}

Buffer Screenshot::Serialize() const
{
	return Buffer();
}

void Screenshot::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{

}

Packet::PacketType Screenshot::GetPacketType() const
{
	return PacketType_Screenshot;
}

}

#include "Transport.h"

namespace Alloclave
{

Transport::Transport()
{

}

Transport::~Transport()
{

}

void Transport::Send(const IPacket& packet)
{
	Buffer buffer = packet.Serialize();
	PacketQueue.Enqueue(buffer);
}

}

#include "Transport.h"
#include "IPacket.h"

namespace Alloclave
{

Transport::Transport()
{

}

Transport::~Transport()
{

}

void Transport::Send(const Packet& packet)
{
	Buffer buffer = packet.Serialize();
	PacketQueue.Enqueue(buffer);

	// TODO: Temporary location
	Flush();
}

Buffer Transport::BuildFinalBuffer(unsigned short version)
{
	Buffer buffer;

	buffer.Add(&version, sizeof(version));

	// TODO: Architecture
	unsigned int numItems = PacketQueue.GetNumItems();
	buffer.Add(&numItems, sizeof(numItems));

	for (unsigned int i = 0; i < numItems; i++)
	{
		buffer.Add(PacketQueue.Dequeue());
	}

	return buffer;
}

}

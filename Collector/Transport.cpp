// Copyright Circular Shift. For license information, see license.txt.

#include "Transport.h"
#include "IPacket.h"
#include "SetArchitecture.h"

namespace Alloclave
{

static bool g_SentArchtecturePacket = false;
Queue Transport::PacketQueue;

Transport::Transport()
{

}

Transport::~Transport()
{

}

void Transport::Send(const Packet& packet)
{
	if (!g_SentArchtecturePacket)
	{
		g_SentArchtecturePacket = true;
		SetArchitecture architecturePacket;
		PacketQueue.Enqueue(architecturePacket.Serialize());
	}

	Buffer buffer = packet.Serialize();
	PacketQueue.Enqueue(buffer);
}

Buffer Transport::BuildFinalBuffer(unsigned short version)
{
	Buffer buffer;

	buffer.Add(&version, sizeof(version));

	unsigned int numItems = PacketQueue.GetNumItems();
	buffer.Add(&numItems, sizeof(numItems));

	for (unsigned int i = 0; i < numItems; i++)
	{
		buffer.Add(PacketQueue.Dequeue());
	}

	return buffer;
}

}

// Copyright Circular Shift. For license information, see license.txt.

#include "Transport.h"
#include "Packet.h"
#include "SetArchitecture.h"

namespace Alloclave
{

static bool g_SentArchtecturePacket = false;

Buffer& GetGlobalBuffer()
{
	static Buffer buffer;
	return buffer;
}

unsigned int Transport::NumItems = 0;
//Queue Transport::PacketQueue;

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
		GetGlobalBuffer().Add(architecturePacket.Serialize());
		NumItems++;
	}

	GetGlobalBuffer().Add(packet.Serialize());
	NumItems++;

	//PacketQueue.Enqueue(buffer);
}

Buffer& Transport::BuildFinalBuffer(unsigned short version)
{
	// This builds up the final packet bundle
	static Buffer buffer;
	buffer.Clear();

	buffer.Add(&version, sizeof(version));

	//unsigned int numItems = PacketQueue.GetNumItems();
	buffer.Add(&NumItems, sizeof(NumItems));

	//for (unsigned int i = 0; i < numItems; i++)
	//{
	//	buffer.Add(PacketQueue.Dequeue());
	//}

	buffer.Add((void*)GetGlobalBuffer().GetData(), GetGlobalBuffer().GetSize());

	NumItems = 0;
	GetGlobalBuffer().Clear();

	return buffer;
}

}

// Copyright Circular Shift. For license information, see license.txt.

#include <time.h>
#include "Packet.h"

namespace Alloclave
{

Buffer Packet::Serialize() const
{
	unsigned char packetType = (unsigned char)GetPacketType();
	__int64 timeStamp = time(NULL);

	Buffer buffer;

	buffer.Add(&packetType, sizeof(packetType));
	buffer.Add(&timeStamp, sizeof(timeStamp));

	return buffer;
}

}

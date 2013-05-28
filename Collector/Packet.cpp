// Copyright Circular Shift. For license information, see license.txt.

#include <time.h>
#include "Packet.h"

namespace Alloclave
{

Buffer& Packet::Serialize() const
{
	unsigned char packetType = (unsigned char)GetPacketType();
	__int64 timeStamp = clock();

	// All the various packet buffers are local statics as a way
	// of reusing data and minimizing dynamic allocations
	static Buffer buffer;

	buffer.Clear();

	buffer.Add(&packetType, sizeof(packetType));
	buffer.Add(&timeStamp, sizeof(timeStamp));

	return buffer;
}

}

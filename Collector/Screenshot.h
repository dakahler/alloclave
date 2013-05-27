// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_SCREENSHOT_H
#define _ALLOCLAVE_SCREENSHOT_H

#include "Packet.h"

namespace Alloclave
{

	// Base class for generating and serializing screenshots
	// Not yet implemented
	class Screenshot : public Packet
	{
	public:

		Screenshot();

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;
	};

};

#endif // _ALLOCLAVE_SCREENSHOT_H

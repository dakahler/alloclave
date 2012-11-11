#ifndef _ALLOCLAVE_SCREENSHOT_H
#define _ALLOCLAVE_SCREENSHOT_H

#include "IPacket.h"

namespace Alloclave
{

	class Screenshot : public Packet
	{
	public:

		Screenshot();

		virtual Buffer Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;
	};

};


#endif // _ALLOCLAVE_SCREENSHOT_H
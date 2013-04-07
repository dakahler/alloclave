#ifndef _ALLOCLAVE_IPACKET_H
#define _ALLOCLAVE_IPACKET_H

#include "ICustomSerializable.h"
#include "Buffer.h"

namespace Alloclave
{

	// TODO: Rename file
	class Packet : public ICustomSerializable
	{
	public:
		virtual Buffer Serialize() const;

	protected:
		enum PacketType
		{
			PacketType_Allocation = 0,
			PacketType_Free,
			PacketType_Screenshot,
		};

		virtual PacketType GetPacketType() const = 0;
	};

};


#endif // _ALLOCLAVE_IPACKET_H

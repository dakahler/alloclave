// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_IPACKET_H
#define _ALLOCLAVE_IPACKET_H

#include "ICustomSerializable.h"
#include "Buffer.h"

namespace Alloclave
{

	// Base packet implementation that should be specialized
	// to provide specific information
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
			PacketType_SetSymbols,
			PacketType_SetArchitecture,
		};

		virtual PacketType GetPacketType() const = 0;
	};

};

#endif // _ALLOCLAVE_IPACKET_H

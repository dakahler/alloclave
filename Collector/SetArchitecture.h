// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_SETARCHITECTURE_H
#define _ALLOCLAVE_SETARCHITECTURE_H

#include "IPacket.h"

namespace Alloclave
{

	class SetArchitecture : public Packet
	{
	public:

		SetArchitecture();

		virtual Buffer Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:

	};

};

#endif // _ALLOCLAVE_SETARCHITECTURE_H

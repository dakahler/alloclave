#ifndef _ALLOCLAVE_SETARCHITECTURE_H
#define _ALLOCLAVE_SETARCHITECTURE_H

#include "Packet.h"

namespace Alloclave
{

	// Sends across information that allows the visualizer
	// to deduce architecture details of this platform
	class SetArchitecture : public Packet
	{
	public:

		SetArchitecture();

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:

	};

};

#endif // _ALLOCLAVE_SETARCHITECTURE_H

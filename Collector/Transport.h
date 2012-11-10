#ifndef _ALLOCLAVE_TRANSPORT_H
#define _ALLOCLAVE_TRANSPORT_H

#include "IPacket.h"
#include "Queue.h"

namespace Alloclave
{

	class Transport
	{
	public:
		Transport();

		virtual ~Transport();

		virtual void Connect() = 0;
		virtual void Disconnect() = 0;

		void Send(const IPacket& packet);

	protected:

		virtual void Flush() = 0;

		Queue PacketQueue;
	};

};


#endif // _ALLOCLAVE_TRANSPORT_H
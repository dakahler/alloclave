#ifndef _ALLOCLAVE_TRANSPORT_H
#define _ALLOCLAVE_TRANSPORT_H

#include "Queue.h"

namespace Alloclave
{
	class Packet;

	class Transport
	{
	public:
		Transport();

		virtual ~Transport();

		virtual void Connect() = 0;
		virtual void Disconnect() = 0;

		void Send(const Packet& packet);

	protected:

		virtual void Flush() = 0;
		virtual Buffer BuildFinalBuffer(unsigned short version);

		Queue PacketQueue;

		static const unsigned short Version = 0;
	};

};


#endif // _ALLOCLAVE_TRANSPORT_H

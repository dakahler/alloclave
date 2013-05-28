// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_TRANSPORT_H
#define _ALLOCLAVE_TRANSPORT_H

#include "Buffer.h"

namespace Alloclave
{
	class Packet;
	class Thread;

	// Base class for transport mechanism used to send data to the visualizer
	class Transport
	{
	public:
		Transport();

		virtual ~Transport();

		static void Send(const Packet& packet);

		virtual void Flush(Thread& callingThread) = 0;

	protected:

		virtual Buffer& BuildFinalBuffer(Thread& callingThread);

		static unsigned int NumItems;

		static const unsigned short Version = 0;
	};

};

#endif // _ALLOCLAVE_TRANSPORT_H

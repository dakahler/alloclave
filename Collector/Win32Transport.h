#ifndef _ALLOCLAVE_WIN32TRANSPORT_H
#define _ALLOCLAVE_WIN32TRANSPORT_H

#include "Transport.h"

namespace Alloclave
{

	class Win32Transport : public Transport
	{
	public:
		Win32Transport();

		virtual ~Win32Transport();

		virtual void Connect();
		virtual void Disconnect();

	protected:

		virtual void Flush();
	};

};


#endif // _ALLOCLAVE_WIN32TRANSPORT_H
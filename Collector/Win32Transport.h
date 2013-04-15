#ifndef _ALLOCLAVE_WIN32TRANSPORT_H
#define _ALLOCLAVE_WIN32TRANSPORT_H

#include "PlatformWrapper.h"
#include "Transport.h"

namespace Alloclave
{

	class Win32Transport : public Transport
	{
	public:
		Win32Transport();

		virtual ~Win32Transport();

		void Flush();

	protected:

	private:

		void FindVisualizer();

		void* VisualizerHandle;
	};

};

#endif // _ALLOCLAVE_WIN32TRANSPORT_H

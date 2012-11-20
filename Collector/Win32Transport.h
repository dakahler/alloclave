#ifndef _ALLOCLAVE_WIN32TRANSPORT_H
#define _ALLOCLAVE_WIN32TRANSPORT_H

#include <Windows.h>
#include "Transport.h"

namespace Alloclave
{

	class Win32Transport : public Transport
	{
	public:
		Win32Transport();

		virtual ~Win32Transport();

		void Connect();
		void Disconnect();

	protected:

		void Flush();

	private:

		void FindVisualizer();

		HWND VisualizerHandle;
	};

};


#endif // _ALLOCLAVE_WIN32TRANSPORT_H
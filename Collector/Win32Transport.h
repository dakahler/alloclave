#ifndef _ALLOCLAVE_WIN32TRANSPORT_H
#define _ALLOCLAVE_WIN32TRANSPORT_H

#include "Transport.h"
#include <Windows.h>

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
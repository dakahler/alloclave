// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_WIN32TRANSPORT_H
#define _ALLOCLAVE_WIN32TRANSPORT_H

#include "Transport.h"

namespace Alloclave
{

	// Win32-specific transport that uses WM_COPYDATA messages
	class Win32Transport : public Transport
	{
	public:
		Win32Transport();

		virtual ~Win32Transport();

		void Flush(Thread& callingThread);

	protected:

	private:

		void FindVisualizer();

		void* VisualizerHandle;
	};

};

#endif // _ALLOCLAVE_WIN32TRANSPORT_H

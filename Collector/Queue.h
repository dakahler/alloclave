#ifndef _ALLOCLAVE_QUEUE_H
#define _ALLOCLAVE_QUEUE_H

#include "Buffer.h"

namespace Alloclave
{

	class Queue
	{
	public:
		void Enqueue(const Buffer& buffer);
		Buffer Dequeue();
		const Buffer& Peek();
	};

};


#endif // _ALLOCLAVE_QUEUE_H
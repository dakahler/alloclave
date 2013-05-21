// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_QUEUE_H
#define _ALLOCLAVE_QUEUE_H

#include "Buffer.h"

namespace Alloclave
{

	// A generic queue for storing a FIFO collection of buffers
	class Queue
	{
	public:
		Queue();
		~Queue();

		void Enqueue(const Buffer& buffer);
		Buffer Dequeue();
		unsigned int GetNumItems() const;

	private:
		struct QueueItem
		{
			QueueItem();

			Buffer Data;
			QueueItem* Next;
		};

		QueueItem* Head;
		QueueItem* Tail;

		unsigned int NumItems;
	};

};

#endif // _ALLOCLAVE_QUEUE_H

// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_QUEUE_H
#define _ALLOCLAVE_QUEUE_H

#include "Buffer.h"

namespace Alloclave
{

	class Queue
	{
	public:
		Queue();
		~Queue();

		void Enqueue(const Buffer& buffer);
		Buffer Dequeue();
		const Buffer& Peek() const;
		unsigned int GetNumItems() const;

	private:
		struct QueueItem
		{
			// TODO: Move to cpp
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

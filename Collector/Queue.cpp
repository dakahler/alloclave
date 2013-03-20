
#include <stdlib.h>
#include <new>

#include "Queue.h"

namespace Alloclave
{

Queue::Queue()
{
	Head = NULL;
	Tail = NULL;
	NumItems = 0;
}

Queue::~Queue()
{
	while (NumItems > 0)
	{
		Dequeue();
	}
}

void Queue::Enqueue(const Buffer& buffer)
{
	QueueItem* newItem = (QueueItem*)real_malloc(sizeof(QueueItem));
	if (newItem == NULL)
	{
		return;
	}

	new (newItem) QueueItem();
	newItem->Data = buffer;

	if (Tail)
	{
		Tail->Next = newItem;
	}

	if (!Head)
	{
		Head = newItem;
	}

	Tail = newItem;
	NumItems++;
}

Buffer Queue::Dequeue()
{
	if (Head)
	{
		Buffer buffer = Head->Data;
		QueueItem* oldHead = Head;
		Head = Head->Next;
		oldHead->~QueueItem();
		real_free(oldHead);
		NumItems--;

		if (Head == NULL)
		{
			Tail = NULL;
		}

		return buffer;
	}

	return Buffer();
}

const Buffer& Queue::Peek() const
{
	if (Head)
	{
		return Head->Data;
	}

	// TODO: Make this better
	static Buffer s_buffer;
	return s_buffer;
}

unsigned int Queue::GetNumItems() const
{
	return NumItems;
}

Queue::QueueItem::QueueItem()
{
	Next = NULL;
}

}


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
	QueueItem* newItem = (QueueItem*)malloc(sizeof(QueueItem));
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
		free(oldHead);
		NumItems--;
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

	return Buffer();
}

unsigned int Queue::GetNumItems() const
{
	return NumItems;
}

}

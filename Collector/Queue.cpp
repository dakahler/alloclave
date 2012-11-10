#include "Queue.h"

namespace Alloclave
{

void Queue::Enqueue(const Buffer& buffer)
{
	// TODO
}

Buffer Queue::Dequeue()
{
	return Buffer(); // TODO
}

const Buffer& Queue::Peek()
{
	// TODO
	static Buffer buffer;
	return buffer;
}

}

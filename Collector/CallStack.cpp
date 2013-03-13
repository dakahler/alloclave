
#include "CallStack.h"

namespace Alloclave
{

CallStack::CallStack()
{
	StackDepth = 0;
}

void CallStack::Rebuild()
{
	StackDepth = 0;
}

Buffer CallStack::Serialize() const
{
	Buffer buffer;

	buffer.Add((void*)&StackDepth, sizeof(StackDepth));
	for (int i = 0; i < StackDepth; i++)
	{
		buffer.Add((void*)&StackAddresses[i], sizeof(StackAddresses[i]));
	}

	return buffer;
}

void CallStack::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{
	assert(false);
}

void CallStack::Push(void* address)
{
	assert(StackDepth < MaxStackDepth);

	StackAddresses[StackDepth] = address;
	StackDepth++;
}

}

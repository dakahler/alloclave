#include "Allocation.h"

namespace Alloclave
{

Allocation::Allocation()
{
	Address = NULL;
	Size = 0;
	Alignment = 0;
	Stack = NULL;
}

Buffer Allocation::Serialize() const
{
	unsigned int bufferSize = sizeof(Address) + sizeof(Size) + sizeof(Alignment);
	Buffer buffer(bufferSize);

	buffer.Add((void*)&Address, sizeof(Address));
	buffer.Add((void*)&Size, sizeof(Size));
	buffer.Add((void*)&Alignment, sizeof(Alignment));

	return buffer;
}

void Allocation::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{

}

}

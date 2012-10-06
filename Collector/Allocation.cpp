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
	return Buffer(); // TODO
}

void Allocation::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{

}

}

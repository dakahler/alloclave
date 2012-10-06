#include "Buffer.h"

namespace Alloclave
{

Buffer::Buffer()
{
	Data = NULL;
	Resize(DefaultSize);
}

Buffer::Buffer(unsigned int initialSize)
{
	Data = NULL;
	Resize(initialSize);
}

Buffer::~Buffer()
{
	delete[] Data;
}

void Buffer::Resize(unsigned int newSize)
{
	if (Data == NULL)
	{
		Data = new char[newSize];
	}
	else
	{
		char* newData = new char[newSize];
		unsigned int numBytesToCopy = MIN(newSize, CurrentSize);
		memmove(newData, Data, numBytesToCopy);
		delete[] Data;
		Data = newData;
	}

	CurrentSize = newSize;
}

}

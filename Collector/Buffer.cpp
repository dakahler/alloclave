#include "Buffer.h"
#include <stdlib.h>

namespace Alloclave
{

Buffer::Buffer()
{
	Data = NULL;
	Position = 0;
	CurrentSize = 0;
	Resize(DefaultSize);
}

Buffer::Buffer(unsigned int initialSize)
{
	Data = NULL;
	Position = 0;
	CurrentSize = 0;
	Resize(initialSize);
}

Buffer::Buffer(const Buffer& other)
{
	CurrentSize = other.CurrentSize;
	Position = other.Position;
	Data = NULL;

	if (CurrentSize > 0)
	{
		Data = (char*)malloc(CurrentSize);
		memcpy(Data, other.Data, CurrentSize);
	}
}

Buffer::~Buffer()
{
	free(Data);
}

void Buffer::Resize(unsigned int newSize)
{
	if (Data == NULL)
	{
		Data = (char*)malloc(newSize);
	}
	else
	{
		char* newData = (char*)malloc(newSize);
		unsigned int numBytesToCopy = MIN(newSize, CurrentSize);
		memmove(newData, Data, numBytesToCopy);
		free(Data);
		Data = newData;

		if (Position > newSize)
		{
			Position = newSize;
		}
	}

	CurrentSize = newSize;
}

void Buffer::Add(void* data, unsigned int dataSize)
{
	// TODO: Error checking

	memcpy(Data + Position, data, dataSize);
}

}

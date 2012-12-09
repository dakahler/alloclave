
#include <stdlib.h>
#include "Buffer.h"

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
		Data = (char*)real_malloc(CurrentSize);
		if (Data)
		{
			memcpy(Data, other.Data, CurrentSize);
		}
		else
		{
			CurrentSize = 0;
			Position = 0;
		}
	}
}

Buffer::~Buffer()
{
	real_free(Data);
}

void Buffer::Resize(unsigned int newSize)
{
	if (Data == NULL)
	{
		Data = (char*)real_malloc(newSize);
	}
	else
	{
		char* newData = (char*)real_malloc(newSize);
		unsigned int numBytesToCopy = MIN(newSize, CurrentSize);
		memcpy(newData, Data, numBytesToCopy);
		real_free(Data);
		Data = newData;

		//if (Position > newSize)
		//{
		//	Position = newSize;
		//}
	}

	CurrentSize = newSize;
}

void Buffer::Add(void* data, unsigned int dataSize)
{
	// TODO: Error checking
	while (Position + dataSize >= CurrentSize)
	{
		// TODO: Better resizing approach?
		Resize(CurrentSize * 2);
	}

	memcpy(Data + Position, data, dataSize);
	Position += dataSize;
}

void Buffer::Add(const Buffer& buffer)
{
	Add(buffer.Data, buffer.CurrentSize);
}

const void* Buffer::GetData() const
{
	return Data;
}

unsigned int Buffer::GetSize() const
{
	return Position;
}

}

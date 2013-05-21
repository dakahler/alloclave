// Copyright Circular Shift. For license information, see license.txt.

#include "Buffer.h"
#include <memory.h>

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
	Copy(other, *this);
}

Buffer& Buffer::operator=(const Buffer& rhs)
{
	if (this == &rhs)
	{
		return *this;
	}

	Copy(rhs, *this);

	return *this;
}

void Buffer::Copy(const Buffer& from, Buffer& to)
{
	to.CurrentSize = from.CurrentSize;
	to.Position = from.Position;
	to.Data = NULL;

	if (to.CurrentSize > 0)
	{
		to.Data = (char*)real_malloc(to.CurrentSize);
		if (to.Data)
		{
			memcpy(to.Data, from.Data, to.CurrentSize);
		}
		else
		{
			to.CurrentSize = 0;
			to.Position = 0;
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
		// Allocate the new size, copy the data, and free the old memory
		char* newData = (char*)real_malloc(newSize);
		unsigned int numBytesToCopy = MIN(newSize, CurrentSize);
		memcpy(newData, Data, numBytesToCopy);
		real_free(Data);
		Data = newData;
	}

	CurrentSize = newSize;
}

void Buffer::Add(void* data, unsigned int dataSize)
{
	assert(dataSize > 0);

	while (Position + dataSize >= CurrentSize)
	{
		// Grow exponentially to accomodate new data
		Resize(CurrentSize * 2);
	}

	memcpy(Data + Position, data, dataSize);
	Position += dataSize;
}

void Buffer::Add(const Buffer& buffer)
{
	Add(buffer.Data, buffer.Position);
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

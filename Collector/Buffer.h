#ifndef _ALLOCLAVE_BUFFER_H
#define _ALLOCLAVE_BUFFER_H

#include <string.h>

// TODO: Better implementation of these
#define MIN(a,b) (((a)<(b))?(a):(b))
#define MAX(a,b) (((a)>(b))?(a):(b))

namespace Alloclave
{

	class Buffer
	{
	public:
		Buffer();
		Buffer(unsigned int initialSize);
		Buffer(const Buffer& other);

		virtual ~Buffer();

		void Resize(unsigned int newSize);

		void Add(void* data, unsigned int dataSize);
		void Add(const Buffer& buffer);

		const void* GetData() const;
		unsigned int GetSize() const;

	private:

		static const unsigned int DefaultSize = 64;
		unsigned int CurrentSize;

		unsigned int Position;

		char* Data;
	};

};


#endif // _ALLOCLAVE_BUFFER_H
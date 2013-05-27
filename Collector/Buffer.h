// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_BUFFER_H
#define _ALLOCLAVE_BUFFER_H

#include "Common.h"

// Define NULL if necessary
#ifndef NULL
#ifdef __cplusplus
#define NULL    0
#else
#define NULL    ((void *)0)
#endif
#endif

#define MIN(a,b) (((a)<(b))?(a):(b))
#define MAX(a,b) (((a)>(b))?(a):(b))

namespace Alloclave
{

	// A generic buffer class that collects streams of raw binary data
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

		void Clear();

		const void* GetData() const;
		unsigned int GetSize() const;

		Buffer& operator=(const Buffer& rhs);

	private:

		static void Copy(const Buffer& from, Buffer& to);

		static const unsigned int DefaultSize = 64;
		unsigned int CurrentSize;

		unsigned int Position;

		char* Data;
	};

};

#endif // _ALLOCLAVE_BUFFER_H

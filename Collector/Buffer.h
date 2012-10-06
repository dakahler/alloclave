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

		virtual ~Buffer();

		void Resize(unsigned int newSize);

		char* Data;

	private:

		static const unsigned int DefaultSize = 64;
		unsigned int CurrentSize;
	};

};


#endif // _ALLOCLAVE_BUFFER_H
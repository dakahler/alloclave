#ifndef _ALLOCLAVE_CALLSTACK_H
#define _ALLOCLAVE_CALLSTACK_H

#include "ICustomSerializable.h"
#include "Buffer.h"

namespace Alloclave
{

	class CallStack : public ICustomSerializable
	{
	public:
		CallStack();

		virtual void Rebuild();
		
		virtual Buffer Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);
	protected:
		virtual void Push(void* address);

		static const int MaxStackDepth = 62;

	private:
		void* StackAddresses[MaxStackDepth];
		int StackDepth;
	};

};


#endif // _ALLOCLAVE_CALLSTACK_H
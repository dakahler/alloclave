#include "SetArchitecture.h"


namespace Alloclave
{

SetArchitecture::SetArchitecture()
{
	
}

Buffer& SetArchitecture::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// This sends along the number of bytes in a pointer
	// so the visualizer can auto-detect 32- or 64-bit
	unsigned short pointerSize = (unsigned short)sizeof(void*);
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&pointerSize, sizeof(pointerSize));

	return buffer;
}

void SetArchitecture::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

Packet::PacketType SetArchitecture::GetPacketType() const
{
	return PacketType_SetArchitecture;
}

}

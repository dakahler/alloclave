#include "SetSymbols.h"
#include <string.h>


namespace Alloclave
{

SetSymbols::SetSymbols(const wchar_t* symbolsPath)
	: SymbolsPath(symbolsPath)
{
	
}

Buffer& SetSymbols::Serialize() const
{
	Buffer& baseBuffer = Packet::Serialize();
	static Buffer buffer;
	buffer.Clear();

	// This sends the path of the symbols file to the visualizer
	unsigned short stringLength = (unsigned short)wcslen(SymbolsPath);
	buffer.Add((void*)baseBuffer.GetData(), baseBuffer.GetSize());
	buffer.Add((void*)&stringLength, sizeof(stringLength));
	buffer.Add((void*)SymbolsPath, stringLength * sizeof(wchar_t));

	return buffer;
}

void SetSymbols::Deserialize(const Buffer& /*buffer*/, unsigned int /*bufferLength*/)
{
	assert(false);
}

Packet::PacketType SetSymbols::GetPacketType() const
{
	return PacketType_SetSymbols;
}

}

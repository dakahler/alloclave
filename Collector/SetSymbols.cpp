
#include "SetSymbols.h"
#include <string.h>


namespace Alloclave
{

SetSymbols::SetSymbols(const char* symbolsPath)
	: SymbolsPath(symbolsPath)
{
	
}

Buffer SetSymbols::Serialize() const
{
	Buffer buffer = Packet::Serialize();

	unsigned short stringLength = (unsigned short)strlen(SymbolsPath);
	buffer.Add((void*)&stringLength, sizeof(stringLength));
	buffer.Add((void*)SymbolsPath, stringLength);

	return buffer;
}

void SetSymbols::Deserialize(const Buffer& buffer, unsigned int bufferLength)
{
	assert(false);
}

Packet::PacketType SetSymbols::GetPacketType() const
{
	return PacketType_SetSymbols;
}

}

#ifndef _ALLOCLAVE_SETSYMBOLS_H
#define _ALLOCLAVE_SETSYMBOLS_H

#include "Packet.h"

namespace Alloclave
{

	// Sends a symbol path
	class SetSymbols : public Packet
	{
	public:

		SetSymbols(const wchar_t* symbolsPath);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		const wchar_t* SymbolsPath;
	};

};

#endif // _ALLOCLAVE_SETSYMBOLS_H

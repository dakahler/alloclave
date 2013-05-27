// Copyright Circular Shift. For license information, see license.txt.

#ifndef _ALLOCLAVE_SETSYMBOLS_H
#define _ALLOCLAVE_SETSYMBOLS_H

#include "Packet.h"

namespace Alloclave
{

	// Sends a symbol path
	class SetSymbols : public Packet
	{
	public:

		SetSymbols(const char* symbolsPath);

		virtual Buffer& Serialize() const;
		virtual void Deserialize(const Buffer& buffer, unsigned int bufferLength);

	protected:
		PacketType GetPacketType() const;

	private:
		const char* SymbolsPath;
	};

};

#endif // _ALLOCLAVE_SETSYMBOLS_H

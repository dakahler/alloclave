
#include "Registration.h"
#include "Win32Transport.h"

using namespace Alloclave;

static Win32Transport s_Transport;

void main()
{
	RegisterTransport(&s_Transport);
	char* testAllocation = new char[100];
}
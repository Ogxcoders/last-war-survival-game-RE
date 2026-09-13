using System;

namespace Mono.Security.Interface;

[Flags]
public enum TlsProtocols
{
	Zero = 0,
	Tls10Client = 0x80,
	Tls10Server = 0x40,
	Tls10 = 0xC0,
	Tls11Client = 0x200,
	Tls11Server = 0x100,
	Tls11 = 0x300,
	Tls12Client = 0x800,
	Tls12Server = 0x400,
	Tls12 = 0xC00,
	ClientMask = 0xA80,
	ServerMask = 0x540
}

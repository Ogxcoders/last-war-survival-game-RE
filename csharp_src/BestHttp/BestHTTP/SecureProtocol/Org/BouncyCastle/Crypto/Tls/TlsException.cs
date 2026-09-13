using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public class TlsException : IOException
{
	public TlsException(string message, Exception cause)
		: base(message, cause)
	{
	}
}

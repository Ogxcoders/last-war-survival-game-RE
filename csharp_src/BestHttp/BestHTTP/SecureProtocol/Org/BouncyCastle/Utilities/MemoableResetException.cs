using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

public class MemoableResetException : InvalidCastException
{
	public MemoableResetException(string msg)
		: base(msg)
	{
	}
}

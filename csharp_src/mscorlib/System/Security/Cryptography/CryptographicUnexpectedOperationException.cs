using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Cryptography;

[Serializable]
[ComVisible(true)]
public class CryptographicUnexpectedOperationException : CryptographicException
{
	public CryptographicUnexpectedOperationException()
	{
		SetErrorCode(-2146233295);
	}

	public CryptographicUnexpectedOperationException(string message)
		: base(message)
	{
		SetErrorCode(-2146233295);
	}

	public CryptographicUnexpectedOperationException(string format, string insert)
		: base(string.Format(CultureInfo.CurrentCulture, format, insert))
	{
		SetErrorCode(-2146233295);
	}

	public CryptographicUnexpectedOperationException(string message, Exception inner)
		: base(message, inner)
	{
		SetErrorCode(-2146233295);
	}

	protected CryptographicUnexpectedOperationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

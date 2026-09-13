using System.Runtime.Serialization;

namespace System.Security;

[Serializable]
public class VerificationException : SystemException
{
	public VerificationException()
		: base("Operation could destabilize the runtime.")
	{
		base.HResult = -2146233075;
	}

	public VerificationException(string message)
		: base(message)
	{
		base.HResult = -2146233075;
	}

	public VerificationException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146233075;
	}

	protected VerificationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

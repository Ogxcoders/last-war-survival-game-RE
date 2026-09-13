using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

[Serializable]
public class SafeArrayRankMismatchException : SystemException
{
	public SafeArrayRankMismatchException()
		: base("Specified array was not of the expected rank.")
	{
		base.HResult = -2146233032;
	}

	public SafeArrayRankMismatchException(string message)
		: base(message)
	{
		base.HResult = -2146233032;
	}

	public SafeArrayRankMismatchException(string message, Exception inner)
		: base(message, inner)
	{
		base.HResult = -2146233032;
	}

	protected SafeArrayRankMismatchException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

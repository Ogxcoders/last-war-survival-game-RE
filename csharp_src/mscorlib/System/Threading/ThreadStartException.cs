using System.Runtime.Serialization;

namespace System.Threading;

[Serializable]
public sealed class ThreadStartException : SystemException
{
	internal ThreadStartException()
		: base("Thread failed to start.")
	{
		base.HResult = -2146233051;
	}

	internal ThreadStartException(Exception reason)
		: base("Thread failed to start.", reason)
	{
		base.HResult = -2146233051;
	}

	private ThreadStartException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

using System.Runtime.Serialization;
using System.Security;

namespace System.Threading;

[Serializable]
public class BarrierPostPhaseException : Exception
{
	public BarrierPostPhaseException()
		: this((string)null)
	{
	}

	public BarrierPostPhaseException(Exception innerException)
		: this(null, innerException)
	{
	}

	public BarrierPostPhaseException(string message)
		: this(message, null)
	{
	}

	public BarrierPostPhaseException(string message, Exception innerException)
		: base((message == null) ? global::SR.GetString("The postPhaseAction failed with an exception.") : message, innerException)
	{
	}

	[SecurityCritical]
	protected BarrierPostPhaseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

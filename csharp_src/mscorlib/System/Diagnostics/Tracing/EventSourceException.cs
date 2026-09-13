using System.Runtime.Serialization;

namespace System.Diagnostics.Tracing;

[Serializable]
public class EventSourceException : Exception
{
	public EventSourceException()
		: base("An error occurred when writing to a listener.")
	{
	}

	public EventSourceException(string message)
		: base(message)
	{
	}

	public EventSourceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected EventSourceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	internal EventSourceException(Exception innerException)
		: base("An error occurred when writing to a listener.", innerException)
	{
	}
}

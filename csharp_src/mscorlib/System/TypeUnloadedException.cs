using System.Runtime.Serialization;

namespace System;

[Serializable]
public class TypeUnloadedException : SystemException
{
	public TypeUnloadedException()
		: base("Type had been unloaded.")
	{
		base.HResult = -2146234349;
	}

	public TypeUnloadedException(string message)
		: base(message)
	{
		base.HResult = -2146234349;
	}

	public TypeUnloadedException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146234349;
	}

	protected TypeUnloadedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

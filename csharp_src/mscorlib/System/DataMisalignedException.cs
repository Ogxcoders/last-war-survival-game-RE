using System.Runtime.Serialization;

namespace System;

[Serializable]
public sealed class DataMisalignedException : SystemException
{
	public DataMisalignedException()
		: base("A datatype misalignment was detected in a load or store instruction.")
	{
		base.HResult = -2146233023;
	}

	public DataMisalignedException(string message)
		: base(message)
	{
		base.HResult = -2146233023;
	}

	public DataMisalignedException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146233023;
	}

	internal DataMisalignedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

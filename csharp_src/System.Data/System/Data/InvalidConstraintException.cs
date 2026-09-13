using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class InvalidConstraintException : DataException
{
	protected InvalidConstraintException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InvalidConstraintException()
		: base("Invalid constraint.")
	{
		base.HResult = -2146232028;
	}

	public InvalidConstraintException(string s)
		: base(s)
	{
		base.HResult = -2146232028;
	}

	public InvalidConstraintException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232028;
	}
}

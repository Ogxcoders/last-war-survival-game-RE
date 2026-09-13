using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class ConstraintException : DataException
{
	protected ConstraintException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public ConstraintException()
		: base("Constraint Exception.")
	{
		base.HResult = -2146232022;
	}

	public ConstraintException(string s)
		: base(s)
	{
		base.HResult = -2146232022;
	}

	public ConstraintException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232022;
	}
}

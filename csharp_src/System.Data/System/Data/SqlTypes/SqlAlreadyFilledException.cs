using System.Runtime.Serialization;

namespace System.Data.SqlTypes;

[Serializable]
public sealed class SqlAlreadyFilledException : SqlTypeException
{
	public SqlAlreadyFilledException()
		: this(SQLResource.AlreadyFilledMessage, null)
	{
	}

	public SqlAlreadyFilledException(string message)
		: this(message, null)
	{
	}

	public SqlAlreadyFilledException(string message, Exception e)
		: base(message, e)
	{
		base.HResult = -2146232015;
	}

	private SqlAlreadyFilledException(SerializationInfo si, StreamingContext sc)
		: base(si, sc)
	{
	}
}

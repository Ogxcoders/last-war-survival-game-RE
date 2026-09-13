using System.Runtime.Serialization;

namespace System.Data.SqlTypes;

[Serializable]
public sealed class SqlNullValueException : SqlTypeException
{
	public SqlNullValueException()
		: this(SQLResource.NullValueMessage, null)
	{
	}

	public SqlNullValueException(string message)
		: this(message, null)
	{
	}

	public SqlNullValueException(string message, Exception e)
		: base(message, e)
	{
		base.HResult = -2146232015;
	}

	private SqlNullValueException(SerializationInfo si, StreamingContext sc)
		: base(SqlNullValueExceptionSerialization(si, sc), sc)
	{
	}

	private static SerializationInfo SqlNullValueExceptionSerialization(SerializationInfo si, StreamingContext sc)
	{
		if (si != null && 1 == si.MemberCount)
		{
			new SqlNullValueException(si.GetString("SqlNullValueExceptionMessage")).GetObjectData(si, sc);
		}
		return si;
	}
}

using System.Runtime.Serialization;

namespace System.Data.SqlTypes;

[Serializable]
public class SqlTypeException : SystemException
{
	public SqlTypeException()
		: this("SqlType error.", null)
	{
	}

	public SqlTypeException(string message)
		: this(message, null)
	{
	}

	public SqlTypeException(string message, Exception e)
		: base(message, e)
	{
		base.HResult = -2146232016;
	}

	protected SqlTypeException(SerializationInfo si, StreamingContext sc)
		: base(SqlTypeExceptionSerialization(si, sc), sc)
	{
	}

	private static SerializationInfo SqlTypeExceptionSerialization(SerializationInfo si, StreamingContext sc)
	{
		if (si != null && 1 == si.MemberCount)
		{
			new SqlTypeException(si.GetString("SqlTypeExceptionMessage")).GetObjectData(si, sc);
		}
		return si;
	}
}

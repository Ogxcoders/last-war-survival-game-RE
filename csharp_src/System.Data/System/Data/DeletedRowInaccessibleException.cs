using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class DeletedRowInaccessibleException : DataException
{
	protected DeletedRowInaccessibleException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public DeletedRowInaccessibleException()
		: base("Deleted rows inaccessible.")
	{
		base.HResult = -2146232031;
	}

	public DeletedRowInaccessibleException(string s)
		: base(s)
	{
		base.HResult = -2146232031;
	}

	public DeletedRowInaccessibleException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232031;
	}
}

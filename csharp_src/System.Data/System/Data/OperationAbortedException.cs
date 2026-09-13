using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public sealed class OperationAbortedException : SystemException
{
	private OperationAbortedException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232010;
	}

	private OperationAbortedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	internal static OperationAbortedException Aborted(Exception inner)
	{
		if (inner == null)
		{
			return new OperationAbortedException(global::SR.GetString("Operation aborted."), null);
		}
		return new OperationAbortedException(global::SR.GetString("Operation aborted due to an exception (see InnerException for details)."), inner);
	}
}

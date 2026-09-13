using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading;

[Serializable]
[ComVisible(true)]
public sealed class ThreadAbortException : SystemException
{
	public object ExceptionState
	{
		[SecuritySafeCritical]
		get
		{
			return Thread.CurrentThread.AbortReason;
		}
	}

	private ThreadAbortException()
		: base(Exception.GetMessageFromNativeResources(ExceptionMessageKind.ThreadAbort))
	{
		SetErrorCode(-2146233040);
	}

	internal ThreadAbortException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}

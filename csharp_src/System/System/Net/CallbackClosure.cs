using System.Threading;

namespace System.Net;

internal class CallbackClosure
{
	private AsyncCallback _savedCallback;

	private ExecutionContext _savedContext;

	internal AsyncCallback AsyncCallback => _savedCallback;

	internal ExecutionContext Context => _savedContext;

	internal CallbackClosure(ExecutionContext context, AsyncCallback callback)
	{
		if (callback != null)
		{
			_savedCallback = callback;
			_savedContext = context;
		}
	}

	internal bool IsCompatible(AsyncCallback callback)
	{
		if (callback == null || _savedCallback == null)
		{
			return false;
		}
		if (!object.Equals(_savedCallback, callback))
		{
			return false;
		}
		return true;
	}
}

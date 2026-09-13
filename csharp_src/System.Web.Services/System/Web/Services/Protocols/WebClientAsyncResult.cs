using System.Net;
using System.Threading;

namespace System.Web.Services.Protocols;

public class WebClientAsyncResult : IAsyncResult
{
	private AsyncCallback _callback;

	private object _asyncState;

	private bool _completedSynchronously;

	private bool _done;

	private ManualResetEvent _waitHandle;

	internal object Result;

	internal Exception Exception;

	internal WebRequest Request;

	public object AsyncState => _asyncState;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			lock (this)
			{
				if (_waitHandle != null)
				{
					return _waitHandle;
				}
				_waitHandle = new ManualResetEvent(_done);
				return _waitHandle;
			}
		}
	}

	public bool CompletedSynchronously => _completedSynchronously;

	public bool IsCompleted
	{
		get
		{
			lock (this)
			{
				return _done;
			}
		}
	}

	internal WebClientAsyncResult(WebRequest request, AsyncCallback callback, object asyncState)
	{
		_callback = callback;
		Request = request;
		_asyncState = asyncState;
	}

	public void Abort()
	{
		Request.Abort();
	}

	internal void SetCompleted(object result, Exception exception, bool async)
	{
		lock (this)
		{
			Exception = exception;
			Result = result;
			_done = true;
			_completedSynchronously = async;
			if (_waitHandle != null)
			{
				_waitHandle.Set();
			}
			Monitor.PulseAll(this);
		}
		if (_callback != null)
		{
			_callback(this);
		}
	}

	internal void WaitForComplete()
	{
		lock (this)
		{
			if (!_done)
			{
				Monitor.Wait(this);
			}
		}
	}
}

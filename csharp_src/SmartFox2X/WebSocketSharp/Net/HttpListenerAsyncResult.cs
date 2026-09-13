using System;
using System.Threading;

namespace WebSocketSharp.Net;

internal class HttpListenerAsyncResult : IAsyncResult
{
	private AsyncCallback _callback;

	private bool _completed;

	private HttpListenerContext _context;

	private Exception _exception;

	private object _state;

	private object _sync;

	private bool _syncCompleted;

	private ManualResetEvent _waitHandle;

	internal bool EndCalled;

	internal bool InGet;

	public object AsyncState => _state;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			lock (_sync)
			{
				return _waitHandle ?? (_waitHandle = new ManualResetEvent(_completed));
			}
		}
	}

	public bool CompletedSynchronously => _syncCompleted;

	public bool IsCompleted
	{
		get
		{
			lock (_sync)
			{
				return _completed;
			}
		}
	}

	public HttpListenerAsyncResult(AsyncCallback callback, object state)
	{
		_callback = callback;
		_state = state;
		_sync = new object();
	}

	private static void complete(HttpListenerAsyncResult asyncResult)
	{
		asyncResult._completed = true;
		asyncResult._waitHandle?.Set();
		AsyncCallback callback = asyncResult._callback;
		if (callback == null)
		{
			return;
		}
		ThreadPool.UnsafeQueueUserWorkItem(delegate
		{
			try
			{
				callback(asyncResult);
			}
			catch
			{
			}
		}, null);
	}

	internal void Complete(Exception exception)
	{
		_exception = ((InGet && exception is ObjectDisposedException) ? new HttpListenerException(500, "Listener closed.") : exception);
		lock (_sync)
		{
			complete(this);
		}
	}

	internal void Complete(HttpListenerContext context)
	{
		Complete(context, syncCompleted: false);
	}

	internal void Complete(HttpListenerContext context, bool syncCompleted)
	{
		HttpListener listener = context.Listener;
		if (!listener.Authenticate(context))
		{
			listener.BeginGetContext(this);
			return;
		}
		_context = context;
		_syncCompleted = syncCompleted;
		lock (_sync)
		{
			complete(this);
		}
	}

	internal HttpListenerContext GetContext()
	{
		if (_exception != null)
		{
			throw _exception;
		}
		return _context;
	}
}

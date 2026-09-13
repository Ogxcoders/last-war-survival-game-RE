using System.Threading;

namespace System.Runtime;

internal abstract class AsyncResult : IAsyncResult
{
	protected delegate bool AsyncCompletion(IAsyncResult result);

	private static AsyncCallback asyncCompletionWrapperCallback;

	private AsyncCallback callback;

	private bool completedSynchronously;

	private bool endCalled;

	private Exception exception;

	private bool isCompleted;

	private AsyncCompletion nextAsyncCompletion;

	private object state;

	private Action beforePrepareAsyncCompletionAction;

	private Func<IAsyncResult, bool> checkSyncValidationFunc;

	private ManualResetEvent manualResetEvent;

	private object thisLock;

	public object AsyncState => state;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			if (manualResetEvent != null)
			{
				return manualResetEvent;
			}
			lock (ThisLock)
			{
				if (manualResetEvent == null)
				{
					manualResetEvent = new ManualResetEvent(isCompleted);
				}
			}
			return manualResetEvent;
		}
	}

	public bool CompletedSynchronously => completedSynchronously;

	public bool HasCallback => callback != null;

	public bool IsCompleted => isCompleted;

	protected Action<AsyncResult, Exception> OnCompleting { get; set; }

	private object ThisLock => thisLock;

	protected Action<AsyncCallback, IAsyncResult> VirtualCallback { get; set; }

	protected AsyncResult(AsyncCallback callback, object state)
	{
		this.callback = callback;
		this.state = state;
		thisLock = new object();
	}

	protected void Complete(bool completedSynchronously)
	{
		if (isCompleted)
		{
			throw Fx.Exception.AsError(new InvalidOperationException(InternalSR.AsyncResultCompletedTwice(GetType())));
		}
		this.completedSynchronously = completedSynchronously;
		if (OnCompleting != null)
		{
			try
			{
				OnCompleting(this, exception);
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				exception = ex;
			}
		}
		if (completedSynchronously)
		{
			isCompleted = true;
		}
		else
		{
			lock (ThisLock)
			{
				isCompleted = true;
				if (manualResetEvent != null)
				{
					manualResetEvent.Set();
				}
			}
		}
		if (callback == null)
		{
			return;
		}
		try
		{
			if (VirtualCallback != null)
			{
				VirtualCallback(callback, this);
			}
			else
			{
				callback(this);
			}
		}
		catch (Exception innerException)
		{
			if (Fx.IsFatal(innerException))
			{
				throw;
			}
			throw Fx.Exception.AsError(new CallbackException("Async Callback Threw Exception", innerException));
		}
	}

	protected void Complete(bool completedSynchronously, Exception exception)
	{
		this.exception = exception;
		Complete(completedSynchronously);
	}

	private static void AsyncCompletionWrapperCallback(IAsyncResult result)
	{
		if (result == null)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("Invalid Null Async Result"));
		}
		if (result.CompletedSynchronously)
		{
			return;
		}
		AsyncResult asyncResult = (AsyncResult)result.AsyncState;
		if (!asyncResult.OnContinueAsyncCompletion(result))
		{
			return;
		}
		AsyncCompletion nextCompletion = asyncResult.GetNextCompletion();
		if (nextCompletion == null)
		{
			ThrowInvalidAsyncResult(result);
		}
		bool flag = false;
		Exception ex = null;
		try
		{
			flag = nextCompletion(result);
		}
		catch (Exception ex2)
		{
			if (Fx.IsFatal(ex2))
			{
				throw;
			}
			flag = true;
			ex = ex2;
		}
		if (flag)
		{
			asyncResult.Complete(completedSynchronously: false, ex);
		}
	}

	protected virtual bool OnContinueAsyncCompletion(IAsyncResult result)
	{
		return true;
	}

	protected void SetBeforePrepareAsyncCompletionAction(Action beforePrepareAsyncCompletionAction)
	{
		this.beforePrepareAsyncCompletionAction = beforePrepareAsyncCompletionAction;
	}

	protected void SetCheckSyncValidationFunc(Func<IAsyncResult, bool> checkSyncValidationFunc)
	{
		this.checkSyncValidationFunc = checkSyncValidationFunc;
	}

	protected AsyncCallback PrepareAsyncCompletion(AsyncCompletion callback)
	{
		if (beforePrepareAsyncCompletionAction != null)
		{
			beforePrepareAsyncCompletionAction();
		}
		nextAsyncCompletion = callback;
		if (asyncCompletionWrapperCallback == null)
		{
			asyncCompletionWrapperCallback = Fx.ThunkCallback(AsyncCompletionWrapperCallback);
		}
		return asyncCompletionWrapperCallback;
	}

	protected bool CheckSyncContinue(IAsyncResult result)
	{
		AsyncCompletion asyncCompletion;
		return TryContinueHelper(result, out asyncCompletion);
	}

	protected bool SyncContinue(IAsyncResult result)
	{
		if (TryContinueHelper(result, out var asyncCompletion))
		{
			return asyncCompletion(result);
		}
		return false;
	}

	private bool TryContinueHelper(IAsyncResult result, out AsyncCompletion callback)
	{
		if (result == null)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("Invalid Null Async Result"));
		}
		callback = null;
		if (checkSyncValidationFunc != null)
		{
			if (!checkSyncValidationFunc(result))
			{
				return false;
			}
		}
		else if (!result.CompletedSynchronously)
		{
			return false;
		}
		callback = GetNextCompletion();
		if (callback == null)
		{
			ThrowInvalidAsyncResult("Only call Check/SyncContinue once per async operation (once per PrepareAsyncCompletion).");
		}
		return true;
	}

	private AsyncCompletion GetNextCompletion()
	{
		AsyncCompletion result = nextAsyncCompletion;
		nextAsyncCompletion = null;
		return result;
	}

	protected static void ThrowInvalidAsyncResult(IAsyncResult result)
	{
		throw Fx.Exception.AsError(new InvalidOperationException(InternalSR.InvalidAsyncResultImplementation(result.GetType())));
	}

	protected static void ThrowInvalidAsyncResult(string debugText)
	{
		string message = "Invalid Async Result Implementation Generic";
		throw Fx.Exception.AsError(new InvalidOperationException(message));
	}

	protected static TAsyncResult End<TAsyncResult>(IAsyncResult result) where TAsyncResult : AsyncResult
	{
		if (result == null)
		{
			throw Fx.Exception.ArgumentNull("result");
		}
		if (!(result is TAsyncResult val))
		{
			throw Fx.Exception.Argument("result", "Invalid Async Result");
		}
		if (val.endCalled)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("Async Result Already Ended"));
		}
		val.endCalled = true;
		if (!val.isCompleted)
		{
			val.AsyncWaitHandle.WaitOne();
		}
		if (val.manualResetEvent != null)
		{
			val.manualResetEvent.Close();
		}
		if (val.exception != null)
		{
			throw Fx.Exception.AsError(val.exception);
		}
		return val;
	}
}

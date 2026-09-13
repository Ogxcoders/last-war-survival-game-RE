namespace System.Runtime;

internal abstract class AsyncEventArgs : IAsyncEventArgs
{
	private enum OperationState
	{
		Created,
		PendingCompletion,
		CompletedSynchronously,
		CompletedAsynchronously
	}

	private OperationState state;

	private object asyncState;

	private AsyncEventArgsCallback callback;

	private Exception exception;

	public Exception Exception => exception;

	public object AsyncState => asyncState;

	private OperationState State
	{
		set
		{
			switch (value)
			{
			case OperationState.PendingCompletion:
				if (state == OperationState.PendingCompletion)
				{
					throw Fx.Exception.AsError(new InvalidOperationException(InternalSR.AsyncEventArgsCompletionPending(GetType())));
				}
				break;
			case OperationState.CompletedSynchronously:
			case OperationState.CompletedAsynchronously:
				if (state != OperationState.PendingCompletion)
				{
					throw Fx.Exception.AsError(new InvalidOperationException(InternalSR.AsyncEventArgsCompletedTwice(GetType())));
				}
				break;
			}
			state = value;
		}
	}

	public void Complete(bool completedSynchronously)
	{
		Complete(completedSynchronously, null);
	}

	public virtual void Complete(bool completedSynchronously, Exception exception)
	{
		this.exception = exception;
		if (completedSynchronously)
		{
			State = OperationState.CompletedSynchronously;
			return;
		}
		State = OperationState.CompletedAsynchronously;
		callback(this);
	}

	protected void SetAsyncState(AsyncEventArgsCallback callback, object state)
	{
		if (callback == null)
		{
			throw Fx.Exception.ArgumentNull("callback");
		}
		State = OperationState.PendingCompletion;
		asyncState = state;
		this.callback = callback;
	}
}
internal class AsyncEventArgs<TArgument> : AsyncEventArgs
{
	public TArgument Arguments { get; private set; }

	public virtual void Set(AsyncEventArgsCallback callback, TArgument arguments, object state)
	{
		SetAsyncState(callback, state);
		Arguments = arguments;
	}
}
internal class AsyncEventArgs<TArgument, TResult> : AsyncEventArgs<TArgument>
{
	public TResult Result { get; set; }
}

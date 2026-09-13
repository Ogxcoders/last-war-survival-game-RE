using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks;

internal class RendezvousAwaitable<TResult> : ICriticalNotifyCompletion, INotifyCompletion
{
	private static readonly Action s_completionSentinel = delegate
	{
	};

	private Action _continuation;

	private ExceptionDispatchInfo _error;

	private TResult _result;

	public bool RunContinuationsAsynchronously { get; set; } = true;

	public bool IsCompleted => Volatile.Read(ref _continuation) != null;

	public RendezvousAwaitable<TResult> GetAwaiter()
	{
		return this;
	}

	public TResult GetResult()
	{
		_continuation = null;
		ExceptionDispatchInfo error = _error;
		if (error != null)
		{
			_error = null;
			error.Throw();
		}
		TResult result = _result;
		_result = default(TResult);
		return result;
	}

	public void SetResult(TResult result)
	{
		_result = result;
		NotifyAwaiter();
	}

	public void SetCanceled(CancellationToken token = default(CancellationToken))
	{
		SetException(token.IsCancellationRequested ? new OperationCanceledException(token) : new OperationCanceledException());
	}

	public void SetException(Exception exception)
	{
		_error = ExceptionDispatchInfo.Capture(exception);
		NotifyAwaiter();
	}

	private void NotifyAwaiter()
	{
		Action action = _continuation ?? Interlocked.CompareExchange(ref _continuation, s_completionSentinel, null);
		if (action != null)
		{
			if (RunContinuationsAsynchronously)
			{
				Task.Run(action);
			}
			else
			{
				action();
			}
		}
	}

	public void OnCompleted(Action continuation)
	{
		if ((_continuation ?? Interlocked.CompareExchange(ref _continuation, continuation, null)) != null)
		{
			Task.Run(continuation);
		}
	}

	public void UnsafeOnCompleted(Action continuation)
	{
		OnCompleted(continuation);
	}

	[Conditional("DEBUG")]
	private void AssertResultConsistency(bool expectedCompleted)
	{
	}
}

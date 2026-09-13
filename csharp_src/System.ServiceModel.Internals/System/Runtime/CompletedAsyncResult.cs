namespace System.Runtime;

internal class CompletedAsyncResult : AsyncResult
{
	public CompletedAsyncResult(AsyncCallback callback, object state)
		: base(callback, state)
	{
		Complete(completedSynchronously: true);
	}

	public static void End(IAsyncResult result)
	{
		Fx.AssertAndThrowFatal(result.IsCompleted, "CompletedAsyncResult was not completed!");
		AsyncResult.End<CompletedAsyncResult>(result);
	}
}
internal class CompletedAsyncResult<T> : AsyncResult
{
	private T data;

	public CompletedAsyncResult(T data, AsyncCallback callback, object state)
		: base(callback, state)
	{
		this.data = data;
		Complete(completedSynchronously: true);
	}

	public static T End(IAsyncResult result)
	{
		Fx.AssertAndThrowFatal(result.IsCompleted, "CompletedAsyncResult<T> was not completed!");
		return AsyncResult.End<CompletedAsyncResult<T>>(result).data;
	}
}
internal class CompletedAsyncResult<TResult, TParameter> : AsyncResult
{
	private TResult resultData;

	private TParameter parameter;

	public CompletedAsyncResult(TResult resultData, TParameter parameter, AsyncCallback callback, object state)
		: base(callback, state)
	{
		this.resultData = resultData;
		this.parameter = parameter;
		Complete(completedSynchronously: true);
	}

	public static TResult End(IAsyncResult result, out TParameter parameter)
	{
		Fx.AssertAndThrowFatal(result.IsCompleted, "CompletedAsyncResult<T> was not completed!");
		CompletedAsyncResult<TResult, TParameter> completedAsyncResult = AsyncResult.End<CompletedAsyncResult<TResult, TParameter>>(result);
		parameter = completedAsyncResult.parameter;
		return completedAsyncResult.resultData;
	}
}

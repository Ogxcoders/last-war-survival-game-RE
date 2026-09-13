namespace System.Runtime;

internal abstract class TypedAsyncResult<T> : AsyncResult
{
	private T data;

	public T Data => data;

	public TypedAsyncResult(AsyncCallback callback, object state)
		: base(callback, state)
	{
	}

	protected void Complete(T data, bool completedSynchronously)
	{
		this.data = data;
		Complete(completedSynchronously);
	}

	public static T End(IAsyncResult result)
	{
		return AsyncResult.End<TypedAsyncResult<T>>(result).Data;
	}
}

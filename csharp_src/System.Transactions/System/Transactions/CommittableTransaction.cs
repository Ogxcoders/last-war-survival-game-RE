using System.Runtime.Serialization;
using System.Threading;

namespace System.Transactions;

[Serializable]
public sealed class CommittableTransaction : Transaction, ISerializable, IDisposable, IAsyncResult
{
	private TransactionOptions options;

	private AsyncCallback callback;

	private object user_defined_state;

	private IAsyncResult asyncResult;

	object IAsyncResult.AsyncState => user_defined_state;

	WaitHandle IAsyncResult.AsyncWaitHandle => asyncResult.AsyncWaitHandle;

	bool IAsyncResult.CompletedSynchronously => asyncResult.CompletedSynchronously;

	bool IAsyncResult.IsCompleted => asyncResult.IsCompleted;

	public CommittableTransaction()
		: this(default(TransactionOptions))
	{
	}

	public CommittableTransaction(TimeSpan timeout)
		: base(IsolationLevel.Serializable)
	{
		options = default(TransactionOptions);
		options.Timeout = timeout;
	}

	public CommittableTransaction(TransactionOptions options)
		: base(options.IsolationLevel)
	{
		this.options = options;
	}

	public IAsyncResult BeginCommit(AsyncCallback asyncCallback, object asyncState)
	{
		callback = asyncCallback;
		user_defined_state = asyncState;
		AsyncCallback asyncCallback2 = null;
		if (asyncCallback != null)
		{
			asyncCallback2 = CommitCallback;
		}
		asyncResult = BeginCommitInternal(asyncCallback2);
		return this;
	}

	public void EndCommit(IAsyncResult asyncResult)
	{
		if (asyncResult != this)
		{
			throw new ArgumentException("The IAsyncResult parameter must be the same parameter as returned by BeginCommit.", "asyncResult");
		}
		EndCommitInternal(this.asyncResult);
	}

	private void CommitCallback(IAsyncResult ar)
	{
		if (asyncResult == null && ar.CompletedSynchronously)
		{
			asyncResult = ar;
		}
		callback(this);
	}

	public void Commit()
	{
		CommitInternal();
	}

	[System.MonoTODO("Not implemented")]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new NotImplementedException();
	}
}

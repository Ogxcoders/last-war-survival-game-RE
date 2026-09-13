using System.Threading;

namespace System.ComponentModel;

public sealed class AsyncOperation
{
	private readonly SynchronizationContext _syncContext;

	private readonly object _userSuppliedState;

	private bool _alreadyCompleted;

	public object UserSuppliedState => _userSuppliedState;

	public SynchronizationContext SynchronizationContext => _syncContext;

	private AsyncOperation(object userSuppliedState, SynchronizationContext syncContext)
	{
		_userSuppliedState = userSuppliedState;
		_syncContext = syncContext;
		_alreadyCompleted = false;
		_syncContext.OperationStarted();
	}

	~AsyncOperation()
	{
		if (!_alreadyCompleted && _syncContext != null)
		{
			_syncContext.OperationCompleted();
		}
	}

	public void Post(SendOrPostCallback d, object arg)
	{
		PostCore(d, arg, markCompleted: false);
	}

	public void PostOperationCompleted(SendOrPostCallback d, object arg)
	{
		PostCore(d, arg, markCompleted: true);
		OperationCompletedCore();
	}

	public void OperationCompleted()
	{
		VerifyNotCompleted();
		_alreadyCompleted = true;
		OperationCompletedCore();
	}

	private void PostCore(SendOrPostCallback d, object arg, bool markCompleted)
	{
		VerifyNotCompleted();
		VerifyDelegateNotNull(d);
		if (markCompleted)
		{
			_alreadyCompleted = true;
		}
		_syncContext.Post(d, arg);
	}

	private void OperationCompletedCore()
	{
		try
		{
			_syncContext.OperationCompleted();
		}
		finally
		{
			GC.SuppressFinalize(this);
		}
	}

	private void VerifyNotCompleted()
	{
		if (_alreadyCompleted)
		{
			throw new InvalidOperationException("This operation has already had OperationCompleted called on it and further calls are illegal.");
		}
	}

	private void VerifyDelegateNotNull(SendOrPostCallback d)
	{
		if (d == null)
		{
			throw new ArgumentNullException("d", "A non-null SendOrPostCallback must be supplied.");
		}
	}

	internal static AsyncOperation CreateOperation(object userSuppliedState, SynchronizationContext syncContext)
	{
		return new AsyncOperation(userSuppliedState, syncContext);
	}
}

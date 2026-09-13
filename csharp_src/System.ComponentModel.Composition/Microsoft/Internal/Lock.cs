using System;
using System.Threading;

namespace Microsoft.Internal;

internal sealed class Lock : IDisposable
{
	private ReaderWriterLockSlim _thisLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

	private int _isDisposed;

	public void EnterReadLock()
	{
		_thisLock.EnterReadLock();
	}

	public void EnterWriteLock()
	{
		_thisLock.EnterWriteLock();
	}

	public void ExitReadLock()
	{
		_thisLock.ExitReadLock();
	}

	public void ExitWriteLock()
	{
		_thisLock.ExitWriteLock();
	}

	public void Dispose()
	{
		if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
		{
			_thisLock.Dispose();
		}
	}
}

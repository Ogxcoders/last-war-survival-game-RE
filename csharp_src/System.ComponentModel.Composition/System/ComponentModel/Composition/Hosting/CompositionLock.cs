using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

internal sealed class CompositionLock : IDisposable
{
	public sealed class CompositionLockHolder : IDisposable
	{
		private CompositionLock _lock;

		private int _isDisposed;

		public CompositionLockHolder(CompositionLock @lock)
		{
			_lock = @lock;
			_isDisposed = 0;
			_lock.EnterCompositionLock();
		}

		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
			{
				_lock.ExitCompositionLock();
			}
		}
	}

	private sealed class EmptyLockHolder : IDisposable
	{
		public void Dispose()
		{
		}
	}

	private readonly Microsoft.Internal.Lock _stateLock;

	private static object _compositionLock = new object();

	private int _isDisposed;

	private bool _isThreadSafe;

	private static readonly EmptyLockHolder _EmptyLockHolder = new EmptyLockHolder();

	public bool IsThreadSafe => _isThreadSafe;

	public CompositionLock(bool isThreadSafe)
	{
		_isThreadSafe = isThreadSafe;
		if (isThreadSafe)
		{
			_stateLock = new Microsoft.Internal.Lock();
		}
	}

	public void Dispose()
	{
		if (_isThreadSafe && Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
		{
			_stateLock.Dispose();
		}
	}

	private void EnterCompositionLock()
	{
		if (_isThreadSafe)
		{
			Monitor.Enter(_compositionLock);
		}
	}

	private void ExitCompositionLock()
	{
		if (_isThreadSafe)
		{
			Monitor.Exit(_compositionLock);
		}
	}

	public IDisposable LockComposition()
	{
		if (_isThreadSafe)
		{
			return new CompositionLockHolder(this);
		}
		return _EmptyLockHolder;
	}

	public IDisposable LockStateForRead()
	{
		if (_isThreadSafe)
		{
			return new ReadLock(_stateLock);
		}
		return _EmptyLockHolder;
	}

	public IDisposable LockStateForWrite()
	{
		if (_isThreadSafe)
		{
			return new WriteLock(_stateLock);
		}
		return _EmptyLockHolder;
	}
}

using System.Diagnostics;

namespace System.Threading;

[DebuggerDisplay("Initial Count={InitialCount}, Current Count={CurrentCount}")]
public class CountdownEvent : IDisposable
{
	private int _initialCount;

	private volatile int _currentCount;

	private ManualResetEventSlim _event;

	private volatile bool _disposed;

	public int CurrentCount
	{
		get
		{
			int currentCount = _currentCount;
			if (currentCount >= 0)
			{
				return currentCount;
			}
			return 0;
		}
	}

	public int InitialCount => _initialCount;

	public bool IsSet => _currentCount <= 0;

	public WaitHandle WaitHandle
	{
		get
		{
			ThrowIfDisposed();
			return _event.WaitHandle;
		}
	}

	public CountdownEvent(int initialCount)
	{
		if (initialCount < 0)
		{
			throw new ArgumentOutOfRangeException("initialCount");
		}
		_initialCount = initialCount;
		_currentCount = initialCount;
		_event = new ManualResetEventSlim();
		if (initialCount == 0)
		{
			_event.Set();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_event.Dispose();
			_disposed = true;
		}
	}

	public bool Signal()
	{
		ThrowIfDisposed();
		if (_currentCount <= 0)
		{
			throw new InvalidOperationException("Invalid attempt made to decrement the event's count below zero.");
		}
		int num = Interlocked.Decrement(ref _currentCount);
		if (num == 0)
		{
			_event.Set();
			return true;
		}
		if (num < 0)
		{
			throw new InvalidOperationException("Invalid attempt made to decrement the event's count below zero.");
		}
		return false;
	}

	public bool Signal(int signalCount)
	{
		if (signalCount <= 0)
		{
			throw new ArgumentOutOfRangeException("signalCount");
		}
		ThrowIfDisposed();
		SpinWait spinWait = default(SpinWait);
		int currentCount;
		while (true)
		{
			currentCount = _currentCount;
			if (currentCount < signalCount)
			{
				throw new InvalidOperationException("Invalid attempt made to decrement the event's count below zero.");
			}
			if (Interlocked.CompareExchange(ref _currentCount, currentCount - signalCount, currentCount) == currentCount)
			{
				break;
			}
			spinWait.SpinOnce();
		}
		if (currentCount == signalCount)
		{
			_event.Set();
			return true;
		}
		return false;
	}

	public void AddCount()
	{
		AddCount(1);
	}

	public bool TryAddCount()
	{
		return TryAddCount(1);
	}

	public void AddCount(int signalCount)
	{
		if (!TryAddCount(signalCount))
		{
			throw new InvalidOperationException("The event is already signaled and cannot be incremented.");
		}
	}

	public bool TryAddCount(int signalCount)
	{
		if (signalCount <= 0)
		{
			throw new ArgumentOutOfRangeException("signalCount");
		}
		ThrowIfDisposed();
		SpinWait spinWait = default(SpinWait);
		while (true)
		{
			int currentCount = _currentCount;
			if (currentCount <= 0)
			{
				return false;
			}
			if (currentCount > int.MaxValue - signalCount)
			{
				throw new InvalidOperationException("The increment operation would cause the CurrentCount to overflow.");
			}
			if (Interlocked.CompareExchange(ref _currentCount, currentCount + signalCount, currentCount) == currentCount)
			{
				break;
			}
			spinWait.SpinOnce();
		}
		return true;
	}

	public void Reset()
	{
		Reset(_initialCount);
	}

	public void Reset(int count)
	{
		ThrowIfDisposed();
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		_currentCount = count;
		_initialCount = count;
		if (count == 0)
		{
			_event.Set();
		}
		else
		{
			_event.Reset();
		}
	}

	public void Wait()
	{
		Wait(-1, default(CancellationToken));
	}

	public void Wait(CancellationToken cancellationToken)
	{
		Wait(-1, cancellationToken);
	}

	public bool Wait(TimeSpan timeout)
	{
		long num = (long)timeout.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("timeout");
		}
		return Wait((int)num, default(CancellationToken));
	}

	public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
	{
		long num = (long)timeout.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("timeout");
		}
		return Wait((int)num, cancellationToken);
	}

	public bool Wait(int millisecondsTimeout)
	{
		return Wait(millisecondsTimeout, default(CancellationToken));
	}

	public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		if (millisecondsTimeout < -1)
		{
			throw new ArgumentOutOfRangeException("millisecondsTimeout");
		}
		ThrowIfDisposed();
		cancellationToken.ThrowIfCancellationRequested();
		bool flag = IsSet;
		if (!flag)
		{
			flag = _event.Wait(millisecondsTimeout, cancellationToken);
		}
		return flag;
	}

	private void ThrowIfDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("CountdownEvent");
		}
	}
}

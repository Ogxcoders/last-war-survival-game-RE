using System.Collections.Generic;
using System.Threading;

namespace System.Runtime;

internal class ThreadNeutralSemaphore
{
	private class EnterAsyncData
	{
		public ThreadNeutralSemaphore Semaphore { get; set; }

		public AsyncWaitHandle Waiter { get; set; }

		public FastAsyncCallback Callback { get; set; }

		public object State { get; set; }

		public EnterAsyncData(ThreadNeutralSemaphore semaphore, AsyncWaitHandle waiter, FastAsyncCallback callback, object state)
		{
			Waiter = waiter;
			Semaphore = semaphore;
			Callback = callback;
			State = state;
		}
	}

	private static Action<object, TimeoutException> enteredAsyncCallback;

	private bool aborted;

	private Func<Exception> abortedExceptionGenerator;

	private int count;

	private int maxCount;

	private object ThisLock = new object();

	private Queue<AsyncWaitHandle> waiters;

	private static Action<object, TimeoutException> EnteredAsyncCallback
	{
		get
		{
			if (enteredAsyncCallback == null)
			{
				enteredAsyncCallback = OnEnteredAsync;
			}
			return enteredAsyncCallback;
		}
	}

	private Queue<AsyncWaitHandle> Waiters
	{
		get
		{
			if (waiters == null)
			{
				waiters = new Queue<AsyncWaitHandle>();
			}
			return waiters;
		}
	}

	public ThreadNeutralSemaphore(int maxCount)
		: this(maxCount, null)
	{
	}

	public ThreadNeutralSemaphore(int maxCount, Func<Exception> abortedExceptionGenerator)
	{
		this.maxCount = maxCount;
		this.abortedExceptionGenerator = abortedExceptionGenerator;
	}

	public bool EnterAsync(TimeSpan timeout, FastAsyncCallback callback, object state)
	{
		AsyncWaitHandle asyncWaitHandle = null;
		lock (ThisLock)
		{
			if (aborted)
			{
				throw Fx.Exception.AsError(CreateObjectAbortedException());
			}
			if (count < maxCount)
			{
				count++;
				return true;
			}
			asyncWaitHandle = new AsyncWaitHandle();
			Waiters.Enqueue(asyncWaitHandle);
		}
		return asyncWaitHandle.WaitAsync(EnteredAsyncCallback, new EnterAsyncData(this, asyncWaitHandle, callback, state), timeout);
	}

	private static void OnEnteredAsync(object state, TimeoutException exception)
	{
		EnterAsyncData enterAsyncData = (EnterAsyncData)state;
		ThreadNeutralSemaphore semaphore = enterAsyncData.Semaphore;
		Exception asyncException = exception;
		if (exception != null && !semaphore.RemoveWaiter(enterAsyncData.Waiter))
		{
			asyncException = null;
		}
		if (semaphore.aborted)
		{
			asyncException = semaphore.CreateObjectAbortedException();
		}
		enterAsyncData.Callback(enterAsyncData.State, asyncException);
	}

	public bool TryEnter()
	{
		lock (ThisLock)
		{
			if (count < maxCount)
			{
				count++;
				return true;
			}
			return false;
		}
	}

	public void Enter(TimeSpan timeout)
	{
		if (!TryEnter(timeout))
		{
			throw Fx.Exception.AsError(CreateEnterTimedOutException(timeout));
		}
	}

	public bool TryEnter(TimeSpan timeout)
	{
		AsyncWaitHandle asyncWaitHandle = EnterCore();
		if (asyncWaitHandle != null)
		{
			bool flag = !asyncWaitHandle.Wait(timeout);
			if (aborted)
			{
				throw Fx.Exception.AsError(CreateObjectAbortedException());
			}
			if (flag && !RemoveWaiter(asyncWaitHandle))
			{
				flag = false;
			}
			return !flag;
		}
		return true;
	}

	internal static TimeoutException CreateEnterTimedOutException(TimeSpan timeout)
	{
		return new TimeoutException(InternalSR.LockTimeoutExceptionMessage(timeout));
	}

	private Exception CreateObjectAbortedException()
	{
		if (abortedExceptionGenerator != null)
		{
			return abortedExceptionGenerator();
		}
		return new OperationCanceledException("Thread Neutral Semaphore Aborted");
	}

	private bool RemoveWaiter(AsyncWaitHandle waiter)
	{
		bool result = false;
		lock (ThisLock)
		{
			for (int num = Waiters.Count; num > 0; num--)
			{
				AsyncWaitHandle asyncWaitHandle = Waiters.Dequeue();
				if (asyncWaitHandle == waiter)
				{
					result = true;
				}
				else
				{
					Waiters.Enqueue(asyncWaitHandle);
				}
			}
			return result;
		}
	}

	private AsyncWaitHandle EnterCore()
	{
		lock (ThisLock)
		{
			if (aborted)
			{
				throw Fx.Exception.AsError(CreateObjectAbortedException());
			}
			if (count < maxCount)
			{
				count++;
				return null;
			}
			AsyncWaitHandle asyncWaitHandle = new AsyncWaitHandle();
			Waiters.Enqueue(asyncWaitHandle);
			return asyncWaitHandle;
		}
	}

	public int Exit()
	{
		int result = -1;
		AsyncWaitHandle asyncWaitHandle;
		lock (ThisLock)
		{
			if (aborted)
			{
				return result;
			}
			if (count == 0)
			{
				string message = "Invalid Semaphore Exit";
				throw Fx.Exception.AsError(new SynchronizationLockException(message));
			}
			if (waiters == null || waiters.Count == 0)
			{
				count--;
				return count;
			}
			asyncWaitHandle = waiters.Dequeue();
			result = count;
		}
		asyncWaitHandle.Set();
		return result;
	}

	public void Abort()
	{
		lock (ThisLock)
		{
			if (aborted)
			{
				return;
			}
			aborted = true;
			if (waiters != null)
			{
				while (waiters.Count > 0)
				{
					waiters.Dequeue().Set();
				}
			}
		}
	}
}

using System.Collections.Generic;
using System.Security;
using System.Threading;

namespace System.Runtime;

internal class AsyncWaitHandle
{
	private class AsyncWaiter : ActionItem
	{
		[SecurityCritical]
		private Action<object, TimeoutException> callback;

		[SecurityCritical]
		private object state;

		private IOThreadTimer timer;

		private TimeSpan originalTimeout;

		public AsyncWaitHandle Parent { get; private set; }

		public bool TimedOut { get; set; }

		[SecuritySafeCritical]
		public AsyncWaiter(AsyncWaitHandle parent, Action<object, TimeoutException> callback, object state)
		{
			Parent = parent;
			this.callback = callback;
			this.state = state;
		}

		[SecuritySafeCritical]
		public void Call()
		{
			Schedule();
		}

		[SecurityCritical]
		protected override void Invoke()
		{
			callback(state, TimedOut ? new TimeoutException(InternalSR.TimeoutOnOperation(originalTimeout)) : null);
		}

		public void SetTimer(Action<object> callback, object state, TimeSpan timeout)
		{
			if (timer != null)
			{
				throw Fx.Exception.AsError(new InvalidOperationException("Must Cancel Old Timer"));
			}
			originalTimeout = timeout;
			timer = new IOThreadTimer(callback, state, isTypicallyCanceledShortlyAfterBeingSet: false);
			timer.Set(timeout);
		}

		public void CancelTimer()
		{
			if (timer != null)
			{
				timer.Cancel();
				timer = null;
			}
		}
	}

	private static Action<object> timerCompleteCallback;

	private List<AsyncWaiter> asyncWaiters;

	private bool isSignaled;

	private EventResetMode resetMode;

	private object syncObject;

	private int syncWaiterCount;

	public AsyncWaitHandle()
		: this(EventResetMode.AutoReset)
	{
	}

	public AsyncWaitHandle(EventResetMode resetMode)
	{
		this.resetMode = resetMode;
		syncObject = new object();
	}

	public bool WaitAsync(Action<object, TimeoutException> callback, object state, TimeSpan timeout)
	{
		if (!isSignaled || (isSignaled && resetMode == EventResetMode.AutoReset))
		{
			lock (syncObject)
			{
				if (isSignaled && resetMode == EventResetMode.AutoReset)
				{
					isSignaled = false;
				}
				else if (!isSignaled)
				{
					AsyncWaiter asyncWaiter = new AsyncWaiter(this, callback, state);
					if (asyncWaiters == null)
					{
						asyncWaiters = new List<AsyncWaiter>();
					}
					asyncWaiters.Add(asyncWaiter);
					if (timeout != TimeSpan.MaxValue)
					{
						if (timerCompleteCallback == null)
						{
							timerCompleteCallback = OnTimerComplete;
						}
						asyncWaiter.SetTimer(timerCompleteCallback, asyncWaiter, timeout);
					}
					return false;
				}
			}
		}
		return true;
	}

	private static void OnTimerComplete(object state)
	{
		AsyncWaiter asyncWaiter = (AsyncWaiter)state;
		AsyncWaitHandle parent = asyncWaiter.Parent;
		bool flag = false;
		lock (parent.syncObject)
		{
			if (parent.asyncWaiters != null && parent.asyncWaiters.Remove(asyncWaiter))
			{
				asyncWaiter.TimedOut = true;
				flag = true;
			}
		}
		asyncWaiter.CancelTimer();
		if (flag)
		{
			asyncWaiter.Call();
		}
	}

	public bool Wait(TimeSpan timeout)
	{
		if (!isSignaled || (isSignaled && resetMode == EventResetMode.AutoReset))
		{
			lock (syncObject)
			{
				if (isSignaled && resetMode == EventResetMode.AutoReset)
				{
					isSignaled = false;
				}
				else if (!isSignaled)
				{
					bool flag = false;
					try
					{
						try
						{
						}
						finally
						{
							syncWaiterCount++;
							flag = true;
						}
						if (timeout == TimeSpan.MaxValue)
						{
							if (!Monitor.Wait(syncObject, -1))
							{
								return false;
							}
						}
						else if (!Monitor.Wait(syncObject, timeout))
						{
							return false;
						}
					}
					finally
					{
						if (flag)
						{
							syncWaiterCount--;
						}
					}
				}
			}
		}
		return true;
	}

	public void Set()
	{
		List<AsyncWaiter> list = null;
		AsyncWaiter asyncWaiter = null;
		if (!isSignaled)
		{
			lock (syncObject)
			{
				if (!isSignaled)
				{
					if (resetMode == EventResetMode.ManualReset)
					{
						isSignaled = true;
						Monitor.PulseAll(syncObject);
						list = asyncWaiters;
						asyncWaiters = null;
					}
					else if (syncWaiterCount > 0)
					{
						Monitor.Pulse(syncObject);
					}
					else if (asyncWaiters != null && asyncWaiters.Count > 0)
					{
						asyncWaiter = asyncWaiters[0];
						asyncWaiters.RemoveAt(0);
					}
					else
					{
						isSignaled = true;
					}
				}
			}
		}
		if (list != null)
		{
			foreach (AsyncWaiter item in list)
			{
				item.CancelTimer();
				item.Call();
			}
		}
		if (asyncWaiter != null)
		{
			asyncWaiter.CancelTimer();
			asyncWaiter.Call();
		}
	}

	public void Reset()
	{
		isSignaled = false;
	}
}

using System.Threading;

namespace System.Runtime;

internal sealed class BackoffTimeoutHelper
{
	private static readonly int maxSkewMilliseconds = (int)(IOThreadTimer.SystemTimeResolutionTicks / 10000);

	private static readonly long maxDriftTicks = IOThreadTimer.SystemTimeResolutionTicks * 2;

	private static readonly TimeSpan defaultInitialWaitTime = TimeSpan.FromMilliseconds(1.0);

	private static readonly TimeSpan defaultMaxWaitTime = TimeSpan.FromMinutes(1.0);

	private DateTime deadline;

	private TimeSpan maxWaitTime;

	private TimeSpan waitTime;

	private IOThreadTimer backoffTimer;

	private Action<object> backoffCallback;

	private object backoffState;

	private Random random;

	private TimeSpan originalTimeout;

	public TimeSpan OriginalTimeout => originalTimeout;

	internal BackoffTimeoutHelper(TimeSpan timeout)
		: this(timeout, defaultMaxWaitTime)
	{
	}

	internal BackoffTimeoutHelper(TimeSpan timeout, TimeSpan maxWaitTime)
		: this(timeout, maxWaitTime, defaultInitialWaitTime)
	{
	}

	internal BackoffTimeoutHelper(TimeSpan timeout, TimeSpan maxWaitTime, TimeSpan initialWaitTime)
	{
		random = new Random(GetHashCode());
		this.maxWaitTime = maxWaitTime;
		originalTimeout = timeout;
		Reset(timeout, initialWaitTime);
	}

	private void Reset(TimeSpan timeout, TimeSpan initialWaitTime)
	{
		if (timeout == TimeSpan.MaxValue)
		{
			deadline = DateTime.MaxValue;
		}
		else
		{
			deadline = DateTime.UtcNow + timeout;
		}
		waitTime = initialWaitTime;
	}

	public bool IsExpired()
	{
		if (deadline == DateTime.MaxValue)
		{
			return false;
		}
		return DateTime.UtcNow >= deadline;
	}

	public void WaitAndBackoff(Action<object> callback, object state)
	{
		if (backoffCallback != callback || backoffState != state)
		{
			if (backoffTimer != null)
			{
				backoffTimer.Cancel();
			}
			backoffCallback = callback;
			backoffState = state;
			backoffTimer = new IOThreadTimer(callback, state, isTypicallyCanceledShortlyAfterBeingSet: false, maxSkewMilliseconds);
		}
		TimeSpan timeFromNow = WaitTimeWithDrift();
		Backoff();
		backoffTimer.Set(timeFromNow);
	}

	public void WaitAndBackoff()
	{
		Thread.Sleep(WaitTimeWithDrift());
		Backoff();
	}

	private TimeSpan WaitTimeWithDrift()
	{
		return Ticks.ToTimeSpan(Math.Max(Ticks.FromTimeSpan(defaultInitialWaitTime), Ticks.Add(Ticks.FromTimeSpan(waitTime), (uint)random.Next() % (2 * maxDriftTicks + 1) - maxDriftTicks)));
	}

	private void Backoff()
	{
		if (waitTime.Ticks >= maxWaitTime.Ticks / 2)
		{
			waitTime = maxWaitTime;
		}
		else
		{
			waitTime = TimeSpan.FromTicks(waitTime.Ticks * 2);
		}
		if (!(deadline != DateTime.MaxValue))
		{
			return;
		}
		TimeSpan timeSpan = deadline - DateTime.UtcNow;
		if (waitTime > timeSpan)
		{
			waitTime = timeSpan;
			if (waitTime < TimeSpan.Zero)
			{
				waitTime = TimeSpan.Zero;
			}
		}
	}
}

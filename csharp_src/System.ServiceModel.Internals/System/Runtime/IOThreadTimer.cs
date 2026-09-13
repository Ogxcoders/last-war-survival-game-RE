using System.ComponentModel;
using System.Runtime.Interop;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime;

internal class IOThreadTimer
{
	private class TimerManager
	{
		private const long maxTimeToWaitForMoreTimers = 10000000L;

		private static TimerManager value = new TimerManager();

		private Action<object> onWaitCallback;

		private TimerGroup stableTimerGroup;

		private TimerGroup volatileTimerGroup;

		private WaitableTimer[] waitableTimers;

		private bool waitScheduled;

		private object ThisLock => this;

		public static TimerManager Value => value;

		public TimerGroup StableTimerGroup => stableTimerGroup;

		public TimerGroup VolatileTimerGroup => volatileTimerGroup;

		public TimerManager()
		{
			onWaitCallback = OnWaitCallback;
			stableTimerGroup = new TimerGroup();
			volatileTimerGroup = new TimerGroup();
			waitableTimers = new WaitableTimer[2] { stableTimerGroup.WaitableTimer, volatileTimerGroup.WaitableTimer };
		}

		public void Set(IOThreadTimer timer, long dueTime)
		{
			long num = dueTime - timer.dueTime;
			if (num < 0)
			{
				num = -num;
			}
			if (num <= timer.maxSkew)
			{
				return;
			}
			lock (ThisLock)
			{
				TimerGroup timerGroup = timer.timerGroup;
				TimerQueue timerQueue = timerGroup.TimerQueue;
				if (timer.index > 0)
				{
					if (timerQueue.UpdateTimer(timer, dueTime))
					{
						UpdateWaitableTimer(timerGroup);
					}
				}
				else if (timerQueue.InsertTimer(timer, dueTime))
				{
					UpdateWaitableTimer(timerGroup);
					if (timerQueue.Count == 1)
					{
						EnsureWaitScheduled();
					}
				}
			}
		}

		public bool Cancel(IOThreadTimer timer)
		{
			lock (ThisLock)
			{
				if (timer.index > 0)
				{
					TimerGroup timerGroup = timer.timerGroup;
					TimerQueue timerQueue = timerGroup.TimerQueue;
					timerQueue.DeleteTimer(timer);
					if (timerQueue.Count > 0)
					{
						UpdateWaitableTimer(timerGroup);
					}
					else
					{
						TimerGroup otherTimerGroup = GetOtherTimerGroup(timerGroup);
						if (otherTimerGroup.TimerQueue.Count == 0)
						{
							long now = Ticks.Now;
							long num = timerGroup.WaitableTimer.DueTime - now;
							long num2 = otherTimerGroup.WaitableTimer.DueTime - now;
							if (num > 10000000 && num2 > 10000000)
							{
								timerGroup.WaitableTimer.Set(Ticks.Add(now, 10000000L));
							}
						}
					}
					return true;
				}
				return false;
			}
		}

		private void EnsureWaitScheduled()
		{
			if (!waitScheduled)
			{
				ScheduleWait();
			}
		}

		private TimerGroup GetOtherTimerGroup(TimerGroup timerGroup)
		{
			if (timerGroup == volatileTimerGroup)
			{
				return stableTimerGroup;
			}
			return volatileTimerGroup;
		}

		private void OnWaitCallback(object state)
		{
			WaitHandle[] waitHandles = waitableTimers;
			WaitHandle.WaitAny(waitHandles);
			long now = Ticks.Now;
			lock (ThisLock)
			{
				waitScheduled = false;
				ScheduleElapsedTimers(now);
				ReactivateWaitableTimers();
				ScheduleWaitIfAnyTimersLeft();
			}
		}

		private void ReactivateWaitableTimers()
		{
			ReactivateWaitableTimer(stableTimerGroup);
			ReactivateWaitableTimer(volatileTimerGroup);
		}

		private void ReactivateWaitableTimer(TimerGroup timerGroup)
		{
			TimerQueue timerQueue = timerGroup.TimerQueue;
			if (timerQueue.Count > 0)
			{
				timerGroup.WaitableTimer.Set(timerQueue.MinTimer.dueTime);
			}
			else
			{
				timerGroup.WaitableTimer.Set(long.MaxValue);
			}
		}

		private void ScheduleElapsedTimers(long now)
		{
			ScheduleElapsedTimers(stableTimerGroup, now);
			ScheduleElapsedTimers(volatileTimerGroup, now);
		}

		private void ScheduleElapsedTimers(TimerGroup timerGroup, long now)
		{
			TimerQueue timerQueue = timerGroup.TimerQueue;
			while (timerQueue.Count > 0)
			{
				IOThreadTimer minTimer = timerQueue.MinTimer;
				if (minTimer.dueTime - now <= minTimer.maxSkew)
				{
					timerQueue.DeleteMinTimer();
					ActionItem.Schedule(minTimer.callback, minTimer.callbackState);
					continue;
				}
				break;
			}
		}

		private void ScheduleWait()
		{
			ActionItem.Schedule(onWaitCallback, null);
			waitScheduled = true;
		}

		private void ScheduleWaitIfAnyTimersLeft()
		{
			if (stableTimerGroup.TimerQueue.Count > 0 || volatileTimerGroup.TimerQueue.Count > 0)
			{
				ScheduleWait();
			}
		}

		private void UpdateWaitableTimer(TimerGroup timerGroup)
		{
			WaitableTimer waitableTimer = timerGroup.WaitableTimer;
			IOThreadTimer minTimer = timerGroup.TimerQueue.MinTimer;
			long num = waitableTimer.DueTime - minTimer.dueTime;
			if (num < 0)
			{
				num = -num;
			}
			if (num > minTimer.maxSkew)
			{
				waitableTimer.Set(minTimer.dueTime);
			}
		}
	}

	private class TimerGroup
	{
		private TimerQueue timerQueue;

		private WaitableTimer waitableTimer;

		public TimerQueue TimerQueue => timerQueue;

		public WaitableTimer WaitableTimer => waitableTimer;

		public TimerGroup()
		{
			waitableTimer = new WaitableTimer();
			waitableTimer.Set(long.MaxValue);
			timerQueue = new TimerQueue();
		}
	}

	private class TimerQueue
	{
		private int count;

		private IOThreadTimer[] timers;

		public int Count => count;

		public IOThreadTimer MinTimer => timers[1];

		public TimerQueue()
		{
			timers = new IOThreadTimer[4];
		}

		public void DeleteMinTimer()
		{
			IOThreadTimer minTimer = MinTimer;
			DeleteMinTimerCore();
			minTimer.index = 0;
			minTimer.dueTime = 0L;
		}

		public void DeleteTimer(IOThreadTimer timer)
		{
			int num = timer.index;
			IOThreadTimer[] array = timers;
			while (true)
			{
				int num2 = num / 2;
				if (num2 < 1)
				{
					break;
				}
				(array[num] = array[num2]).index = num;
				num = num2;
			}
			timer.index = 0;
			timer.dueTime = 0L;
			array[1] = null;
			DeleteMinTimerCore();
		}

		public bool InsertTimer(IOThreadTimer timer, long dueTime)
		{
			IOThreadTimer[] array = timers;
			int num = count + 1;
			if (num == array.Length)
			{
				array = new IOThreadTimer[array.Length * 2];
				Array.Copy(timers, array, timers.Length);
				timers = array;
			}
			count = num;
			if (num > 1)
			{
				while (true)
				{
					int num2 = num / 2;
					if (num2 == 0)
					{
						break;
					}
					IOThreadTimer iOThreadTimer = array[num2];
					if (iOThreadTimer.dueTime <= dueTime)
					{
						break;
					}
					array[num] = iOThreadTimer;
					iOThreadTimer.index = num;
					num = num2;
				}
			}
			array[num] = timer;
			timer.index = num;
			timer.dueTime = dueTime;
			return num == 1;
		}

		public bool UpdateTimer(IOThreadTimer timer, long dueTime)
		{
			int index = timer.index;
			IOThreadTimer[] array = timers;
			int num = count;
			int num2 = index / 2;
			if (num2 == 0 || array[num2].dueTime <= dueTime)
			{
				int num3 = index * 2;
				if (num3 > num || array[num3].dueTime >= dueTime)
				{
					int num4 = num3 + 1;
					if (num4 > num || array[num4].dueTime >= dueTime)
					{
						timer.dueTime = dueTime;
						return index == 1;
					}
				}
			}
			DeleteTimer(timer);
			InsertTimer(timer, dueTime);
			return true;
		}

		private void DeleteMinTimerCore()
		{
			int num = count;
			if (num == 1)
			{
				count = 0;
				timers[1] = null;
				return;
			}
			IOThreadTimer[] array = timers;
			IOThreadTimer iOThreadTimer = array[num];
			num = (count = num - 1);
			int num2 = 1;
			int num3;
			do
			{
				num3 = num2 * 2;
				if (num3 > num)
				{
					break;
				}
				IOThreadTimer iOThreadTimer4;
				int num5;
				if (num3 < num)
				{
					IOThreadTimer iOThreadTimer2 = array[num3];
					int num4 = num3 + 1;
					IOThreadTimer iOThreadTimer3 = array[num4];
					if (iOThreadTimer3.dueTime < iOThreadTimer2.dueTime)
					{
						iOThreadTimer4 = iOThreadTimer3;
						num5 = num4;
					}
					else
					{
						iOThreadTimer4 = iOThreadTimer2;
						num5 = num3;
					}
				}
				else
				{
					num5 = num3;
					iOThreadTimer4 = array[num5];
				}
				if (iOThreadTimer.dueTime <= iOThreadTimer4.dueTime)
				{
					break;
				}
				array[num2] = iOThreadTimer4;
				iOThreadTimer4.index = num2;
				num2 = num5;
			}
			while (num3 < num);
			array[num2] = iOThreadTimer;
			iOThreadTimer.index = num2;
			array[num + 1] = null;
		}
	}

	private class WaitableTimer : WaitHandle
	{
		[SecurityCritical]
		private static class TimerHelper
		{
			public static SafeWaitHandle CreateWaitableTimer()
			{
				SafeWaitHandle safeWaitHandle = UnsafeNativeMethods.CreateWaitableTimer(IntPtr.Zero, manualReset: false, null);
				if (safeWaitHandle.IsInvalid)
				{
					Exception exception = new Win32Exception();
					safeWaitHandle.SetHandleAsInvalid();
					throw Fx.Exception.AsError(exception);
				}
				return safeWaitHandle;
			}

			public static long Set(SafeWaitHandle timer, long dueTime)
			{
				if (!UnsafeNativeMethods.SetWaitableTimer(timer, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, resume: false))
				{
					throw Fx.Exception.AsError(new Win32Exception());
				}
				return dueTime;
			}
		}

		private long dueTime;

		public long DueTime => dueTime;

		[SecuritySafeCritical]
		public WaitableTimer()
		{
			base.SafeWaitHandle = TimerHelper.CreateWaitableTimer();
		}

		[SecuritySafeCritical]
		public void Set(long dueTime)
		{
			this.dueTime = TimerHelper.Set(base.SafeWaitHandle, dueTime);
		}
	}

	private const int maxSkewInMillisecondsDefault = 100;

	private static long systemTimeResolutionTicks = -1L;

	private Action<object> callback;

	private object callbackState;

	private long dueTime;

	private int index;

	private long maxSkew;

	private TimerGroup timerGroup;

	public static long SystemTimeResolutionTicks
	{
		get
		{
			if (systemTimeResolutionTicks == -1)
			{
				systemTimeResolutionTicks = GetSystemTimeResolution();
			}
			return systemTimeResolutionTicks;
		}
	}

	public IOThreadTimer(Action<object> callback, object callbackState, bool isTypicallyCanceledShortlyAfterBeingSet)
		: this(callback, callbackState, isTypicallyCanceledShortlyAfterBeingSet, 100)
	{
	}

	public IOThreadTimer(Action<object> callback, object callbackState, bool isTypicallyCanceledShortlyAfterBeingSet, int maxSkewInMilliseconds)
	{
		this.callback = callback;
		this.callbackState = callbackState;
		maxSkew = Ticks.FromMilliseconds(maxSkewInMilliseconds);
		timerGroup = (isTypicallyCanceledShortlyAfterBeingSet ? TimerManager.Value.VolatileTimerGroup : TimerManager.Value.StableTimerGroup);
	}

	[SecuritySafeCritical]
	private static long GetSystemTimeResolution()
	{
		if (UnsafeNativeMethods.GetSystemTimeAdjustment(out var _, out var increment, out var _) != 0)
		{
			return increment;
		}
		return 150000L;
	}

	public bool Cancel()
	{
		return TimerManager.Value.Cancel(this);
	}

	public void Set(TimeSpan timeFromNow)
	{
		if (timeFromNow != TimeSpan.MaxValue)
		{
			SetAt(Ticks.Add(Ticks.Now, Ticks.FromTimeSpan(timeFromNow)));
		}
	}

	public void Set(int millisecondsFromNow)
	{
		SetAt(Ticks.Add(Ticks.Now, Ticks.FromMilliseconds(millisecondsFromNow)));
	}

	public void SetAt(long dueTime)
	{
		TimerManager.Value.Set(this, dueTime);
	}
}

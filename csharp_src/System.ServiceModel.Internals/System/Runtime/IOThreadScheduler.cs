using System.Security;
using System.Threading;

namespace System.Runtime;

internal class IOThreadScheduler
{
	private static class Bits
	{
		public const int HiShift = 16;

		public const int HiOne = 65536;

		public const int LoHiBit = 32768;

		public const int HiHiBit = int.MinValue;

		public const int LoCountMask = 32767;

		public const int HiCountMask = 2147418112;

		public const int LoMask = 65535;

		public const int HiMask = -65536;

		public const int HiBits = -2147450880;

		public static int Count(int slot)
		{
			return (((slot >> 16) - slot + 2) & 0xFFFF) - 1;
		}

		public static int CountNoIdle(int slot)
		{
			return ((slot >> 16) - slot + 1) & 0xFFFF;
		}

		public static int IncrementLo(int slot)
		{
			return ((slot + 1) & 0xFFFF) | (slot & -65536);
		}

		public static bool IsComplete(int gate)
		{
			return (gate & -65536) == gate << 16;
		}
	}

	private struct Slot
	{
		private int gate;

		private Action<object> callback;

		private object state;

		public bool TryEnqueueWorkItem(Action<object> callback, object state, out bool wrapped)
		{
			int num = Interlocked.Increment(ref gate);
			wrapped = (num & 0x7FFF) != 1;
			if (wrapped)
			{
				if ((num & 0x8000) != 0 && Bits.IsComplete(num))
				{
					Interlocked.CompareExchange(ref gate, 0, num);
				}
				return false;
			}
			this.state = state;
			this.callback = callback;
			num = Interlocked.Add(ref gate, 32768);
			if ((num & 0x7FFF0000) == 0)
			{
				return true;
			}
			this.state = null;
			this.callback = null;
			if (num >> 16 != (num & 0x7FFF) || Interlocked.CompareExchange(ref gate, 0, num) != num)
			{
				num = Interlocked.Add(ref gate, int.MinValue);
				if (Bits.IsComplete(num))
				{
					Interlocked.CompareExchange(ref gate, 0, num);
				}
			}
			return false;
		}

		public void DequeueWorkItem(out Action<object> callback, out object state)
		{
			int num = Interlocked.Add(ref gate, 65536);
			if ((num & 0x8000) == 0)
			{
				callback = null;
				state = null;
			}
			else if ((num & 0x7FFF0000) == 65536)
			{
				callback = this.callback;
				state = this.state;
				this.state = null;
				this.callback = null;
				if ((num & 0x7FFF) != 1 || Interlocked.CompareExchange(ref gate, 0, num) != num)
				{
					num = Interlocked.Add(ref gate, int.MinValue);
					if (Bits.IsComplete(num))
					{
						Interlocked.CompareExchange(ref gate, 0, num);
					}
				}
			}
			else
			{
				callback = null;
				state = null;
				if (Bits.IsComplete(num))
				{
					Interlocked.CompareExchange(ref gate, 0, num);
				}
			}
		}
	}

	[SecurityCritical]
	private class ScheduledOverlapped
	{
		private unsafe readonly NativeOverlapped* nativeOverlapped;

		private IOThreadScheduler scheduler;

		public unsafe ScheduledOverlapped()
		{
			nativeOverlapped = new Overlapped().UnsafePack(Fx.ThunkCallback(IOCallback), null);
		}

		private unsafe void IOCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			IOThreadScheduler iOThreadScheduler = scheduler;
			scheduler = null;
			Action<object> callback;
			object state;
			try
			{
			}
			finally
			{
				iOThreadScheduler.CompletionCallback(out callback, out state);
			}
			bool flag = true;
			while (flag)
			{
				callback?.Invoke(state);
				try
				{
				}
				finally
				{
					flag = iOThreadScheduler.TryCoalesce(out callback, out state);
				}
			}
		}

		public unsafe void Post(IOThreadScheduler iots)
		{
			scheduler = iots;
			ThreadPool.UnsafeQueueNativeOverlapped(nativeOverlapped);
		}

		public unsafe void Cleanup()
		{
			if (scheduler != null)
			{
				throw Fx.AssertAndThrowFatal("Cleanup called on an overlapped that is in-flight.");
			}
			Overlapped.Free(nativeOverlapped);
		}
	}

	private const int MaximumCapacity = 32768;

	private static IOThreadScheduler current = new IOThreadScheduler(32, 32);

	private readonly ScheduledOverlapped overlapped;

	[SecurityCritical]
	private readonly Slot[] slots;

	[SecurityCritical]
	private readonly Slot[] slotsLowPri;

	private int headTail = -131072;

	private int headTailLowPri = -65536;

	private int SlotMask
	{
		[SecurityCritical]
		get
		{
			return slots.Length - 1;
		}
	}

	private int SlotMaskLowPri
	{
		[SecurityCritical]
		get
		{
			return slotsLowPri.Length - 1;
		}
	}

	[SecuritySafeCritical]
	private IOThreadScheduler(int capacity, int capacityLowPri)
	{
		slots = new Slot[capacity];
		slotsLowPri = new Slot[capacityLowPri];
		overlapped = new ScheduledOverlapped();
	}

	[SecurityCritical]
	public static void ScheduleCallbackNoFlow(Action<object> callback, object state)
	{
		if (callback == null)
		{
			throw Fx.Exception.ArgumentNull("callback");
		}
		bool flag = false;
		while (!flag)
		{
			try
			{
			}
			finally
			{
				flag = current.ScheduleCallbackHelper(callback, state);
			}
		}
	}

	[SecurityCritical]
	public static void ScheduleCallbackLowPriNoFlow(Action<object> callback, object state)
	{
		if (callback == null)
		{
			throw Fx.Exception.ArgumentNull("callback");
		}
		bool flag = false;
		while (!flag)
		{
			try
			{
			}
			finally
			{
				flag = current.ScheduleCallbackLowPriHelper(callback, state);
			}
		}
	}

	[SecurityCritical]
	private bool ScheduleCallbackHelper(Action<object> callback, object state)
	{
		int num = Interlocked.Add(ref headTail, 65536);
		bool flag = Bits.Count(num) == 0;
		if (flag)
		{
			num = Interlocked.Add(ref headTail, 65536);
		}
		if (Bits.Count(num) == -1)
		{
			throw Fx.AssertAndThrowFatal("Head/Tail overflow!");
		}
		bool wrapped;
		bool result = slots[(num >> 16) & SlotMask].TryEnqueueWorkItem(callback, state, out wrapped);
		if (wrapped)
		{
			IOThreadScheduler value = new IOThreadScheduler(Math.Min(slots.Length * 2, 32768), slotsLowPri.Length);
			Interlocked.CompareExchange(ref current, value, this);
		}
		if (flag)
		{
			overlapped.Post(this);
		}
		return result;
	}

	[SecurityCritical]
	private bool ScheduleCallbackLowPriHelper(Action<object> callback, object state)
	{
		int num = Interlocked.Add(ref headTailLowPri, 65536);
		bool flag = false;
		if (Bits.CountNoIdle(num) == 1)
		{
			int num2 = headTail;
			if (Bits.Count(num2) == -1)
			{
				int num3 = Interlocked.CompareExchange(ref headTail, num2 + 65536, num2);
				if (num2 == num3)
				{
					flag = true;
				}
			}
		}
		if (Bits.CountNoIdle(num) == 0)
		{
			throw Fx.AssertAndThrowFatal("Low-priority Head/Tail overflow!");
		}
		bool wrapped;
		bool result = slotsLowPri[(num >> 16) & SlotMaskLowPri].TryEnqueueWorkItem(callback, state, out wrapped);
		if (wrapped)
		{
			IOThreadScheduler value = new IOThreadScheduler(slots.Length, Math.Min(slotsLowPri.Length * 2, 32768));
			Interlocked.CompareExchange(ref current, value, this);
		}
		if (flag)
		{
			overlapped.Post(this);
		}
		return result;
	}

	[SecurityCritical]
	private void CompletionCallback(out Action<object> callback, out object state)
	{
		int num = headTail;
		while (true)
		{
			bool flag = Bits.Count(num) == 0;
			if (flag)
			{
				int num2 = headTailLowPri;
				while (Bits.CountNoIdle(num2) != 0)
				{
					if (num2 == (num2 = Interlocked.CompareExchange(ref headTailLowPri, Bits.IncrementLo(num2), num2)))
					{
						overlapped.Post(this);
						slotsLowPri[num2 & SlotMaskLowPri].DequeueWorkItem(out callback, out state);
						return;
					}
				}
			}
			if (num == (num = Interlocked.CompareExchange(ref headTail, Bits.IncrementLo(num), num)))
			{
				if (!flag)
				{
					overlapped.Post(this);
					slots[num & SlotMask].DequeueWorkItem(out callback, out state);
					return;
				}
				int num2 = headTailLowPri;
				if (Bits.CountNoIdle(num2) == 0)
				{
					break;
				}
				num = Bits.IncrementLo(num);
				if (num != Interlocked.CompareExchange(ref headTail, num + 65536, num))
				{
					break;
				}
				num += 65536;
			}
		}
		callback = null;
		state = null;
	}

	[SecurityCritical]
	private bool TryCoalesce(out Action<object> callback, out object state)
	{
		int num = headTail;
		while (true)
		{
			if (Bits.Count(num) > 0)
			{
				if (num == (num = Interlocked.CompareExchange(ref headTail, Bits.IncrementLo(num), num)))
				{
					slots[num & SlotMask].DequeueWorkItem(out callback, out state);
					return true;
				}
				continue;
			}
			int num2 = headTailLowPri;
			if (Bits.CountNoIdle(num2) <= 0)
			{
				break;
			}
			if (num2 == (num2 = Interlocked.CompareExchange(ref headTailLowPri, Bits.IncrementLo(num2), num2)))
			{
				slotsLowPri[num2 & SlotMaskLowPri].DequeueWorkItem(out callback, out state);
				return true;
			}
			num = headTail;
		}
		callback = null;
		state = null;
		return false;
	}

	~IOThreadScheduler()
	{
		if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
		{
			Cleanup();
		}
	}

	[SecuritySafeCritical]
	private void Cleanup()
	{
		if (overlapped != null)
		{
			overlapped.Cleanup();
		}
	}
}

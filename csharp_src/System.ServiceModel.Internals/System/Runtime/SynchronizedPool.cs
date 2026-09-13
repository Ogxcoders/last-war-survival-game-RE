using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime;

internal class SynchronizedPool<T> where T : class
{
	private struct Entry
	{
		public int threadID;

		public T value;
	}

	private struct PendingEntry
	{
		public int returnCount;

		public int threadID;
	}

	private static class SynchronizedPoolHelper
	{
		public static readonly int ProcessorCount = GetProcessorCount();

		[SecuritySafeCritical]
		[EnvironmentPermission(SecurityAction.Assert, Read = "NUMBER_OF_PROCESSORS")]
		private static int GetProcessorCount()
		{
			return Environment.ProcessorCount;
		}
	}

	private class GlobalPool
	{
		private Stack<T> items;

		private int maxCount;

		public int MaxCount
		{
			get
			{
				return maxCount;
			}
			set
			{
				lock (ThisLock)
				{
					while (items.Count > value)
					{
						items.Pop();
					}
					maxCount = value;
				}
			}
		}

		private object ThisLock => this;

		public GlobalPool(int maxCount)
		{
			items = new Stack<T>();
			this.maxCount = maxCount;
		}

		public void DecrementMaxCount()
		{
			lock (ThisLock)
			{
				if (items.Count == maxCount)
				{
					items.Pop();
				}
				maxCount--;
			}
		}

		public T Take()
		{
			if (items.Count > 0)
			{
				lock (ThisLock)
				{
					if (items.Count > 0)
					{
						return items.Pop();
					}
				}
			}
			return null;
		}

		public bool Return(T value)
		{
			if (items.Count < MaxCount)
			{
				lock (ThisLock)
				{
					if (items.Count < MaxCount)
					{
						items.Push(value);
						return true;
					}
				}
			}
			return false;
		}

		public void Clear()
		{
			lock (ThisLock)
			{
				items.Clear();
			}
		}
	}

	private const int maxPendingEntries = 128;

	private const int maxPromotionFailures = 64;

	private const int maxReturnsBeforePromotion = 64;

	private const int maxThreadItemsPerProcessor = 16;

	private Entry[] entries;

	private GlobalPool globalPool;

	private int maxCount;

	private PendingEntry[] pending;

	private int promotionFailures;

	private object ThisLock => this;

	public SynchronizedPool(int maxCount)
	{
		int num = maxCount;
		int num2 = 16 + SynchronizedPoolHelper.ProcessorCount;
		if (num > num2)
		{
			num = num2;
		}
		this.maxCount = maxCount;
		entries = new Entry[num];
		pending = new PendingEntry[4];
		globalPool = new GlobalPool(maxCount);
	}

	public void Clear()
	{
		Entry[] array = entries;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].value = null;
		}
		globalPool.Clear();
	}

	private void HandlePromotionFailure(int thisThreadID)
	{
		int num = promotionFailures + 1;
		if (num >= 64)
		{
			lock (ThisLock)
			{
				entries = new Entry[entries.Length];
				globalPool.MaxCount = maxCount;
			}
			PromoteThread(thisThreadID);
		}
		else
		{
			promotionFailures = num;
		}
	}

	private bool PromoteThread(int thisThreadID)
	{
		lock (ThisLock)
		{
			for (int i = 0; i < entries.Length; i++)
			{
				int threadID = entries[i].threadID;
				if (threadID == thisThreadID)
				{
					return true;
				}
				if (threadID == 0)
				{
					globalPool.DecrementMaxCount();
					entries[i].threadID = thisThreadID;
					return true;
				}
			}
		}
		return false;
	}

	private void RecordReturnToGlobalPool(int thisThreadID)
	{
		PendingEntry[] array = pending;
		for (int i = 0; i < array.Length; i++)
		{
			int threadID = array[i].threadID;
			if (threadID == thisThreadID)
			{
				int num = array[i].returnCount + 1;
				if (num >= 64)
				{
					array[i].returnCount = 0;
					if (!PromoteThread(thisThreadID))
					{
						HandlePromotionFailure(thisThreadID);
					}
				}
				else
				{
					array[i].returnCount = num;
				}
				break;
			}
			if (threadID == 0)
			{
				break;
			}
		}
	}

	private void RecordTakeFromGlobalPool(int thisThreadID)
	{
		PendingEntry[] array = pending;
		for (int i = 0; i < array.Length; i++)
		{
			int threadID = array[i].threadID;
			if (threadID == thisThreadID)
			{
				return;
			}
			if (threadID != 0)
			{
				continue;
			}
			lock (array)
			{
				if (array[i].threadID == 0)
				{
					array[i].threadID = thisThreadID;
					return;
				}
			}
		}
		if (array.Length >= 128)
		{
			pending = new PendingEntry[array.Length];
			return;
		}
		PendingEntry[] destinationArray = new PendingEntry[array.Length * 2];
		Array.Copy(array, destinationArray, array.Length);
		pending = destinationArray;
	}

	public bool Return(T value)
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		if (managedThreadId == 0)
		{
			return false;
		}
		if (ReturnToPerThreadPool(managedThreadId, value))
		{
			return true;
		}
		return ReturnToGlobalPool(managedThreadId, value);
	}

	private bool ReturnToPerThreadPool(int thisThreadID, T value)
	{
		Entry[] array = entries;
		for (int i = 0; i < array.Length; i++)
		{
			int threadID = array[i].threadID;
			if (threadID == thisThreadID)
			{
				if (array[i].value == null)
				{
					array[i].value = value;
					return true;
				}
				return false;
			}
			if (threadID == 0)
			{
				break;
			}
		}
		return false;
	}

	private bool ReturnToGlobalPool(int thisThreadID, T value)
	{
		RecordReturnToGlobalPool(thisThreadID);
		return globalPool.Return(value);
	}

	public T Take()
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		if (managedThreadId == 0)
		{
			return null;
		}
		T val = TakeFromPerThreadPool(managedThreadId);
		if (val != null)
		{
			return val;
		}
		return TakeFromGlobalPool(managedThreadId);
	}

	private T TakeFromPerThreadPool(int thisThreadID)
	{
		Entry[] array = entries;
		for (int i = 0; i < array.Length; i++)
		{
			int threadID = array[i].threadID;
			if (threadID == thisThreadID)
			{
				T value = array[i].value;
				if (value != null)
				{
					array[i].value = null;
					return value;
				}
				return null;
			}
			if (threadID == 0)
			{
				break;
			}
		}
		return null;
	}

	private T TakeFromGlobalPool(int thisThreadID)
	{
		RecordTakeFromGlobalPool(thisThreadID);
		return globalPool.Take();
	}
}

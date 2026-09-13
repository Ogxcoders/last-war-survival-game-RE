using System.Collections.Generic;

namespace System.Runtime.Collections;

internal class ObjectCache<TKey, TValue> where TValue : class
{
	private class Item : ObjectCacheItem<TValue>
	{
		private readonly ObjectCache<TKey, TValue> parent;

		private readonly TKey key;

		private readonly Action<TValue> disposeItemCallback;

		private TValue value;

		private int referenceCount;

		public int ReferenceCount => referenceCount;

		public override TValue Value => value;

		public DateTime CreationTime { get; set; }

		public DateTime LastUsage { get; set; }

		public Item(TKey key, TValue value, Action<TValue> disposeItemCallback)
			: this(key, value)
		{
			this.disposeItemCallback = disposeItemCallback;
		}

		public Item(TKey key, TValue value, ObjectCache<TKey, TValue> parent)
			: this(key, value)
		{
			this.parent = parent;
		}

		private Item(TKey key, TValue value)
		{
			this.key = key;
			this.value = value;
			referenceCount = 1;
		}

		public override bool TryAddReference()
		{
			bool result;
			if (parent == null || referenceCount == -1)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				lock (parent.ThisLock)
				{
					if (referenceCount == -1)
					{
						result = false;
					}
					else if (referenceCount == 0 && parent.ShouldPurgeItem(this, DateTime.UtcNow))
					{
						LockedDispose();
						flag = true;
						result = false;
						parent.cacheItems.Remove(key);
					}
					else
					{
						referenceCount++;
						result = true;
					}
				}
				if (flag)
				{
					LocalDispose();
				}
			}
			return result;
		}

		public override void ReleaseReference()
		{
			bool flag;
			if (parent == null)
			{
				referenceCount = -1;
				flag = true;
			}
			else
			{
				lock (parent.ThisLock)
				{
					if (referenceCount > 1)
					{
						InternalReleaseReference();
						flag = false;
					}
					else
					{
						flag = parent.Return(key, this);
					}
				}
			}
			if (flag)
			{
				LocalDispose();
			}
		}

		internal void InternalAddReference()
		{
			referenceCount++;
		}

		internal void InternalReleaseReference()
		{
			referenceCount--;
		}

		public void LockedDispose()
		{
			referenceCount = -1;
		}

		public void Dispose()
		{
			if (Value != null)
			{
				Action<TValue> action = disposeItemCallback;
				if (parent != null)
				{
					action = parent.DisposeItemCallback;
				}
				if (action != null)
				{
					action(Value);
				}
				else if (Value is IDisposable)
				{
					((IDisposable)Value).Dispose();
				}
			}
			value = null;
			referenceCount = -1;
		}

		public void LocalDispose()
		{
			Dispose();
		}
	}

	private const int timerThreshold = 1;

	private ObjectCacheSettings settings;

	private Dictionary<TKey, Item> cacheItems;

	private bool idleTimeoutEnabled;

	private bool leaseTimeoutEnabled;

	private IOThreadTimer idleTimer;

	private static Action<object> onIdle;

	private bool disposed;

	private object ThisLock => this;

	public Action<TValue> DisposeItemCallback { get; set; }

	public int Count => cacheItems.Count;

	public ObjectCache(ObjectCacheSettings settings)
		: this(settings, (IEqualityComparer<TKey>)null)
	{
	}

	public ObjectCache(ObjectCacheSettings settings, IEqualityComparer<TKey> comparer)
	{
		this.settings = settings.Clone();
		cacheItems = new Dictionary<TKey, Item>(comparer);
		idleTimeoutEnabled = settings.IdleTimeout != TimeSpan.MaxValue;
		leaseTimeoutEnabled = settings.LeaseTimeout != TimeSpan.MaxValue;
	}

	public ObjectCacheItem<TValue> Add(TKey key, TValue value)
	{
		lock (ThisLock)
		{
			if (Count >= settings.CacheLimit || cacheItems.ContainsKey(key))
			{
				return new Item(key, value, DisposeItemCallback);
			}
			return InternalAdd(key, value);
		}
	}

	public ObjectCacheItem<TValue> Take(TKey key)
	{
		return Take(key, null);
	}

	public ObjectCacheItem<TValue> Take(TKey key, Func<TValue> initializerDelegate)
	{
		Item value = null;
		lock (ThisLock)
		{
			if (cacheItems.TryGetValue(key, out value))
			{
				value.InternalAddReference();
			}
			else
			{
				if (initializerDelegate == null)
				{
					return null;
				}
				TValue value2 = initializerDelegate();
				if (Count >= settings.CacheLimit)
				{
					return new Item(key, value2, DisposeItemCallback);
				}
				value = InternalAdd(key, value2);
			}
		}
		return value;
	}

	private Item InternalAdd(TKey key, TValue value)
	{
		Item item = new Item(key, value, this);
		if (leaseTimeoutEnabled)
		{
			item.CreationTime = DateTime.UtcNow;
		}
		cacheItems.Add(key, item);
		StartTimerIfNecessary();
		return item;
	}

	private bool Return(TKey key, Item cacheItem)
	{
		bool result = false;
		if (disposed)
		{
			result = true;
		}
		else
		{
			cacheItem.InternalReleaseReference();
			DateTime utcNow = DateTime.UtcNow;
			if (idleTimeoutEnabled)
			{
				cacheItem.LastUsage = utcNow;
			}
			if (ShouldPurgeItem(cacheItem, utcNow))
			{
				cacheItems.Remove(key);
				cacheItem.LockedDispose();
				result = true;
			}
		}
		return result;
	}

	private void StartTimerIfNecessary()
	{
		if (!idleTimeoutEnabled || Count <= 1)
		{
			return;
		}
		if (idleTimer == null)
		{
			if (onIdle == null)
			{
				onIdle = OnIdle;
			}
			idleTimer = new IOThreadTimer(onIdle, this, isTypicallyCanceledShortlyAfterBeingSet: false);
		}
		idleTimer.Set(settings.IdleTimeout);
	}

	private static void OnIdle(object state)
	{
		((ObjectCache<TKey, TValue>)state).PurgeCache(calledFromTimer: true);
	}

	private static void Add<T>(ref List<T> list, T item)
	{
		if (list == null)
		{
			list = new List<T>();
		}
		list.Add(item);
	}

	private bool ShouldPurgeItem(Item cacheItem, DateTime now)
	{
		if (cacheItem.ReferenceCount > 0)
		{
			return false;
		}
		if (idleTimeoutEnabled && now >= cacheItem.LastUsage + settings.IdleTimeout)
		{
			return true;
		}
		if (leaseTimeoutEnabled && now - cacheItem.CreationTime >= settings.LeaseTimeout)
		{
			return true;
		}
		return false;
	}

	private void GatherExpiredItems(ref List<KeyValuePair<TKey, Item>> expiredItems, bool calledFromTimer)
	{
		if (Count == 0 || (!leaseTimeoutEnabled && !idleTimeoutEnabled))
		{
			return;
		}
		DateTime utcNow = DateTime.UtcNow;
		bool flag = false;
		lock (ThisLock)
		{
			foreach (KeyValuePair<TKey, Item> cacheItem in cacheItems)
			{
				if (ShouldPurgeItem(cacheItem.Value, utcNow))
				{
					cacheItem.Value.LockedDispose();
					Add(ref expiredItems, cacheItem);
				}
			}
			if (expiredItems != null)
			{
				for (int i = 0; i < expiredItems.Count; i++)
				{
					cacheItems.Remove(expiredItems[i].Key);
				}
			}
			flag = calledFromTimer && Count > 0;
		}
		if (flag)
		{
			idleTimer.Set(settings.IdleTimeout);
		}
	}

	private void PurgeCache(bool calledFromTimer)
	{
		List<KeyValuePair<TKey, Item>> expiredItems = null;
		lock (ThisLock)
		{
			GatherExpiredItems(ref expiredItems, calledFromTimer);
		}
		if (expiredItems != null)
		{
			for (int i = 0; i < expiredItems.Count; i++)
			{
				expiredItems[i].Value.LocalDispose();
			}
		}
	}

	public void Dispose()
	{
		lock (ThisLock)
		{
			foreach (Item value in cacheItems.Values)
			{
				value?.Dispose();
			}
			cacheItems.Clear();
			settings.CacheLimit = 0;
			disposed = true;
			if (idleTimer != null)
			{
				idleTimer.Cancel();
				idleTimer = null;
			}
		}
	}
}

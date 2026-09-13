using System.Collections.Generic;

namespace System.Runtime;

internal class MruCache<TKey, TValue> where TKey : class where TValue : class
{
	private struct CacheEntry
	{
		internal TValue value;

		internal LinkedListNode<TKey> node;
	}

	private LinkedList<TKey> mruList;

	private Dictionary<TKey, CacheEntry> items;

	private int lowWatermark;

	private int highWatermark;

	private CacheEntry mruEntry;

	public int Count => items.Count;

	public MruCache(int watermark)
		: this(watermark * 4 / 5, watermark)
	{
	}

	public MruCache(int lowWatermark, int highWatermark)
		: this(lowWatermark, highWatermark, (IEqualityComparer<TKey>)null)
	{
	}

	public MruCache(int lowWatermark, int highWatermark, IEqualityComparer<TKey> comparer)
	{
		this.lowWatermark = lowWatermark;
		this.highWatermark = highWatermark;
		mruList = new LinkedList<TKey>();
		if (comparer == null)
		{
			items = new Dictionary<TKey, CacheEntry>();
		}
		else
		{
			items = new Dictionary<TKey, CacheEntry>(comparer);
		}
	}

	public void Add(TKey key, TValue value)
	{
		bool flag = false;
		try
		{
			if (items.Count == highWatermark)
			{
				int num = highWatermark - lowWatermark;
				for (int i = 0; i < num; i++)
				{
					TKey value2 = mruList.Last.Value;
					mruList.RemoveLast();
					TValue value3 = items[value2].value;
					items.Remove(value2);
					OnSingleItemRemoved(value3);
					OnItemAgedOutOfCache(value3);
				}
			}
			CacheEntry value4 = default(CacheEntry);
			value4.node = mruList.AddFirst(key);
			value4.value = value;
			items.Add(key, value4);
			mruEntry = value4;
			flag = true;
		}
		finally
		{
			if (!flag)
			{
				Clear();
			}
		}
	}

	public void Clear()
	{
		mruList.Clear();
		items.Clear();
		mruEntry.value = null;
		mruEntry.node = null;
	}

	public bool Remove(TKey key)
	{
		if (items.TryGetValue(key, out var value))
		{
			items.Remove(key);
			OnSingleItemRemoved(value.value);
			mruList.Remove(value.node);
			if (mruEntry.node == value.node)
			{
				mruEntry.value = null;
				mruEntry.node = null;
			}
			return true;
		}
		return false;
	}

	protected virtual void OnSingleItemRemoved(TValue item)
	{
	}

	protected virtual void OnItemAgedOutOfCache(TValue item)
	{
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		if (mruEntry.node != null && key != null && key.Equals(mruEntry.node.Value))
		{
			value = mruEntry.value;
			return true;
		}
		CacheEntry value2;
		bool num = items.TryGetValue(key, out value2);
		value = value2.value;
		if (num && mruList.Count > 1 && mruList.First != value2.node)
		{
			mruList.Remove(value2.node);
			mruList.AddFirst(value2.node);
			mruEntry = value2;
		}
		return num;
	}
}

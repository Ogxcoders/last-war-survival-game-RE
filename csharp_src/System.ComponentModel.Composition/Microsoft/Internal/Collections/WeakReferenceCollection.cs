using System;
using System.Collections.Generic;

namespace Microsoft.Internal.Collections;

internal class WeakReferenceCollection<T> where T : class
{
	private readonly List<WeakReference> _items = new List<WeakReference>();

	public void Add(T item)
	{
		if (_items.Capacity == _items.Count)
		{
			CleanupDeadReferences();
		}
		_items.Add(new WeakReference(item));
	}

	public void Remove(T item)
	{
		int num = IndexOf(item);
		if (num != -1)
		{
			_items.RemoveAt(num);
		}
	}

	public bool Contains(T item)
	{
		return IndexOf(item) >= 0;
	}

	public void Clear()
	{
		_items.Clear();
	}

	private int IndexOf(T item)
	{
		int count = _items.Count;
		for (int i = 0; i < count; i++)
		{
			if (_items[i].Target == item)
			{
				return i;
			}
		}
		return -1;
	}

	private void CleanupDeadReferences()
	{
		_items.RemoveAll((WeakReference w) => !w.IsAlive);
	}

	public List<T> AliveItemsToList()
	{
		List<T> list = new List<T>();
		foreach (WeakReference item2 in _items)
		{
			if (item2.Target is T item)
			{
				list.Add(item);
			}
		}
		return list;
	}
}

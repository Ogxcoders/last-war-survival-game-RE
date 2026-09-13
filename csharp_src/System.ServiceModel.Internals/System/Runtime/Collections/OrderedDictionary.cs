using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Runtime.Collections;

internal class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection
{
	private OrderedDictionary privateDictionary;

	public int Count => privateDictionary.Count;

	public bool IsReadOnly => false;

	public TValue this[TKey key]
	{
		get
		{
			if (key == null)
			{
				throw Fx.Exception.ArgumentNull("key");
			}
			if (privateDictionary.Contains(key))
			{
				return (TValue)privateDictionary[key];
			}
			throw Fx.Exception.AsError(new KeyNotFoundException("Key Not Found In Dictionary"));
		}
		set
		{
			if (key == null)
			{
				throw Fx.Exception.ArgumentNull("key");
			}
			privateDictionary[key] = value;
		}
	}

	public ICollection<TKey> Keys
	{
		get
		{
			List<TKey> list = new List<TKey>(privateDictionary.Count);
			foreach (TKey key in privateDictionary.Keys)
			{
				list.Add(key);
			}
			return list;
		}
	}

	public ICollection<TValue> Values
	{
		get
		{
			List<TValue> list = new List<TValue>(privateDictionary.Count);
			foreach (TValue value in privateDictionary.Values)
			{
				list.Add(value);
			}
			return list;
		}
	}

	bool IDictionary.IsFixedSize => ((IDictionary)privateDictionary).IsFixedSize;

	bool IDictionary.IsReadOnly => privateDictionary.IsReadOnly;

	ICollection IDictionary.Keys => privateDictionary.Keys;

	ICollection IDictionary.Values => privateDictionary.Values;

	object IDictionary.this[object key]
	{
		get
		{
			return privateDictionary[key];
		}
		set
		{
			privateDictionary[key] = value;
		}
	}

	int ICollection.Count => privateDictionary.Count;

	bool ICollection.IsSynchronized => ((ICollection)privateDictionary).IsSynchronized;

	object ICollection.SyncRoot => ((ICollection)privateDictionary).SyncRoot;

	public OrderedDictionary()
	{
		privateDictionary = new OrderedDictionary();
	}

	public OrderedDictionary(IDictionary<TKey, TValue> dictionary)
	{
		if (dictionary == null)
		{
			return;
		}
		privateDictionary = new OrderedDictionary();
		foreach (KeyValuePair<TKey, TValue> item in dictionary)
		{
			privateDictionary.Add(item.Key, item.Value);
		}
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		Add(item.Key, item.Value);
	}

	public void Add(TKey key, TValue value)
	{
		if (key == null)
		{
			throw Fx.Exception.ArgumentNull("key");
		}
		privateDictionary.Add(key, value);
	}

	public void Clear()
	{
		privateDictionary.Clear();
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		if (item.Key == null || !privateDictionary.Contains(item.Key))
		{
			return false;
		}
		return privateDictionary[item.Key].Equals(item.Value);
	}

	public bool ContainsKey(TKey key)
	{
		if (key == null)
		{
			throw Fx.Exception.ArgumentNull("key");
		}
		return privateDictionary.Contains(key);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		if (array == null)
		{
			throw Fx.Exception.ArgumentNull("array");
		}
		if (arrayIndex < 0)
		{
			throw Fx.Exception.AsError(new ArgumentOutOfRangeException("arrayIndex"));
		}
		if (array.Rank > 1 || arrayIndex >= array.Length || array.Length - arrayIndex < privateDictionary.Count)
		{
			throw Fx.Exception.Argument("array", "Bad Copy To Array");
		}
		int num = arrayIndex;
		foreach (DictionaryEntry item in privateDictionary)
		{
			array[num] = new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value);
			num++;
		}
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		foreach (DictionaryEntry item in privateDictionary)
		{
			yield return new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		if (Contains(item))
		{
			privateDictionary.Remove(item.Key);
			return true;
		}
		return false;
	}

	public bool Remove(TKey key)
	{
		if (key == null)
		{
			throw Fx.Exception.ArgumentNull("key");
		}
		if (privateDictionary.Contains(key))
		{
			privateDictionary.Remove(key);
			return true;
		}
		return false;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		if (key == null)
		{
			throw Fx.Exception.ArgumentNull("key");
		}
		bool flag = privateDictionary.Contains(key);
		value = (flag ? ((TValue)privateDictionary[key]) : default(TValue));
		return flag;
	}

	void IDictionary.Add(object key, object value)
	{
		privateDictionary.Add(key, value);
	}

	void IDictionary.Clear()
	{
		privateDictionary.Clear();
	}

	bool IDictionary.Contains(object key)
	{
		return privateDictionary.Contains(key);
	}

	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		return privateDictionary.GetEnumerator();
	}

	void IDictionary.Remove(object key)
	{
		privateDictionary.Remove(key);
	}

	void ICollection.CopyTo(Array array, int index)
	{
		privateDictionary.CopyTo(array, index);
	}
}

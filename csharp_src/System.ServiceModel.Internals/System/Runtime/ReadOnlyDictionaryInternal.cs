using System.Collections;
using System.Collections.Generic;

namespace System.Runtime;

[Serializable]
internal class ReadOnlyDictionaryInternal<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	private IDictionary<TKey, TValue> dictionary;

	public int Count => dictionary.Count;

	public bool IsReadOnly => true;

	public ICollection<TKey> Keys => dictionary.Keys;

	public ICollection<TValue> Values => dictionary.Values;

	public TValue this[TKey key]
	{
		get
		{
			return dictionary[key];
		}
		set
		{
			throw Fx.Exception.AsError(CreateReadOnlyException());
		}
	}

	public ReadOnlyDictionaryInternal(IDictionary<TKey, TValue> dictionary)
	{
		this.dictionary = dictionary;
	}

	public static IDictionary<TKey, TValue> Create(IDictionary<TKey, TValue> dictionary)
	{
		if (dictionary.IsReadOnly)
		{
			return dictionary;
		}
		return new ReadOnlyDictionaryInternal<TKey, TValue>(dictionary);
	}

	private Exception CreateReadOnlyException()
	{
		return new InvalidOperationException("Dictionary Is Read Only");
	}

	public void Add(TKey key, TValue value)
	{
		throw Fx.Exception.AsError(CreateReadOnlyException());
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		throw Fx.Exception.AsError(CreateReadOnlyException());
	}

	public void Clear()
	{
		throw Fx.Exception.AsError(CreateReadOnlyException());
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return dictionary.Contains(item);
	}

	public bool ContainsKey(TKey key)
	{
		return dictionary.ContainsKey(key);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		dictionary.CopyTo(array, arrayIndex);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return dictionary.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public bool Remove(TKey key)
	{
		throw Fx.Exception.AsError(CreateReadOnlyException());
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		throw Fx.Exception.AsError(CreateReadOnlyException());
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		return dictionary.TryGetValue(key, out value);
	}
}

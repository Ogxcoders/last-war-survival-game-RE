using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Joker.Client;

public class SerializableDictionary
{
}
[Serializable]
public class SerializableDictionary<TKey, TValue> : SerializableDictionary, ISerializationCallbackReceiver, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	[Serializable]
	private struct SerializableKeyValuePair
	{
		public TKey Key;

		public TValue Value;

		public SerializableKeyValuePair(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}
	}

	[SerializeField]
	private List<SerializableKeyValuePair> list = new List<SerializableKeyValuePair>();

	private Lazy<Dictionary<TKey, int>> _keyPositions;

	private Dictionary<TKey, int> KeyPositions => _keyPositions.Value;

	public TValue this[TKey key]
	{
		get
		{
			return list[KeyPositions[key]].Value;
		}
		set
		{
			SerializableKeyValuePair serializableKeyValuePair = new SerializableKeyValuePair(key, value);
			if (KeyPositions.ContainsKey(key))
			{
				list[KeyPositions[key]] = serializableKeyValuePair;
				return;
			}
			KeyPositions[key] = list.Count;
			list.Add(serializableKeyValuePair);
		}
	}

	public ICollection<TKey> Keys => list.Select((SerializableKeyValuePair tuple) => tuple.Key).ToArray();

	public ICollection<TValue> Values => list.Select((SerializableKeyValuePair tuple) => tuple.Value).ToArray();

	public int Count => list.Count;

	public bool IsReadOnly => false;

	public SerializableDictionary()
	{
		_keyPositions = new Lazy<Dictionary<TKey, int>>(MakeKeyPositions);
	}

	private Dictionary<TKey, int> MakeKeyPositions()
	{
		Dictionary<TKey, int> dictionary = new Dictionary<TKey, int>(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			dictionary[list[i].Key] = i;
		}
		return dictionary;
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		_keyPositions = new Lazy<Dictionary<TKey, int>>(MakeKeyPositions);
	}

	public void Add(TKey key, TValue value)
	{
		if (KeyPositions.ContainsKey(key))
		{
			throw new ArgumentException("An element with the same key already exists in the dictionary.");
		}
		KeyPositions[key] = list.Count;
		list.Add(new SerializableKeyValuePair(key, value));
	}

	public bool ContainsKey(TKey key)
	{
		return KeyPositions.ContainsKey(key);
	}

	public bool Remove(TKey key)
	{
		if (KeyPositions.TryGetValue(key, out var value))
		{
			KeyPositions.Remove(key);
			list.RemoveAt(value);
			for (int i = value; i < list.Count; i++)
			{
				KeyPositions[list[i].Key] = i;
			}
			return true;
		}
		return false;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		if (KeyPositions.TryGetValue(key, out var value2))
		{
			value = list[value2].Value;
			return true;
		}
		value = default(TValue);
		return false;
	}

	public void Add(KeyValuePair<TKey, TValue> kvp)
	{
		Add(kvp.Key, kvp.Value);
	}

	public void Clear()
	{
		list.Clear();
	}

	public bool Contains(KeyValuePair<TKey, TValue> kvp)
	{
		return KeyPositions.ContainsKey(kvp.Key);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		int count = list.Count;
		if (array.Length - arrayIndex < count)
		{
			throw new ArgumentException("arrayIndex");
		}
		int num = 0;
		while (num < count)
		{
			SerializableKeyValuePair serializableKeyValuePair = list[num];
			array[arrayIndex] = new KeyValuePair<TKey, TValue>(serializableKeyValuePair.Key, serializableKeyValuePair.Value);
			num++;
			arrayIndex++;
		}
	}

	public bool Remove(KeyValuePair<TKey, TValue> kvp)
	{
		return Remove(kvp.Key);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return list.Select(ToKeyValuePair).GetEnumerator();
	}

	private static KeyValuePair<TKey, TValue> ToKeyValuePair(SerializableKeyValuePair skvp)
	{
		return new KeyValuePair<TKey, TValue>(skvp.Key, skvp.Value);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

using System.Collections;
using System.Collections.Generic;

namespace System.Runtime.Collections;

internal class NullableKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	private class NullKeyDictionaryKeyCollection<TypeKey, TypeValue> : ICollection<TypeKey>, IEnumerable<TypeKey>, IEnumerable
	{
		private NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary;

		public int Count
		{
			get
			{
				int num = nullKeyDictionary.innerDictionary.Keys.Count;
				if (nullKeyDictionary.isNullKeyPresent)
				{
					num++;
				}
				return num;
			}
		}

		public bool IsReadOnly => true;

		public NullKeyDictionaryKeyCollection(NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary)
		{
			this.nullKeyDictionary = nullKeyDictionary;
		}

		public void Add(TypeKey item)
		{
			throw Fx.Exception.AsError(new NotSupportedException("Key Collection Updates Not Allowed"));
		}

		public void Clear()
		{
			throw Fx.Exception.AsError(new NotSupportedException("Key Collection Updates Not Allowed"));
		}

		public bool Contains(TypeKey item)
		{
			if (item != null)
			{
				return nullKeyDictionary.innerDictionary.Keys.Contains(item);
			}
			return nullKeyDictionary.isNullKeyPresent;
		}

		public void CopyTo(TypeKey[] array, int arrayIndex)
		{
			nullKeyDictionary.innerDictionary.Keys.CopyTo(array, arrayIndex);
			if (nullKeyDictionary.isNullKeyPresent)
			{
				array[arrayIndex + nullKeyDictionary.innerDictionary.Keys.Count] = default(TypeKey);
			}
		}

		public bool Remove(TypeKey item)
		{
			throw Fx.Exception.AsError(new NotSupportedException("Key Collection Updates Not Allowed"));
		}

		public IEnumerator<TypeKey> GetEnumerator()
		{
			foreach (TypeKey key in nullKeyDictionary.innerDictionary.Keys)
			{
				yield return key;
			}
			if (nullKeyDictionary.isNullKeyPresent)
			{
				yield return default(TypeKey);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TypeKey>)this).GetEnumerator();
		}
	}

	private class NullKeyDictionaryValueCollection<TypeKey, TypeValue> : ICollection<TypeValue>, IEnumerable<TypeValue>, IEnumerable
	{
		private NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary;

		public int Count
		{
			get
			{
				int num = nullKeyDictionary.innerDictionary.Values.Count;
				if (nullKeyDictionary.isNullKeyPresent)
				{
					num++;
				}
				return num;
			}
		}

		public bool IsReadOnly => true;

		public NullKeyDictionaryValueCollection(NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary)
		{
			this.nullKeyDictionary = nullKeyDictionary;
		}

		public void Add(TypeValue item)
		{
			throw Fx.Exception.AsError(new NotSupportedException("Value Collection Updates Not Allowed"));
		}

		public void Clear()
		{
			throw Fx.Exception.AsError(new NotSupportedException("Value Collection Updates Not Allowed"));
		}

		public bool Contains(TypeValue item)
		{
			if (!nullKeyDictionary.innerDictionary.Values.Contains(item))
			{
				if (nullKeyDictionary.isNullKeyPresent)
				{
					ref TypeValue nullKeyValue = ref nullKeyDictionary.nullKeyValue;
					object obj = item;
					return nullKeyValue.Equals(obj);
				}
				return false;
			}
			return true;
		}

		public void CopyTo(TypeValue[] array, int arrayIndex)
		{
			nullKeyDictionary.innerDictionary.Values.CopyTo(array, arrayIndex);
			if (nullKeyDictionary.isNullKeyPresent)
			{
				array[arrayIndex + nullKeyDictionary.innerDictionary.Values.Count] = nullKeyDictionary.nullKeyValue;
			}
		}

		public bool Remove(TypeValue item)
		{
			throw Fx.Exception.AsError(new NotSupportedException("Value Collection Updates Not Allowed"));
		}

		public IEnumerator<TypeValue> GetEnumerator()
		{
			foreach (TypeValue value in nullKeyDictionary.innerDictionary.Values)
			{
				yield return value;
			}
			if (nullKeyDictionary.isNullKeyPresent)
			{
				yield return nullKeyDictionary.nullKeyValue;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TypeValue>)this).GetEnumerator();
		}
	}

	private bool isNullKeyPresent;

	private TValue nullKeyValue;

	private IDictionary<TKey, TValue> innerDictionary;

	public int Count => innerDictionary.Count + (isNullKeyPresent ? 1 : 0);

	public bool IsReadOnly => false;

	public ICollection<TKey> Keys => new NullKeyDictionaryKeyCollection<TKey, TValue>(this);

	public ICollection<TValue> Values => new NullKeyDictionaryValueCollection<TKey, TValue>(this);

	public TValue this[TKey key]
	{
		get
		{
			if (key == null)
			{
				if (isNullKeyPresent)
				{
					return nullKeyValue;
				}
				throw Fx.Exception.AsError(new KeyNotFoundException());
			}
			return innerDictionary[key];
		}
		set
		{
			if (key == null)
			{
				isNullKeyPresent = true;
				nullKeyValue = value;
			}
			else
			{
				innerDictionary[key] = value;
			}
		}
	}

	public NullableKeyDictionary()
	{
		innerDictionary = new Dictionary<TKey, TValue>();
	}

	public void Add(TKey key, TValue value)
	{
		if (key == null)
		{
			if (isNullKeyPresent)
			{
				throw Fx.Exception.Argument("key", "Null Key Already Present");
			}
			isNullKeyPresent = true;
			nullKeyValue = value;
		}
		else
		{
			innerDictionary.Add(key, value);
		}
	}

	public bool ContainsKey(TKey key)
	{
		if (key != null)
		{
			return innerDictionary.ContainsKey(key);
		}
		return isNullKeyPresent;
	}

	public bool Remove(TKey key)
	{
		if (key == null)
		{
			bool result = isNullKeyPresent;
			isNullKeyPresent = false;
			nullKeyValue = default(TValue);
			return result;
		}
		return innerDictionary.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		if (key == null)
		{
			if (isNullKeyPresent)
			{
				value = nullKeyValue;
				return true;
			}
			value = default(TValue);
			return false;
		}
		return innerDictionary.TryGetValue(key, out value);
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		Add(item.Key, item.Value);
	}

	public void Clear()
	{
		isNullKeyPresent = false;
		nullKeyValue = default(TValue);
		innerDictionary.Clear();
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		if (item.Key == null)
		{
			if (isNullKeyPresent)
			{
				if (item.Value != null)
				{
					return item.Value.Equals(nullKeyValue);
				}
				return nullKeyValue == null;
			}
			return false;
		}
		return innerDictionary.Contains(item);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		innerDictionary.CopyTo(array, arrayIndex);
		if (isNullKeyPresent)
		{
			array[arrayIndex + innerDictionary.Count] = new KeyValuePair<TKey, TValue>(default(TKey), nullKeyValue);
		}
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		if (item.Key == null)
		{
			if (Contains(item))
			{
				isNullKeyPresent = false;
				nullKeyValue = default(TValue);
				return true;
			}
			return false;
		}
		return innerDictionary.Remove(item);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		IEnumerator<KeyValuePair<TKey, TValue>> innerEnumerator = innerDictionary.GetEnumerator();
		while (innerEnumerator.MoveNext())
		{
			yield return innerEnumerator.Current;
		}
		if (isNullKeyPresent)
		{
			yield return new KeyValuePair<TKey, TValue>(default(TKey), nullKeyValue);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
	}
}

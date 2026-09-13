using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels;

[ComVisible(true)]
internal class AggregateDictionary : IDictionary, ICollection, IEnumerable
{
	private IDictionary[] dictionaries;

	private ArrayList _values;

	private ArrayList _keys;

	public bool IsFixedSize => true;

	public bool IsReadOnly => true;

	public object this[object key]
	{
		get
		{
			IDictionary[] array = dictionaries;
			foreach (IDictionary dictionary in array)
			{
				if (dictionary.Contains(key))
				{
					return dictionary[key];
				}
			}
			return null;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public ICollection Keys
	{
		get
		{
			if (_keys != null)
			{
				return _keys;
			}
			_keys = new ArrayList();
			IDictionary[] array = dictionaries;
			foreach (IDictionary dictionary in array)
			{
				_keys.AddRange(dictionary.Keys);
			}
			return _keys;
		}
	}

	public ICollection Values
	{
		get
		{
			if (_values != null)
			{
				return _values;
			}
			_values = new ArrayList();
			IDictionary[] array = dictionaries;
			foreach (IDictionary dictionary in array)
			{
				_values.AddRange(dictionary.Values);
			}
			return _values;
		}
	}

	public int Count
	{
		get
		{
			int num = 0;
			IDictionary[] array = dictionaries;
			foreach (IDictionary dictionary in array)
			{
				num += dictionary.Count;
			}
			return num;
		}
	}

	public bool IsSynchronized => false;

	public object SyncRoot => this;

	public AggregateDictionary(IDictionary[] dics)
	{
		dictionaries = dics;
	}

	public void Add(object key, object value)
	{
		throw new NotSupportedException();
	}

	public void Clear()
	{
		throw new NotSupportedException();
	}

	public bool Contains(object ob)
	{
		IDictionary[] array = dictionaries;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Contains(ob))
			{
				return true;
			}
		}
		return false;
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		return new AggregateEnumerator(dictionaries);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new AggregateEnumerator(dictionaries);
	}

	public void Remove(object ob)
	{
		throw new NotSupportedException();
	}

	public void CopyTo(Array array, int index)
	{
		IDictionaryEnumerator dictionaryEnumerator = GetEnumerator();
		try
		{
			while (dictionaryEnumerator.MoveNext())
			{
				object current = dictionaryEnumerator.Current;
				array.SetValue(current, index++);
			}
		}
		finally
		{
			IDisposable disposable = dictionaryEnumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}

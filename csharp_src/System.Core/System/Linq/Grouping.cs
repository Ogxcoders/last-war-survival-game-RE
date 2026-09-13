using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq;

[DebuggerDisplay("Key = {Key}")]
[DebuggerTypeProxy(typeof(SystemLinq_GroupingDebugView<, >))]
internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IEnumerable<TElement>, IEnumerable, IList<TElement>, ICollection<TElement>
{
	internal TKey _key;

	internal int _hashCode;

	internal TElement[] _elements;

	internal int _count;

	internal Grouping<TKey, TElement> _hashNext;

	internal Grouping<TKey, TElement> _next;

	public TKey Key => _key;

	int ICollection<TElement>.Count => _count;

	bool ICollection<TElement>.IsReadOnly => true;

	TElement IList<TElement>.this[int index]
	{
		get
		{
			if (index < 0 || index >= _count)
			{
				throw Error.ArgumentOutOfRange("index");
			}
			return _elements[index];
		}
		set
		{
			throw Error.NotSupported();
		}
	}

	internal Grouping()
	{
	}

	internal void Add(TElement element)
	{
		if (_elements.Length == _count)
		{
			Array.Resize(ref _elements, checked(_count * 2));
		}
		_elements[_count] = element;
		_count++;
	}

	internal void Trim()
	{
		if (_elements.Length != _count)
		{
			Array.Resize(ref _elements, _count);
		}
	}

	public IEnumerator<TElement> GetEnumerator()
	{
		for (int i = 0; i < _count; i++)
		{
			yield return _elements[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	void ICollection<TElement>.Add(TElement item)
	{
		throw Error.NotSupported();
	}

	void ICollection<TElement>.Clear()
	{
		throw Error.NotSupported();
	}

	bool ICollection<TElement>.Contains(TElement item)
	{
		return Array.IndexOf(_elements, item, 0, _count) >= 0;
	}

	void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
	{
		Array.Copy(_elements, 0, array, arrayIndex, _count);
	}

	bool ICollection<TElement>.Remove(TElement item)
	{
		throw Error.NotSupported();
	}

	int IList<TElement>.IndexOf(TElement item)
	{
		return Array.IndexOf(_elements, item, 0, _count);
	}

	void IList<TElement>.Insert(int index, TElement item)
	{
		throw Error.NotSupported();
	}

	void IList<TElement>.RemoveAt(int index)
	{
		throw Error.NotSupported();
	}
}

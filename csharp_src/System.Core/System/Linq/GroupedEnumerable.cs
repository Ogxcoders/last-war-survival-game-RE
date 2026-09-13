using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

internal sealed class GroupedEnumerable<TSource, TKey, TElement> : IIListProvider<IGrouping<TKey, TElement>>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
{
	private readonly IEnumerable<TSource> _source;

	private readonly Func<TSource, TKey> _keySelector;

	private readonly Func<TSource, TElement> _elementSelector;

	private readonly IEqualityComparer<TKey> _comparer;

	public GroupedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
	{
		_source = source ?? throw Error.ArgumentNull("source");
		_keySelector = keySelector ?? throw Error.ArgumentNull("keySelector");
		_elementSelector = elementSelector ?? throw Error.ArgumentNull("elementSelector");
		_comparer = comparer;
	}

	public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
	{
		return Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IGrouping<TKey, TElement>[] ToArray()
	{
		return ((IIListProvider<IGrouping<TKey, TElement>>)Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer)).ToArray();
	}

	public List<IGrouping<TKey, TElement>> ToList()
	{
		return ((IIListProvider<IGrouping<TKey, TElement>>)Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer)).ToList();
	}

	public int GetCount(bool onlyIfCheap)
	{
		if (!onlyIfCheap)
		{
			return Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer).Count;
		}
		return -1;
	}
}
internal sealed class GroupedEnumerable<TSource, TKey> : IIListProvider<IGrouping<TKey, TSource>>, IEnumerable<IGrouping<TKey, TSource>>, IEnumerable
{
	private readonly IEnumerable<TSource> _source;

	private readonly Func<TSource, TKey> _keySelector;

	private readonly IEqualityComparer<TKey> _comparer;

	public GroupedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
	{
		_source = source ?? throw Error.ArgumentNull("source");
		_keySelector = keySelector ?? throw Error.ArgumentNull("keySelector");
		_comparer = comparer;
	}

	public IEnumerator<IGrouping<TKey, TSource>> GetEnumerator()
	{
		return Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IGrouping<TKey, TSource>[] ToArray()
	{
		return ((IIListProvider<IGrouping<TKey, TSource>>)Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer)).ToArray();
	}

	public List<IGrouping<TKey, TSource>> ToList()
	{
		return ((IIListProvider<IGrouping<TKey, TSource>>)Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer)).ToList();
	}

	public int GetCount(bool onlyIfCheap)
	{
		if (!onlyIfCheap)
		{
			return Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer).Count;
		}
		return -1;
	}
}

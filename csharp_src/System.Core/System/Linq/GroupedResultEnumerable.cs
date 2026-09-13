using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

internal sealed class GroupedResultEnumerable<TSource, TKey, TElement, TResult> : IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
{
	private readonly IEnumerable<TSource> _source;

	private readonly Func<TSource, TKey> _keySelector;

	private readonly Func<TSource, TElement> _elementSelector;

	private readonly IEqualityComparer<TKey> _comparer;

	private readonly Func<TKey, IEnumerable<TElement>, TResult> _resultSelector;

	public GroupedResultEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
	{
		_source = source ?? throw Error.ArgumentNull("source");
		_keySelector = keySelector ?? throw Error.ArgumentNull("keySelector");
		_elementSelector = elementSelector ?? throw Error.ArgumentNull("elementSelector");
		_comparer = comparer;
		_resultSelector = resultSelector ?? throw Error.ArgumentNull("resultSelector");
	}

	public IEnumerator<TResult> GetEnumerator()
	{
		return Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer).ApplyResultSelector(_resultSelector).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public TResult[] ToArray()
	{
		return Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer).ToArray(_resultSelector);
	}

	public List<TResult> ToList()
	{
		return Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer).ToList(_resultSelector);
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
internal sealed class GroupedResultEnumerable<TSource, TKey, TResult> : IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
{
	private readonly IEnumerable<TSource> _source;

	private readonly Func<TSource, TKey> _keySelector;

	private readonly IEqualityComparer<TKey> _comparer;

	private readonly Func<TKey, IEnumerable<TSource>, TResult> _resultSelector;

	public GroupedResultEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
	{
		_source = source ?? throw Error.ArgumentNull("source");
		_keySelector = keySelector ?? throw Error.ArgumentNull("keySelector");
		_resultSelector = resultSelector ?? throw Error.ArgumentNull("resultSelector");
		_comparer = comparer;
	}

	public IEnumerator<TResult> GetEnumerator()
	{
		return Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer).ApplyResultSelector(_resultSelector).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public TResult[] ToArray()
	{
		return Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer).ToArray(_resultSelector);
	}

	public List<TResult> ToList()
	{
		return Lookup<TKey, TSource>.Create(_source, _keySelector, _comparer).ToList(_resultSelector);
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

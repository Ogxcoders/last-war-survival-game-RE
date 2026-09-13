using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

internal sealed class EmptyPartition<TElement> : IPartition<TElement>, IIListProvider<TElement>, IEnumerable<TElement>, IEnumerable, IEnumerator<TElement>, IDisposable, IEnumerator
{
	public static readonly IPartition<TElement> Instance = new EmptyPartition<TElement>();

	[ExcludeFromCodeCoverage]
	public TElement Current => default(TElement);

	[ExcludeFromCodeCoverage]
	object IEnumerator.Current => default(TElement);

	private EmptyPartition()
	{
	}

	public IEnumerator<TElement> GetEnumerator()
	{
		return this;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}

	public bool MoveNext()
	{
		return false;
	}

	void IEnumerator.Reset()
	{
		throw Error.NotSupported();
	}

	void IDisposable.Dispose()
	{
	}

	public IPartition<TElement> Skip(int count)
	{
		return this;
	}

	public IPartition<TElement> Take(int count)
	{
		return this;
	}

	public TElement TryGetElementAt(int index, out bool found)
	{
		found = false;
		return default(TElement);
	}

	public TElement TryGetFirst(out bool found)
	{
		found = false;
		return default(TElement);
	}

	public TElement TryGetLast(out bool found)
	{
		found = false;
		return default(TElement);
	}

	public TElement[] ToArray()
	{
		return Array.Empty<TElement>();
	}

	public List<TElement> ToList()
	{
		return new List<TElement>();
	}

	public int GetCount(bool onlyIfCheap)
	{
		return 0;
	}
}

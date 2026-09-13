using System.Collections.Generic;

namespace System.Linq.Parallel;

internal class ReverseComparer<T> : IComparer<T>
{
	private IComparer<T> _comparer;

	internal ReverseComparer(IComparer<T> comparer)
	{
		_comparer = comparer;
	}

	public int Compare(T x, T y)
	{
		return _comparer.Compare(y, x);
	}
}

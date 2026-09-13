using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel;

internal class EmptyEnumerator<T> : QueryOperatorEnumerator<T, int>, IEnumerator<T>, IDisposable, IEnumerator
{
	public T Current => default(T);

	object IEnumerator.Current => null;

	internal override bool MoveNext(ref T currentElement, ref int currentKey)
	{
		return false;
	}

	public bool MoveNext()
	{
		return false;
	}

	void IEnumerator.Reset()
	{
	}
}

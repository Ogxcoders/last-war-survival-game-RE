using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel;

internal class GroupByGrouping<TGroupKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
{
	private KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>> _keyValues;

	TGroupKey IGrouping<TGroupKey, TElement>.Key => _keyValues.Key.Value;

	internal GroupByGrouping(KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>> keyValues)
	{
		_keyValues = keyValues;
	}

	IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
	{
		return _keyValues.Value.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<TElement>)this).GetEnumerator();
	}
}

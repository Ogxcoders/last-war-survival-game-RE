using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel;

internal class OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
{
	private class KeyAndValuesComparer : IComparer<KeyValuePair<TOrderKey, TElement>>
	{
		private IComparer<TOrderKey> myComparer;

		public KeyAndValuesComparer(IComparer<TOrderKey> comparer)
		{
			myComparer = comparer;
		}

		public int Compare(KeyValuePair<TOrderKey, TElement> x, KeyValuePair<TOrderKey, TElement> y)
		{
			return myComparer.Compare(x.Key, y.Key);
		}
	}

	private TGroupKey _groupKey;

	private GrowingArray<TElement> _values;

	private GrowingArray<TOrderKey> _orderKeys;

	private IComparer<TOrderKey> _orderComparer;

	private KeyAndValuesComparer _wrappedComparer;

	TGroupKey IGrouping<TGroupKey, TElement>.Key => _groupKey;

	internal OrderedGroupByGrouping(TGroupKey groupKey, IComparer<TOrderKey> orderComparer)
	{
		_groupKey = groupKey;
		_values = new GrowingArray<TElement>();
		_orderKeys = new GrowingArray<TOrderKey>();
		_orderComparer = orderComparer;
		_wrappedComparer = new KeyAndValuesComparer(_orderComparer);
	}

	IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
	{
		int valueCount = _values.Count;
		TElement[] valueArray = _values.InternalArray;
		for (int i = 0; i < valueCount; i++)
		{
			yield return valueArray[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<TElement>)this).GetEnumerator();
	}

	internal void Add(TElement value, TOrderKey orderKey)
	{
		_values.Add(value);
		_orderKeys.Add(orderKey);
	}

	internal void DoneAdding()
	{
		List<KeyValuePair<TOrderKey, TElement>> list = new List<KeyValuePair<TOrderKey, TElement>>();
		for (int i = 0; i < _orderKeys.InternalArray.Length; i++)
		{
			list.Add(new KeyValuePair<TOrderKey, TElement>(_orderKeys.InternalArray[i], _values.InternalArray[i]));
		}
		list.Sort(0, _values.Count, _wrappedComparer);
		for (int j = 0; j < _values.InternalArray.Length; j++)
		{
			_orderKeys.InternalArray[j] = list[j].Key;
			_values.InternalArray[j] = list[j].Value;
		}
	}
}

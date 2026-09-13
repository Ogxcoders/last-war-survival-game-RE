namespace System.Collections.Generic;

internal struct SparseArrayBuilder<T>
{
	private LargeArrayBuilder<T> _builder;

	private ArrayBuilder<Marker> _markers;

	private int _reservedCount;

	public int Count => checked(_builder.Count + _reservedCount);

	public ArrayBuilder<Marker> Markers => _markers;

	public SparseArrayBuilder(bool initialize)
	{
		this = default(SparseArrayBuilder<T>);
		_builder = new LargeArrayBuilder<T>(initialize: true);
	}

	public void Add(T item)
	{
		_builder.Add(item);
	}

	public void AddRange(IEnumerable<T> items)
	{
		_builder.AddRange(items);
	}

	public void CopyTo(T[] array, int arrayIndex, int count)
	{
		int num = 0;
		CopyPosition position = CopyPosition.Start;
		for (int i = 0; i < _markers.Count; i++)
		{
			Marker marker = _markers[i];
			int num2 = Math.Min(marker.Index - num, count);
			if (num2 > 0)
			{
				position = _builder.CopyTo(position, array, arrayIndex, num2);
				arrayIndex += num2;
				num += num2;
				count -= num2;
			}
			if (count == 0)
			{
				return;
			}
			int num3 = Math.Min(marker.Count, count);
			arrayIndex += num3;
			num += num3;
			count -= num3;
		}
		if (count > 0)
		{
			_builder.CopyTo(position, array, arrayIndex, count);
		}
	}

	public void Reserve(int count)
	{
		_markers.Add(new Marker(count, Count));
		checked
		{
			_reservedCount += count;
		}
	}

	public bool ReserveOrAdd(IEnumerable<T> items)
	{
		if (EnumerableHelpers.TryGetCount(items, out var count))
		{
			if (count > 0)
			{
				Reserve(count);
				return true;
			}
		}
		else
		{
			AddRange(items);
		}
		return false;
	}

	public T[] ToArray()
	{
		if (_markers.Count == 0)
		{
			return _builder.ToArray();
		}
		T[] array = new T[Count];
		CopyTo(array, 0, array.Length);
		return array;
	}
}

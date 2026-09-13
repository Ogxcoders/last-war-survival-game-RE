using System;
using System.Collections.Generic;

namespace GameKit.Base;

public class FastUpdateSet<T>
{
	private T[] _datas;

	private Dictionary<T, int> _indices;

	public int Count => _indices.Count;

	public int Capacity => _datas.Length;

	public T[] RawArrayData => _datas;

	public T this[int index] => _datas[index];

	public FastUpdateSet(int initialCapacity = 4)
	{
		_datas = new T[initialCapacity];
		_indices = new Dictionary<T, int>();
	}

	public void Add(T item)
	{
		int count = Count;
		if (count == Capacity)
		{
			SetCapacity(_datas.Length * 2);
		}
		_datas[count] = item;
		_indices[item] = count;
	}

	public bool Remove(T item)
	{
		if (!_indices.TryGetValue(item, out var value))
		{
			return false;
		}
		int num = Count - 1;
		if (num != value)
		{
			T val = _datas[num];
			_datas[value] = val;
			_indices[val] = value;
		}
		_datas[num] = default(T);
		_indices.Remove(item);
		return true;
	}

	public T Get(int index)
	{
		if (index < 0 || index >= Count)
		{
			throw new ArgumentOutOfRangeException("index", "Index out of range.");
		}
		return _datas[index];
	}

	public int IndexOf(T item)
	{
		if (_indices.TryGetValue(item, out var value))
		{
			return value;
		}
		return -1;
	}

	public void SetCapacity(int newCapacity)
	{
		if (newCapacity < Count)
		{
			throw new ArgumentException("New capacity cannot be less than the current count.");
		}
		T[] array = new T[newCapacity];
		Array.Copy(_datas, array, Count);
		_datas = array;
	}
}

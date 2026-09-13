using System.Collections.Generic;

namespace System.Linq;

internal sealed class Set<TElement>
{
	private struct Slot
	{
		internal int _hashCode;

		internal int _next;

		internal TElement _value;
	}

	private readonly IEqualityComparer<TElement> _comparer;

	private int[] _buckets;

	private Slot[] _slots;

	private int _count;

	public int Count => _count;

	public Set(IEqualityComparer<TElement> comparer)
	{
		_comparer = comparer ?? EqualityComparer<TElement>.Default;
		_buckets = new int[7];
		_slots = new Slot[7];
	}

	public bool Add(TElement value)
	{
		int num = InternalGetHashCode(value);
		for (int num2 = _buckets[num % _buckets.Length] - 1; num2 >= 0; num2 = _slots[num2]._next)
		{
			if (_slots[num2]._hashCode == num && _comparer.Equals(_slots[num2]._value, value))
			{
				return false;
			}
		}
		if (_count == _slots.Length)
		{
			Resize();
		}
		int count = _count;
		_count++;
		int num3 = num % _buckets.Length;
		_slots[count]._hashCode = num;
		_slots[count]._value = value;
		_slots[count]._next = _buckets[num3] - 1;
		_buckets[num3] = count + 1;
		return true;
	}

	public bool Remove(TElement value)
	{
		int num = InternalGetHashCode(value);
		int num2 = num % _buckets.Length;
		int num3 = -1;
		for (int num4 = _buckets[num2] - 1; num4 >= 0; num4 = _slots[num4]._next)
		{
			if (_slots[num4]._hashCode == num && _comparer.Equals(_slots[num4]._value, value))
			{
				if (num3 < 0)
				{
					_buckets[num2] = _slots[num4]._next + 1;
				}
				else
				{
					_slots[num3]._next = _slots[num4]._next;
				}
				_slots[num4]._hashCode = -1;
				_slots[num4]._value = default(TElement);
				_slots[num4]._next = -1;
				return true;
			}
			num3 = num4;
		}
		return false;
	}

	private void Resize()
	{
		int num = checked(_count * 2 + 1);
		int[] array = new int[num];
		Slot[] array2 = new Slot[num];
		Array.Copy(_slots, 0, array2, 0, _count);
		for (int i = 0; i < _count; i++)
		{
			int num2 = array2[i]._hashCode % num;
			array2[i]._next = array[num2] - 1;
			array[num2] = i + 1;
		}
		_buckets = array;
		_slots = array2;
	}

	public TElement[] ToArray()
	{
		TElement[] array = new TElement[_count];
		for (int i = 0; i != array.Length; i++)
		{
			array[i] = _slots[i]._value;
		}
		return array;
	}

	public List<TElement> ToList()
	{
		int count = _count;
		List<TElement> list = new List<TElement>(count);
		for (int i = 0; i != count; i++)
		{
			list.Add(_slots[i]._value);
		}
		return list;
	}

	public void UnionWith(IEnumerable<TElement> other)
	{
		foreach (TElement item in other)
		{
			Add(item);
		}
	}

	private int InternalGetHashCode(TElement value)
	{
		if (value != null)
		{
			return _comparer.GetHashCode(value) & 0x7FFFFFFF;
		}
		return 0;
	}
}

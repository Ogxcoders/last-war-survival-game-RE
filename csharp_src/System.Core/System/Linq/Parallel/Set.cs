using System.Collections.Generic;

namespace System.Linq.Parallel;

internal class Set<TElement>
{
	internal struct Slot
	{
		internal int hashCode;

		internal int next;

		internal TElement value;
	}

	private int[] _buckets;

	private Slot[] _slots;

	private int _count;

	private readonly IEqualityComparer<TElement> _comparer;

	private const int InitialSize = 7;

	private const int HashCodeMask = int.MaxValue;

	public Set(IEqualityComparer<TElement> comparer)
	{
		if (comparer == null)
		{
			comparer = EqualityComparer<TElement>.Default;
		}
		_comparer = comparer;
		_buckets = new int[7];
		_slots = new Slot[7];
	}

	public bool Add(TElement value)
	{
		return !Find(value, add: true);
	}

	public bool Contains(TElement value)
	{
		return Find(value, add: false);
	}

	public bool Remove(TElement value)
	{
		int num = InternalGetHashCode(value);
		int num2 = num % _buckets.Length;
		int num3 = -1;
		for (int num4 = _buckets[num2] - 1; num4 >= 0; num4 = _slots[num4].next)
		{
			if (_slots[num4].hashCode == num && _comparer.Equals(_slots[num4].value, value))
			{
				if (num3 < 0)
				{
					_buckets[num2] = _slots[num4].next + 1;
				}
				else
				{
					_slots[num3].next = _slots[num4].next;
				}
				_slots[num4].hashCode = -1;
				_slots[num4].value = default(TElement);
				_slots[num4].next = -1;
				return true;
			}
			num3 = num4;
		}
		return false;
	}

	private bool Find(TElement value, bool add)
	{
		int num = InternalGetHashCode(value);
		for (int num2 = _buckets[num % _buckets.Length] - 1; num2 >= 0; num2 = _slots[num2].next)
		{
			if (_slots[num2].hashCode == num && _comparer.Equals(_slots[num2].value, value))
			{
				return true;
			}
		}
		if (add)
		{
			if (_count == _slots.Length)
			{
				Resize();
			}
			int count = _count;
			_count++;
			int num3 = num % _buckets.Length;
			_slots[count].hashCode = num;
			_slots[count].value = value;
			_slots[count].next = _buckets[num3] - 1;
			_buckets[num3] = count + 1;
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
			int num2 = array2[i].hashCode % num;
			array2[i].next = array[num2] - 1;
			array[num2] = i + 1;
		}
		_buckets = array;
		_slots = array2;
	}

	internal int InternalGetHashCode(TElement value)
	{
		if (value != null)
		{
			return _comparer.GetHashCode(value) & 0x7FFFFFFF;
		}
		return 0;
	}
}

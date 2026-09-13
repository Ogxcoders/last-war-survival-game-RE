using UnityEngine;

namespace EnhancedUI;

public class SmallList<T>
{
	public T[] data;

	public int Count;

	public T this[int i]
	{
		get
		{
			return data[i];
		}
		set
		{
			data[i] = value;
		}
	}

	private void ResizeArray()
	{
		T[] array = ((data == null) ? new T[64] : new T[Mathf.Max(data.Length << 1, 64)]);
		if (data != null && Count > 0)
		{
			data.CopyTo(array, 0);
		}
		data = array;
	}

	public void Clear()
	{
		Count = 0;
	}

	public T First()
	{
		if (data == null || Count == 0)
		{
			return default(T);
		}
		return data[0];
	}

	public T Last()
	{
		if (data == null || Count == 0)
		{
			return default(T);
		}
		return data[Count - 1];
	}

	public void Add(T item)
	{
		if (data == null || Count == data.Length)
		{
			ResizeArray();
		}
		data[Count] = item;
		Count++;
	}

	public void AddStart(T item)
	{
		Insert(item, 0);
	}

	public void Insert(T item, int index)
	{
		if (data == null || Count == data.Length)
		{
			ResizeArray();
		}
		for (int num = Count; num > index; num--)
		{
			data[num] = data[num - 1];
		}
		data[index] = item;
		Count++;
	}

	public T RemoveStart()
	{
		return RemoveAt(0);
	}

	public T RemoveAt(int index)
	{
		if (data != null && Count != 0)
		{
			T result = data[index];
			for (int i = index; i < Count - 1; i++)
			{
				data[i] = data[i + 1];
			}
			Count--;
			data[Count] = default(T);
			return result;
		}
		return default(T);
	}

	public T Remove(T item)
	{
		if (data != null && Count != 0)
		{
			for (int i = 0; i < Count; i++)
			{
				ref readonly T reference = ref data[i];
				object obj = item;
				if (reference.Equals(obj))
				{
					return RemoveAt(i);
				}
			}
		}
		return default(T);
	}

	public T RemoveEnd()
	{
		if (data != null && Count != 0)
		{
			Count--;
			T result = data[Count];
			data[Count] = default(T);
			return result;
		}
		return default(T);
	}

	public bool Contains(T item)
	{
		if (data == null)
		{
			return false;
		}
		for (int i = 0; i < Count; i++)
		{
			ref readonly T reference = ref data[i];
			object obj = item;
			if (reference.Equals(obj))
			{
				return true;
			}
		}
		return false;
	}
}

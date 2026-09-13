using UnityEngine;

public class GPUSkinningBetterList<T>
{
	public T[] buffer;

	public int size;

	private int bufferIncrement;

	public T this[int i]
	{
		get
		{
			return buffer[i];
		}
		set
		{
			buffer[i] = value;
		}
	}

	public GPUSkinningBetterList(int bufferIncrement)
	{
		this.bufferIncrement = Mathf.Max(1, bufferIncrement);
	}

	private void AllocateMore()
	{
		T[] array = ((buffer != null) ? new T[buffer.Length + bufferIncrement] : new T[bufferIncrement]);
		if (buffer != null && size > 0)
		{
			buffer.CopyTo(array, 0);
		}
		buffer = array;
	}

	public void Clear()
	{
		size = 0;
	}

	public void Release()
	{
		size = 0;
		buffer = null;
	}

	public void Add(T item)
	{
		if (buffer == null || size == buffer.Length)
		{
			AllocateMore();
		}
		buffer[size++] = item;
	}

	public void AddRange(T[] items)
	{
		if (items == null)
		{
			return;
		}
		int num = items.Length;
		if (num == 0)
		{
			return;
		}
		if (buffer == null)
		{
			buffer = new T[Mathf.Max(bufferIncrement, num)];
			items.CopyTo(buffer, 0);
			size = num;
			return;
		}
		if (size + num > buffer.Length)
		{
			T[] array = new T[Mathf.Max(buffer.Length + bufferIncrement, size + num)];
			buffer.CopyTo(array, 0);
			items.CopyTo(array, size);
			buffer = array;
		}
		else
		{
			items.CopyTo(buffer, size);
		}
		size += num;
	}

	public void RemoveAt(int index)
	{
		if (buffer != null && index > -1 && index < size)
		{
			size--;
			buffer[index] = default(T);
			for (int i = index; i < size; i++)
			{
				buffer[i] = buffer[i + 1];
			}
			buffer[size] = default(T);
		}
	}

	public T Pop()
	{
		if (buffer == null || size == 0)
		{
			return default(T);
		}
		size--;
		T result = buffer[size];
		buffer[size] = default(T);
		return result;
	}

	public T Peek()
	{
		if (buffer == null || size == 0)
		{
			return default(T);
		}
		return buffer[size - 1];
	}
}

using System;
using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier;

internal sealed class ResizableArray<T>
{
	private T[] items;

	private int length;

	private static T[] emptyArr = new T[0];

	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return length;
		}
	}

	public T[] Data
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return items;
		}
	}

	public T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return items[index];
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			items[index] = value;
		}
	}

	public ResizableArray(int capacity)
		: this(capacity, 0)
	{
	}

	public ResizableArray(int capacity, int length)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (length < 0 || length > capacity)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (capacity > 0)
		{
			items = new T[capacity];
		}
		else
		{
			items = emptyArr;
		}
		this.length = length;
	}

	public ResizableArray(T[] initialArray)
	{
		if (initialArray == null)
		{
			throw new ArgumentNullException("initialArray");
		}
		if (initialArray.Length != 0)
		{
			items = new T[initialArray.Length];
			length = initialArray.Length;
			Array.Copy(initialArray, 0, items, 0, initialArray.Length);
		}
		else
		{
			items = emptyArr;
			length = 0;
		}
	}

	private void IncreaseCapacity(int capacity)
	{
		T[] destinationArray = new T[capacity];
		Array.Copy(items, 0, destinationArray, 0, Math.Min(length, capacity));
		items = destinationArray;
	}

	public void Clear()
	{
		Array.Clear(items, 0, length);
		length = 0;
	}

	public void Resize(int length, bool trimExess = false, bool clearMemory = false)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (length > items.Length)
		{
			IncreaseCapacity(length);
		}
		else if (length < this.length && clearMemory)
		{
			Array.Clear(items, length, this.length - length);
		}
		this.length = length;
		if (trimExess)
		{
			TrimExcess();
		}
	}

	public void TrimExcess()
	{
		if (items.Length != length)
		{
			T[] destinationArray = new T[length];
			Array.Copy(items, 0, destinationArray, 0, length);
			items = destinationArray;
		}
	}

	public void Add(T item)
	{
		if (length >= items.Length)
		{
			IncreaseCapacity(items.Length << 1);
		}
		items[length++] = item;
	}

	public T[] ToArray()
	{
		T[] array = new T[length];
		Array.Copy(items, 0, array, 0, length);
		return array;
	}
}

using System;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe;

public struct UnsafeList
{
	public unsafe void* m_pointer;

	public int m_size;

	public int m_capacity;

	public void Dispose<T>(Allocator allocator = Allocator.Persistent) where T : struct
	{
		SetCapacity<T>(0, allocator);
	}

	public void Resize<T>(int targetSize, Allocator allocator = Allocator.Persistent) where T : struct
	{
		SetCapacity<T>(targetSize, allocator);
		m_size = targetSize;
	}

	private unsafe void SetCapacity(int sizeOf, int alignOf, int targetCapacity, Allocator allocator)
	{
		if (targetCapacity > 0)
		{
			int num = 64 / sizeOf;
			if (targetCapacity < num)
			{
				targetCapacity = num;
			}
			targetCapacity = math.ceilpow2(targetCapacity);
		}
		int num2 = targetCapacity;
		if (num2 == m_capacity)
		{
			return;
		}
		void* ptr = null;
		if (num2 > 0)
		{
			ptr = UnsafeUtility.Malloc(sizeOf * num2, alignOf, allocator);
			if (m_capacity > 0)
			{
				int num3 = ((num2 < m_capacity) ? num2 : m_capacity) * sizeOf;
				UnsafeUtility.MemCpy(ptr, m_pointer, num3);
			}
		}
		if (m_capacity > 0)
		{
			UnsafeUtility.Free(m_pointer, allocator);
		}
		m_pointer = ptr;
		m_capacity = num2;
		if (m_size > m_capacity)
		{
			m_size = m_capacity;
		}
	}

	public void SetCapacity<T>(int targetCapacity, Allocator allocator = Allocator.Persistent) where T : struct
	{
		SetCapacity(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), targetCapacity, allocator);
	}

	public unsafe int IndexOf<T>(T t) where T : struct, IEquatable<T>
	{
		for (int num = m_size - 1; num >= 0; num--)
		{
			if (UnsafeUtility.ReadArrayElement<T>(m_pointer, num).Equals(t))
			{
				return num;
			}
		}
		return -1;
	}

	public bool Contains<T>(T t) where T : struct, IEquatable<T>
	{
		return IndexOf(t) != -1;
	}

	public unsafe void Add<T>(T t, Allocator allocator = Allocator.Persistent) where T : struct
	{
		Resize<T>(m_size + 1, allocator);
		UnsafeUtility.WriteArrayElement(m_pointer, m_size - 1, t);
	}

	public unsafe void AddRange<T>(void* t, int count, Allocator allocator = Allocator.Persistent) where T : struct
	{
		int size = m_size;
		Resize<T>(size + count, allocator);
		int num = UnsafeUtility.SizeOf<T>();
		UnsafeUtility.MemCpy((byte*)m_pointer + size * num, t, count * num);
	}

	private unsafe void RemoveRangeSwapBack(int sizeOf, int begin, int end)
	{
		int num = end - begin;
		void* destination = (byte*)m_pointer + begin * sizeOf;
		void* source = (byte*)m_pointer + (m_size - num) * sizeOf;
		UnsafeUtility.MemCpy(destination, source, num * sizeOf);
		m_size -= num;
	}

	public void RemoveRangeSwapBack<T>(int begin, int end) where T : struct
	{
		RemoveRangeSwapBack(UnsafeUtility.SizeOf<T>(), begin, end);
	}

	public void RemoveAtSwapBack<T>(int index, T t) where T : struct, IEquatable<T>
	{
		RemoveAtSwapBack<T>(index);
	}

	public void RemoveAtSwapBack<T>(int index) where T : struct
	{
		RemoveRangeSwapBack<T>(index, index + 1);
	}

	private unsafe void Append(int sizeOf, UnsafeList src)
	{
		int size = src.m_size;
		int size2 = m_size;
		int size3 = m_size + size;
		m_size = size3;
		void* destination = (byte*)m_pointer + size2 * sizeOf;
		void* pointer = src.m_pointer;
		UnsafeUtility.MemCpy(destination, pointer, size * sizeOf);
	}

	public void Append<T>(UnsafeList src) where T : struct
	{
		Append(UnsafeUtility.SizeOf<T>(), src);
	}
}

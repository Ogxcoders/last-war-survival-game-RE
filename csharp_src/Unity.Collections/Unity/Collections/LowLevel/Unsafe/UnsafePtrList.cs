using System;
using System.Diagnostics;

namespace Unity.Collections.LowLevel.Unsafe;

[DebuggerTypeProxy(typeof(UnsafePtrListDebugView))]
public struct UnsafePtrList
{
	public unsafe void** m_pointer;

	public int m_size;

	public int m_capacity;

	public unsafe ref UnsafeList GetUnsafeList()
	{
		return ref *(UnsafeList*)UnsafeUtility.AddressOf(ref this);
	}

	public void Dispose(Allocator allocator = Allocator.Persistent)
	{
		SetCapacity(0, allocator);
	}

	public void Resize(int targetSize, Allocator allocator = Allocator.Persistent)
	{
		GetUnsafeList().Resize<IntPtr>(targetSize, allocator);
	}

	public void SetCapacity(int targetCapacity, Allocator allocator = Allocator.Persistent)
	{
		GetUnsafeList().SetCapacity<IntPtr>(targetCapacity, allocator);
	}

	public unsafe int IndexOf(void* t)
	{
		for (int num = m_size - 1; num >= 0; num--)
		{
			if (m_pointer[num] == t)
			{
				return num;
			}
		}
		return -1;
	}

	public unsafe bool Contains(void* t)
	{
		return IndexOf(t) != -1;
	}

	public unsafe void Add(void* t, Allocator allocator = Allocator.Persistent)
	{
		Resize(m_size + 1, allocator);
		m_pointer[m_size - 1] = t;
	}

	public void RemoveRangeSwapBack(int begin, int end)
	{
		GetUnsafeList().RemoveRangeSwapBack<IntPtr>(begin, end);
	}

	public unsafe void RemoveAtSwapBack(int index, void* expectedValue)
	{
		RemoveAtSwapBack(index);
	}

	public void RemoveAtSwapBack(int index)
	{
		RemoveRangeSwapBack(index, index + 1);
	}

	public void Append(UnsafePtrList src)
	{
		GetUnsafeList().Append<IntPtr>(src.GetUnsafeList());
	}
}

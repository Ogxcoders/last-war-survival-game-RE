using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections.Experimental;

public struct NativeArrayFullSOA<T> : IDisposable where T : struct
{
	private static StructLayoutData4 ms_CachedLayout;

	private unsafe byte* m_Base;

	private int m_Length;

	private Allocator m_Allocator;

	public int Length => m_Length;

	public Allocator Allocator => m_Allocator;

	public unsafe T this[int index]
	{
		get
		{
			T output = default(T);
			uint* ptr = (uint*)UnsafeUtility.AddressOf(ref output);
			int fieldCount = ms_CachedLayout.FieldCount;
			int length = m_Length;
			uint* ptr2 = (uint*)(m_Base + 4 * index);
			for (int i = 0; i < fieldCount; i++)
			{
				ptr[ms_CachedLayout.GetFieldInfo(i).Offset / 4] = *ptr2;
				ptr2 += length;
			}
			return output;
		}
		set
		{
			int fieldCount = ms_CachedLayout.FieldCount;
			int length = m_Length;
			uint* ptr = (uint*)UnsafeUtility.AddressOf(ref value);
			uint* ptr2 = (uint*)(m_Base + 4 * index);
			for (int i = 0; i < fieldCount; i++)
			{
				*ptr2 = ptr[ms_CachedLayout.GetFieldInfo(i).Offset / 4];
				ptr2 += length;
			}
		}
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckReadAccess(int index)
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckWriteAccess(int index)
	{
	}

	public NativeArrayFullSOA(int length, Allocator label)
		: this(length, label, 1)
	{
	}

	public unsafe NativeArrayFullSOA(int length, Allocator label, int stackDepth)
	{
		if (!ms_CachedLayout.IsCreated)
		{
			ms_CachedLayout = new StructLayoutData4(typeof(T));
		}
		m_Base = (byte*)UnsafeUtility.Malloc(4 * length * ms_CachedLayout.FieldCount, 32, label);
		m_Length = length;
		m_Allocator = label;
	}

	public unsafe void Dispose()
	{
		if (m_Base != null)
		{
			UnsafeUtility.Free(m_Base, m_Allocator);
		}
	}
}

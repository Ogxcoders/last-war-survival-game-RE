using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections.Experimental;

public struct NativeArrayChunked8<T> : IDisposable where T : struct
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
			int num = ms_CachedLayout.ChunkIndex(index);
			int num2 = ms_CachedLayout.ChunkOffset(index);
			int fieldCount = ms_CachedLayout.FieldCount;
			uint* ptr = (uint*)(m_Base + 32 * ms_CachedLayout.FieldCount * num + 4 * num2);
			uint* ptr2 = (uint*)UnsafeUtility.AddressOf(ref output);
			for (int i = 0; i < fieldCount; i++)
			{
				ptr2[ms_CachedLayout.GetFieldInfo(i).Offset / 4] = *ptr;
				ptr += 8;
			}
			return output;
		}
		set
		{
			int num = ms_CachedLayout.ChunkIndex(index);
			int num2 = ms_CachedLayout.ChunkOffset(index);
			int fieldCount = ms_CachedLayout.FieldCount;
			uint* ptr = (uint*)UnsafeUtility.AddressOf(ref value);
			uint* ptr2 = (uint*)(m_Base + 32 * ms_CachedLayout.FieldCount * num + 4 * num2);
			for (int i = 0; i < fieldCount; i++)
			{
				*ptr2 = ptr[ms_CachedLayout.GetFieldInfo(i).Offset / 4];
				ptr2 += 8;
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

	public NativeArrayChunked8(int length, Allocator label)
		: this(length, label, 1)
	{
	}

	public unsafe NativeArrayChunked8(int length, Allocator label, int stackDepth)
	{
		if (!ms_CachedLayout.IsCreated)
		{
			ms_CachedLayout = new StructLayoutData4(typeof(T));
		}
		m_Base = (byte*)UnsafeUtility.Malloc(32 * ms_CachedLayout.ChunksNeeded(length) * ms_CachedLayout.FieldCount, 32, label);
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

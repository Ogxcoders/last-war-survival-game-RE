using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

[NativeContainer]
[DebuggerDisplay("Length = {Length}")]
[DebuggerTypeProxy(typeof(NativeListDebugView<>))]
public struct NativeList<T> : IDisposable where T : struct
{
	[NativeDisableUnsafePtrRestriction]
	internal unsafe NativeListData* m_ListData;

	private Allocator m_Allocator;

	public unsafe T this[int index]
	{
		get
		{
			return UnsafeUtility.ReadArrayElement<T>(m_ListData->buffer, index);
		}
		set
		{
			UnsafeUtility.WriteArrayElement(m_ListData->buffer, index, value);
		}
	}

	public unsafe int Length => m_ListData->length;

	public unsafe int Capacity
	{
		get
		{
			return m_ListData->capacity;
		}
		set
		{
			if (m_ListData->capacity != value)
			{
				void* ptr = UnsafeUtility.Malloc(value * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), m_Allocator);
				UnsafeUtility.MemCpy(ptr, m_ListData->buffer, m_ListData->length * UnsafeUtility.SizeOf<T>());
				UnsafeUtility.Free(m_ListData->buffer, m_Allocator);
				m_ListData->buffer = ptr;
				m_ListData->capacity = value;
			}
		}
	}

	public unsafe bool IsCreated => m_ListData != null;

	public NativeList(Allocator i_label)
		: this(1, i_label, 2)
	{
	}

	public NativeList(int capacity, Allocator i_label)
		: this(capacity, i_label, 2)
	{
	}

	private unsafe NativeList(int capacity, Allocator i_label, int stackDepth)
	{
		capacity = Math.Max(1, capacity);
		long size = (long)UnsafeUtility.SizeOf<T>() * (long)capacity;
		m_Allocator = i_label;
		m_ListData = (NativeListData*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<NativeListData>(), UnsafeUtility.AlignOf<NativeListData>(), m_Allocator);
		m_ListData->buffer = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<T>(), m_Allocator);
		m_ListData->length = 0;
		m_ListData->capacity = capacity;
	}

	[BurstDiscard]
	internal static void IsBlittableAndThrow()
	{
		if (!UnsafeUtility.IsBlittable<T>())
		{
			throw new ArgumentException(string.Format("{0} used in NativeList<{0}> must be blittable", typeof(T)));
		}
	}

	public unsafe void Add(T element)
	{
		if (m_ListData->length >= m_ListData->capacity)
		{
			Capacity = m_ListData->length + m_ListData->capacity * 2;
		}
		this[m_ListData->length++] = element;
	}

	public unsafe void AddRange(NativeArray<T> elements)
	{
		AddRange(elements.GetUnsafeReadOnlyPtr(), elements.Length);
	}

	public unsafe void AddRange(void* elements, int count)
	{
		if (m_ListData->length + count > m_ListData->capacity)
		{
			Capacity = m_ListData->length + count * 2;
		}
		int num = UnsafeUtility.SizeOf<T>();
		UnsafeUtility.MemCpy((byte*)m_ListData->buffer + m_ListData->length * num, elements, num * count);
		m_ListData->length += count;
	}

	public unsafe void RemoveAtSwapBack(int index)
	{
		int num = m_ListData->length - 1;
		this[index] = this[num];
		m_ListData->length = num;
	}

	public unsafe void Dispose()
	{
		if (m_ListData != null)
		{
			UnsafeUtility.Free(m_ListData->buffer, m_Allocator);
			UnsafeUtility.Free(m_ListData, m_Allocator);
			m_ListData = null;
		}
	}

	public void Clear()
	{
		ResizeUninitialized(0);
	}

	public static implicit operator NativeArray<T>(NativeList<T> nativeList)
	{
		return nativeList.AsArray();
	}

	public unsafe NativeArray<T> AsArray()
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(m_ListData->buffer, m_ListData->length, Allocator.Invalid);
	}

	[Obsolete("Please use AsDeferredJobArray")]
	public NativeArray<T> ToDeferredJobArray()
	{
		return AsDeferredJobArray();
	}

	public unsafe NativeArray<T> AsDeferredJobArray()
	{
		byte* listData = (byte*)m_ListData;
		listData++;
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(listData, 0, Allocator.Invalid);
	}

	public T[] ToArray()
	{
		return ((NativeArray<T>)this).ToArray();
	}

	public NativeArray<T> ToArray(Allocator allocator)
	{
		NativeArray<T> result = new NativeArray<T>(Length, allocator, NativeArrayOptions.UninitializedMemory);
		result.CopyFrom(this);
		return result;
	}

	public void CopyFrom(T[] array)
	{
		Capacity = array.Length;
		((NativeArray<T>)this).CopyFrom(array);
	}

	public unsafe void ResizeUninitialized(int length)
	{
		Capacity = Math.Max(length, Capacity);
		m_ListData->length = length;
	}
}

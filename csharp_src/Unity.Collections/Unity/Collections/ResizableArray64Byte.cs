using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

public struct ResizableArray64Byte<T> where T : struct
{
	private const int totalSizeInBytes = 64;

	private const int bufferSizeInBytes = 60;

	private const int bufferSizeInInts = 15;

	private int m_count;

	private unsafe fixed int m_buffer[15];

	public int Length
	{
		get
		{
			return m_count;
		}
		[WriteAccessRequired]
		set
		{
			m_count = value;
		}
	}

	public int Capacity => 60 / UnsafeUtility.SizeOf<T>();

	public unsafe T this[int index]
	{
		get
		{
			fixed (int* buffer = m_buffer)
			{
				void* source = buffer;
				return UnsafeUtility.ReadArrayElement<T>(source, index);
			}
		}
		[WriteAccessRequired]
		set
		{
			fixed (int* buffer = m_buffer)
			{
				void* destination = buffer;
				UnsafeUtility.WriteArrayElement(destination, index, value);
			}
		}
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckElementAccess(int index)
	{
		if (index < 0 || index >= Length)
		{
			throw new IndexOutOfRangeException($"Index {index} is out of range of '{Length}' Length.");
		}
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckResize(int newCount)
	{
		if (newCount < 0 || newCount > Capacity)
		{
			throw new IndexOutOfRangeException($"NewCount {newCount} is out of range of '{Capacity}' Capacity.");
		}
	}

	[WriteAccessRequired]
	public unsafe void* GetUnsafePointer()
	{
		fixed (int* buffer = m_buffer)
		{
			return buffer;
		}
	}

	[WriteAccessRequired]
	public void Add(T a)
	{
		this[Length++] = a;
	}

	public ResizableArray64Byte(T a)
	{
		m_count = 1;
		this[0] = a;
	}

	public ResizableArray64Byte(T a, T b)
	{
		m_count = 2;
		this[0] = a;
		this[1] = b;
	}

	public ResizableArray64Byte(T a, T b, T c)
	{
		m_count = 3;
		this[0] = a;
		this[1] = b;
		this[2] = c;
	}

	public ResizableArray64Byte(T a, T b, T c, T d)
	{
		m_count = 4;
		this[0] = a;
		this[1] = b;
		this[2] = c;
		this[3] = d;
	}

	public ResizableArray64Byte(T a, T b, T c, T d, T e)
	{
		m_count = 5;
		this[0] = a;
		this[1] = b;
		this[2] = c;
		this[3] = d;
		this[4] = e;
	}
}

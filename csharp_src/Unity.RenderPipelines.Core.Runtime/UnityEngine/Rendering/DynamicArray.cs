using System;

namespace UnityEngine.Rendering;

public class DynamicArray<T> where T : new()
{
	private T[] m_Array;

	public int size { get; private set; }

	public ref T this[int index] => ref m_Array[index];

	public DynamicArray()
	{
		m_Array = new T[32];
		size = 32;
	}

	public DynamicArray(int size)
	{
		m_Array = new T[size];
		this.size = size;
	}

	public void Clear()
	{
		size = 0;
	}

	public int Add(in T value)
	{
		int num = size;
		if (num >= m_Array.Length)
		{
			T[] array = new T[m_Array.Length * 2];
			Array.Copy(m_Array, array, m_Array.Length);
			m_Array = array;
		}
		m_Array[num] = value;
		size++;
		return num;
	}

	public void Resize(int newSize, bool keepContent = false)
	{
		if (newSize > m_Array.Length)
		{
			if (keepContent)
			{
				T[] array = new T[newSize];
				Array.Copy(m_Array, array, m_Array.Length);
				m_Array = array;
			}
			else
			{
				m_Array = new T[newSize];
			}
		}
		size = newSize;
	}
}

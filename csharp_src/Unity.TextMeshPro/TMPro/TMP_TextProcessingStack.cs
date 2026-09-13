using System;
using System.Diagnostics;

namespace TMPro;

[DebuggerDisplay("Item count = {m_Count}")]
public struct TMP_TextProcessingStack<T>
{
	public T[] itemStack;

	public int index;

	private T m_DefaultItem;

	private int m_Capacity;

	private int m_RolloverSize;

	private int m_Count;

	private const int k_DefaultCapacity = 4;

	public int Count => m_Count;

	public T current
	{
		get
		{
			if (index > 0)
			{
				return itemStack[index - 1];
			}
			return itemStack[0];
		}
	}

	public int rolloverSize
	{
		get
		{
			return m_RolloverSize;
		}
		set
		{
			m_RolloverSize = value;
		}
	}

	public TMP_TextProcessingStack(T[] stack)
	{
		itemStack = stack;
		m_Capacity = stack.Length;
		index = 0;
		m_RolloverSize = 0;
		m_DefaultItem = default(T);
		m_Count = 0;
	}

	public TMP_TextProcessingStack(int capacity)
	{
		itemStack = new T[capacity];
		m_Capacity = capacity;
		index = 0;
		m_RolloverSize = 0;
		m_DefaultItem = default(T);
		m_Count = 0;
	}

	public TMP_TextProcessingStack(int capacity, int rolloverSize)
	{
		itemStack = new T[capacity];
		m_Capacity = capacity;
		index = 0;
		m_RolloverSize = rolloverSize;
		m_DefaultItem = default(T);
		m_Count = 0;
	}

	internal static void SetDefault(TMP_TextProcessingStack<T>[] stack, T item)
	{
		for (int i = 0; i < stack.Length; i++)
		{
			stack[i].SetDefault(item);
		}
	}

	public void Clear()
	{
		index = 0;
		m_Count = 0;
	}

	public void SetDefault(T item)
	{
		if (itemStack == null)
		{
			m_Capacity = 4;
			itemStack = new T[m_Capacity];
			m_DefaultItem = default(T);
		}
		itemStack[0] = item;
		index = 1;
	}

	public void Add(T item)
	{
		if (index < itemStack.Length)
		{
			itemStack[index] = item;
			index++;
		}
	}

	public T Remove()
	{
		index--;
		if (index <= 0)
		{
			index = 1;
			return itemStack[0];
		}
		return itemStack[index - 1];
	}

	public void Push(T item)
	{
		if (index == m_Capacity)
		{
			m_Capacity *= 2;
			if (m_Capacity == 0)
			{
				m_Capacity = 4;
			}
			Array.Resize(ref itemStack, m_Capacity);
		}
		itemStack[index] = item;
		if (m_RolloverSize == 0)
		{
			index++;
			m_Count++;
		}
		else
		{
			index = (index + 1) % m_RolloverSize;
			m_Count = ((m_Count < m_RolloverSize) ? (m_Count + 1) : m_RolloverSize);
		}
	}

	public T Pop()
	{
		if (index == 0 && m_RolloverSize == 0)
		{
			return default(T);
		}
		if (m_RolloverSize == 0)
		{
			index--;
		}
		else
		{
			index = (index - 1) % m_RolloverSize;
			index = ((index < 0) ? (index + m_RolloverSize) : index);
		}
		T result = itemStack[index];
		itemStack[index] = m_DefaultItem;
		m_Count = ((m_Count > 0) ? (m_Count - 1) : 0);
		return result;
	}

	public T Peek()
	{
		if (index == 0)
		{
			return m_DefaultItem;
		}
		return itemStack[index - 1];
	}

	public T CurrentItem()
	{
		if (index > 0)
		{
			return itemStack[index - 1];
		}
		return itemStack[0];
	}

	public T PreviousItem()
	{
		if (index > 1)
		{
			return itemStack[index - 2];
		}
		return itemStack[0];
	}
}

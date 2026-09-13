using System.Collections.Generic;

public class CircularArry<T>
{
	private int m_count = 2;

	private readonly LinkedList<T> m_linkList;

	public int Count
	{
		get
		{
			return m_count;
		}
		set
		{
			m_count = value;
		}
	}

	public LinkedListNode<T> First => m_linkList.First;

	public LinkedListNode<T> Last => m_linkList.Last;

	public CircularArry()
	{
		m_linkList = new LinkedList<T>();
	}

	public CircularArry(int count)
		: this()
	{
		m_count = count;
	}

	public void Add(T item)
	{
		if (m_linkList.Count >= m_count)
		{
			LinkedListNode<T> first = m_linkList.First;
			m_linkList.RemoveFirst();
			first.Value = item;
			m_linkList.AddLast(first);
		}
		else
		{
			m_linkList.AddLast(item);
		}
	}

	public T[] ToArray()
	{
		T[] array = new T[m_linkList.Count];
		int num = 0;
		for (LinkedListNode<T> linkedListNode = m_linkList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			array[num] = linkedListNode.Value;
			num++;
		}
		return array;
	}

	public void Clear()
	{
		m_linkList.Clear();
	}
}

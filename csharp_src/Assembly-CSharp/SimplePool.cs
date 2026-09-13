using System;
using System.Collections.Generic;

public class SimplePool<T> where T : new()
{
	private readonly Stack<T> m_Stack = new Stack<T>();

	private readonly Action<T> m_ActionOnGet;

	private readonly Action<T> m_ActionOnRelease;

	public int countAll { get; private set; }

	public int countActive => countAll - countInactive;

	public int countInactive => m_Stack.Count;

	public SimplePool(Action<T> actionOnGet, Action<T> actionOnRelease)
	{
		m_ActionOnGet = actionOnGet;
		m_ActionOnRelease = actionOnRelease;
	}

	public T Get()
	{
		T val;
		if (m_Stack.Count == 0)
		{
			val = new T();
			countAll++;
		}
		else
		{
			val = m_Stack.Pop();
		}
		m_ActionOnGet?.Invoke(val);
		return val;
	}

	public void Release(T element)
	{
		m_ActionOnRelease?.Invoke(element);
		m_Stack.Push(element);
	}
}

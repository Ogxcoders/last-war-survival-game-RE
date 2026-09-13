#define UNITY_ASSERTIONS
using System;

namespace UnityEngine.UIElements.UIR;

internal class LinkedPool<T> where T : LinkedPoolItem<T>
{
	private readonly Func<T> m_CreateFunc;

	private readonly Action<T> m_ResetAction;

	private readonly int m_Limit;

	private T m_PoolFirst;

	public int Count { get; private set; }

	public LinkedPool(Func<T> createFunc, Action<T> resetAction, int limit = 10000)
	{
		Debug.Assert(createFunc != null);
		m_CreateFunc = createFunc;
		Debug.Assert(resetAction != null);
		m_ResetAction = resetAction;
		Debug.Assert(limit > 0);
		m_Limit = limit;
	}

	public void Clear()
	{
		m_PoolFirst = null;
		Count = 0;
	}

	public T Get()
	{
		T val = m_PoolFirst;
		if (m_PoolFirst != null)
		{
			int count = Count - 1;
			Count = count;
			m_PoolFirst = val.poolNext;
			m_ResetAction(val);
		}
		else
		{
			val = m_CreateFunc();
		}
		return val;
	}

	public void Return(T item)
	{
		if (Count < m_Limit)
		{
			item.poolNext = m_PoolFirst;
			m_PoolFirst = item;
			int count = Count + 1;
			Count = count;
		}
	}
}

#define UNITY_ASSERTIONS
namespace UnityEngine.UIElements.UIR;

internal class Pool<T> where T : PoolItem, new()
{
	private PoolItem m_Pool;

	public T Get()
	{
		if (m_Pool == null)
		{
			return new T();
		}
		Debug.Assert(m_Pool != null);
		T val = (T)m_Pool;
		m_Pool = m_Pool.poolNext;
		val.poolNext = null;
		return val;
	}

	public void Return(T obj)
	{
		obj.poolNext = m_Pool;
		m_Pool = obj;
	}
}

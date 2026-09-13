using System;

namespace GameKit.Base;

public static class GenericPool<T> where T : new()
{
	private static ObjectPool<T> _msPool;

	public static ObjectPool<T> Pool => _msPool ?? Init(null, null);

	public static ObjectPool<T> Init(Action<T> actionOnGet, Action<T> actionOnRelease)
	{
		_msPool = new ObjectPool<T>(actionOnGet, actionOnRelease);
		return _msPool;
	}

	public static T Get()
	{
		return Pool.Get();
	}

	public static ObjectPool<T>.PooledObject Get(out T value)
	{
		return Pool.Get(out value);
	}

	public static ObjectPool<T>.PooledObject GetDisposable()
	{
		return Pool.GetDisposable();
	}

	public static void Release(T toRelease)
	{
		Pool.Release(toRelease);
	}
}

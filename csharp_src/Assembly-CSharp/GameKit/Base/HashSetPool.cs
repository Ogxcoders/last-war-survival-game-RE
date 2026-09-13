using System;
using System.Collections.Generic;

namespace GameKit.Base;

public static class HashSetPool<T>
{
	private static ObjectPool<HashSet<T>> _msPool;

	public static ObjectPool<HashSet<T>> Pool => _msPool ?? Init(null, null);

	public static ObjectPool<HashSet<T>> Init(Action<HashSet<T>> actionOnGet, Action<HashSet<T>> actionOnRelease)
	{
		_msPool = new ObjectPool<HashSet<T>>(actionOnGet, actionOnRelease);
		return _msPool;
	}

	public static HashSet<T> Get()
	{
		return Pool.Get();
	}

	public static ObjectPool<HashSet<T>>.PooledObject Get(out HashSet<T> value)
	{
		return Pool.Get(out value);
	}

	public static ObjectPool<HashSet<T>>.PooledObject GetDisposable()
	{
		return Pool.GetDisposable();
	}

	public static void Release(HashSet<T> toRelease)
	{
		Pool.Release(toRelease);
	}
}

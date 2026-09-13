using System;
using System.Collections.Generic;

namespace Joker;

public static class DictionaryPool<TKey, TValue>
{
	private static ObjectPool<Dictionary<TKey, TValue>> _msPool;

	public static ObjectPool<Dictionary<TKey, TValue>> Pool => _msPool ?? Init(null, null);

	public static ObjectPool<Dictionary<TKey, TValue>> Init(Action<Dictionary<TKey, TValue>> actionOnGet, Action<Dictionary<TKey, TValue>> actionOnRelease)
	{
		Log.Assert(_msPool == null);
		_msPool = new ObjectPool<Dictionary<TKey, TValue>>(actionOnGet, actionOnRelease);
		return _msPool;
	}

	public static Dictionary<TKey, TValue> Get()
	{
		return Pool.Get();
	}

	public static ObjectPool<Dictionary<TKey, TValue>>.PooledObject Get(out Dictionary<TKey, TValue> value)
	{
		return Pool.Get(out value);
	}

	public static ObjectPool<Dictionary<TKey, TValue>>.PooledObject GetDisposable()
	{
		return Pool.GetDisposable();
	}

	public static void Release(Dictionary<TKey, TValue> toRelease)
	{
		Pool.Release(toRelease);
	}
}

using System;
using System.Collections.Generic;

namespace Joker;

public static class ListPool<T>
{
	private static ObjectPool<List<T>> _msPool;

	public static ObjectPool<List<T>> Pool => _msPool ?? Init(null, OnReleaseList);

	public static ObjectPool<List<T>> Init(Action<List<T>> actionOnGet, Action<List<T>> actionOnRelease)
	{
		Log.Assert(_msPool == null);
		_msPool = new ObjectPool<List<T>>(actionOnGet, actionOnRelease);
		return _msPool;
	}

	private static void OnReleaseList(List<T> obj)
	{
		obj.Clear();
	}

	public static List<T> Get()
	{
		return Pool.Get();
	}

	public static ObjectPool<List<T>>.PooledObject Get(out List<T> value)
	{
		return Pool.Get(out value);
	}

	public static ObjectPool<List<T>>.PooledObject GetDisposable()
	{
		return Pool.GetDisposable();
	}

	public static void Release(List<T> toRelease)
	{
		Pool.Release(toRelease);
	}
}

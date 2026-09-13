using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class ThreadSafeObjectPool<T> : IDisposable where T : class, IDisposable, new()
{
	private ConcurrentQueue<T> _recycled;

	private Func<T> _factoryFunc;

	public int Count
	{
		get
		{
			if (_recycled == null)
			{
				return 0;
			}
			return _recycled.Count;
		}
	}

	public ThreadSafeObjectPool(Func<T> factoryFunc = null)
	{
		_recycled = new ConcurrentQueue<T>();
		_factoryFunc = factoryFunc ?? new Func<T>(DefaultFactoryFunc);
	}

	private T DefaultFactoryFunc()
	{
		return new T();
	}

	public T Allocate()
	{
		if (!_recycled.TryDequeue(out var result))
		{
			return _factoryFunc();
		}
		return result;
	}

	public void Recycle(T t)
	{
		if (t != null)
		{
			t.Dispose();
			_recycled.Enqueue(t);
		}
	}

	public void Recycle(ICollection<T> collection)
	{
		if (collection != null && collection.Count != 0)
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Dispose();
				_recycled.Enqueue(enumerator.Current);
			}
			collection.Clear();
		}
	}

	public void Release(int remainCount = 0)
	{
		if (_recycled == null || _recycled.Count <= 0)
		{
			return;
		}
		if (remainCount <= 0)
		{
			_recycled = new ConcurrentQueue<T>();
			return;
		}
		while (_recycled.Count > remainCount)
		{
			_recycled.TryDequeue(out var _);
		}
	}

	public void Dispose()
	{
		_recycled = null;
	}
}

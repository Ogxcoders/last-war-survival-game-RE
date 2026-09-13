using System;
using System.Collections.Generic;

namespace FibMatrix;

public class ObjectPool<T> : IDisposable where T : class, IDisposable, new()
{
	private Queue<T> _recycled;

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

	public ObjectPool(int capacity = -1, Func<T> factoryFunc = null)
	{
		_recycled = ((capacity < 0) ? new Queue<T>() : new Queue<T>(capacity));
		_factoryFunc = factoryFunc ?? new Func<T>(DefaultFactoryFunc);
	}

	private T DefaultFactoryFunc()
	{
		return new T();
	}

	public T Allocate()
	{
		if (_recycled.Count <= 0)
		{
			return _factoryFunc();
		}
		return _recycled.Dequeue();
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
		RecycleNoClear(collection);
		collection.Clear();
	}

	public void RecycleNoClear(ICollection<T> collection)
	{
		if (collection != null && collection.Count != 0)
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Dispose();
				_recycled.Enqueue(enumerator.Current);
			}
		}
	}

	public void Release(int remainCount = 0)
	{
		if (_recycled == null)
		{
			return;
		}
		if (remainCount <= 0)
		{
			_recycled.Clear();
			return;
		}
		while (_recycled.Count > remainCount)
		{
			_recycled.Dequeue();
		}
	}

	public void Dispose()
	{
		if (_recycled != null)
		{
			_recycled.Clear();
			_recycled = null;
		}
	}
}

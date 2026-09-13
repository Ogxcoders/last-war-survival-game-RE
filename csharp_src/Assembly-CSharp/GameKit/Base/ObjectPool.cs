using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GameKit.Base;

public class ObjectPool<T> where T : new()
{
	public class PooledObject : IDisposable
	{
		private readonly ObjectPool<T> _pool;

		public T Value { get; private set; }

		internal PooledObject(T value, ObjectPool<T> pool)
		{
			_pool = pool;
			Value = value;
		}

		public void Dispose()
		{
			_pool.Release(Value);
		}
	}

	private readonly Stack<T> _stack = new Stack<T>();

	private readonly Action<T> _actionOnGet;

	private readonly Action<T> _actionOnRelease;

	private readonly bool _collectionCheck;

	public int CountAll { get; private set; }

	public int CountActive => CountAll - CountInactive;

	public int CountInactive => _stack.Count;

	public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease, bool collectionCheck = true)
	{
		_actionOnGet = actionOnGet;
		_actionOnRelease = actionOnRelease;
		_collectionCheck = collectionCheck;
	}

	public T Get()
	{
		T val;
		if (_stack.Count == 0)
		{
			val = new T();
			CountAll++;
		}
		else
		{
			val = _stack.Pop();
		}
		if (_actionOnGet != null)
		{
			_actionOnGet(val);
		}
		return val;
	}

	public PooledObject Get(out T v)
	{
		return new PooledObject(v = Get(), this);
	}

	public PooledObject GetDisposable()
	{
		return new PooledObject(Get(), this);
	}

	public void Release(T element)
	{
		if (_actionOnRelease != null)
		{
			_actionOnRelease(element);
		}
		_stack.Push(element);
	}
}
public static class ObjectPool
{
	private class Pool
	{
		private readonly Type ObjectType;

		private readonly int MaxCapacity;

		private int NumItems;

		private readonly ConcurrentQueue<object> _items = new ConcurrentQueue<object>();

		private object FastItem;

		public Pool(Type objectType, int maxCapacity)
		{
			ObjectType = objectType;
			MaxCapacity = maxCapacity;
		}

		public object Get()
		{
			object result = FastItem;
			if (result == null || Interlocked.CompareExchange(ref FastItem, null, result) != result)
			{
				if (_items.TryDequeue(out result))
				{
					Interlocked.Decrement(ref NumItems);
					return result;
				}
				return Activator.CreateInstance(ObjectType);
			}
			return result;
		}

		public void Return(object obj)
		{
			if (FastItem != null || Interlocked.CompareExchange(ref FastItem, obj, null) != null)
			{
				if (Interlocked.Increment(ref NumItems) <= MaxCapacity)
				{
					_items.Enqueue(obj);
				}
				else
				{
					Interlocked.Decrement(ref NumItems);
				}
			}
		}
	}

	private static readonly ConcurrentDictionary<Type, Pool> objPool = new ConcurrentDictionary<Type, Pool>();

	private static readonly Func<Type, Pool> AddPoolFunc = (Type type) => new Pool(type, 1000);

	public static T Fetch<T>() where T : class
	{
		return Fetch(typeof(T)) as T;
	}

	public static object Fetch(Type type, bool isFromPool = true)
	{
		if (!isFromPool)
		{
			return Activator.CreateInstance(type);
		}
		object obj = GetPool(type).Get();
		if (obj is IPool pool)
		{
			pool.IsFromPool = true;
		}
		return obj;
	}

	public static void Recycle(object obj)
	{
		if (obj is IPool pool)
		{
			if (!pool.IsFromPool)
			{
				return;
			}
			pool.IsFromPool = false;
		}
		GetPool(obj.GetType()).Return(obj);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Pool GetPool(Type type)
	{
		return objPool.GetOrAdd(type, AddPoolFunc);
	}
}

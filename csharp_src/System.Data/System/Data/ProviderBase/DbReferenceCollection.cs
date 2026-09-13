using System.Threading;

namespace System.Data.ProviderBase;

internal abstract class DbReferenceCollection
{
	private struct CollectionEntry
	{
		private int _tag;

		private WeakReference _weak;

		public bool HasTarget
		{
			get
			{
				if (_tag != 0)
				{
					return _weak.IsAlive;
				}
				return false;
			}
		}

		public int Tag => _tag;

		public object Target
		{
			get
			{
				if (_tag != 0)
				{
					return _weak.Target;
				}
				return null;
			}
		}

		public void NewTarget(int tag, object target)
		{
			if (_weak == null)
			{
				_weak = new WeakReference(target, trackResurrection: false);
			}
			else
			{
				_weak.Target = target;
			}
			_tag = tag;
		}

		public void RemoveTarget()
		{
			_tag = 0;
		}
	}

	private const int LockPollTime = 100;

	private const int DefaultCollectionSize = 20;

	private CollectionEntry[] _items;

	private readonly object _itemLock;

	private int _optimisticCount;

	private int _lastItemIndex;

	private volatile bool _isNotifying;

	protected DbReferenceCollection()
	{
		_items = new CollectionEntry[20];
		_itemLock = new object();
		_optimisticCount = 0;
		_lastItemIndex = 0;
	}

	public abstract void Add(object value, int tag);

	protected void AddItem(object value, int tag)
	{
		bool flag = false;
		lock (_itemLock)
		{
			for (int i = 0; i <= _lastItemIndex; i++)
			{
				if (_items[i].Tag == 0)
				{
					_items[i].NewTarget(tag, value);
					flag = true;
					break;
				}
			}
			if (!flag && _lastItemIndex + 1 < _items.Length)
			{
				_lastItemIndex++;
				_items[_lastItemIndex].NewTarget(tag, value);
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j <= _lastItemIndex; j++)
				{
					if (!_items[j].HasTarget)
					{
						_items[j].NewTarget(tag, value);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				Array.Resize(ref _items, _items.Length * 2);
				_lastItemIndex++;
				_items[_lastItemIndex].NewTarget(tag, value);
			}
			_optimisticCount++;
		}
	}

	internal T FindItem<T>(int tag, Func<T, bool> filterMethod) where T : class
	{
		bool lockObtained = false;
		try
		{
			TryEnterItemLock(ref lockObtained);
			if (lockObtained && _optimisticCount > 0)
			{
				for (int i = 0; i <= _lastItemIndex; i++)
				{
					if (_items[i].Tag == tag)
					{
						object target = _items[i].Target;
						if (target != null && target is T val && filterMethod(val))
						{
							return val;
						}
					}
				}
			}
		}
		finally
		{
			ExitItemLockIfNeeded(lockObtained);
		}
		return null;
	}

	public void Notify(int message)
	{
		bool lockObtained = false;
		try
		{
			TryEnterItemLock(ref lockObtained);
			if (!lockObtained)
			{
				return;
			}
			try
			{
				_isNotifying = true;
				if (_optimisticCount > 0)
				{
					for (int i = 0; i <= _lastItemIndex; i++)
					{
						object target = _items[i].Target;
						if (target != null)
						{
							NotifyItem(message, _items[i].Tag, target);
							_items[i].RemoveTarget();
						}
					}
					_optimisticCount = 0;
				}
				if (_items.Length > 100)
				{
					_lastItemIndex = 0;
					_items = new CollectionEntry[20];
				}
			}
			finally
			{
				_isNotifying = false;
			}
		}
		finally
		{
			ExitItemLockIfNeeded(lockObtained);
		}
	}

	protected abstract void NotifyItem(int message, int tag, object value);

	public abstract void Remove(object value);

	protected void RemoveItem(object value)
	{
		bool lockObtained = false;
		try
		{
			TryEnterItemLock(ref lockObtained);
			if (!lockObtained || _optimisticCount <= 0)
			{
				return;
			}
			for (int i = 0; i <= _lastItemIndex; i++)
			{
				if (value == _items[i].Target)
				{
					_items[i].RemoveTarget();
					_optimisticCount--;
					break;
				}
			}
		}
		finally
		{
			ExitItemLockIfNeeded(lockObtained);
		}
	}

	private void TryEnterItemLock(ref bool lockObtained)
	{
		lockObtained = false;
		while (!_isNotifying && !lockObtained)
		{
			Monitor.TryEnter(_itemLock, 100, ref lockObtained);
		}
	}

	private void ExitItemLockIfNeeded(bool lockObtained)
	{
		if (lockObtained)
		{
			Monitor.Exit(_itemLock);
		}
	}
}

using System;
using System.Collections.Generic;

namespace Joker;

public sealed class LifeCycleGroup<T> : IInitializeSync, IStartupSync, IShutdownSync, IUpdate, ILateUpdate, IFixedUpdate, IDisplay where T : class
{
	private readonly Dictionary<Type, T> _objects = new Dictionary<Type, T>();

	private readonly List<object> _addList = new List<object>();

	private readonly List<object> _delList = new List<object>();

	private readonly List<IInitializeSync> _initializes = new List<IInitializeSync>();

	private readonly List<IStartupSync> _startups = new List<IStartupSync>();

	private readonly List<IShutdownSync> _shutdowns = new List<IShutdownSync>();

	private readonly List<IUpdate> _updates = new List<IUpdate>();

	private readonly List<ILateUpdate> _lateUpdates = new List<ILateUpdate>();

	private readonly List<IFixedUpdate> _fixedUpdates = new List<IFixedUpdate>();

	private readonly List<IDisplay> _displays = new List<IDisplay>();

	private bool _lockList;

	private bool _shutdown;

	public void Add(T obj)
	{
		if (_shutdown)
		{
			throw new InvalidOperationException("Cannot add more objects in shutdown state.");
		}
		_objects.Add(obj.GetType(), obj);
		AddImpl(obj);
	}

	public void Del(T obj)
	{
		if (_shutdown)
		{
			throw new InvalidOperationException("Cannot del more objects in shutdown state.");
		}
		if (_objects.ContainsKey(obj.GetType()))
		{
			_objects.Remove(obj.GetType());
			DelImpl(obj);
		}
	}

	public T Get(Type type)
	{
		return _objects[type];
	}

	public TS Get<TS>() where TS : T
	{
		return (TS)_objects[typeof(TS)];
	}

	public void Initialize()
	{
		_lockList = true;
		foreach (IInitializeSync initialize in _initializes)
		{
			if (!_delList.Contains(initialize))
			{
				initialize.Initialize();
			}
		}
		_lockList = false;
	}

	public void Startup()
	{
		UpdateDirtyQueue(init: true, startup: false);
		_lockList = true;
		foreach (IStartupSync startup in _startups)
		{
			if (!_delList.Contains(startup))
			{
				startup.Startup();
			}
		}
		_lockList = false;
	}

	public void Shutdown()
	{
		UpdateDirtyQueue();
		_shutdown = true;
		_lockList = true;
		foreach (IShutdownSync shutdown in _shutdowns)
		{
			if (!_delList.Contains(shutdown))
			{
				shutdown.Shutdown();
			}
		}
		_lockList = false;
	}

	public void Update()
	{
		UpdateDirtyQueue();
		_lockList = true;
		foreach (IUpdate update in _updates)
		{
			if (!_delList.Contains(update))
			{
				update.Update();
			}
		}
		_lockList = false;
	}

	public void LateUpdate()
	{
		UpdateDirtyQueue();
		_lockList = true;
		foreach (ILateUpdate lateUpdate in _lateUpdates)
		{
			if (!_delList.Contains(lateUpdate))
			{
				lateUpdate.LateUpdate();
			}
		}
		_lockList = false;
	}

	public void FixedUpdate()
	{
		UpdateDirtyQueue();
		_lockList = true;
		foreach (IFixedUpdate fixedUpdate in _fixedUpdates)
		{
			if (!_delList.Contains(fixedUpdate))
			{
				fixedUpdate.FixedUpdate();
			}
		}
		_lockList = false;
	}

	public void Display()
	{
		UpdateDirtyQueue();
		_lockList = true;
		foreach (IDisplay display in _displays)
		{
			if (!_delList.Contains(display))
			{
				display.Display();
			}
		}
		_lockList = false;
	}

	private void UpdateDirtyQueue(bool init = true, bool startup = true)
	{
		if (_addList.Count > 0)
		{
			for (int i = 0; i < _addList.Count; i++)
			{
				T val = (T)_addList[i];
				AddImpl(val);
				if (init && val is IInitializeSync initializeSync)
				{
					initializeSync.Initialize();
				}
				if (startup && val is IStartupSync startupSync)
				{
					startupSync.Startup();
				}
			}
			_addList.Clear();
		}
		if (_delList.Count > 0)
		{
			for (int j = 0; j < _delList.Count; j++)
			{
				DelImpl((T)_delList[j]);
			}
			_delList.Clear();
		}
	}

	private void AddImpl(T obj)
	{
		if (_delList.Contains(obj))
		{
			_delList.Remove(obj);
			throw new ArgumentException("Cannot add object after del.");
		}
		if (_lockList)
		{
			_addList.Add(obj);
			return;
		}
		if (obj is IInitializeSync item)
		{
			_initializes.Add(item);
		}
		if (obj is IStartupSync item2)
		{
			_startups.Add(item2);
		}
		if (obj is IShutdownSync item3)
		{
			_shutdowns.Add(item3);
		}
		if (obj is IUpdate item4)
		{
			_updates.Add(item4);
		}
		if (obj is ILateUpdate item5)
		{
			_lateUpdates.Add(item5);
		}
		if (obj is IFixedUpdate item6)
		{
			_fixedUpdates.Add(item6);
		}
		if (obj is IDisplay item7)
		{
			_displays.Add(item7);
		}
	}

	private void DelImpl(T obj)
	{
		if (_addList.Contains(obj))
		{
			_addList.Remove(obj);
			return;
		}
		if (_lockList)
		{
			_delList.Add(obj);
			return;
		}
		if (obj is IInitializeSync item)
		{
			_initializes.Remove(item);
		}
		if (obj is IStartupSync item2)
		{
			_startups.Remove(item2);
		}
		if (obj is IShutdownSync shutdownSync)
		{
			shutdownSync.Shutdown();
			_shutdowns.Remove(shutdownSync);
		}
		if (obj is IUpdate item3)
		{
			_updates.Remove(item3);
		}
		if (obj is ILateUpdate item4)
		{
			_lateUpdates.Remove(item4);
		}
		if (obj is IFixedUpdate item5)
		{
			_fixedUpdates.Remove(item5);
		}
		if (obj is IDisplay item6)
		{
			_displays.Remove(item6);
		}
	}
}

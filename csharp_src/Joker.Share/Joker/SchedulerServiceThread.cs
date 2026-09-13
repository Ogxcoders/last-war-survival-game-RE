using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Joker;

public class SchedulerServiceThread : Singleton<SchedulerServiceThread>, ISchedulerService, IService, IUpdateService, ILateUpdateService
{
	private readonly ConcurrentDictionary<int, IScheduler> _schedulers = new ConcurrentDictionary<int, IScheduler>();

	private readonly ConcurrentDictionary<int, SchedulerThreadPool> _poolSchedulers = new ConcurrentDictionary<int, SchedulerThreadPool>();

	private readonly List<SchedulerThreadMain> _mainSchedulers = new List<SchedulerThreadMain>();

	private readonly List<SchedulerThreadMain> _updateSchedulers = new List<SchedulerThreadMain>();

	private int _autoSchedulerId = 1073741824;

	private SpinLock _spin = new SpinLock(enableThreadOwnerTracking: false);

	private bool _started;

	public void Awake()
	{
	}

	public void Startup()
	{
		foreach (IScheduler value in _schedulers.Values)
		{
			value.Start();
		}
		_started = true;
	}

	public void Shutdown()
	{
		foreach (SchedulerThreadPool value in _poolSchedulers.Values)
		{
			value.Dispose();
		}
		_poolSchedulers.Clear();
		foreach (IScheduler value2 in _schedulers.Values)
		{
			value2.Dispose();
		}
		_schedulers.Clear();
		SchedulerThreadPool.WaitForShutdown();
	}

	public void Destroy()
	{
	}

	public IScheduler Create(ESchedulerType type, int id = -1)
	{
		bool lockTaken = false;
		_spin.Enter(ref lockTaken);
		try
		{
			if (id < 0)
			{
				id = ++_autoSchedulerId;
			}
			if (_schedulers.TryGetValue(id, out var value))
			{
				Log.Error($"ThreadSchedulerService Create failed, id={id}");
			}
			if (type == ESchedulerType.Main)
			{
				value = new SchedulerThreadMain(type, id);
				_mainSchedulers.Add((SchedulerThreadMain)value);
			}
			else if (ESchedulerType.SingleThread == type)
			{
				value = new SchedulerThreadSingle(type, id);
			}
			else
			{
				if (ESchedulerType.PooledThread != type)
				{
					Log.Error($"ThreadSchedulerService Create failed, type={type}, id={id}, not supported!");
					return null;
				}
				value = new SchedulerThreadPool(type, id);
			}
			_schedulers.TryAdd(id, value);
			if (_started)
			{
				value.Start();
			}
			return value;
		}
		catch (Exception arg)
		{
			Log.Error($"ThreadSchedulerService Create failed, type={type}, id={id}, e={arg}");
		}
		finally
		{
			_spin.Exit(useMemoryBarrier: false);
		}
		return null;
	}

	public IScheduler Find(int id)
	{
		if (!_schedulers.TryGetValue(id, out var value))
		{
			return null;
		}
		return value;
	}

	public void Finish(int id)
	{
		_schedulers.TryRemove(id, out var value);
		value?.Dispose();
	}

	public void Update(float deltaTime)
	{
		bool lockTaken = false;
		_updateSchedulers.Clear();
		_spin.Enter(ref lockTaken);
		try
		{
			_updateSchedulers.AddRange(_mainSchedulers);
		}
		finally
		{
			_spin.Exit(useMemoryBarrier: false);
		}
		foreach (SchedulerThreadMain updateScheduler in _updateSchedulers)
		{
			updateScheduler.DoUpdate();
		}
		_updateSchedulers.Clear();
	}

	public void LateUpdate(float deltaTime)
	{
		bool lockTaken = false;
		_updateSchedulers.Clear();
		_spin.Enter(ref lockTaken);
		try
		{
			_updateSchedulers.AddRange(_mainSchedulers);
		}
		finally
		{
			_spin.Exit(useMemoryBarrier: false);
		}
		foreach (SchedulerThreadMain updateScheduler in _updateSchedulers)
		{
			updateScheduler.DoLateUpdate();
		}
		_updateSchedulers.Clear();
	}
}

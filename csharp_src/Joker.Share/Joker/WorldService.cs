using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace Joker;

public class WorldService : Singleton<WorldService>, IService
{
	private readonly ConcurrentDictionary<string, World> _name2worlds = new ConcurrentDictionary<string, World>();

	private readonly ConcurrentDictionary<int, World> _id2Worlds = new ConcurrentDictionary<int, World>();

	private long _createCount;

	private long _deleteCount;

	public int NamedWorldCount => _name2worlds.Count;

	public long NamedWorldCreateCount => Interlocked.Read(ref _createCount);

	public long NamedWorldDeleteCount => Interlocked.Read(ref _deleteCount);

	public void Create<T>(ESchedulerType scheduler, int id = -1, string name = null, params object[] args) where T : World
	{
		Create(typeof(T), scheduler, id, name, args);
	}

	public void Create(Type type, ESchedulerType schedulerType, int id = -1, string name = null, params object[] args)
	{
		if (string.IsNullOrEmpty(name))
		{
			name = type.Name;
		}
		IScheduler scheduler = Singleton<SchedulerServiceThread>.Instance.Create(schedulerType, id);
		if (scheduler == null)
		{
			Log.Error($"WorldService Create failed, type={type}, id={id}, schedulerType={schedulerType}");
			return;
		}
		scheduler.Post(delegate
		{
			if (schedulerType == ESchedulerType.SingleThread)
			{
				if (Thread.CurrentThread.Name != null)
				{
					Log.Error($"Use same single thread as a scheduler to create world, name={name}, id={id}");
				}
				else
				{
					Thread.CurrentThread.Name = $"World Thread {name} : {id}";
				}
			}
			World world = (World)Activator.CreateInstance(type, args);
			Interlocked.Increment(ref _createCount);
			world.OnCreate(name, id, scheduler);
			if (!_name2worlds.TryAdd(name, world))
			{
				throw new Exception("World name " + name + " already exists");
			}
			if (id != -1 && !_id2Worlds.TryAdd(id, world))
			{
				throw new Exception($"World id {id} already exists");
			}
			world._InitializeInternal();
			world._StartupInternal();
			scheduler.Updater += world._UpdateInternal;
			scheduler.LateUpdater += world._LateUpdateInternal;
		});
	}

	public void Awake()
	{
	}

	public void Startup()
	{
	}

	public World MainWorld()
	{
		if (_id2Worlds.TryGetValue(0, out var value))
		{
			return value;
		}
		return null;
	}

	public World GetWorld(int id)
	{
		if (!_id2Worlds.TryGetValue(id, out var value))
		{
			return null;
		}
		return value;
	}

	public World GetWorld(string name)
	{
		if (!_name2worlds.TryGetValue(name, out var value))
		{
			return null;
		}
		return value;
	}

	public void DestroyWorld(World world)
	{
		_id2Worlds.TryRemove(world.Id, out var value);
		if (_name2worlds.TryRemove(world.Name, out value))
		{
			Interlocked.Increment(ref _deleteCount);
		}
		if (world.IsValid)
		{
			world.Scheduler.Post(world._ShutdownInternal);
		}
	}

	public IPEndPoint GetWorldAddress(int id)
	{
		if (id == 1000)
		{
			return new IPEndPoint(IPAddress.Loopback, 2525);
		}
		return null;
	}

	public IPEndPoint GetWorldAddress(string name)
	{
		return null;
	}

	public static int GetTargetId(string id, int baseId, int count)
	{
		return GetTargetId(id.HashCodeFNV1A32(), baseId, count);
	}

	public static int GetTargetId(int id, int baseId, int count)
	{
		return baseId + id % count;
	}

	public int GetTargetId(int id)
	{
		return 10000 + id % 2;
	}

	public int GetTargetId(string id)
	{
		int num = id.HashCodeFNV1A32();
		if (num < 0)
		{
			num = -num;
		}
		return GetTargetId(num);
	}

	public int GetTargetId<T>(int id)
	{
		return 10000 + id % 2;
	}

	public int GetTargetId<T>(string id)
	{
		return GetTargetId<T>(id.HashCodeFNV1A32());
	}

	public void Shutdown()
	{
		foreach (World value in _name2worlds.Values)
		{
			value._ShutdownInternal();
		}
	}

	public void Destroy()
	{
	}
}

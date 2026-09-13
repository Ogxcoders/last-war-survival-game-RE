using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Joker;

public abstract class World
{
	public const int kMainWorldID = 0;

	protected readonly LifeCycleGroup<ISystem> _systemLifeCycleGroup = new LifeCycleGroup<ISystem>();

	protected readonly ConcurrentQueue<(IMessage Message, object Sender)> _messages = new ConcurrentQueue<(IMessage, object)>();

	protected readonly Stopwatch _timer = new Stopwatch();

	protected IInitializeSync _initializeSync;

	protected IStartupSync _startupSync;

	protected IShutdownSync _shutdownSync;

	protected IUpdate _update;

	protected ILateUpdate _lateUpdate;

	protected IScheduler _scheduler;

	public string Name { get; internal set; }

	public int Id { get; internal set; }

	public bool IsValid { get; internal set; } = true;

	public long Now => _timer.ElapsedMilliseconds;

	public IScheduler Scheduler => _scheduler;

	internal virtual void OnCreate(string name, int id, IScheduler scheduler)
	{
		Name = name;
		Id = id;
		_initializeSync = this as IInitializeSync;
		_startupSync = this as IStartupSync;
		_shutdownSync = this as IShutdownSync;
		_update = this as IUpdate;
		_lateUpdate = this as ILateUpdate;
		_scheduler = scheduler;
	}

	internal void _InitializeInternal()
	{
		_initializeSync?.Initialize();
	}

	internal virtual void _UpdateInternal()
	{
		_systemLifeCycleGroup.Update();
		_update?.Update();
	}

	internal virtual void _LateUpdateInternal()
	{
		_systemLifeCycleGroup.LateUpdate();
		_lateUpdate?.LateUpdate();
	}

	internal void _StartupInternal()
	{
		_startupSync?.Startup();
		_systemLifeCycleGroup.Startup();
		_timer.Start();
	}

	internal void _ShutdownInternal()
	{
		if (IsValid)
		{
			IsValid = false;
			_timer.Stop();
			_systemLifeCycleGroup.Shutdown();
			_shutdownSync?.Shutdown();
			_scheduler.Dispose();
		}
	}

	public void EnqueueMessage(IMessage message, object sender)
	{
		_messages.Enqueue((message, sender));
	}

	public (IMessage Message, object Sender) DequeueMessage()
	{
		if (_messages.TryDequeue(out (IMessage, object) result))
		{
			return result;
		}
		throw new Exception("No message");
	}

	public bool TryDequeueMessage(out (IMessage Message, object Sender) message)
	{
		return _messages.TryDequeue(out message);
	}

	public ISystem GetSystem(Type type)
	{
		return _systemLifeCycleGroup.Get(type);
	}

	public T GetSystem<T>() where T : ISystem
	{
		return _systemLifeCycleGroup.Get<T>();
	}

	public void DelSystem(ISystem system)
	{
		_systemLifeCycleGroup.Del(system);
	}

	public ISystem AddSystem<T>(T system) where T : ISystem
	{
		system.World = this;
		_systemLifeCycleGroup.Add(system);
		return system;
	}

	public ISystem AddSystem(Type sysType)
	{
		if (typeof(ISystem).IsAssignableFrom(sysType))
		{
			ISystem system = Activator.CreateInstance(sysType) as ISystem;
			system.World = this;
			_systemLifeCycleGroup.Add(system);
			return system;
		}
		throw new ArgumentException(sysType.FullName + " is not ISystem");
	}

	public T AddSystem<T>() where T : ISystem
	{
		T val = new T
		{
			World = this
		};
		_systemLifeCycleGroup.Add(val);
		return val;
	}

	public T AddSystem<T, TP0>(TP0 p0) where T : ISystem
	{
		T val = (T)Activator.CreateInstance(typeof(T), p0);
		val.World = this;
		_systemLifeCycleGroup.Add(val);
		return val;
	}

	public T AddSystem<T, TP0, TP1>(TP0 p0, TP1 p1) where T : ISystem
	{
		T val = (T)Activator.CreateInstance(typeof(T), p0, p1);
		val.World = this;
		_systemLifeCycleGroup.Add(val);
		return val;
	}

	public T AddSystem<T, TP0, TP1, TP2>(TP0 p0, TP1 p1, TP2 p2) where T : ISystem
	{
		T val = (T)Activator.CreateInstance(typeof(T), p0, p1, p2);
		val.World = this;
		_systemLifeCycleGroup.Add(val);
		return val;
	}
}

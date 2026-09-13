using System;
using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public class EcsSystems : IEcsSystems
{
	private readonly EcsWorld _defaultWorld;

	private readonly Dictionary<string, EcsWorld> _worlds;

	private readonly List<IEcsSystem> _allSystems;

	private readonly List<IEcsRunSystem> _runSystems;

	private readonly List<IEcsPostRunSystem> _postRunSystems;

	public EcsSystems(EcsWorld defaultWorld)
	{
		_defaultWorld = defaultWorld;
		_worlds = new Dictionary<string, EcsWorld>(8);
		_allSystems = new List<IEcsSystem>(128);
		_runSystems = new List<IEcsRunSystem>(128);
		_postRunSystems = new List<IEcsPostRunSystem>(128);
	}

	public virtual IEcsSystems AddWorld(EcsWorld world, string name)
	{
		_worlds[name] = world;
		return this;
	}

	public virtual EcsWorld GetWorld(string name = null)
	{
		if (name == null)
		{
			return _defaultWorld;
		}
		_worlds.TryGetValue(name, out var value);
		return value;
	}

	public virtual Dictionary<string, EcsWorld> GetAllNamedWorlds()
	{
		return _worlds;
	}

	public virtual IEcsSystems Add(IEcsSystem system)
	{
		_allSystems.Add(system);
		if (system is IEcsRunSystem item)
		{
			_runSystems.Add(item);
		}
		if (system is IEcsPostRunSystem item2)
		{
			_postRunSystems.Add(item2);
		}
		return this;
	}

	public virtual List<IEcsSystem> GetAllSystems()
	{
		return _allSystems;
	}

	public virtual void Init()
	{
		foreach (IEcsSystem allSystem in _allSystems)
		{
			if (allSystem is IEcsPreInitSystem ecsPreInitSystem)
			{
				_defaultWorld.LogDebug("[EcsWorld] PreInit System " + allSystem.GetType().Name + " Start");
				ecsPreInitSystem.PreInit(this);
				_defaultWorld.LogDebug("[EcsWorld] PreInit System " + allSystem.GetType().Name + " End");
			}
		}
		foreach (IEcsSystem allSystem2 in _allSystems)
		{
			if (allSystem2 is IEcsInitSystem ecsInitSystem)
			{
				_defaultWorld.LogDebug("[EcsWorld] Init System " + allSystem2.GetType().Name + " Start");
				ecsInitSystem.Init(this);
				_defaultWorld.LogDebug("[EcsWorld] Init System " + allSystem2.GetType().Name + " End");
			}
		}
	}

	public virtual void Run()
	{
		int i = 0;
		for (int count = _runSystems.Count; i < count; i++)
		{
			_runSystems[i].Run(this);
		}
		int j = 0;
		for (int count2 = _postRunSystems.Count; j < count2; j++)
		{
			_postRunSystems[j].PostRun(this);
		}
	}

	public virtual void RunProfiler(Action<Type> begin, Action<Type> end)
	{
	}

	public virtual void Destroy()
	{
		for (int num = _allSystems.Count - 1; num >= 0; num--)
		{
			if (_allSystems[num] is IEcsDestroySystem ecsDestroySystem)
			{
				ecsDestroySystem.Destroy(this);
			}
		}
		for (int num2 = _allSystems.Count - 1; num2 >= 0; num2--)
		{
			if (_allSystems[num2] is IEcsPostDestroySystem ecsPostDestroySystem)
			{
				ecsPostDestroySystem.PostDestroy(this);
			}
		}
		_worlds.Clear();
		_allSystems.Clear();
		_runSystems.Clear();
		_postRunSystems.Clear();
	}
}

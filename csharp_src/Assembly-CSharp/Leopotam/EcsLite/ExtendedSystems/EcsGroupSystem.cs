using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite.ExtendedSystems;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public class EcsGroupSystem : IEcsPreInitSystem, IEcsSystem, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsDestroySystem, IEcsPostDestroySystem
{
	private readonly IEcsSystem[] _allSystems;

	private readonly IEcsRunSystem[] _runSystems;

	private readonly int _runSystemsCount;

	private readonly IEcsPostRunSystem[] _postRunSystems;

	private readonly int _postRunSystemsCount;

	private readonly string _eventsWorldName;

	private readonly string _name;

	private EcsFilter _filter;

	private EcsPool<EcsGroupSystemState> _pool;

	private bool _state;

	public IEcsSystem[] GetNestedSystems()
	{
		return _allSystems;
	}

	public EcsGroupSystem(string name, bool defaultState, string eventsWorldName, params IEcsSystem[] systems)
	{
		_name = name;
		_state = defaultState;
		_eventsWorldName = eventsWorldName;
		_allSystems = systems;
		_runSystemsCount = 0;
		_runSystems = new IEcsRunSystem[_allSystems.Length];
		_postRunSystems = new IEcsPostRunSystem[_allSystems.Length];
		for (int i = 0; i < _allSystems.Length; i++)
		{
			if (_allSystems[i] is IEcsRunSystem ecsRunSystem)
			{
				_runSystems[_runSystemsCount++] = ecsRunSystem;
			}
			if (_allSystems[i] is IEcsPostRunSystem ecsPostRunSystem)
			{
				_postRunSystems[_postRunSystemsCount++] = ecsPostRunSystem;
			}
		}
	}

	public void PreInit(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld(_eventsWorldName);
		_pool = world.GetPool<EcsGroupSystemState>();
		_filter = world.Filter<EcsGroupSystemState>().End();
		for (int i = 0; i < _allSystems.Length; i++)
		{
			if (_allSystems[i] is IEcsPreInitSystem ecsPreInitSystem)
			{
				ecsPreInitSystem.PreInit(systems);
			}
		}
	}

	public void Init(IEcsSystems systems)
	{
		for (int i = 0; i < _allSystems.Length; i++)
		{
			if (_allSystems[i] is IEcsInitSystem ecsInitSystem)
			{
				ecsInitSystem.Init(systems);
			}
		}
	}

	public void Run(IEcsSystems systems)
	{
		foreach (int item in _filter)
		{
			ref EcsGroupSystemState reference = ref _pool.Get(item);
			if (reference.Name == _name)
			{
				_state = reference.State;
				_pool.Del(item);
			}
		}
		if (_state)
		{
			for (int i = 0; i < _runSystemsCount; i++)
			{
				_runSystems[i].Run(systems);
			}
		}
	}

	public void PostRun(IEcsSystems systems)
	{
		foreach (int item in _filter)
		{
			ref EcsGroupSystemState reference = ref _pool.Get(item);
			if (reference.Name == _name)
			{
				_state = reference.State;
				_pool.Del(item);
			}
		}
		if (_state)
		{
			for (int i = 0; i < _postRunSystemsCount; i++)
			{
				_postRunSystems[i].PostRun(systems);
			}
		}
	}

	public void Destroy(IEcsSystems systems)
	{
		for (int num = _allSystems.Length - 1; num >= 0; num--)
		{
			if (_allSystems[num] is IEcsDestroySystem ecsDestroySystem)
			{
				ecsDestroySystem.Destroy(systems);
			}
		}
	}

	public void PostDestroy(IEcsSystems systems)
	{
		for (int num = _allSystems.Length - 1; num >= 0; num--)
		{
			if (_allSystems[num] is IEcsPostDestroySystem ecsPostDestroySystem)
			{
				ecsPostDestroySystem.PostDestroy(systems);
			}
		}
	}
}

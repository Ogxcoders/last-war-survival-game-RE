using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite.ExtendedSystems;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public sealed class DelHereSystem<T> : IEcsRunSystem, IEcsSystem where T : struct
{
	private readonly EcsFilter _filter;

	private readonly EcsPool<T> _pool;

	public DelHereSystem(EcsWorld world)
	{
		_filter = world.Filter<T>().End();
		_pool = world.GetPool<T>();
	}

	public void Run(IEcsSystems systems)
	{
		foreach (int item in _filter)
		{
			_pool.Del(item);
		}
	}
}

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemEmptyUIAdapt : IEcsInitSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsPoolInject<ComponentUIClient> _poolUI;

	public void Init(IEcsSystems systems)
	{
		int entity = _world.Value.NewEntity();
		_poolUI.Value.Add(entity);
	}
}

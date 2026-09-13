using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Core;

public class SystemTemplateInstantiate : IEcsPreInitSystem, IEcsSystem
{
	private protected EcsSharedInject<IGameSharedEnv> _shared;

	private protected EcsFilterInject<Inc<ComponentTemplate>> _filter;

	public void PreInit(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		EcsPool<ComponentTemplate> inc = _filter.Pools.Inc1;
		IResourceLoader resourceLoader = _shared.Value.ResourceLoader;
		foreach (int item in _filter.Value)
		{
			GameEntityTemplate.NewEntity(world, item, inc, resourceLoader);
		}
	}
}

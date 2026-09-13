using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Core;

public class SystemActivity : IEcsRunSystem, IEcsSystem
{
	private EcsFilterInject<Inc<ComponentActivitySnapshot, ComponentActivityChanged>> _filter;

	private List<IEcsPool> _cachedPools;

	public void Run(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		object shared = world.GetShared();
		EcsPool<ComponentActivitySnapshot> inc = _filter.Pools.Inc1;
		EcsPool<ComponentActivityChanged> inc2 = _filter.Pools.Inc2;
		foreach (int item in _filter.Value)
		{
			ref ComponentActivitySnapshot reference = ref inc.Get(item);
			if (inc2.Get(item).IsActive)
			{
				if (!reference.IsActive)
				{
					world.DoSnapshotRestoreEntityPartial(item, reference.Snapshot, shared);
					reference.IsActive = true;
				}
			}
			else if (reference.IsActive)
			{
				reference.IsActive = false;
				List<IEcsPool> snapshotPools = world.GetSnapshotPools(reference.Includes, reference.Excludes, _cachedPools);
				reference.Snapshot = world.DoSnapshotTakeEntityPartial(item, snapshotPools);
				for (int i = 0; i < snapshotPools.Count; i++)
				{
					snapshotPools[i].Del(item);
				}
			}
			inc2.Del(item);
		}
	}
}

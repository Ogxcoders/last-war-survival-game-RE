using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemGun : IEcsRunSystem, IEcsSystem
{
	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsWorldInject _world;

	private readonly EcsFilterInject<Inc<ComponentGunReload, ComponentPlayer>> _filterReload;

	private readonly EcsPoolInject<ComponentGunReload> _poolGunReload;

	public void Run(IEcsSystems systems)
	{
		RunReload(systems);
	}

	private void RunReload(IEcsSystems systems)
	{
		FP y = _shared.Value.LogicTickDelta;
		foreach (int item in _filterReload.Value)
		{
			ref ComponentGunReload reference = ref _poolGunReload.Value.Get(item);
			ref FP leftTime = ref reference.LeftTime;
			leftTime -= y;
			if (!(reference.LeftTime > FP.Zero))
			{
				FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(_world.Value), new EventTrigger(_world.Value.PackEntity(item), EcsPackedEntity.Invalid, EventTriggerType.GunReloadFinish));
				_poolGunReload.Value.Del(item);
			}
		}
	}
}

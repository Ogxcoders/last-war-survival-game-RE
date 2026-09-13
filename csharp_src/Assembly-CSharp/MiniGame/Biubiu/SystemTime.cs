using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SystemTime : IEcsRunSystem, IEcsSystem
{
	private readonly EcsSharedInject<IGameSharedEnv> _shared;

	private readonly EcsFilterInject<Inc<ComponentTime>> _filter;

	public void Run(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		EcsPool<ComponentTime> inc = _filter.Pools.Inc1;
		FP logicTime = _shared.Value.LogicTime;
		foreach (int item in _filter.Value)
		{
			ref ComponentTime reference = ref inc.Get(item);
			if (logicTime >= reference.TriggerNext)
			{
				ref FP triggerNext = ref reference.TriggerNext;
				triggerNext += reference.Interval;
				reference.TriggerCount++;
				if (reference.TriggerCount >= reference.TriggerMax)
				{
					FuncEvent.Broadcast(world, new EventTime(world.PackEntity(item), reference.ID, reference.TriggerCount, reference.Owner));
					inc.Del(item);
				}
			}
		}
	}
}

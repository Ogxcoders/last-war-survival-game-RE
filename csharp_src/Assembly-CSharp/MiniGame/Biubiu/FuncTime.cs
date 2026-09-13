using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public static class FuncTime
{
	public static int CreateLoopTimer(EcsWorld world, FP interval, FP delay, int id)
	{
		return CreateTimer(world, interval, delay, int.MaxValue, id);
	}

	public static int CreateTimer(EcsWorld world, FP interval, FP delay, int triggerCount, int id, int ownerEntity = -1)
	{
		int num = world.NewEntity();
		ref ComponentTime reference = ref world.GetPool<ComponentTime>().Add(num);
		reference.ID = id;
		reference.TriggerMax = triggerCount;
		reference.Start = world.GetShared<IGameSharedEnv>().LogicTime + delay;
		reference.Interval = interval;
		reference.TriggerNext = reference.Start + reference.Interval;
		reference.TriggerCount = 0;
		if (ownerEntity != -1)
		{
			reference.Owner = world.PackEntity(ownerEntity);
		}
		return num;
	}
}

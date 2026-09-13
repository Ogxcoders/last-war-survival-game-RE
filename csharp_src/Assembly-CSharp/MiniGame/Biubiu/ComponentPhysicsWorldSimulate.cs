using System;
using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentPhysicsWorldSimulate : TEcsPoolDelegate<ComponentPhysicsWorldSimulate>, IEcsPoolDelegate, IEcsAutoReset<ComponentPhysicsWorldSimulate>, IEcsAutoCopy<ComponentPhysicsWorldSimulate>, IEcsAutoSnapshot<ComponentPhysicsWorldSimulate>
{
	public S5Game Game;

	public int SimulatePhysicsFrame;

	public FP SimulateLogicTime;

	public FP SimulateLogicTargetTime;

	public FP SimulateSpeedFactor;

	public bool IsDebugShown;

	public Type DelegateType => typeof(ComponentPhysicsWorldSimulate);

	public void AutoReset(ref ComponentPhysicsWorldSimulate c, EcsWorld world, int entity)
	{
		if (c.Game != null)
		{
			c.Game.Dispose();
			c.Game = null;
		}
	}

	public void AutoCopy(ref ComponentPhysicsWorldSimulate src, ref ComponentPhysicsWorldSimulate dst)
	{
		throw new NotSupportedException();
	}

	public object TakeSnapshot(ref ComponentPhysicsWorldSimulate c, EcsWorld world, int entity, object env)
	{
		throw new NotSupportedException();
	}

	public void RestoreSnapshot(ref ComponentPhysicsWorldSimulate c, EcsWorld world, int entity, object data, object env)
	{
		throw new NotSupportedException();
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}

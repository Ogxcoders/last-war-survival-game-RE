using System;
using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class ComponentPhysicsWorldDelegate : TEcsPoolDelegate<ComponentPhysicsWorld>, IEcsPoolDelegate, IEcsAutoReset<ComponentPhysicsWorld>, IEcsAutoCopy<ComponentPhysicsWorld>, IEcsAutoSnapshot<ComponentPhysicsWorld>
{
	public Type DelegateType => typeof(ComponentPhysicsWorld);

	public void AutoReset(ref ComponentPhysicsWorld c, EcsWorld world, int entity)
	{
		if (c.Game != null)
		{
			c.Game.Dispose();
			c.Game = null;
		}
	}

	public object TakeSnapshot(ref ComponentPhysicsWorld c, EcsWorld world, int entity, object env)
	{
		if (((SharedRuntime)env).SharedSnapshotType != SharedSnapshotType.Shared)
		{
			return (0, FP.Zero);
		}
		return (c.PhysicsFrame, c.PhysicsTime);
	}

	public void RestoreSnapshot(ref ComponentPhysicsWorld c, EcsWorld world, int entity, object data, object env)
	{
		(int, FP) obj = ((int, FP))data;
		int item = obj.Item1;
		FP item2 = obj.Item2;
		c.Game = new S5Game();
		c.Game.Build(new S5GameSettings());
		c.Game.BindTrigger(new Box2DTriggerAdapt(world, (SharedRuntime)env, c.Game));
		c.PhysicsFrame = item;
		c.PhysicsTime = item2;
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}

	public void AutoCopy(ref ComponentPhysicsWorld src, ref ComponentPhysicsWorld dst)
	{
	}
}

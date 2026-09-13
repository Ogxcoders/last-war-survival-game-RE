using System;
using Box2DSharp.Dynamics;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentPhysicsSimulate : TEcsPoolDelegate<ComponentPhysicsSimulate>, IEcsPoolDelegate, IEcsAutoReset<ComponentPhysicsSimulate>, IEcsAutoCopy<ComponentPhysicsSimulate>, IEcsAutoSnapshot<ComponentPhysicsSimulate>
{
	public Body Body;

	public S5Game Game;

	public Type DelegateType => typeof(ComponentPhysicsSimulate);

	public void AutoReset(ref ComponentPhysicsSimulate c, EcsWorld world, int entity)
	{
		if (c.Body != null)
		{
			c.Game.DestroyBody(c.Body);
			c.Body = null;
			c.Game = null;
		}
	}

	public void AutoCopy(ref ComponentPhysicsSimulate src, ref ComponentPhysicsSimulate dst)
	{
		throw new NotSupportedException();
	}

	public object TakeSnapshot(ref ComponentPhysicsSimulate c, EcsWorld world, int entity, object env)
	{
		return null;
	}

	public void RestoreSnapshot(ref ComponentPhysicsSimulate c, EcsWorld world, int entity, object data, object env)
	{
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}

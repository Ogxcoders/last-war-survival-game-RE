using System;
using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public class ComponentPhysicsDelegate : TEcsPoolDelegate<ComponentPhysics>, IEcsPoolDelegate, IEcsAutoReset<ComponentPhysics>, IEcsAutoCopy<ComponentPhysics>, IEcsAutoSnapshot<ComponentPhysics>
{
	public Type DelegateType => typeof(ComponentPhysics);

	public void AutoReset(ref ComponentPhysics c, EcsWorld world, int entity)
	{
		if (c.Body != null)
		{
			c.Game.DestroyBody(c.Body);
			c.Body = null;
			c.Game = null;
		}
	}

	public void AutoCopy(ref ComponentPhysics src, ref ComponentPhysics dst)
	{
	}

	public object TakeSnapshot(ref ComponentPhysics c, EcsWorld world, int entity, object env)
	{
		PhysicsSnapShot.ComponentPhysicsSnapshotData componentPhysicsSnapshotData = new PhysicsSnapShot.ComponentPhysicsSnapshotData();
		componentPhysicsSnapshotData = c.Body.TakeSnapShot();
		if (componentPhysicsSnapshotData.BodyDefData.UserData is IBodyLogic bodyLogic)
		{
			componentPhysicsSnapshotData.BodyDefData.UserData = bodyLogic.Clone();
		}
		return componentPhysicsSnapshotData;
	}

	public void RestoreSnapshot(ref ComponentPhysics c, EcsWorld world, int entity, object data, object env)
	{
		SharedRuntime shared = env as SharedRuntime;
		PhysicsSnapShot.ComponentPhysicsSnapshotData componentPhysicsSnapshotData = (PhysicsSnapShot.ComponentPhysicsSnapshotData)data;
		c.Game = FuncPhysics.GetPhysicsWorld(shared, world).Game;
		c.Body = PhysicsSnapShot.CreateRestoreSnapshotBody(c.Game.World);
		EcsPool<ComponentPosition> pool = world.GetPool<ComponentPosition>();
		EcsPool<ComponentRotation> pool2 = world.GetPool<ComponentRotation>();
		if (pool.Has(entity))
		{
			componentPhysicsSnapshotData.BodyDefData.Position = pool.Get(entity).Position;
		}
		if (pool2.Has(entity))
		{
			componentPhysicsSnapshotData.BodyDefData.Rotation = pool2.Get(entity).Rotation * FP.Deg2Rad;
		}
		c.Body.RestoreSnapshot(componentPhysicsSnapshotData);
		if (c.Body.UserData is IBodyLogic bodyLogic)
		{
			c.Body.UserData = bodyLogic.Clone();
		}
		FuncPhysics.SetUpDataBodyLogic(c.Body, world, entity);
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}

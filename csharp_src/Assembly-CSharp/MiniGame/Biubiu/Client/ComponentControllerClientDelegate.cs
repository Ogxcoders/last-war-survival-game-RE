using System;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public class ComponentControllerClientDelegate : TEcsPoolDelegate<ComponentControllerClient>, IEcsPoolDelegate, IEcsAutoReset<ComponentControllerClient>, IEcsAutoCopy<ComponentControllerClient>, IEcsAutoSnapshot<ComponentControllerClient>
{
	public Type DelegateType => typeof(ComponentControllerClient);

	public void AutoReset(ref ComponentControllerClient c, EcsWorld world, int entity)
	{
		c.Controller = null;
	}

	public void AutoCopy(ref ComponentControllerClient src, ref ComponentControllerClient dst)
	{
		throw new NotImplementedException();
	}

	public object TakeSnapshot(ref ComponentControllerClient c, EcsWorld world, int entity, object env)
	{
		return null;
	}

	public void RestoreSnapshot(ref ComponentControllerClient c, EcsWorld world, int entity, object data, object env)
	{
		EcsPool<ComponentPrefabClient> pool = world.GetPool<ComponentPrefabClient>();
		if (pool.Has(entity))
		{
			ComponentPrefabClient comp = pool.Get(entity);
			c.Controller = comp.GetPrefab()?.GetComponentInChildren<DataUIPlayerController>();
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}

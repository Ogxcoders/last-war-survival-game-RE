using System;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public class ComponentControllerDelegate : TEcsPoolDelegate<ComponentControllerClient>, IEcsPoolDelegate, IEcsAutoReset<ComponentControllerClient>, IEcsAutoCopy<ComponentControllerClient>, IEcsAutoSnapshot<ComponentControllerClient>
{
	public Type DelegateType => typeof(ComponentControllerClient);

	public void AutoReset(ref ComponentControllerClient c, EcsWorld world, int entity)
	{
		c.Controller = null;
	}

	public void AutoCopy(ref ComponentControllerClient src, ref ComponentControllerClient dst)
	{
		throw new NotSupportedException();
	}

	public object TakeSnapshot(ref ComponentControllerClient c, EcsWorld world, int entity, object env)
	{
		throw new NotSupportedException();
	}

	public void RestoreSnapshot(ref ComponentControllerClient c, EcsWorld world, int entity, object data, object env)
	{
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		throw new NotSupportedException();
	}
}

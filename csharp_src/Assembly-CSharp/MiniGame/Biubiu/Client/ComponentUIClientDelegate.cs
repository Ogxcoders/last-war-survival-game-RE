using System;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public class ComponentUIClientDelegate : TEcsPoolDelegate<MiniGame.Biubiu.ComponentUIClient>, IEcsPoolDelegate, IEcsAutoReset<MiniGame.Biubiu.ComponentUIClient>, IEcsAutoCopy<MiniGame.Biubiu.ComponentUIClient>, IEcsAutoSnapshot<MiniGame.Biubiu.ComponentUIClient>
{
	public Type DelegateType => typeof(MiniGame.Biubiu.ComponentUIClient);

	public void AutoReset(ref MiniGame.Biubiu.ComponentUIClient c, EcsWorld world, int entity)
	{
		UIAdaptHolder uIHolder = c.GetUIHolder();
		if (uIHolder != null)
		{
			uIHolder.Dispose();
			c.UIHolder = null;
		}
	}

	public object TakeSnapshot(ref MiniGame.Biubiu.ComponentUIClient c, EcsWorld world, int entity, object env)
	{
		return null;
	}

	public void RestoreSnapshot(ref MiniGame.Biubiu.ComponentUIClient c, EcsWorld world, int entity, object data, object env)
	{
		c.InitComponent(world);
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}

	public void AutoCopy(ref MiniGame.Biubiu.ComponentUIClient src, ref MiniGame.Biubiu.ComponentUIClient dst)
	{
	}
}

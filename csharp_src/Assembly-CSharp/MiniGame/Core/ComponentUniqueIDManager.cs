using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public struct ComponentUniqueIDManager : TEcsPoolDelegate<ComponentUniqueIDManager>, IEcsPoolDelegate, IEcsAutoReset<ComponentUniqueIDManager>, IEcsAutoCopy<ComponentUniqueIDManager>, IEcsAutoSnapshot<ComponentUniqueIDManager>
{
	public Dictionary<int, int> UniqueIDToEntity;

	public Type DelegateType => typeof(ComponentUniqueIDManager);

	public void AutoReset(ref ComponentUniqueIDManager c, EcsWorld world, int entity)
	{
		if (c.UniqueIDToEntity != null)
		{
			c.UniqueIDToEntity.Clear();
		}
	}

	public void AutoCopy(ref ComponentUniqueIDManager src, ref ComponentUniqueIDManager dst)
	{
		throw new NotSupportedException("ComponentUniqueIDManager不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentUniqueIDManager c, EcsWorld world, int entity, object env)
	{
		ComponentUniqueIDManager c2 = default(ComponentUniqueIDManager);
		RestoreSnapshot(ref c2, world, entity, c, env);
		return c2;
	}

	public void RestoreSnapshot(ref ComponentUniqueIDManager c, EcsWorld world, int entity, object data, object env)
	{
		c.UniqueIDToEntity = new Dictionary<int, int>(((ComponentUniqueIDManager)data).UniqueIDToEntity);
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		throw new NotSupportedException("这玩意不能是个模板，是全局的，被比较就不对了");
	}
}

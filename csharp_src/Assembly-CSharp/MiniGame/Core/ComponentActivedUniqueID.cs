using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public struct ComponentActivedUniqueID : TEcsPoolDelegate<ComponentActivedUniqueID>, IEcsPoolDelegate, IEcsAutoReset<ComponentActivedUniqueID>, IEcsAutoCopy<ComponentActivedUniqueID>, IEcsAutoSnapshot<ComponentActivedUniqueID>
{
	public int ID;

	public Type DelegateType => typeof(ComponentActivedUniqueID);

	public void AutoReset(ref ComponentActivedUniqueID c, EcsWorld world, int entity)
	{
		if (c.ID > 0 && world.GetShared<IGameUniqueIDRegister>().UniqueIDManager.Unpack(world, out var entity2))
		{
			FuncUniqueID.Unregister(ref world.GetPool<ComponentUniqueIDManager>().Get(entity2), entity, c.ID);
			c.ID = 0;
		}
	}

	public void AutoCopy(ref ComponentActivedUniqueID src, ref ComponentActivedUniqueID dst)
	{
		throw new NotSupportedException("ComponentActivedUniqueID不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentActivedUniqueID c, EcsWorld world, int entity, object env)
	{
		return c.MemberwiseClone();
	}

	public void RestoreSnapshot(ref ComponentActivedUniqueID c, EcsWorld world, int entity, object data, object env)
	{
		c.ID = ((ComponentActivedUniqueID)data).ID;
	}

	public bool IsSnapshotEqual(object lhs, object rhs, EcsWorld world)
	{
		ComponentActivedUniqueID obj = (ComponentActivedUniqueID)lhs;
		ComponentActivedUniqueID componentActivedUniqueID = (ComponentActivedUniqueID)rhs;
		return obj.ID == componentActivedUniqueID.ID;
	}
}

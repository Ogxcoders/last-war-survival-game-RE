using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public static class FuncUniqueID
{
	public static void Register(ref ComponentUniqueIDManager mgr, int entity, int id)
	{
		if (id >= 0)
		{
			if (mgr.UniqueIDToEntity.TryGetValue(id, out var _))
			{
				throw new ArgumentException($"唯一ID{id}已被注册");
			}
			mgr.UniqueIDToEntity.Add(id, entity);
		}
	}

	public static void Unregister(ref ComponentUniqueIDManager mgr, int entity, int id)
	{
		if (id > 0)
		{
			if (!mgr.UniqueIDToEntity.TryGetValue(id, out var _))
			{
				throw new ArgumentException($"唯一ID{id}未被注册");
			}
			mgr.UniqueIDToEntity.Remove(id);
		}
	}

	public static bool TryGetEntityByUniqueID(EcsWorld world, int id, out int entity)
	{
		if (id < 0)
		{
			entity = 0;
			return false;
		}
		if (!world.GetShared<IGameUniqueIDRegister>().UniqueIDManager.Unpack(world, out var entity2))
		{
			entity = 0;
			return false;
		}
		return world.GetPool<ComponentUniqueIDManager>().Get(entity2).UniqueIDToEntity.TryGetValue(id, out entity);
	}

	public static bool TryGetUniqueIDByEntity(EcsWorld world, int entity, out int id)
	{
		id = 0;
		EcsPool<ComponentActivedUniqueID> pool = world.GetPool<ComponentActivedUniqueID>();
		if (pool.Has(entity))
		{
			id = pool.Get(entity).ID;
			return true;
		}
		EcsPool<ComponentUniqueID> pool2 = world.GetPool<ComponentUniqueID>();
		if (pool2.Has(entity))
		{
			id = pool2.Get(entity).ID;
			return true;
		}
		return false;
	}
}

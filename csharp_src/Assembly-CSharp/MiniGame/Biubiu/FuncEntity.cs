using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public static class FuncEntity
{
	public static bool TryGetControllerEntity(EcsWorld world, out int entity)
	{
		if (world.GetShared<SharedRuntime>().ControllerEntity.Unpack(world, out entity))
		{
			return true;
		}
		entity = -1;
		return false;
	}

	public static bool HasEntity(EcsWorld world, int entity)
	{
		return world.IsEntityAliveInternal(entity);
	}

	public static void DelEntity(EcsWorld world, int entity)
	{
		S5Game.S5GameColliderLayer layer = S5Game.S5GameColliderLayer.None;
		EcsPool<ComponentPhysics> pool = world.GetPool<ComponentPhysics>();
		if (pool != null && pool.Has(entity))
		{
			layer = (S5Game.S5GameColliderLayer)((IBodyLogic)pool.Get(entity).Body.UserData).ILayer;
		}
		bool flag = world.GetPool<ComponentPlayer>().Has(entity) || world.GetPool<ComponentEnemy>().Has(entity);
		if (flag && FuncData.GetIntData(world, entity, PropertyID.Die) == 1)
		{
			return;
		}
		FuncUniqueID.TryGetUniqueIDByEntity(world, entity, out var id);
		FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(world), new EventEntityDie(world.PackEntity(entity), EcsPackedEntity.Invalid, layer, id));
		if (flag)
		{
			FuncData.SetData(world, entity, PropertyID.Die, 1);
			if (pool != null && pool.Has(entity) && pool.Get(entity).Body.UserData is IBodyLogic bodyLogic)
			{
				bodyLogic.Die = true;
			}
		}
		else
		{
			world.DelEntity(entity);
		}
	}
}

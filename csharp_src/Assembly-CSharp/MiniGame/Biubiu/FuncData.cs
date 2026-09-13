using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public class FuncData
{
	public static void SetData(EcsWorld world, int entity, PropertyID propertyID, FP value, bool notify = true)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			pool.Get(entity).SetPropertyValue(propertyID, value);
			if (notify)
			{
				FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(world), new EventDataChange(world.PackEntity(entity), EcsPackedEntity.Invalid, propertyID));
			}
		}
	}

	public static FP GetFpData(EcsWorld world, int entity, PropertyID propertyID)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).GetPropertyValue(propertyID);
		}
		return 0;
	}

	public static FP GetFpData(EcsWorld world, int entity, PropertyID propertyID, FP df)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).GetPropertyValue(propertyID, df);
		}
		return df;
	}

	public static int GetIntData(EcsWorld world, int entity, PropertyID propertyID)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).GetPropertyValue(propertyID).AsInt;
		}
		return 0;
	}

	public static int GetIntData(EcsWorld world, int entity, PropertyID propertyID, int defaultValue)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).GetPropertyValue(propertyID, defaultValue).AsInt;
		}
		return defaultValue;
	}

	public static float GetFloatData(EcsWorld world, int entity, PropertyID propertyID)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).GetPropertyValue(propertyID).AsFloat;
		}
		return 0f;
	}

	public static bool GetBoolData(EcsWorld world, int entity, PropertyID propertyID)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).GetPropertyValue(propertyID).AsInt == 1;
		}
		return false;
	}
}

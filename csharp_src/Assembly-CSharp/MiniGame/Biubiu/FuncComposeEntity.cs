using System;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public static class FuncComposeEntity
{
	public static void AddComposeEntities(EcsWorld world, int entity)
	{
		EcsPool<ComponentComposeEntityRoot> pool = world.GetPool<ComponentComposeEntityRoot>();
		if (pool.Has(entity))
		{
			ref ComponentPosition orAdd = ref world.GetPool<ComponentPosition>().GetOrAdd(entity);
			orAdd.Position = FVector2.Zero;
			ref ComponentRotation orAdd2 = ref world.GetPool<ComponentRotation>().GetOrAdd(entity);
			orAdd2.Rotation = FP.Zero;
			ref ComponentComposeEntityRoot compose = ref pool.Get(entity);
			InitComposeEntities(world, ref compose, entity);
			SynComposeEntitiesPosition(world, ref compose, orAdd.Position);
			SynComposeEntitiesRotation(world, ref compose, orAdd2.Rotation);
		}
	}

	public static void DelComposeEntities(EcsWorld world, int entity)
	{
		EcsPool<ComponentComposeEntityRoot> pool = world.GetPool<ComponentComposeEntityRoot>();
		if (!pool.Has(entity))
		{
			return;
		}
		EcsPool<ComponentComposeEntity> pool2 = world.GetPool<ComponentComposeEntity>();
		ref ComponentComposeEntityRoot reference = ref pool.Get(entity);
		for (int i = 0; i < reference.Entities.Length; i++)
		{
			ref ComponentComposeEntityRoot.Item reference2 = ref reference.Entities[i];
			if (reference2.ID >= 0 && world.IsEntityAliveInternal(reference2.ID) && pool2.Has(reference2.ID) && pool2.Get(reference2.ID).Root == entity)
			{
				world.DelEntity(reference2.ID);
			}
		}
	}

	public static void InitComposeEntities(EcsWorld world, ref ComponentComposeEntityRoot compose, int entity = -1)
	{
		IResourceLoader resourceLoader = world.GetShared<IGameSharedEnv>().ResourceLoader;
		EcsPool<ComponentComposeEntity> pool = world.GetPool<ComponentComposeEntity>();
		for (int i = 0; i < compose.Entities.Length; i++)
		{
			ref ComponentComposeEntityRoot.Item reference = ref compose.Entities[i];
			IResourceHolder resourceHolder = resourceLoader.LoadAsset<EcsEntitySnapshot>(reference.Template);
			if (resourceHolder == null)
			{
				compose.Entities[i].ID = -1;
				throw new Exception("Cannot init compose entities with template name: " + reference.Template);
			}
			EcsEntitySnapshot snapshot = resourceHolder.Data as EcsEntitySnapshot;
			int num = GameEntityTemplate.NewEntity(world, snapshot);
			resourceHolder.Dispose();
			reference.ID = num;
			if (entity >= 0)
			{
				pool.Add(num).Root = entity;
			}
		}
	}

	public static void SynComposeEntitiesPosition(EcsWorld world, ref ComponentComposeEntityRoot compose, FVector2 position)
	{
		EcsPool<ComponentPosition> pool = world.GetPool<ComponentPosition>();
		EcsPool<ComponentPhysics> pool2 = world.GetPool<ComponentPhysics>();
		for (int i = 0; i < compose.Entities.Length; i++)
		{
			ref ComponentComposeEntityRoot.Item reference = ref compose.Entities[i];
			if (reference.ID >= 0)
			{
				if (world.IsEntityAliveInternal(reference.ID) && pool.Has(reference.ID))
				{
					pool.Get(reference.ID).Position = position + reference.Position;
				}
				if (world.IsEntityAliveInternal(reference.ID) && pool2.Has(reference.ID))
				{
					ref ComponentPhysics reference2 = ref pool2.Get(reference.ID);
					reference2.Body.SetTransform(position + reference.Position, reference2.Body.GetAngle());
				}
			}
		}
	}

	public static void SynComposeEntitiesRotation(EcsWorld world, ref ComponentComposeEntityRoot compose, FP rotation)
	{
		EcsPool<ComponentRotation> pool = world.GetPool<ComponentRotation>();
		EcsPool<ComponentPhysics> pool2 = world.GetPool<ComponentPhysics>();
		for (int i = 0; i < compose.Entities.Length; i++)
		{
			ref ComponentComposeEntityRoot.Item reference = ref compose.Entities[i];
			if (reference.ID >= 0)
			{
				if (pool.Has(reference.ID))
				{
					pool.Get(reference.ID).Rotation = rotation + reference.Angle;
				}
				if (pool2.Has(reference.ID))
				{
					ref ComponentPhysics reference2 = ref pool2.Get(reference.ID);
					reference2.Body.SetTransform(reference2.Body.GetPosition(), reference.Angle * FP.Deg2Rad);
				}
			}
		}
	}

	public static FP GetAngleByComponentEntityRoot(EcsWorld world, int entity)
	{
		EcsPool<ComponentComposeEntity> pool = world.GetPool<ComponentComposeEntity>();
		if (!pool.Has(entity))
		{
			return FP.Zero;
		}
		int root = pool.Get(entity).Root;
		EcsPool<ComponentComposeEntityRoot> pool2 = world.GetPool<ComponentComposeEntityRoot>();
		if (!pool2.Has(root))
		{
			return FP.Zero;
		}
		ComponentComposeEntityRoot.Item[] entities = pool2.Get(root).Entities;
		for (int i = 0; i < entities.Length; i++)
		{
			if (entities[i].ID == entity)
			{
				return entities[i].Angle;
			}
		}
		return FP.Zero;
	}

	public static ComponentComposeEntityRoot.Item GetEntitiesByComponentEntityRoot(EcsWorld world, int entity, int index, out bool hasValue)
	{
		EcsPool<ComponentComposeEntity> pool = world.GetPool<ComponentComposeEntity>();
		if (!pool.Has(entity))
		{
			hasValue = false;
			return default(ComponentComposeEntityRoot.Item);
		}
		int root = pool.Get(entity).Root;
		EcsPool<ComponentComposeEntityRoot> pool2 = world.GetPool<ComponentComposeEntityRoot>();
		if (!pool2.Has(root))
		{
			hasValue = false;
			return default(ComponentComposeEntityRoot.Item);
		}
		ComponentComposeEntityRoot.Item[] entities = pool2.Get(root).Entities;
		if (index > entities.Length - 1)
		{
			hasValue = false;
			return default(ComponentComposeEntityRoot.Item);
		}
		hasValue = true;
		return entities[index];
	}
}

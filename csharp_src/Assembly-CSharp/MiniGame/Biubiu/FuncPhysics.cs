using System;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public static class FuncPhysics
{
	public static void SetUpDataBodyLogic(Body body, EcsWorld world, int entity)
	{
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		IBodyLogic bodyLogic = (IBodyLogic)body.UserData;
		if (pool.Has(entity))
		{
			body.SetGravityScale(world.GetPool<ComponentData>().Get(entity).GetPropertyValue(PropertyID.Gravity));
			FuncUniqueID.TryGetUniqueIDByEntity(world, entity, out var id);
			bodyLogic.ID = id;
		}
		bodyLogic.EntityId = world.PackEntity(entity);
	}

	public static void SetPosition(ref ComponentPhysics c, ref ComponentPosition pos)
	{
		FP angle = c.Body.GetAngle();
		c.Body.SetTransform(in pos.Position, angle);
	}

	public static bool HasGravity(EcsWorld world, int entity)
	{
		if (world.GetPool<ComponentData>().Has(entity))
		{
			return world.GetPool<ComponentData>().Get(entity).GetPropertyValue(PropertyID.Gravity) > FP.EN2;
		}
		return true;
	}

	public static void SetBodyOwner(Body body, int owner)
	{
		if (body.UserData is IBodyLogic bodyLogic)
		{
			bodyLogic.OwnerID = owner;
		}
	}

	public static void ColliderLayerInclude(Body body, S5Game.S5GameColliderLayer layer)
	{
		foreach (Fixture fixture in body.FixtureList)
		{
			Filter filter = fixture.Filter;
			filter.MaskBits |= (ushort)layer;
			fixture.Filter = filter;
		}
	}

	public static void ColliderLayerExclude(Body body, S5Game.S5GameColliderLayer layer)
	{
		foreach (Fixture fixture in body.FixtureList)
		{
			Filter filter = fixture.Filter;
			filter.MaskBits &= (ushort)(~layer);
			fixture.Filter = filter;
		}
	}

	public static void ColliderLayerIntersection(Body body, S5Game.S5GameColliderLayer layer)
	{
		foreach (Fixture fixture in body.FixtureList)
		{
			Filter filter = fixture.Filter;
			filter.MaskBits &= (ushort)layer;
			fixture.Filter = filter;
		}
	}

	public static void CreateBulletBody(S5Game s5Game, int entity, int ownerID, EcsPool<ComponentPhysics> pool, FP radius, FVector2 position, FVector2 velocity)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.FireBullet(ownerID, (float)radius, position, velocity);
		reference.Game = s5Game;
	}

	public static void CreatePvpBulletBody(S5Game s5Game, int entity, int ownerID, EcsPool<ComponentPhysics> pool, FP radius, FVector2 position, FVector2 velocity, FP baseRadius, FVector2 basePos)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.FirePvpBullet(ownerID, (float)radius, position, velocity, baseRadius, basePos);
		reference.Game = s5Game;
	}

	public static void CreateWallBody(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2[][] vector2)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreatWall(vector2, BodyType.StaticBody);
		reference.Game = s5Game;
	}

	public static void CreateBoxBody(S5Game s5Game, int entity, bool controller, EcsPool<ComponentPhysics> pool, Shape headShape, Shape bodyShape)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		if (controller)
		{
			reference.Body = s5Game.CreatePlayer(entity, headShape, bodyShape, BodyType.DynamicBody);
		}
		else
		{
			reference.Body = s5Game.CreateEnemy(entity, headShape, bodyShape, BodyType.DynamicBody);
		}
		reference.Game = s5Game;
	}

	public static void CreateObstacle(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2 center, FVector2 size, FVector2 position, BodyType bodyType)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateObstacle(center, size, position, bodyType);
		reference.Game = s5Game;
	}

	public static void CreateStaticObstacle(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2[] points, BodyType bodyType)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateStaticObstacle(points, bodyType, FVector2.Zero);
		reference.Game = s5Game;
	}

	public static void CreateStaticObstacle(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2 center, FVector2 size, FVector2 position, BodyType bodyType)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateStaticObstacle(center, size, position, bodyType);
		reference.Game = s5Game;
	}

	public static void CreateWoodBarrel(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2 center, FVector2 size, FVector2 position)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateWoodBarrel(center, size, position, BodyType.DynamicBody);
		reference.Game = s5Game;
	}

	public static void CreateBomb(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2 center, FVector2 size, FVector2 position)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateBomb(center, size, position, BodyType.DynamicBody);
		reference.Game = s5Game;
	}

	public static void CreateBombNew(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2[] points)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateBombNew(points, BodyType.DynamicBody);
		reference.Game = s5Game;
	}

	public static void CreateToggle(S5Game s5Game, int entity, EcsPool<ComponentPhysics> pool, FVector2 center, FVector2 size, FP angle, FVector2 position)
	{
		ref ComponentPhysics reference = ref pool.Add(entity);
		reference.Body = s5Game.CreateToggle(center, size, position, angle, BodyType.StaticBody);
		reference.Game = s5Game;
	}

	public static bool HasPhysicsWorld(SharedRuntime shared, EcsWorld world)
	{
		int entity;
		return shared.PhysicsWorld.Unpack(world, out entity);
	}

	public static ref ComponentPhysicsWorld GetPhysicsWorld(SharedRuntime shared, EcsWorld world)
	{
		shared.PhysicsWorld.Unpack(world, out var entity);
		return ref world.GetPool<ComponentPhysicsWorld>().Get(entity);
	}

	public static ref ComponentPhysicsWorld GetPhysicsWorld(SharedRuntime shared, EcsPool<ComponentPhysicsWorld> pool)
	{
		EcsWorld world = pool.GetWorld();
		if (!shared.PhysicsWorld.Unpack(world, out var entity))
		{
			throw new Exception("PhysicsWorld not found");
		}
		return ref pool.Get(entity);
	}

	public static FVector2 GetLinearVelocity(this ref ComponentPhysics physics)
	{
		return new FVector2(physics.Body.LinearVelocity.X.AsFloat, physics.Body.LinearVelocity.Y.AsFloat);
	}

	public static FP GetAngularVelocity(this ref ComponentPhysics physics)
	{
		return default(FP);
	}
}

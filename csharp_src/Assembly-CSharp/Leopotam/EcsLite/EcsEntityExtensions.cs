using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public static class EcsEntityExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EcsPackedEntity PackEntity(this EcsWorld world, int entity)
	{
		EcsPackedEntity result = default(EcsPackedEntity);
		result.Id = entity;
		result.Gen = world.GetEntityGen(entity);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Unpack(this in EcsPackedEntity packed, EcsWorld world, out int entity)
	{
		entity = packed.Id;
		if (world != null && world.IsAlive() && world.IsEntityAliveInternal(packed.Id))
		{
			return world.GetEntityGen(packed.Id) == packed.Gen;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsValid(this in EcsPackedEntity packed, EcsWorld world)
	{
		if (world != null && world.IsAlive() && world.IsEntityAliveInternal(packed.Id))
		{
			return world.GetEntityGen(packed.Id) == packed.Gen;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsTo(this in EcsPackedEntity a, in EcsPackedEntity b)
	{
		if (a.Id == b.Id)
		{
			return a.Gen == b.Gen;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EcsPackedEntityWithWorld PackEntityWithWorld(this EcsWorld world, int entity)
	{
		EcsPackedEntityWithWorld result = default(EcsPackedEntityWithWorld);
		result.World = world;
		result.Id = entity;
		result.Gen = world.GetEntityGen(entity);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Unpack(this in EcsPackedEntityWithWorld packedEntity, out EcsWorld world, out int entity)
	{
		world = packedEntity.World;
		entity = packedEntity.Id;
		if (world != null && world.IsAlive() && world.IsEntityAliveInternal(packedEntity.Id))
		{
			return world.GetEntityGen(packedEntity.Id) == packedEntity.Gen;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsTo(this in EcsPackedEntityWithWorld a, in EcsPackedEntityWithWorld b)
	{
		if (a.Id == b.Id && a.Gen == b.Gen)
		{
			return a.World == b.World;
		}
		return false;
	}
}

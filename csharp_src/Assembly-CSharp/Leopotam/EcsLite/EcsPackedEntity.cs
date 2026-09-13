using System.Runtime.CompilerServices;

namespace Leopotam.EcsLite;

public struct EcsPackedEntity
{
	public int Id;

	public int Gen;

	public static readonly EcsPackedEntity Invalid = new EcsPackedEntity
	{
		Id = 0,
		Gen = 0
	};

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(EcsPackedEntity a, EcsPackedEntity b)
	{
		if (a.Id == b.Id)
		{
			return a.Gen == b.Gen;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(EcsPackedEntity a, EcsPackedEntity b)
	{
		if (a.Id == b.Id)
		{
			return a.Gen != b.Gen;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		if (obj is EcsPackedEntity ecsPackedEntity)
		{
			return this == ecsPackedEntity;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (713 + Id) * 31 + Gen;
	}
}

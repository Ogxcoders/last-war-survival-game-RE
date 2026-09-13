namespace Leopotam.EcsLite;

public struct EcsPackedEntityWithWorld
{
	public int Id;

	public int Gen;

	public EcsWorld World;

	public override int GetHashCode()
	{
		return ((713 + Id) * 31 + Gen) * 31 + (World?.GetHashCode() ?? 0);
	}
}

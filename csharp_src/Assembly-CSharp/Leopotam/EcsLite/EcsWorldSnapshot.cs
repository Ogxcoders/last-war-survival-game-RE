using System;

namespace Leopotam.EcsLite;

public class EcsWorldSnapshot : IDisposable
{
	public int EntitiesItemSize;

	public int EntitiesCapacity;

	public int EntitiesCount;

	public int[] RecycledEntities;

	public int RecycledEntitiesCount;

	public int RecycledEntitiesCapacity;

	public EcsFilterSnapshot[] AllFilters;

	public EcsEntitySnapshot[] AllEntities;

	public Type[] AllPools;

	public int PoolCount;

	public void Dispose()
	{
		for (int i = 0; i < AllEntities.Length; i++)
		{
			EcsEntitySnapshot ecsEntitySnapshot = AllEntities[i];
			if (ecsEntitySnapshot.Components == null)
			{
				continue;
			}
			for (int j = 0; j < ecsEntitySnapshot.Components.Length; j++)
			{
				if (ecsEntitySnapshot.Components[j].Data is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		RecycledEntities = null;
		AllFilters = null;
		AllEntities = null;
		AllPools = null;
	}
}

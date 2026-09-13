using System;

namespace Leopotam.EcsLite;

public class EcsAliveEntitiesSnapshot : IDisposable
{
	public int EntitiesItemSize;

	public int EntitiesCapacity;

	public int EntitiesCount;

	public int RecycledEntitiesCount;

	public int RecycledEntitiesCapacity;

	public EcsEntitySnapshot[] AllEntities;

	public int PoolCount;

	public virtual void Dispose()
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
		AllEntities = null;
	}
}

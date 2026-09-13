using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public static class ComponentControllerClientExt
{
	public static DataUIPlayerController GetController(this ComponentControllerClient comp, EcsWorld world, int entity)
	{
		DataUIPlayerController dataUIPlayerController = comp.Controller as DataUIPlayerController;
		if (dataUIPlayerController == null && world.GetPool<ComponentPrefabClient>().Has(entity))
		{
			dataUIPlayerController = world.GetPool<ComponentPrefabClient>().Get(entity).GetPrefab()?.GetComponentInChildren<DataUIPlayerController>();
		}
		return dataUIPlayerController;
	}
}

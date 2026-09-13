using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public static class FuncUI
{
	public static ref MiniGame.Biubiu.ComponentUIClient GetClientUI(SharedRuntime shared, EcsWorld world)
	{
		shared.InitData.UIAdaptEntity.Unpack(world, out var entity);
		return ref world.GetPool<MiniGame.Biubiu.ComponentUIClient>().Get(entity);
	}

	public static bool HasClientUI(SharedRuntime shared, EcsWorld world)
	{
		int entity;
		return shared.InitData.UIAdaptEntity.Unpack(world, out entity);
	}

	public static void FireRender(IRender render, EcsWorld world)
	{
		GetClientUI(world.GetShared<SharedRuntime>(), world).GetUIAdapt().FireRender(render, world);
	}
}

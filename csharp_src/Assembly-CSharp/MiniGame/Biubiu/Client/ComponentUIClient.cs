using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public static class ComponentUIClient
{
	public static DataUIAdapt GetUIAdapt(this MiniGame.Biubiu.ComponentUIClient c)
	{
		if (c.UIHolder == null)
		{
			return null;
		}
		return (DataUIAdapt)((UIAdaptHolder)c.UIHolder).Data;
	}

	public static UIAdaptHolder GetUIHolder(this MiniGame.Biubiu.ComponentUIClient c)
	{
		return (UIAdaptHolder)c.UIHolder;
	}

	public static void InitComponent(this ref MiniGame.Biubiu.ComponentUIClient c, EcsWorld world)
	{
		SharedRuntime shared = world.GetShared<SharedRuntime>();
		c.UIHolder = shared.ResourceLoader.LoadAsset<DataUIAdapt>(null);
		c.GetUIAdapt().BindEnv(world);
	}
}

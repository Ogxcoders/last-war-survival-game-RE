using System;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public class ComponentPrefabClientDelegate : TEcsPoolDelegate<ComponentPrefabClient>, IEcsPoolDelegate, IEcsAutoReset<ComponentPrefabClient>, IEcsAutoCopy<ComponentPrefabClient>, IEcsAutoSnapshot<ComponentPrefabClient>
{
	public Type DelegateType => typeof(ComponentPrefabClient);

	public void AutoReset(ref ComponentPrefabClient c, EcsWorld world, int entity)
	{
		c.Prefab = null;
		if (c.ResourceHolder != null)
		{
			c.ResourceHolder.Dispose();
			c.ResourceHolder = null;
		}
	}

	public void AutoCopy(ref ComponentPrefabClient src, ref ComponentPrefabClient dst)
	{
		throw new NotImplementedException();
	}

	public object TakeSnapshot(ref ComponentPrefabClient c, EcsWorld world, int entity, object env)
	{
		string asset = world.GetPool<ComponentResource>().Get(entity).Asset;
		return new ComponentPrefabSnapshot
		{
			Prefab = asset
		};
	}

	public void RestoreSnapshot(ref ComponentPrefabClient c, EcsWorld world, int entity, object data, object env)
	{
		if (c.ResourceHolder != null)
		{
			c.ResourceHolder.Dispose();
			c.ResourceHolder = null;
		}
		ComponentPrefabSnapshot componentPrefabSnapshot = data as ComponentPrefabSnapshot;
		GameBiubiuEnvClient gameBiubiuEnvClient = env as GameBiubiuEnvClient;
		c.ResourceHolder = gameBiubiuEnvClient.ResourceLoader.LoadAssetAsync<GameObjectHolder>(componentPrefabSnapshot.Prefab);
		(c.ResourceHolder as GameObjectHolder)?.WithWorld(world.PackEntityWithWorld(entity));
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}

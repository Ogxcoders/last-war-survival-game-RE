using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu.Client;

public class SystemClientPrefab : IEcsRunSystem, IEcsSystem
{
	protected readonly EcsSharedInject<SharedRuntime> _shared;

	protected readonly EcsWorldInject _world;

	protected readonly EcsFilterInject<Inc<ComponentResource, ComponentPosition>, Exc<ComponentPrefabClient>> _filterAdd;

	protected readonly EcsFilterInject<Inc<ComponentPrefabClient>, Exc<ComponentResource>> _filterDel;

	protected readonly EcsPoolInject<ComponentPrefabClient> _poolPrefab;

	protected readonly EcsPoolInject<ComponentResource> _poolRes;

	protected readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	private string CheckAsset(int entity, string res)
	{
		if (_world.Value.GetPool<ComponentBullet>().Has(entity) && _poolPhysics.Value.Has(entity) && _poolPhysics.Value.Get(entity).Body.UserData is IBodyLogic bodyLogic)
		{
			EcsPool<ComponentPlayer> pool = _world.Value.GetPool<ComponentPlayer>();
			if (_world.Value.IsEntityAliveInternal(bodyLogic.OwnerID) && pool.Has(bodyLogic.OwnerID) && pool.Get(bodyLogic.OwnerID).PlayerID != _shared.Value.InitData.PlayerID)
			{
				return res.Replace(".prefab", "_Other.prefab");
			}
		}
		return res;
	}

	public virtual void Run(IEcsSystems systems)
	{
		RunDel(systems);
		RunAdd(systems);
	}

	protected void RunDel(IEcsSystems systems)
	{
		foreach (int item in _filterDel.Value)
		{
			_poolPrefab.Value.Del(item);
		}
	}

	protected void RunAdd(IEcsSystems systems)
	{
		foreach (int item in _filterAdd.Value)
		{
			string name = CheckAsset(item, _poolRes.Value.Get(item).Asset);
			if (_shared.Value.ResourceLoader.LoadAssetAsync<GameObjectHolder>(name) is GameObjectHolder gameObjectHolder)
			{
				gameObjectHolder.WithWorld(_world.Value.PackEntityWithWorld(item));
				_poolPrefab.Value.Add(item).ResourceHolder = gameObjectHolder;
			}
		}
	}
}

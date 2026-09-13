using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemPlayerInit : IEcsInitSystem, IEcsSystem
{
	private readonly EcsFilterInject<Inc<ComponentEnemy>> _filterEnemy;

	private readonly EcsFilterInject<Inc<ComponentPlayer>> _filterPlayer;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsWorldInject _world;

	private readonly EcsPoolInject<ComponentPlayer> _poolPlayer;

	private readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	private readonly EcsPoolInject<ComponentControllerClient> _poolController;

	public void Init(IEcsSystems systems)
	{
		EPlayerID playerID = _shared.Value.InitData.PlayerID;
		_shared.Value.ControllerEntity.Id = -1;
		_shared.Value.ControllerEntity.Gen = -1;
		foreach (int item in _filterPlayer.Value)
		{
			ref ComponentPlayer reference = ref _poolPlayer.Value.Get(item);
			ref ComponentPhysics reference2 = ref _poolPhysics.Value.Get(item);
			FuncPhysics.ColliderLayerInclude(reference2.Body, S5Game.S5GameColliderLayer.Bullet);
			FuncPhysics.SetBodyOwner(reference2.Body, item);
			InitPlayer(item, reference.PlayerID == playerID);
		}
	}

	private void InitPlayer(int entity, bool controller = false)
	{
		_poolController.Value.Add(entity);
		if (controller)
		{
			_shared.Value.ControllerEntity = _world.Value.PackEntity(entity);
		}
	}
}

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu.Client;

public class SystemClientPlayerInit : IEcsRunSystem, IEcsSystem
{
	private readonly EcsFilterInject<Inc<ComponentPlayer, ComponentPrefabClient, ComponentControllerClient>> _filterPlayer;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsWorldInject _world;

	private readonly EcsPoolInject<ComponentPlayer> _poolPlayer;

	private readonly EcsPoolInject<ComponentControllerClient> _poolController;

	public void Run(IEcsSystems systems)
	{
		EPlayerID playerID = _shared.Value.InitData.PlayerID;
		foreach (int item in _filterPlayer.Value)
		{
			ref ComponentPlayer reference = ref _poolPlayer.Value.Get(item);
			DataUIPlayerController controller = _poolController.Value.Get(item).GetController(_world.Value, item);
			DataUIPlayerController dataUIPlayerController;
			if (controller != null && (object)(dataUIPlayerController = controller) != null && !dataUIPlayerController.Inited)
			{
				dataUIPlayerController.Init(reference.PlayerID == playerID, reference.PlayerID);
				FuncUI.FireRender(new DataUIRenderMessage.UIPlayerBind
				{
					Controller = dataUIPlayerController
				}, _world.Value);
			}
		}
	}
}

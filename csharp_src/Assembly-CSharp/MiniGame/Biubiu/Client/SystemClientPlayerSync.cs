using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SystemClientPlayerSync : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsPoolInject<ComponentControllerClient> _playerControllerPool;

	private readonly EcsPoolInject<ComponentPlayer> _playerPool;

	private readonly EcsFilterInject<Inc<ComponentPlayer, ComponentControllerClient>> _injectPlayer;

	private static readonly float HighFrequency = 0.3f;

	private static readonly float LowFrequency = 1f;

	private static readonly float IdleFrequency = -1f;

	private static readonly float HighThreshold = 1f;

	private static readonly float LowThreshold = 0.6f;

	private static readonly float SyncTime = 1f;

	private bool _syncIng;

	private float _curSyncFrequency;

	private float _syncTime = 0.9f;

	private float _syncFrequency;

	private float EvaluateSyncFrequency(float delta)
	{
		if (delta > HighThreshold)
		{
			return HighFrequency;
		}
		if (delta >= LowThreshold && delta <= HighThreshold)
		{
			return LowFrequency;
		}
		return IdleFrequency;
	}

	private void DoSync(GameBiuBiuPlayerBase player, DataUIPlayerController controller)
	{
		Vector3 gunAimPosition = controller.GunAimPosition;
		GameBiubiuFrameSyncResp.ViewGunAim message = new GameBiubiuFrameSyncResp.ViewGunAim
		{
			PlayerID = (int)player.PlayerID,
			X = gunAimPosition.x,
			Y = gunAimPosition.y,
			Z = gunAimPosition.z
		};
		player.Send(message);
	}

	public void Run(IEcsSystems systems)
	{
		GameLoader gameLoader = _shared.Value.ResourceLoader as GameLoader;
		if (_shared.Value.GameType == EGameType.PvpClient && FuncEntity.TryGetControllerEntity(_world.Value, out var entity))
		{
			DataUIPlayerController controller = _playerControllerPool.Value.Get(entity).GetController(_world.Value, entity);
			if (!(controller == null))
			{
				GameBiuBiuPlayerBase player = (gameLoader.LoaderEnv as GameBiubiuPlayerEnv).Player;
				RunSync(player, controller);
				RunSyncRender(entity);
			}
		}
	}

	private void RunSync(GameBiuBiuPlayerBase player, DataUIPlayerController controller)
	{
		if (_shared.Value.GameState != EGameWorldState.Running)
		{
			return;
		}
		if (_syncTime < 0f)
		{
			_syncFrequency = EvaluateSyncFrequency(controller.TargetChangeDt);
			if (_syncFrequency > 0f)
			{
				_curSyncFrequency = _syncFrequency;
				_syncTime = 1f;
				_syncIng = true;
				DoSync(player, controller);
			}
			else if (_syncIng)
			{
				_syncIng = false;
				DoSync(player, controller);
			}
		}
		_curSyncFrequency -= Time.deltaTime;
		_syncTime -= Time.deltaTime;
		if (_syncFrequency > 0f && _curSyncFrequency <= 0f)
		{
			_curSyncFrequency = _syncFrequency;
			DoSync(player, controller);
		}
	}

	private void RunSyncRender(int controllerEntity)
	{
		foreach (int item in _injectPlayer.Value)
		{
			if (item != controllerEntity)
			{
				DataUIPlayerController controller = _playerControllerPool.Value.Get(item).GetController(_world.Value, item);
				if (!(controller == null) && !((double)controller.GunAimPositionSyn.sqrMagnitude < 0.001))
				{
					Vector3 gunAimPositionSyn = controller.GunAimPositionSyn;
					gunAimPositionSyn += controller.transform.position;
					controller._target.position = Vector3.Lerp(controller._target.position, gunAimPositionSyn, 0.2f);
					int num = ((gunAimPositionSyn.x - controller.transform.position.x > 0f) ? 180 : 0);
					controller.transform.rotation = Quaternion.Euler(0f, num, 0f);
				}
			}
		}
	}
}

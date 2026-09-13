using Joker;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core.Server;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SystemClientValidation : IEcsRunSystem, IEcsSystem
{
	private EcsWorldInject _world;

	private EcsSharedInject<GameBiubiuEnvClient> _shared;

	private const int kValidStateNone = 0;

	private const int kValidStateSend = 1;

	private const int kValidStateReceiveSucessed = 2;

	private const int kValidStateReceiveFailed = 3;

	public void Run(IEcsSystems systems)
	{
		GameBiuBiuPlayerBase player = (_shared.Value.ResourceLoader as GameLoader).LoaderEnv.Player;
		if (player == null)
		{
			return;
		}
		(IMessage, object) message;
		if (_shared.Value.ValidState == 0)
		{
			_shared.Value.ValidState = 1;
			GameBiubiuReplay replay = _shared.Value.GameResult.Replay;
			player.Send(new GameCheckValidation
			{
				Data = GameBiubiuShare.Serializer.ToJson(replay)
			});
		}
		else if (_shared.Value.ValidState == 1 && player.TryDequeueMessage(out message) && message.Item1 is S2CGameRoomVerify { Data: GameCheckValidationResp data })
		{
			if (data.Code == 0)
			{
				_shared.Value.ValidState = 2;
				Debug.Log($"校验成功{data.Code}");
			}
			else
			{
				_shared.Value.ValidState = 3;
				Debug.LogError($"校验失败{data.Code}");
			}
		}
	}
}

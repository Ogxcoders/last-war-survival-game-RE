using Box2DSharp.Common;
using Joker;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Biubiu.Client;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu;

public class SystemClientGame : IEcsRunSystem, IEcsSystem, IEcsInitSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private bool set;

	private float _waitClientOver;

	private readonly float WaitTime = 8f;

	public void Init(IEcsSystems systems)
	{
		_waitClientOver = WaitTime;
		if (_shared.Value.InitData.ReEnter)
		{
			_shared.Value.CanInputTime = _shared.Value.LogicTime;
		}
		else
		{
			_shared.Value.CanInputTime = _shared.Value.LogicTime + (FP)4f;
		}
	}

	public void Run(IEcsSystems systems)
	{
		switch (_shared.Value.GameType)
		{
		case EGameType.PvpClient:
			CheckPvpGameOver();
			break;
		case EGameType.PveClient:
			CheckPveGameOver();
			break;
		}
	}

	private void CheckPveGameOver()
	{
		if (_shared.Value.GameOver && !_shared.Value.ClientGameOver)
		{
			_shared.Value.GameOver = true;
			_shared.Value.ClientGameOver = true;
			_shared.Value.GameState = EGameWorldState.Settlement;
			FuncTCClient.DoSettlementAction(_world.Value);
		}
	}

	private void CheckPvpGameOver()
	{
		if (_shared.Value.GameOver)
		{
			return;
		}
		if (_shared.Value.ServerGameOver && !_shared.Value.ClientGameOver && _waitClientOver > 0f)
		{
			_waitClientOver -= Time.deltaTime;
			if (_waitClientOver < 0f)
			{
				FuncUI.FireRender(new DataUIRenderMessage.UINetWork
				{
					SuccOrFair = false
				}, _world.Value);
				Log.Error("[BiuBiu] ClientGameOver Fair Please Check");
				return;
			}
		}
		if (_shared.Value.ClientGameOver && _shared.Value.ServerGameOver)
		{
			_shared.Value.GameOver = true;
			_shared.Value.ClientGameOver = true;
			_shared.Value.GameState = EGameWorldState.Settlement;
			FuncTCClient.DoSettlementAction(_world.Value);
		}
	}
}

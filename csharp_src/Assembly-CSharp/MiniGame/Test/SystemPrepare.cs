using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using MiniGame.Test.Client;
using UnityEngine;

namespace MiniGame.Test;

public class SystemPrepare : IEcsRunSystem, IEcsSystem
{
	private EcsSharedInject<GameTestEnvClient> _share;

	public void Run(IEcsSystems systems)
	{
		GameTestEnvClient value = _share.Value;
		value.PrepareTime += (FP)Time.deltaTime;
		if (_share.Value.PrepareTime > 1)
		{
			_share.Value.GameState = EGameWorldState.Running;
		}
	}
}

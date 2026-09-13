using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Test;

public class SystemLogic : IEcsRunSystem, IEcsSystem
{
	private EcsSharedInject<GameTestEnv> _share;

	public void Run(IEcsSystems systems)
	{
		if (_share.Value.LogicTickCount > 200)
		{
			_share.Value.GameOver = true;
			_share.Value.GameState = EGameWorldState.Settlement;
		}
	}
}

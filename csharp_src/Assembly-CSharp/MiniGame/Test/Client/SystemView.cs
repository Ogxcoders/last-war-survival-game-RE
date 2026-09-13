using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Test.Client;

public class SystemView : IEcsRunSystem, IEcsSystem
{
	private EcsSharedInject<GameTestEnvClient> _share;

	public void Run(IEcsSystems systems)
	{
		((GameTestLoader)_share.Value.ResourceLoader).Runtime.ViewTickCount++;
	}
}

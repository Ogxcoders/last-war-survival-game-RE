using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu.Client;

public class SystemClientPhysicsSimulateDebug : IEcsRunSystem, IEcsSystem
{
	private readonly EcsSharedInject<GameBiubiuEnvClient> _shared;

	private readonly EcsPoolInject<ComponentPhysicsWorldSimulate> _pool;

	public void Run(IEcsSystems systems)
	{
	}
}

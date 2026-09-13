using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu.Client;

public class SystemPhysicsDrawDebug : IEcsInitSystem, IEcsSystem
{
	private readonly EcsSharedInject<GameBiubiuEnvClient> _shared;

	private readonly EcsPoolInject<ComponentPhysicsWorld> _pool;

	public void Init(IEcsSystems systems)
	{
	}
}

using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu.Client;

public class SystemClientMove : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsFilterInject<Inc<ComponentPosition, ComponentPrefabClient>> _filter;

	public void Run(IEcsSystems systems)
	{
		throw new NotImplementedException();
	}
}

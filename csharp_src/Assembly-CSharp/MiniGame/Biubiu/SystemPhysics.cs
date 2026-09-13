using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemPhysics : IEcsRunSystem, IEcsSystem, IEcsInitSystem
{
	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsWorldInject _world;

	private readonly EcsFilterInject<Inc<ComponentPosition, ComponentPhysics>> _filter;

	private readonly EcsPoolInject<ComponentPhysicsWorld> _poolWorld;

	public void Init(IEcsSystems systems)
	{
		if (!FuncPhysics.HasPhysicsWorld(_shared.Value, _world.Value))
		{
			int entity = _world.Value.NewEntity();
			ref ComponentPhysicsWorld reference = ref _poolWorld.Value.Add(entity);
			reference.Game = new S5Game();
			reference.Game.Build(new S5GameSettings());
			reference.Game.BindTrigger(new Box2DTriggerAdapt(_world.Value, _shared.Value, reference.Game));
			_shared.Value.MapData.PhysicsWorld = _world.Value.PackEntity(entity);
		}
	}

	public void Run(IEcsSystems systems)
	{
		ref ComponentPhysicsWorld physicsWorld = ref FuncPhysics.GetPhysicsWorld(_shared.Value, _poolWorld.Value);
		physicsWorld.Game.Step(_shared.Value.PhysicsTickDelta);
		physicsWorld.PhysicsFrame++;
		ref FP physicsTime = ref physicsWorld.PhysicsTime;
		physicsTime += _shared.Value.PhysicsTickDelta;
		EcsPool<ComponentPosition> inc = _filter.Pools.Inc1;
		EcsPool<ComponentPhysics> inc2 = _filter.Pools.Inc2;
		foreach (int item in _filter.Value)
		{
			inc.Get(item).Position = inc2.Get(item).Body.GetPosition();
		}
	}
}

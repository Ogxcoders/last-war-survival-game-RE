using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Biubiu;

namespace MiniGame;

public class SystemEditorPhysics : IEcsInitSystem, IEcsSystem, IEcsRunSystem
{
	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsWorldInject _world;

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
			if (_shared.Value.MapData == null)
			{
				_shared.Value.MapData = new SharedMapData();
			}
			_shared.Value.MapData.PhysicsWorld = _world.Value.PackEntity(entity);
		}
	}

	public void Run(IEcsSystems systems)
	{
		FuncPhysics.GetPhysicsWorld(_shared.Value, _world.Value).Game.ClearBody();
	}
}

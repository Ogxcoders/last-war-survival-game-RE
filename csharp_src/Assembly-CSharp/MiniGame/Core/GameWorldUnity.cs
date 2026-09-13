using Leopotam.EcsLite;

namespace MiniGame.Core;

public class GameWorldUnity : GameWorld
{
	public GameWorldUnity(IGameSharedEnv env, IGameSerializer serializer)
		: base(env, serializer)
	{
	}

	public override SnapshotWorld TakeSnapshot()
	{
		return base.TakeSnapshot();
	}

	public override void Init()
	{
		base.Init();
	}

	protected override void Init(IEcsSystems systems)
	{
		base.Init(systems);
	}
}

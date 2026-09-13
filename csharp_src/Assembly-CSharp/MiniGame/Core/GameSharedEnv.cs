using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public abstract class GameSharedEnv : IGameSharedEnv, IGameUniqueIDRegister, ISnapshot
{
	public EGameWorldState GameState { get; set; }

	public bool IsPaused { get; set; }

	public bool GameOver { get; set; }

	public bool FrameSyncIsNeeded { get; set; } = true;

	public int FrameSyncTickInterval { get; set; } = 30;

	public int WaitToStartTime { get; } = 3;

	public FP FrameSyncLerpFactor { get; set; } = 0.6;

	public FP PhysicsTickDelta { get; set; } = 0.033333f;

	public FP LogicTickDelta { get; set; } = 0.033333f;

	public FP PrepareTime { get; set; }

	public FP GameTime { get; set; }

	public FP LogicTime { get; set; }

	public FP SettlementTime { get; set; }

	public int LogicTickCount { get; set; }

	public int LogicTickLockStep { get; set; } = int.MaxValue;

	public int PhysicsTickCount { get; set; }

	public EcsPackedEntity UniqueIDManager { get; set; }

	public abstract IResourceLoader ResourceLoader { get; set; }

	public abstract object TakeSnapshot();

	public abstract void RestoreSnapshot(object snapshot);
}

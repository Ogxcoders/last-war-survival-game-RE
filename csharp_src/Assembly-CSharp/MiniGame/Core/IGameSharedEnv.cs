using Box2DSharp.Common;
using Newtonsoft.Json;

namespace MiniGame.Core;

public interface IGameSharedEnv : IGameUniqueIDRegister, ISnapshot
{
	EGameWorldState GameState { get; set; }

	bool IsPaused { get; set; }

	bool GameOver { get; set; }

	bool FrameSyncIsNeeded { get; }

	int FrameSyncTickInterval { get; }

	int WaitToStartTime { get; }

	FP FrameSyncLerpFactor { get; }

	FP PhysicsTickDelta { get; }

	FP LogicTickDelta { get; }

	FP PrepareTime { get; set; }

	FP GameTime { get; set; }

	FP LogicTime { get; set; }

	FP SettlementTime { get; set; }

	int LogicTickCount { get; set; }

	int LogicTickLockStep { get; set; }

	int PhysicsTickCount { get; set; }

	[JsonIgnore]
	IResourceLoader ResourceLoader { get; }
}

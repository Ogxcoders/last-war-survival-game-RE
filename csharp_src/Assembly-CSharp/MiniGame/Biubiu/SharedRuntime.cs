using System;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SharedRuntime : GameSharedEnv
{
	public bool ClientGameOver;

	public bool ServerGameOver;

	public bool CanCheckGameOver;

	public EGameType GameType;

	public SharedGameResult GameResult = new SharedGameResult();

	public SharedInitData InitData;

	public SharedMapData MapData = new SharedMapData();

	public EcsPackedEntity ControllerEntity;

	public EcsPackedEntity EventManager;

	public static bool Dump;

	public FP CanInputTime { get; set; } = FP.MaxValue;

	public EcsPackedEntity PhysicsWorld => MapData?.PhysicsWorld ?? default(EcsPackedEntity);

	public SharedSnapshotType SharedSnapshotType { get; set; }

	public override IResourceLoader ResourceLoader { get; set; }

	public ServiceCommand Commands { get; set; }

	public override object TakeSnapshot()
	{
		if (SharedSnapshotType == SharedSnapshotType.Config)
		{
			return ResourceLoader.LoadConfig<SharedConfigData>(0);
		}
		if (SharedSnapshotType == SharedSnapshotType.Map)
		{
			return MapData;
		}
		if (SharedSnapshotType == SharedSnapshotType.Shared)
		{
			return new SharedRuntime
			{
				InitData = InitData,
				ControllerEntity = ControllerEntity,
				SharedSnapshotType = SharedSnapshotType,
				GameState = base.GameState,
				IsPaused = base.IsPaused,
				PhysicsTickDelta = base.PhysicsTickDelta,
				LogicTickDelta = base.LogicTickDelta,
				Commands = (Commands.Clone() as ServiceCommand)
			};
		}
		throw new Exception($"Not Support TakeSnapshot Type {SharedSnapshotType}");
	}

	public override void RestoreSnapshot(object snapshot)
	{
		if (snapshot == null)
		{
			return;
		}
		if (snapshot is SharedRuntime sharedRuntime)
		{
			InitData = sharedRuntime.InitData;
			SharedSnapshotType = sharedRuntime.SharedSnapshotType;
			base.PhysicsTickDelta = sharedRuntime.PhysicsTickDelta;
			base.LogicTickDelta = sharedRuntime.LogicTickDelta;
			Commands = sharedRuntime.Commands;
		}
		else
		{
			if (!(snapshot is SharedMapData mapData))
			{
				throw new Exception($"Not Support TakeSnapshot Type {SharedSnapshotType}");
			}
			MapData = mapData;
		}
	}
}

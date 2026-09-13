using System;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Core;

public class GameWorld : IDisposable
{
	internal EcsWorld _world;

	private GameSystemUpdator _logicUpdater;

	private GameSystemUpdator _physicsUpdater;

	public IEcsSystems PrepareSystems { get; private set; }

	public IEcsSystems LogicSystems { get; private set; }

	public IEcsSystems PhysicsSystems { get; private set; }

	public IEcsSystems ViewSystems { get; private set; }

	public IEcsSystems SettlementSystems { get; private set; }

	public IGameSerializer Serializer { get; private set; }

	public IGameSharedEnv Env { get; private set; }

	public EcsWorld World => _world;

	public EGameWorldState State => Env.GameState;

	public bool IsPaused => Env.IsPaused;

	private FP tickMinDelta
	{
		get
		{
			if (!(Env.LogicTickDelta < Env.PhysicsTickDelta))
			{
				return Env.PhysicsTickDelta;
			}
			return Env.LogicTickDelta;
		}
	}

	public GameWorld(IGameSharedEnv env, IGameSerializer serializer, IEcsDebugger debugger = null)
	{
		Serializer = serializer;
		_world = new EcsWorld(env, Serializer.GetPoolDelegates(), debugger, default(EcsWorld.Config));
		Env = env;
		PrepareSystems = new EcsSystems(_world);
		LogicSystems = new EcsSystems(_world);
		PhysicsSystems = new EcsSystems(_world);
		ViewSystems = new EcsSystems(_world);
		SettlementSystems = new EcsSystems(_world);
		_logicUpdater = new GameSystemUpdator(env.LogicTickDelta, UpdateLogics);
		_physicsUpdater = new GameSystemUpdator(env.PhysicsTickDelta, UpdatePhysics);
	}

	public virtual void Init()
	{
		Init(PrepareSystems);
		Init(LogicSystems);
		Init(PhysicsSystems);
		Init(ViewSystems);
		Init(SettlementSystems);
		Env.GameState = EGameWorldState.Initialized;
	}

	public virtual void Update(float deltaTime)
	{
		if (IsPaused)
		{
			ViewSystems.Run();
		}
		else if (Env.GameState == EGameWorldState.Initialized)
		{
			Env.GameState = EGameWorldState.Preparing;
		}
		else if (Env.GameState == EGameWorldState.Preparing)
		{
			if (PrepareSystems.GetAllSystems().Count > 0)
			{
				IGameSharedEnv env = Env;
				env.PrepareTime += (FP)deltaTime;
				PrepareSystems.Run();
				ViewSystems.Run();
			}
			else
			{
				Env.GameState = EGameWorldState.Running;
			}
		}
		else if (Env.GameState == EGameWorldState.Running)
		{
			FP x = AdjustLockedLogicTime(deltaTime);
			while (tickMinDelta > 0 && tickMinDelta <= x)
			{
				x -= tickMinDelta;
				if (!TickLogic(tickMinDelta))
				{
					ViewSystems?.Run();
					return;
				}
				if (Env.GameState != EGameWorldState.Running)
				{
					ViewSystems?.Run();
					return;
				}
			}
			TickLogic(x);
			ViewSystems?.Run();
		}
		else if (State == EGameWorldState.Settlement)
		{
			FP y = deltaTime;
			IGameSharedEnv env2 = Env;
			env2.GameTime += y;
			IGameSharedEnv env3 = Env;
			env3.SettlementTime += y;
			ViewSystems?.Run();
			SettlementSystems?.Run();
		}
	}

	private FP AdjustLockedLogicTime(FP dt)
	{
		if (Env.LogicTickCount >= Env.LogicTickLockStep || Env.LogicTickLockStep < 0 || Env.LogicTickLockStep == int.MaxValue)
		{
			return dt;
		}
		return ((FP)(Env.LogicTickLockStep - Env.LogicTickCount) * Env.LogicTickDelta + dt) * Env.FrameSyncLerpFactor;
	}

	private void UpdateLogics()
	{
		LogicSystems.Run();
		Env.LogicTickCount = _logicUpdater.TickCount;
	}

	private void UpdatePhysics()
	{
		PhysicsSystems.Run();
		Env.PhysicsTickCount = _physicsUpdater.TickCount;
	}

	public virtual void Dispose()
	{
		LogicSystems?.Destroy();
		PhysicsSystems?.Destroy();
		ViewSystems?.Destroy();
		SettlementSystems?.Destroy();
		LogicSystems = null;
		PhysicsSystems = null;
		ViewSystems = null;
		SettlementSystems = null;
		_logicUpdater = null;
		_physicsUpdater = null;
		_world?.Destroy();
		_world = null;
		Env.GameState = EGameWorldState.Disposed;
	}

	public virtual bool IsLogicTickLocked()
	{
		return Env.LogicTickCount >= Env.LogicTickLockStep;
	}

	protected virtual bool TickLogic(FP dt)
	{
		IGameSharedEnv env = Env;
		env.GameTime += dt;
		if (IsLogicTickLocked())
		{
			return false;
		}
		_physicsUpdater.Update(dt);
		_logicUpdater.Update(dt);
		Env.LogicTime = (FP)Env.LogicTickCount * Env.LogicTickDelta;
		return true;
	}

	public virtual bool Pause()
	{
		if (IsPaused)
		{
			return false;
		}
		Env.IsPaused = true;
		return true;
	}

	public virtual bool Resume()
	{
		if (!IsPaused)
		{
			return false;
		}
		Env.IsPaused = false;
		return true;
	}

	public virtual SnapshotWorld TakeSnapshot()
	{
		SnapshotWorld snapshotWorld = new SnapshotWorld();
		snapshotWorld.TakeSnapshot(this);
		return snapshotWorld;
	}

	public virtual void RestoreSnapshot(SnapshotWorld snapshot)
	{
		snapshot.RestoreSnapshot(this);
	}

	protected virtual void Init(IEcsSystems systems)
	{
		systems.Inject().Init();
	}
}

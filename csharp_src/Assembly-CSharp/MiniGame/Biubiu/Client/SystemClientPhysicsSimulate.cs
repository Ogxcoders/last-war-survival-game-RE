using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SystemClientPhysicsSimulate : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsFilterInject<Inc<ComponentPhysics>, Exc<ComponentPhysicsSimulate>> _filterInit;

	private readonly EcsFilterInject<Inc<ComponentPhysicsSimulate>, Exc<ComponentPhysics>> _filterDelete;

	private readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPhysicsSimulate>> _filterRefresh;

	private readonly EcsFilterInject<Inc<ComponentBullet, ComponentPhysicsSimulate>> _filterBullet;

	private readonly EcsFilterInject<Inc<ComponentVelocityTarget, ComponentPhysics, ComponentPhysicsSimulate>> _filterVelocityTarget;

	private readonly EcsPoolInject<ComponentPhysicsSimulate> _poolSimulate;

	private readonly EcsPoolInject<ComponentPhysicsWorldSimulate> _poolSimulateWorld;

	private readonly EcsPoolInject<ComponentPhysics> _poolLogic;

	private readonly EcsPoolInject<ComponentPhysicsWorld> _poolLogicWorld;

	private readonly EcsPoolInject<ComponentBullet> _poolBullet;

	public void Run(IEcsSystems systems)
	{
		InitPhysicsSimulate();
		DeletePhysicsSimulate();
		RefreshRestoredPhysics();
		CalcSimulateSpeedFactor();
		SyncLogicRunningTime();
		SyncLogicWorld();
		SimulateWorld();
	}

	private void SimulateWorld()
	{
		if (_shared.Value.PhysicsWorld.Unpack(_world.Value, out var entity))
		{
			ref ComponentPhysicsWorldSimulate reference = ref _poolSimulateWorld.Value.Get(entity);
			FP physicsTickDelta = _shared.Value.PhysicsTickDelta;
			while (reference.SimulateLogicTargetTime - reference.SimulateLogicTime >= physicsTickDelta)
			{
				SimulateOneFrame(ref reference);
			}
		}
	}

	private void CalcSimulateSpeedFactor()
	{
		FP x = FP.EN1;
		FP righ = 1.8f;
		FP righ2 = 0.1f;
		int num = 30;
		ref ComponentPhysicsWorldSimulate simulateWorld = ref GetSimulateWorld();
		int num2 = ((_shared.Value.LogicTickLockStep > 32767) ? _shared.Value.LogicTickCount : _shared.Value.LogicTickLockStep);
		int simulatePhysicsFrame = simulateWorld.SimulatePhysicsFrame;
		int num3 = num2 - simulatePhysicsFrame;
		if (num3 > 0)
		{
			ref FP simulateSpeedFactor = ref simulateWorld.SimulateSpeedFactor;
			simulateSpeedFactor += x * (FP)num3;
			simulateWorld.SimulateSpeedFactor = FP.Min(simulateWorld.SimulateSpeedFactor, righ);
		}
		else if (num3 < 0)
		{
			if (-num3 <= num)
			{
				simulateWorld.SimulateSpeedFactor = FP.One;
				return;
			}
			ref FP simulateSpeedFactor2 = ref simulateWorld.SimulateSpeedFactor;
			simulateSpeedFactor2 -= simulateWorld.SimulateSpeedFactor * (FP)(-num3 - num);
			simulateWorld.SimulateSpeedFactor = FP.Max(simulateWorld.SimulateSpeedFactor, righ2);
		}
		else
		{
			simulateWorld.SimulateSpeedFactor = FP.One;
		}
	}

	private void SyncLogicRunningTime()
	{
		ref ComponentPhysicsWorldSimulate simulateWorld = ref GetSimulateWorld();
		if (simulateWorld.SimulateLogicTargetTime == 0)
		{
			if (_shared.Value.LogicTime != 0)
			{
				simulateWorld.SimulateLogicTargetTime = _shared.Value.LogicTime;
			}
		}
		else
		{
			ref FP simulateLogicTargetTime = ref simulateWorld.SimulateLogicTargetTime;
			simulateLogicTargetTime += (FP)Time.deltaTime * simulateWorld.SimulateSpeedFactor;
		}
	}

	private void SyncLogicWorld()
	{
		ref ComponentPhysicsWorldSimulate simulateWorld = ref GetSimulateWorld();
		if (_shared.Value.LogicTickLockStep - simulateWorld.SimulatePhysicsFrame > 45)
		{
			_shared.Value.FrameSyncIsNeeded = false;
			SyncLogicPhysicsWorld();
			simulateWorld.SimulateLogicTargetTime = _shared.Value.LogicTime;
		}
		else if (_shared.Value.FrameSyncIsNeeded && _shared.Value.PhysicsTickCount <= simulateWorld.SimulatePhysicsFrame)
		{
			_shared.Value.FrameSyncIsNeeded = false;
			SyncLogicPhysicsWorld();
		}
	}

	private void SyncLogicPhysicsWorld()
	{
		ref ComponentPhysicsWorldSimulate simulateWorld = ref GetSimulateWorld();
		simulateWorld.SimulatePhysicsFrame = _shared.Value.PhysicsTickCount;
		simulateWorld.SimulateLogicTime = (FP)_shared.Value.PhysicsTickCount * _shared.Value.PhysicsTickDelta;
		foreach (int item in _filterRefresh.Value)
		{
			SyncComponentPhysicsSimulate(item);
		}
	}

	private void InitPhysicsSimulate()
	{
		ref ComponentPhysicsWorldSimulate simulateWorld = ref GetSimulateWorld();
		foreach (int item in _filterInit.Value)
		{
			InitPhysicsSimulateEntity(item, ref simulateWorld);
		}
	}

	private void DeletePhysicsSimulate()
	{
		foreach (int item in _filterDelete.Value)
		{
			_poolSimulate.Value.Del(item);
		}
	}

	private void RefreshRestoredPhysics()
	{
		EcsPool<ComponentPhysicsSimulate> value = _poolSimulate.Value;
		ref ComponentPhysicsWorldSimulate simulateWorld = ref GetSimulateWorld();
		foreach (int item in _filterRefresh.Value)
		{
			if (value.Get(item).Game == null)
			{
				InitPhysicsSimulateEntity(item, ref simulateWorld);
			}
		}
	}

	private void InitPhysicsSimulateEntity(int entity, ref ComponentPhysicsWorldSimulate simulateWorld)
	{
		ref ComponentPhysics reference = ref _poolLogic.Value.Get(entity);
		ref ComponentPhysicsSimulate orAdd = ref _poolSimulate.Value.GetOrAdd(entity);
		orAdd.Game = simulateWorld.Game;
		orAdd.Body = PhysicsSnapShot.CreateRestoreSnapshotBody(orAdd.Game.World);
		PhysicsSnapShot.ComponentPhysicsSnapshotData componentPhysicsSnapshotData = reference.Body.TakeSnapShot();
		if (componentPhysicsSnapshotData.BodyDefData.UserData is IBodyLogic bodyLogic)
		{
			componentPhysicsSnapshotData.BodyDefData.UserData = bodyLogic.Clone();
		}
		orAdd.Body.RestoreSnapshot(componentPhysicsSnapshotData);
		FuncPhysics.SetUpDataBodyLogic(orAdd.Body, _world.Value, entity);
		if (_poolBullet.Value.Has(entity))
		{
			orAdd.Body.SetTransform(reference.Body.GetPosition(), FP.Zero);
			orAdd.Body.SetLinearVelocity(reference.Body.LinearVelocity);
			if (orAdd.Body.UserData is IBodyLogic bodyLogic2)
			{
				bodyLogic2.ID = -1;
			}
			if (reference.Body.UserData is IBodyLogic bodyLogic3)
			{
				FuncPhysics.SetBodyOwner(orAdd.Body, bodyLogic3.OwnerID);
			}
		}
	}

	private void SyncComponentPhysicsSimulate(int entity)
	{
		Body body = _poolLogic.Value.Get(entity).Body;
		Body body2 = _poolSimulate.Value.Get(entity).Body;
		Box2DSharp.Common.Transform transform = body.GetTransform();
		body2.SetTransform(in transform.Position, transform.Rotation.Angle);
		body2.SetLinearVelocity(body.LinearVelocity);
		body2.SetAngularVelocity(body.AngularVelocity);
	}

	private ref ComponentPhysicsWorldSimulate GetSimulateWorld()
	{
		_shared.Value.PhysicsWorld.Unpack(_world.Value, out var entity);
		ref ComponentPhysicsWorldSimulate orAdd = ref _poolSimulateWorld.Value.GetOrAdd(entity);
		if (orAdd.Game == null)
		{
			ref ComponentPhysicsWorld reference = ref _poolLogicWorld.Value.Get(entity);
			orAdd.Game = new S5Game();
			orAdd.Game.Build(new S5GameSettings());
			orAdd.Game.BindTrigger(new ClientSimulateTrigger(_world.Value, _shared.Value, orAdd.Game));
			orAdd.SimulatePhysicsFrame = reference.PhysicsFrame;
			orAdd.SimulateLogicTargetTime = 0;
			orAdd.SimulateSpeedFactor = FP.One;
			orAdd.IsDebugShown = false;
		}
		return ref orAdd;
	}

	private void SimulateOneFrame(ref ComponentPhysicsWorldSimulate simulateWorld)
	{
		AdjustVelocityTarget(ref simulateWorld);
		simulateWorld.Game.Step(_shared.Value.PhysicsTickDelta);
		simulateWorld.SimulatePhysicsFrame++;
		ref FP simulateLogicTime = ref simulateWorld.SimulateLogicTime;
		simulateLogicTime += _shared.Value.PhysicsTickDelta;
		KeepBulletSpeed();
	}

	private void AdjustVelocityTarget(ref ComponentPhysicsWorldSimulate simulateWorld)
	{
		int simulatePhysicsFrame = simulateWorld.SimulatePhysicsFrame;
		EcsPool<ComponentVelocityTarget> inc = _filterVelocityTarget.Pools.Inc1;
		EcsPool<ComponentPhysics> inc2 = _filterVelocityTarget.Pools.Inc2;
		EcsPool<ComponentPhysicsSimulate> inc3 = _filterVelocityTarget.Pools.Inc3;
		foreach (int item in _filterVelocityTarget.Value)
		{
			if (inc.Get(item).TargetFrame > simulatePhysicsFrame)
			{
				continue;
			}
			inc3.Get(item).Body.SetLinearVelocity(FVector2.Zero);
			if (!(inc2.Get(item).Body.UserData is IPlatformLogic { OnPlatformIDs: not null } platformLogic))
			{
				continue;
			}
			for (int i = 0; i < platformLogic.OnPlatformIDs.Count; i++)
			{
				int id = platformLogic.OnPlatformIDs[i];
				if (FuncUniqueID.TryGetEntityByUniqueID(_world.Value, id, out var entity) && inc2.Has(entity))
				{
					inc3.Get(entity).Body.SetLinearVelocity(FVector2.Zero);
				}
			}
		}
	}

	private void KeepBulletSpeed()
	{
		EcsPool<ComponentBullet> inc = _filterBullet.Pools.Inc1;
		EcsPool<ComponentPhysicsSimulate> inc2 = _filterBullet.Pools.Inc2;
		EcsPool<ComponentPhysics> value = _poolLogic.Value;
		foreach (int item in _filterBullet.Value)
		{
			ref ComponentPhysicsSimulate reference = ref inc2.Get(item);
			ref ComponentBullet reference2 = ref inc.Get(item);
			value.Get(item);
			if (reference.Body != null)
			{
				FVector2 linearVelocity = reference.Body.LinearVelocity;
				FP fP = linearVelocity.LengthSquared() - reference2.Speed * reference2.Speed;
				if (fP > FP.EN2 || fP < -FP.EN2)
				{
					reference.Body.SetLinearVelocity(linearVelocity.normalized * reference2.Speed);
				}
			}
		}
	}
}

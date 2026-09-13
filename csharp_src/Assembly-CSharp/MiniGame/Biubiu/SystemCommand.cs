using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SystemCommand : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsPoolInject<ComponentPosition> _poolPos;

	private readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	private readonly EcsPoolInject<ComponentBullet> _poolBullet;

	private readonly EcsPoolInject<ComponentGunReload> _poolGunReload;

	private readonly EcsPoolInject<ComponentData> _poolData;

	private readonly EcsPoolInject<ComponentPlayer> _poolPlayer;

	private readonly EcsPoolInject<ComponentTriggers> _poolTriggers;

	public void Run(IEcsSystems systems)
	{
		RunCommand(systems);
		RunCanInput();
	}

	public void RunCanInput()
	{
		if (_shared.Value.GameType == EGameType.PvpServer)
		{
			SharedRuntime value = _shared.Value;
			value.CanInputTime -= _shared.Value.LogicTickDelta;
		}
	}

	private void RunCommand(IEcsSystems systems)
	{
		ServiceCommand commands = _shared.Value.Commands;
		int logicTickCount = _shared.Value.LogicTickCount;
		ISyncCommand command;
		while (commands.TryPickEvent(logicTickCount, out command))
		{
			_shared.Value.FrameSyncIsNeeded = true;
			if (command.TypeID == 0)
			{
				command.IsValid = DoCreateBullet(systems, (command as CommandCreateBullet?) ?? default(CommandCreateBullet));
				continue;
			}
			throw new ArgumentException($"Unknown command type: {command.TypeID}");
		}
	}

	private bool DoCreateBullet(IEcsSystems systems, CommandCreateBullet command)
	{
		ref ComponentData componentData = ref _poolData.Value.Get(command.EntityID);
		ref ComponentEventManager componentEventManager = ref FuncEvent.GetComponentEventManager(_world.Value);
		int asInt = componentData.GetPropertyValue(PropertyID.BulletCount).AsInt;
		if (asInt <= 0)
		{
			return false;
		}
		if (_poolGunReload.Value.Has(command.EntityID))
		{
			return false;
		}
		ref ComponentPhysics reference = ref _poolPhysics.Value.Get(command.EntityID);
		FVector2 fVector = command.Position - command.BodyPosition;
		FVector2 position = reference.Body.GetPosition() + fVector;
		int intData = FuncData.GetIntData(_world.Value, command.EntityID, PropertyID.ConfigID);
		PlayerConfig playerConfig = _shared.Value.ResourceLoader.LoadConfig<PlayerConfig>(intData);
		bool flag = FVector2.Angle(fVector - new FVector2(0, playerConfig.Spine3BoneHeight), command.Direction) > 35;
		if (!flag)
		{
			FVector2 fVector2 = fVector;
			flag = !(fVector2.X >= playerConfig.LowerBound.X) || !(fVector2.X <= playerConfig.UpperBound.X) || !(fVector2.Y >= playerConfig.LowerBound.Y) || !(fVector2.Y <= playerConfig.UpperBound.Y);
		}
		if (flag)
		{
			_shared.Value.GameResult.Statistics.ExceptionCount++;
			return false;
		}
		componentData.SetPropertyValue(PropertyID.BulletCount, asInt - 1);
		_poolGunReload.Value.Add(command.EntityID).LeftTime = componentData.GetPropertyValue(PropertyID.BulletReloadTime);
		FuncEvent.Broadcast(ref componentEventManager, new EventTrigger(_world.Value.PackEntity(command.EntityID), EcsPackedEntity.Invalid, EventTriggerType.GunReloadStart));
		ref ComponentPlayer reference2 = ref _poolPlayer.Value.Get(command.EntityID);
		EcsEntitySnapshot snapshot = _shared.Value.ResourceLoader.LoadAsset<EcsEntitySnapshot>(reference2.Bullet).As<EcsEntitySnapshot>();
		int num = GameEntityTemplate.NewEntity(_world.Value, snapshot);
		ref ComponentBullet reference3 = ref _poolBullet.Value.Get(num);
		reference3.ID = command.BulletID;
		reference3.Speed = componentData.GetPropertyValue(PropertyID.BulletSpeed);
		_poolPos.Value.Get(num).Position = position;
		FuncEvent.Broadcast(ref componentEventManager, new EventTrigger(_world.Value.PackEntity(command.EntityID), EcsPackedEntity.Invalid, EventTriggerType.Fire));
		ref ComponentPhysics reference4 = ref _poolPhysics.Value.Get(num);
		reference4.Body.SetLinearVelocity(command.Direction * reference3.Speed);
		reference4.Body.SetTransform(in position, FP.Zero);
		if (FuncData.GetBoolData(_world.Value, num, PropertyID.BulletOtherCollider))
		{
			IReadOnlyList<Fixture> fixtureList = reference4.Body.FixtureList;
			for (int i = 0; i < fixtureList.Count; i++)
			{
				Filter filter = fixtureList[i].Filter;
				filter.GroupIndex = Math.Abs(filter.GroupIndex);
				fixtureList[i].Filter = filter;
			}
		}
		if (reference4.Body.UserData is IBodyLogic bodyLogic)
		{
			bodyLogic.ID = -1;
		}
		FuncPhysics.SetBodyOwner(reference4.Body, command.EntityID);
		int intData2 = FuncData.GetIntData(_world.Value, command.EntityID, PropertyID.BulletFireCount);
		FuncData.SetData(_world.Value, command.EntityID, PropertyID.BulletFireCount, intData2 + 1, notify: false);
		int entity = FuncTime.CreateTimer(_world.Value, 10f, 0, 1, -1, num);
		ref ComponentTriggers reference5 = ref _poolTriggers.Value.Add(entity);
		reference5.Triggers = new List<Trigger>();
		Trigger item = new Trigger
		{
			Events = new List<Type> { typeof(EventTime) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				}
			},
			Actions = new List<IAction> { default(DieAction) }
		};
		reference5.Triggers.Add(item);
		return true;
	}
}

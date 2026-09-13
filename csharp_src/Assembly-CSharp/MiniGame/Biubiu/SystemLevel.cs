using System;
using System.Collections.Generic;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemLevel : IEcsInitSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsFilterInject<Inc<ComponentDecoration>> _filter;

	private readonly EcsFilterInject<Inc<ComponentPhysics>> _filterPhysics;

	private readonly EcsFilterInject<Inc<ComponentPlayer, ComponentData>> _filterPlayer;

	private readonly EcsPoolInject<ComponentDecoration> _poolDecoration;

	private readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	private readonly EcsPoolInject<ComponentTriggers> _poolTriggers;

	public void Init(IEcsSystems systems)
	{
		InitInputLimit();
		InitTimelimit();
		InitBulletLimit();
		InitToggle();
		InitPhysicsLogic();
	}

	private void InitInputLimit()
	{
		int entity = _world.Value.NewEntity();
		ref ComponentTriggers reference = ref _poolTriggers.Value.Add(entity);
		FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(_world.Value), default(EventGameStart));
		reference.Triggers = new List<Trigger>
		{
			new Trigger
			{
				Events = new List<Type> { typeof(EventGameStart) },
				Conditions = new List<ICondition>(),
				Actions = new List<IAction> { default(GameWaitAction) }
			}
		};
	}

	private void InitToggle()
	{
		foreach (int item in _filter.Value)
		{
			if (_poolDecoration.Value.Get(item).DecorationType == DecorationType.Toggle)
			{
				int intData = FuncData.GetIntData(_world.Value, item, PropertyID.ConfigID);
				ToggleConfig toggleConfig = _world.Value.GetShared<SharedRuntime>().ResourceLoader.LoadConfig<ToggleConfig>(intData);
				if (toggleConfig.Type == ToggleConfig.ToggleType.RemoteSensing)
				{
					_world.Value.GetPool<ComponentRotation>().Get(item).Rotation = (FuncData.GetBoolData(_world.Value, item, PropertyID.ToggleState) ? (0f - toggleConfig.Angle) : toggleConfig.Angle);
				}
				FuncData.SetData(_world.Value, item, PropertyID.ToggleState, (FuncData.GetIntData(_world.Value, item, PropertyID.ToggleState) != 1) ? 1 : 0);
				FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(_world.Value), new EventTrigger(_world.Value.PackEntity(item), EcsPackedEntity.Invalid, EventTriggerType.Toggle, 1));
			}
		}
	}

	private void InitTimelimit()
	{
		EcsWorld value = _world.Value;
		SharedRuntime shared = value.GetShared<SharedRuntime>();
		int entity = FuncTime.CreateTimer(value, shared.MapData.TimeLimit, 0, 1, -1);
		ref ComponentTriggers reference = ref _poolTriggers.Value.Add(entity);
		reference.Triggers = new List<Trigger>();
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
			Actions = new List<IAction> { default(GameOverAction) }
		};
		reference.Triggers.Add(item);
	}

	private void InitBulletLimit()
	{
		foreach (int item2 in _filterPlayer.Value)
		{
			ref ComponentTriggers orAdd = ref _poolTriggers.Value.GetOrAdd(item2);
			if (orAdd.Triggers == null)
			{
				orAdd.Triggers = new List<Trigger>();
			}
			Trigger item = new Trigger
			{
				Events = new List<Type> { typeof(EventEntityDie) },
				Conditions = new List<ICondition>
				{
					new EntityLayerCondition
					{
						Layer = S5Game.S5GameColliderLayer.Bullet
					},
					default(TargetDataCheckByPlayerBullet),
					new BulletCountCondition
					{
						Count = 0,
						Op = ConditionOp.LessThanOrEqual
					}
				},
				Actions = new List<IAction> { default(GameOverAction) }
			};
			orAdd.Triggers.Add(item);
		}
	}

	private void InitPhysicsLogic()
	{
		foreach (int item in _filterPhysics.Value)
		{
			FuncPhysics.SetUpDataBodyLogic(_poolPhysics.Value.Get(item).Body, _world.Value, item);
		}
	}
}

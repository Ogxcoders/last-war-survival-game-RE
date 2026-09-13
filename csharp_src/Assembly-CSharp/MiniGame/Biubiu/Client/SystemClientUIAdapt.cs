using System;
using System.Collections.Generic;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class SystemClientUIAdapt : IEcsInitSystem, IEcsSystem
{
	private EcsWorldInject _world;

	private EcsSharedInject<GameBiubiuEnvClient> _shared;

	private readonly EcsPoolInject<MiniGame.Biubiu.ComponentUIClient> _poolUI;

	private readonly EcsPoolInject<ComponentTriggers> _poolTriggers;

	public void Init(IEcsSystems systems)
	{
		int entity = _world.Value.NewEntity();
		_poolUI.Value.Add(entity).InitComponent(_world.Value);
		_shared.Value.InitData.UIAdaptEntity = _world.Value.PackEntity(entity);
		ref ComponentTriggers reference = ref _poolTriggers.Value.Add(entity);
		reference.Triggers = new List<Trigger>
		{
			new Trigger
			{
				Events = new List<Type> { typeof(EventTrigger) },
				Conditions = new List<ICondition>
				{
					new EventTriggerCondition
					{
						IsMe = false,
						Type = EventTriggerType.Fire
					}
				},
				Actions = new List<IAction>
				{
					new RefreshUIAction
					{
						UIState = RefreshUIAction.RefreshUIType.Fire
					}
				}
			},
			new Trigger
			{
				Events = new List<Type> { typeof(EventTrigger) },
				Conditions = new List<ICondition>
				{
					new EventTriggerCondition
					{
						IsMe = false,
						Type = EventTriggerType.GunReloadStart
					}
				},
				Actions = new List<IAction>
				{
					new RefreshUIAction
					{
						UIState = RefreshUIAction.RefreshUIType.ReloadStart
					}
				}
			},
			new Trigger
			{
				Events = new List<Type> { typeof(EventTrigger) },
				Conditions = new List<ICondition>
				{
					new EventTriggerCondition
					{
						IsMe = false,
						Type = EventTriggerType.GunReloadFinish
					}
				},
				Actions = new List<IAction>
				{
					new RefreshUIAction
					{
						UIState = RefreshUIAction.RefreshUIType.ReloadFinish
					}
				}
			},
			new Trigger
			{
				Events = new List<Type> { typeof(EventPhysicCollection) },
				Conditions = new List<ICondition>
				{
					new CollectLayerEachOtherCondition
					{
						ALayer = S5Game.S5GameColliderLayer.Bullet,
						BLayer = (S5Game.S5GameColliderLayer.Player | S5Game.S5GameColliderLayer.Enemy)
					},
					new CollectionFixtureTypeCondition
					{
						InculdeType = true,
						FixtureType = S5Game.FixtureType.Head
					}
				},
				Actions = new List<IAction>
				{
					new EffectAction
					{
						EEffectType = EffectAction.EffectType.Hit
					},
					new EffectAction
					{
						EEffectType = EffectAction.EffectType.HeadHit
					}
				}
			},
			new Trigger
			{
				Events = new List<Type> { typeof(EventPhysicCollection) },
				Conditions = new List<ICondition>
				{
					new CollectLayerEachOtherCondition
					{
						ALayer = S5Game.S5GameColliderLayer.Bullet,
						BLayer = (S5Game.S5GameColliderLayer.Player | S5Game.S5GameColliderLayer.Enemy)
					},
					new CollectionFixtureTypeCondition
					{
						InculdeType = false,
						FixtureType = S5Game.FixtureType.Head
					}
				},
				Actions = new List<IAction>
				{
					new EffectAction
					{
						EEffectType = EffectAction.EffectType.Hit
					}
				}
			},
			new Trigger
			{
				Events = new List<Type> { typeof(EventDataChange) },
				Conditions = new List<ICondition>
				{
					new DataChangeCondition
					{
						PropertyID = PropertyID.Die,
						ConditionOp = ConditionOp.Equal,
						ComparableValue = 1
					}
				},
				Actions = new List<IAction>
				{
					new UIEntityAction
					{
						Type = UIEntityAction.ActionType.Die
					}
				}
			},
			new Trigger
			{
				Events = new List<Type> { typeof(EventPhysicCollection) },
				Conditions = new List<ICondition>
				{
					new CollectLayerEachOtherCondition
					{
						ALayer = S5Game.S5GameColliderLayer.Bullet,
						BLayer = S5Game.S5GameColliderLayer.Bullet
					}
				},
				Actions = new List<IAction>
				{
					new EffectAction
					{
						EEffectType = EffectAction.EffectType.BulletBullet
					}
				}
			}
		};
		if (_shared.Value.GameType == EGameType.PveClient)
		{
			reference.Triggers.Add(new Trigger
			{
				Events = new List<Type> { typeof(EventPhysicCollection) },
				Conditions = new List<ICondition>
				{
					new CollectLayerEachOtherCondition
					{
						ALayer = S5Game.S5GameColliderLayer.Bullet,
						BLayer = (S5Game.S5GameColliderLayer.Environment | S5Game.S5GameColliderLayer.Obstacle | S5Game.S5GameColliderLayer.WoodBarrel | S5Game.S5GameColliderLayer.Toggle)
					}
				},
				Actions = new List<IAction>
				{
					new EffectAction
					{
						EEffectType = EffectAction.EffectType.BulletWall
					}
				}
			});
		}
	}
}

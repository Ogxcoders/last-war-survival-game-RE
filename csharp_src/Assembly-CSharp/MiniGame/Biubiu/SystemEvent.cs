using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemEvent : IEcsInitSystem, IEcsSystem, IEcsRunSystem
{
	private EcsSharedInject<SharedRuntime> _shared;

	private EcsPoolInject<ComponentEventManager> _eventPool;

	private EcsPoolInject<ComponentTriggers> _poolTriggers;

	private EcsPoolInject<ComponentActivedTriggers> _poolActiveTriggers;

	public void Init(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		if (!_shared.Value.EventManager.Unpack(world, out var _))
		{
			int entity2 = world.NewEntity();
			ref ComponentEventManager reference = ref _eventPool.Value.Add(entity2);
			reference.TargetListeners = new Dictionary<Type, List<int>>();
			reference.EventQueue = new Queue<IEvent>();
			_shared.Value.EventManager = world.PackEntity(entity2);
		}
	}

	public void Run(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		EcsPool<ComponentActivedTriggers> value = _poolActiveTriggers.Value;
		EcsPool<ComponentTriggers> value2 = _poolTriggers.Value;
		if (!_shared.Value.EventManager.Unpack(world, out var entity))
		{
			throw new Exception("EventManager not init");
		}
		ref ComponentEventManager reference = ref _eventPool.Value.Get(entity);
		int num = 0;
		bool flag = false;
		while (reference.EventQueue.Count > 0)
		{
			num++;
			if (num > 1000)
			{
				throw new Exception("Too many events, may be looped!");
			}
			IEvent obj = reference.EventQueue.Dequeue();
			Type type = obj.GetType();
			if (!reference.TargetListeners.TryGetValue(type, out var value3))
			{
				continue;
			}
			for (int num2 = value3.Count - 1; num2 >= 0; num2--)
			{
				int entity2 = value3[num2];
				ref ComponentActivedTriggers reference2 = ref value.Get(entity2);
				ref ComponentTriggers reference3 = ref value2.Get(entity2);
				List<int> list = reference2.Triggers[type];
				ref List<Trigger> triggers = ref reference3.Triggers;
				for (int i = 0; i < list.Count; i++)
				{
					if (triggers != null && triggers.Count > 0)
					{
						Trigger trigger = triggers[list[i]];
						if (FuncCondition.CheckConditions(world, entity2, trigger, obj))
						{
							flag = true;
							FuncAction.DoActions(world, entity2, trigger, obj);
						}
					}
				}
			}
		}
		if (flag)
		{
			_shared.Value.FrameSyncIsNeeded = true;
		}
	}
}

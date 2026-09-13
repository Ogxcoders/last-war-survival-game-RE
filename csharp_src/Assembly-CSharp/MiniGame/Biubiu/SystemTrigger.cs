using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemTrigger : IEcsRunSystem, IEcsSystem
{
	private EcsSharedInject<SharedRuntime> _shared;

	private EcsFilterInject<Inc<ComponentTriggers>, Exc<ComponentActivedTriggers>> _filterTriggersRegister;

	private EcsPoolInject<ComponentActivedTriggers> _poolActivedTriggers;

	private EcsPoolInject<ComponentTriggers> _poolTriggers;

	private EcsPoolInject<ComponentEventManager> _poolEvents;

	public void Run(IEcsSystems systems)
	{
		if (!_shared.Value.EventManager.Unpack(systems.GetWorld(), out var entity))
		{
			throw new Exception("EventManager not init");
		}
		ref ComponentEventManager evtMgr = ref _poolEvents.Value.Get(entity);
		foreach (int item in _filterTriggersRegister.Value)
		{
			ref ComponentActivedTriggers reference = ref _poolActivedTriggers.Value.Add(item);
			reference.Triggers = new Dictionary<Type, List<int>>();
			ref ComponentTriggers reference2 = ref _poolTriggers.Value.Get(item);
			if (reference2.Triggers != null)
			{
				for (int i = 0; i < reference2.Triggers.Count; i++)
				{
					Trigger trigger = reference2.Triggers[i];
					FuncEvent.Subscribe(ref evtMgr, item, i, reference.Triggers, trigger);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public static class FuncEvent
{
	public static ref ComponentEventManager GetComponentEventManager(EcsWorld world)
	{
		world.GetShared<SharedRuntime>().EventManager.Unpack(world, out var entity);
		return ref world.GetPool<ComponentEventManager>().Get(entity);
	}

	public static bool HasEventManager(EcsWorld world)
	{
		int entity;
		return world.GetShared<SharedRuntime>().EventManager.Unpack(world, out entity);
	}

	public static void Broadcast(EcsWorld world, IEvent e)
	{
		Broadcast(ref GetComponentEventManager(world), e);
	}

	public static void Broadcast(ref ComponentEventManager evtMgr, IEvent e)
	{
		evtMgr.EventQueue.Enqueue(e);
	}

	public static void Subscribe(ref ComponentEventManager evtMgr, int entity, int index, Dictionary<Type, List<int>> regs, Trigger trigger)
	{
		for (int i = 0; i < trigger.Events.Count; i++)
		{
			Type key = trigger.Events[i];
			if (!regs.TryGetValue(key, out var value))
			{
				value = (regs[key] = new List<int>());
			}
			if (!value.Contains(index))
			{
				value.Add(index);
			}
			if (!evtMgr.TargetListeners.TryGetValue(key, out var value2))
			{
				value2 = new List<int>();
				evtMgr.TargetListeners.Add(key, value2);
			}
			if (!value2.Contains(entity))
			{
				value2.Add(entity);
			}
		}
	}

	public static void UnSubscribe(ref ComponentEventManager evtMgr, int entity, Dictionary<Type, List<int>> triggers)
	{
		foreach (KeyValuePair<Type, List<int>> trigger in triggers)
		{
			if (evtMgr.TargetListeners.TryGetValue(trigger.Key, out var value) && value.Contains(entity))
			{
				value.Remove(entity);
			}
		}
	}
}

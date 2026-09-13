using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentEventManager : TEcsPoolDelegate<ComponentEventManager>, IEcsPoolDelegate, IEcsAutoReset<ComponentEventManager>, IEcsAutoCopy<ComponentEventManager>, IEcsAutoSnapshot<ComponentEventManager>
{
	public Dictionary<Type, List<int>> TargetListeners;

	public Queue<IEvent> EventQueue;

	public Type DelegateType => typeof(ComponentEventManager);

	public void AutoReset(ref ComponentEventManager c, EcsWorld world, int entity)
	{
		if (c.TargetListeners != null)
		{
			c.TargetListeners.Clear();
		}
		if (c.EventQueue != null)
		{
			c.EventQueue.Clear();
		}
	}

	public void AutoCopy(ref ComponentEventManager src, ref ComponentEventManager dst)
	{
		throw new NotSupportedException("ComponentEventManager不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentEventManager c, EcsWorld world, int entity, object env)
	{
		ComponentEventManager componentEventManager = new ComponentEventManager
		{
			TargetListeners = new Dictionary<Type, List<int>>(),
			EventQueue = ((c.EventQueue != null) ? new Queue<IEvent>(c.EventQueue) : new Queue<IEvent>())
		};
		foreach (KeyValuePair<Type, List<int>> targetListener in c.TargetListeners)
		{
			componentEventManager.TargetListeners.Add(targetListener.Key, new List<int>(targetListener.Value));
		}
		return componentEventManager;
	}

	public void RestoreSnapshot(ref ComponentEventManager c, EcsWorld world, int entity, object data, object env)
	{
		ComponentEventManager componentEventManager = (ComponentEventManager)data;
		if (c.TargetListeners != null)
		{
			c.TargetListeners.Clear();
		}
		else
		{
			c.TargetListeners = new Dictionary<Type, List<int>>();
		}
		if (componentEventManager.TargetListeners != null)
		{
			foreach (KeyValuePair<Type, List<int>> targetListener in componentEventManager.TargetListeners)
			{
				c.TargetListeners.Add(targetListener.Key, new List<int>(targetListener.Value));
			}
		}
		c.EventQueue = ((componentEventManager.EventQueue == null) ? new Queue<IEvent>() : new Queue<IEvent>(componentEventManager.EventQueue));
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		throw new NotSupportedException("这玩意是全局的，被比较就不对了");
	}
}

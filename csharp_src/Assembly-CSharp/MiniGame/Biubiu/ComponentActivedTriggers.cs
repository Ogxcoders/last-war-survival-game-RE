using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentActivedTriggers : TEcsPoolDelegate<ComponentActivedTriggers>, IEcsPoolDelegate, IEcsAutoReset<ComponentActivedTriggers>, IEcsAutoCopy<ComponentActivedTriggers>, IEcsAutoSnapshot<ComponentActivedTriggers>
{
	public class ActivedTriggerSnapshot
	{
		public Type Event;

		public List<int> Triggers;
	}

	public Dictionary<Type, List<int>> Triggers;

	public Type DelegateType => typeof(ComponentActivedTriggers);

	public void AutoReset(ref ComponentActivedTriggers c, EcsWorld world, int entity)
	{
		if (c.Triggers != null)
		{
			if (FuncEvent.HasEventManager(world))
			{
				FuncEvent.UnSubscribe(ref FuncEvent.GetComponentEventManager(world), entity, c.Triggers);
			}
			c.Triggers.Clear();
		}
	}

	public void AutoCopy(ref ComponentActivedTriggers src, ref ComponentActivedTriggers dst)
	{
		throw new NotSupportedException("ComponentActivedTriggers不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentActivedTriggers c, EcsWorld world, int entity, object env)
	{
		List<ActivedTriggerSnapshot> list = new List<ActivedTriggerSnapshot>();
		foreach (KeyValuePair<Type, List<int>> trigger in c.Triggers)
		{
			list.Add(new ActivedTriggerSnapshot
			{
				Event = trigger.Key,
				Triggers = new List<int>(trigger.Value)
			});
		}
		return list;
	}

	public void RestoreSnapshot(ref ComponentActivedTriggers c, EcsWorld world, int entity, object data, object env)
	{
		if (c.Triggers == null)
		{
			c.Triggers = new Dictionary<Type, List<int>>();
		}
		else
		{
			c.Triggers.Clear();
		}
		if (data.GetType() == typeof(List<ActivedTriggerSnapshot>))
		{
			foreach (ActivedTriggerSnapshot item in (List<ActivedTriggerSnapshot>)data)
			{
				c.Triggers.Add(item.Event, new List<int>(item.Triggers));
			}
			return;
		}
		if (!(data.GetType() == typeof(ComponentActivedTriggers)))
		{
			return;
		}
		foreach (KeyValuePair<Type, List<int>> trigger in ((ComponentActivedTriggers)data).Triggers)
		{
			c.Triggers.Add(trigger.Key, new List<int>(trigger.Value));
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}

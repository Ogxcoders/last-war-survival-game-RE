using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentTriggers : TEcsPoolDelegate<ComponentTriggers>, IEcsPoolDelegate, IEcsAutoReset<ComponentTriggers>, IEcsAutoCopy<ComponentTriggers>, IEcsAutoSnapshot<ComponentTriggers>
{
	public List<Trigger> Triggers;

	public Type DelegateType => typeof(ComponentTriggers);

	public void AutoReset(ref ComponentTriggers c, EcsWorld world, int entity)
	{
		if (c.Triggers != null)
		{
			c.Triggers.Clear();
		}
	}

	public void AutoCopy(ref ComponentTriggers src, ref ComponentTriggers dst)
	{
		throw new NotSupportedException("ComponentTriggers不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentTriggers c, EcsWorld world, int entity, object env)
	{
		ComponentTriggers c2 = default(ComponentTriggers);
		c2.RestoreSnapshot(ref c2, world, entity, c, env);
		return c2;
	}

	public void RestoreSnapshot(ref ComponentTriggers c, EcsWorld world, int entity, object data, object env)
	{
		ComponentTriggers componentTriggers = (ComponentTriggers)data;
		if (c.Triggers == null)
		{
			c.Triggers = new List<Trigger>();
		}
		else
		{
			c.Triggers.Clear();
		}
		for (int i = 0; i < componentTriggers.Triggers.Count; i++)
		{
			Trigger trigger = componentTriggers.Triggers[i];
			c.Triggers.Add(trigger.Clone());
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		ComponentTriggers componentTriggers = (ComponentTriggers)a;
		ComponentTriggers componentTriggers2 = (ComponentTriggers)b;
		int num = componentTriggers.Triggers?.Count ?? 0;
		int num2 = componentTriggers2.Triggers?.Count ?? 0;
		if (num == num2 && num == 0)
		{
			return true;
		}
		if (num != num2)
		{
			return false;
		}
		for (int i = 0; i < componentTriggers.Triggers.Count; i++)
		{
			if (!componentTriggers.Triggers[i].Equals(componentTriggers2.Triggers[i]))
			{
				return false;
			}
		}
		return true;
	}
}

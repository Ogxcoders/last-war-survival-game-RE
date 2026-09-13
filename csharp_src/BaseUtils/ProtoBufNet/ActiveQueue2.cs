using System;
using System.Collections.Generic;

namespace ProtoBufNet;

internal class ActiveQueue2
{
	private LinkedList<Action<NetIOService>> actions;

	public ActiveQueue2(int size)
	{
		actions = new LinkedList<Action<NetIOService>>();
	}

	public void Put(Action<NetIOService> act)
	{
		lock (actions)
		{
			actions.AddLast(act);
		}
	}

	public Action<NetIOService> TryGet()
	{
		Action<NetIOService> result = null;
		lock (actions)
		{
			if (actions.Count != 0)
			{
				result = actions.First.Value;
				actions.RemoveFirst();
			}
		}
		return result;
	}
}

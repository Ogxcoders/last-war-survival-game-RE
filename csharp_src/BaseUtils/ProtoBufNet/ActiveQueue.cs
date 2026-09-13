using System;
using System.Collections.Generic;
using System.Threading;

namespace ProtoBufNet;

internal class ActiveQueue
{
	private LinkedList<Action<NetIOService>> actions;

	private Semaphore semNotFull;

	private Semaphore semNotEmpty;

	public ActiveQueue(int size)
	{
		actions = new LinkedList<Action<NetIOService>>();
		semNotFull = new Semaphore(size, size);
		semNotEmpty = new Semaphore(0, size);
	}

	public void Put(Action<NetIOService> act)
	{
		semNotFull.WaitOne();
		lock (actions)
		{
			actions.AddLast(act);
		}
		semNotEmpty.Release();
	}

	public Action<NetIOService> Get()
	{
		semNotEmpty.WaitOne();
		Action<NetIOService> result = null;
		lock (actions)
		{
			result = actions.First.Value;
			actions.RemoveFirst();
		}
		semNotFull.Release();
		return result;
	}

	public Action<NetIOService> TryGet()
	{
		Action<NetIOService> result = null;
		if (semNotEmpty.WaitOne(0))
		{
			lock (actions)
			{
				result = actions.First.Value;
				actions.RemoveFirst();
			}
		}
		return result;
	}
}

using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore;

internal sealed class Subscription
{
	public List<CallbackDescriptor> callbacks = new List<CallbackDescriptor>(1);

	public void Add(Type[] paramTypes, Action<object[]> callback)
	{
		lock (callbacks)
		{
			callbacks.Add(new CallbackDescriptor(paramTypes, callback));
		}
	}

	public void Remove(Action<object[]> callback)
	{
		lock (callbacks)
		{
			int num = -1;
			for (int i = 0; i < callbacks.Count; i++)
			{
				if (num != -1)
				{
					break;
				}
				if (callbacks[i].Callback == callback)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				callbacks.RemoveAt(num);
			}
		}
	}
}

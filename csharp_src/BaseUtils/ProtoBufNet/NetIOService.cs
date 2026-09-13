using System;
using System.Collections.Concurrent;

namespace ProtoBufNet;

public class NetIOService
{
	public readonly int QueueSize = 10000;

	private bool isRunning;

	private ConcurrentQueue<Action<NetIOService>> actionQueue;

	private ConcurrentQueue<NetPacket> msgQueue;

	private MessageDispather messageDispatcher;

	public NetIOService(MessageDispather dispatcher)
	{
		actionQueue = new ConcurrentQueue<Action<NetIOService>>();
		msgQueue = new ConcurrentQueue<NetPacket>();
		messageDispatcher = dispatcher;
		Start();
	}

	public void Post(Action<NetIOService> callback)
	{
		actionQueue.Enqueue(callback);
	}

	public void EnqueueMsg(NetPacket packet)
	{
		msgQueue.Enqueue(packet);
	}

	public void Poll()
	{
		if (!isRunning)
		{
			return;
		}
		Action<NetIOService> result = null;
		do
		{
			if (actionQueue.TryDequeue(out result))
			{
				result?.Invoke(this);
			}
		}
		while (actionQueue.Count > 0 && isRunning);
		do
		{
			if (msgQueue.TryDequeue(out var result2))
			{
				messageDispatcher.Dispatch(null, result2);
			}
		}
		while (msgQueue.Count > 0 && isRunning);
	}

	public void Start()
	{
		isRunning = true;
	}

	public void Stop()
	{
		isRunning = false;
	}
}

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Joker;

public class ThreadService : Singleton<ThreadService>, IService, IUpdateService
{
	private int _threadId;

	private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

	public void Awake()
	{
		_threadId = Thread.CurrentThread.ManagedThreadId;
	}

	public void Startup()
	{
	}

	public void Shutdown()
	{
	}

	public void Destroy()
	{
	}

	public bool IsMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == _threadId;
	}

	public void PostMainThread(Action action)
	{
		if (Thread.CurrentThread.ManagedThreadId == _threadId)
		{
			try
			{
				action();
				return;
			}
			catch (Exception e)
			{
				Log.Exception(e);
				return;
			}
		}
		_queue.Enqueue(action);
	}

	public void Update(float deltaTime)
	{
		Action result;
		while (_queue.TryDequeue(out result))
		{
			try
			{
				result?.Invoke();
			}
			catch (Exception e)
			{
				Log.Exception(e);
			}
		}
	}
}

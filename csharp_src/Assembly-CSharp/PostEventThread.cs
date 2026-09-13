using System;
using System.Collections.Generic;
using System.Threading;
using GameFramework;

internal class PostEventThread
{
	private Thread thread;

	private AutoResetEvent wakeupEvent = new AutoResetEvent(initialState: false);

	private volatile bool stop;

	private Queue<PostEventThreadTask> tasks = new Queue<PostEventThreadTask>(10);

	private volatile PostEventThreadTask currentTask;

	private byte[] buffer = new byte[16384];

	public void Start()
	{
		stop = false;
		thread = new Thread(ThreadProc);
		thread.Name = "PostEventThread";
		thread.Start();
	}

	public void Stop()
	{
		try
		{
			stop = true;
			Wakeup();
			if (currentTask != null)
			{
				currentTask.Abort();
			}
			if (thread != null)
			{
				thread.Join();
				thread = null;
			}
			tasks.Clear();
		}
		catch (Exception ex)
		{
			Log.Error("Stop exception " + ex.Message);
		}
	}

	public void AddTask(PostEventThreadTask task)
	{
		lock (tasks)
		{
			tasks.Enqueue(task);
		}
		Wakeup();
	}

	private void Sleep()
	{
		wakeupEvent.WaitOne();
	}

	private void Wakeup()
	{
		wakeupEvent.Set();
	}

	private void ThreadProc()
	{
		while (!stop)
		{
			currentTask = null;
			lock (tasks)
			{
				if (tasks.Count > 0)
				{
					currentTask = tasks.Dequeue();
				}
			}
			if (currentTask != null)
			{
				try
				{
					currentTask.BeginProcess(buffer);
					if (stop)
					{
						currentTask.Abort();
						break;
					}
					currentTask.WaitProcessDone();
				}
				catch (Exception arg)
				{
					Log.Info("post thread excep", arg);
				}
			}
			else
			{
				Sleep();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using UnityEngine;

public class FileContentManager : MonoBehaviour
{
	private static FileContentManager _instance;

	private static readonly object _lock = new object();

	private QueuedThread thread;

	private Queue<FileActionTask> taskQueue = new Queue<FileActionTask>();

	private bool isInited;

	public static FileContentManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = UnityEngine.Object.FindObjectOfType<FileContentManager>();
						if (UnityEngine.Object.FindObjectsOfType<FileContentManager>().Length > 1)
						{
							return _instance;
						}
						if (_instance == null)
						{
							GameObject obj = new GameObject("(Singleton) " + typeof(FileContentManager));
							_instance = obj.AddComponent<FileContentManager>();
							UnityEngine.Object.DontDestroyOnLoad(obj);
						}
					}
				}
			}
			return _instance;
		}
	}

	public void Initialize()
	{
		thread = new QueuedThread("FileContentManager");
		thread.Start();
		isInited = true;
	}

	public void Release()
	{
		taskQueue.Clear();
		if (thread != null)
		{
			thread.Stop();
			thread = null;
		}
	}

	private void Update()
	{
		UpdateTask();
	}

	private void UpdateTask()
	{
		if (thread == null || taskQueue.Count <= 0 || !taskQueue.Peek().Processed)
		{
			return;
		}
		try
		{
			taskQueue.Dequeue().CallBack();
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
	}

	public bool IsFileExist(string id)
	{
		return File.Exists(Path.Combine(FileContentHelper.GetRootDirectory(), id + ".bin"));
	}

	public void ExecuteCreate(string id, string content, Action callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke();
			return;
		}
		CreateTask createTask = new CreateTask(id, content, callback);
		thread.AddTask(createTask);
		taskQueue.Enqueue(createTask);
	}

	public void ExecuteRead(string id, Action<string> callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke(null);
			return;
		}
		ReadTask readTask = new ReadTask(id, callback);
		thread.AddTask(readTask);
		taskQueue.Enqueue(readTask);
	}

	public void ExecuteDelete(string id, Action callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke();
			return;
		}
		DeleteTask deleteTask = new DeleteTask(id, callback);
		thread.AddTask(deleteTask);
		taskQueue.Enqueue(deleteTask);
	}
}

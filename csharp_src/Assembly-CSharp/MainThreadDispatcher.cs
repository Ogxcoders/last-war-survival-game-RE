using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[HideInInspector]
public class MainThreadDispatcher : MonoBehaviour
{
	private static int MaxExecutionTimeInMilliseconds = 10;

	private static readonly object _lock = new object();

	private static MainThreadDispatcher _instance;

	private static Queue<Action> _executionQueue = new Queue<Action>();

	private static Stopwatch _stopwatch = new Stopwatch();

	public static MainThreadDispatcher Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject obj = new GameObject("MainThreadDispatcher");
				_instance = obj.AddComponent<MainThreadDispatcher>();
				UnityEngine.Object.DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}

	private MainThreadDispatcher()
	{
	}

	public void Update()
	{
		_stopwatch.Start();
		Action action = null;
		do
		{
			lock (_lock)
			{
				if (_executionQueue.Count > 0)
				{
					action = _executionQueue.Dequeue();
					goto IL_0042;
				}
			}
			break;
			IL_0042:
			action?.Invoke();
		}
		while (_stopwatch.ElapsedMilliseconds < MaxExecutionTimeInMilliseconds);
		_stopwatch.Stop();
		_stopwatch.Reset();
	}

	public void Enqueue(Action action)
	{
		lock (_lock)
		{
			_executionQueue.Enqueue(action);
		}
	}
}

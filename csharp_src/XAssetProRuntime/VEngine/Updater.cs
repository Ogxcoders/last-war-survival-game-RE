using System;
using UnityEngine;

namespace VEngine;

public sealed class Updater : MonoBehaviour
{
	public static float maxUpdateTimeSlice = 0.01f;

	public static bool useAllTimeSlice = false;

	public static double time { get; private set; }

	public static bool busy => (double)Time.realtimeSinceStartup >= time;

	public static Action onUpdate { get; set; }

	private void Awake()
	{
		Download.InitStatic();
		AddUpdateCallback(Download.UpdateDownloads);
		AddUpdateCallback(Loadable.UpdateLoadables);
		AddUpdateCallback(Operation.UpdateOperations);
	}

	private void Update()
	{
		time = Time.realtimeSinceStartup + maxUpdateTimeSlice;
		if (onUpdate != null)
		{
			onUpdate();
		}
		while (useAllTimeSlice && onUpdate != null && !busy)
		{
			onUpdate();
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void InitializeOnLoad()
	{
		if (UnityEngine.Object.FindObjectOfType<Updater>() == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("Updater").AddComponent<Updater>());
		}
	}

	public static void AddUpdateCallback(Action callback)
	{
		onUpdate = (Action)Delegate.Combine(onUpdate, callback);
	}

	public static void RemoveUpdateCallback(Action callback)
	{
		onUpdate = (Action)Delegate.Remove(onUpdate, callback);
	}
}

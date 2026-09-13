using System.Collections.Generic;
using UnityEngine;

internal class GameObjectNeedHighFPSCheckStrategy : NeedHighFPSCheckStrategyBase
{
	private static float time = 0f;

	private static bool logged = false;

	public static HashSet<GameObject> tweenings { get; } = new HashSet<GameObject>();

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		if (tweenings.Count > 0 && Time.time - time > 30f)
		{
			tweenings.Clear();
			if (!logged)
			{
				Debug.LogError("30s dotween detected ...");
				logged = true;
			}
		}
		pass = tweenings.Count > 0;
		keep = 0f;
	}

	public static void AcquireHighFPSLockerForGameObject(GameObject go)
	{
		if (go != null)
		{
			tweenings.Add(go);
			time = Time.time;
		}
	}

	public static void FreeHighFPSLockerForGameObject(GameObject go)
	{
		if (go != null)
		{
			tweenings.Remove(go);
		}
	}
}

using System;
using UnityEngine;

public class CameraLODCheckStrategy : NeedHighFPSCheckStrategyBase
{
	public static WeakReference<Camera> cemara { get; } = new WeakReference<Camera>(null);

	public CameraLODCheckStrategy()
	{
		if (!cemara.TryGetTarget(out var _))
		{
			cemara.SetTarget(CameraUtilities.GetOrFindMainCamera());
		}
	}

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		pass = false;
		if (SceneManager.CurrSceneID == 2 && cemara.TryGetTarget(out var target))
		{
			pass = target.transform.position.y < 75f;
		}
	}
}

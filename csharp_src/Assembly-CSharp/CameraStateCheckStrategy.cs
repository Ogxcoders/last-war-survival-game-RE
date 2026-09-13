using System;
using BitBenderGames;
using UnityEngine;

public class CameraStateCheckStrategy : NeedHighFPSCheckStrategyBase
{
	public static WeakReference<MobileTouchCamera> cemara { get; } = new WeakReference<MobileTouchCamera>(null);

	public CameraStateCheckStrategy()
	{
		if (!cemara.TryGetTarget(out var _))
		{
			cemara.SetTarget(UnityEngine.Object.FindObjectOfType(typeof(MobileTouchCamera)) as MobileTouchCamera);
		}
	}

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		pass = false;
		if (cemara.TryGetTarget(out var target))
		{
			pass = target.CurrentState != MobileTouchCamera.State.Idle;
		}
	}
}

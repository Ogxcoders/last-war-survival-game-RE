using System;
using System.Collections.Generic;
using RiverGame.PerformanceAnalysis;
using UnityEngine;

[DefaultExecutionOrder(1073741823)]
public class DynamicFPSAutoHighBehaviour : MonoBehaviour
{
	public float countdownDefault = 1f;

	public Func<int> invalidId;

	public Func<int> acquire;

	public Func<int, int> free;

	private int handle;

	public List<NeedHighFPSCheckStrategyBase> checkers { get; } = new List<NeedHighFPSCheckStrategyBase>();

	public float countdown { get; private set; }

	public static float countdownCustom { get; private set; }

	public static int lastFrameCount { get; private set; }

	public bool acquireHighEnabled { get; private set; }

	public bool shouldHigh { get; private set; }

	public bool ShouldHigh()
	{
		return shouldHigh;
	}

	public static void Initialize()
	{
		Camera orFindMainCamera = CameraUtilities.GetOrFindMainCamera();
		if (orFindMainCamera == null)
		{
			Debug.LogError("InitializeDynamicFPSAutoHighBehaviour error for Camera.main == null");
			return;
		}
		try
		{
			DynamicFPSAutoHighBehaviour dynamicFPSAutoHighBehaviour = null;
			object[] array = GameEntry.Lua.Env.DoString($"local LuaEntry = require \"DataCenter.Global.LuaEntry\"; return LuaEntry.DataConfig:TryGetNum(\"dynamic_high_fps_on\", \"k1\", {0});");
			if (array != null && array.Length == 1)
			{
				if ((dynamicFPSAutoHighBehaviour = orFindMainCamera.gameObject.GetComponent<DynamicFPSAutoHighBehaviour1>()) == null)
				{
					dynamicFPSAutoHighBehaviour = Camera.main.gameObject.AddComponent<DynamicFPSAutoHighBehaviour1>();
					dynamicFPSAutoHighBehaviour.hideFlags = HideFlags.DontSave;
				}
				bool flag = Convert.ToInt32(array[0]) == 1 && DynamicFPSConfig.enabled;
				flag |= GrayUtils.isGrayServer;
				flag |= Debug.isDebugBuild;
				dynamicFPSAutoHighBehaviour.enabled = flag || true;
				dynamicFPSAutoHighBehaviour.acquireHighEnabled = false;
				dynamicFPSAutoHighBehaviour.checkers.Clear();
				dynamicFPSAutoHighBehaviour.checkers.Add(new TransformChangedCheckStrategy(Camera.main.transform));
				dynamicFPSAutoHighBehaviour.checkers.Add(new CameraStateCheckStrategy());
				dynamicFPSAutoHighBehaviour.checkers.Add(new SceneCameraCheckStrategy(SceneManager.SceneID.World));
				dynamicFPSAutoHighBehaviour.checkers.Add(new GameObjectNeedHighFPSCheckStrategy());
				dynamicFPSAutoHighBehaviour.checkers.Add(new UIScrollComponentValueChangedCheckStrategy());
				FPSShouldHighTimeCounter.ShouldHighFps = (Func<bool>)Delegate.Remove(FPSShouldHighTimeCounter.ShouldHighFps, new Func<bool>(dynamicFPSAutoHighBehaviour.ShouldHigh));
				FPSShouldHighTimeCounter.ShouldHighFps = (Func<bool>)Delegate.Combine(FPSShouldHighTimeCounter.ShouldHighFps, new Func<bool>(dynamicFPSAutoHighBehaviour.ShouldHigh));
				Debug.Log($"dynamic_high_fps_on: enable: {array[0]}");
				if (array != null && array.Length == 1)
				{
					if ((dynamicFPSAutoHighBehaviour = orFindMainCamera.gameObject.GetComponent<DynamicFPSAutoHighBehaviour2>()) == null)
					{
						dynamicFPSAutoHighBehaviour = Camera.main.gameObject.AddComponent<DynamicFPSAutoHighBehaviour2>();
						dynamicFPSAutoHighBehaviour.hideFlags = HideFlags.DontSave;
					}
					bool flag2 = Convert.ToInt32(array[0]) == 1 && DynamicFPSConfig.enabled;
					flag2 |= GrayUtils.isGrayServer;
					flag2 |= Debug.isDebugBuild;
					dynamicFPSAutoHighBehaviour.enabled = flag2 || true;
					dynamicFPSAutoHighBehaviour.acquireHighEnabled = flag2;
					array = GameEntry.Lua.Env.DoString($"local LuaEntry = require \"DataCenter.Global.LuaEntry\"; return LuaEntry.DataConfig:TryGetNum(\"dynamic_high_fps_on\", \"k2\", {4});");
					if (array != null && array.Length == 1 && dynamicFPSAutoHighBehaviour != null)
					{
						dynamicFPSAutoHighBehaviour.countdownDefault = Convert.ToSingle(array[0]);
						Debug.Log($"dynamic_high_fps_on:   keep: {array[0]}");
					}
					dynamicFPSAutoHighBehaviour.checkers.Clear();
					dynamicFPSAutoHighBehaviour.checkers.Add(new TransformChangedCheckStrategy(Camera.main.transform));
					dynamicFPSAutoHighBehaviour.checkers.Add(new CameraStateCheckStrategy());
					dynamicFPSAutoHighBehaviour.checkers.Add(new SceneCameraCheckStrategy(SceneManager.SceneID.World));
					dynamicFPSAutoHighBehaviour.checkers.Add(new DraggedCheckStrategy(dynamicFPSAutoHighBehaviour.countdownDefault));
					dynamicFPSAutoHighBehaviour.checkers.Add(new GameObjectNeedHighFPSCheckStrategy());
					FPSShouldHighTimeCounter.ShouldHighFps2 = (Func<bool>)Delegate.Remove(FPSShouldHighTimeCounter.ShouldHighFps2, new Func<bool>(dynamicFPSAutoHighBehaviour.ShouldHigh));
					FPSShouldHighTimeCounter.ShouldHighFps2 = (Func<bool>)Delegate.Combine(FPSShouldHighTimeCounter.ShouldHighFps2, new Func<bool>(dynamicFPSAutoHighBehaviour.ShouldHigh));
					Debug.Log($"dynamic_high_fps_on: enable: {array[0]}");
				}
				else
				{
					Debug.LogWarning("no dynamic_high_fps_on key in item table");
				}
			}
			else
			{
				Debug.LogWarning("no dynamic_high_fps_on key in item table");
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public static void AcquireHighFPSLockerForSeconds(float countdown)
	{
		countdownCustom = Mathf.Max(countdownCustom, countdown);
	}

	private void Awake()
	{
		acquire = () => DynamicFPSConfig.AcquireHighFPSLocker();
		free = (int handle) => DynamicFPSConfig.FreeHighFPSLocker(handle);
		invalidId = () => -1;
		handle = invalidId();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		Stop();
	}

	private void LateUpdate()
	{
		InputHelper.RecordInputState();
		InputHelper.InputState lastInput = InputHelper.LastInput;
		InputHelper.InputState currInput = InputHelper.CurrInput;
		if (lastInput == null || currInput == null)
		{
			return;
		}
		bool flag = false;
		foreach (NeedHighFPSCheckStrategyBase checker in checkers)
		{
			checker.Check(lastInput, currInput);
			if (checker.pass)
			{
				flag = true;
				countdown = Mathf.Max(countdown, checker.keep);
			}
		}
		if (flag || countdown > 0f || countdownCustom > 0f)
		{
			shouldHigh = true;
			if (acquireHighEnabled && handle == invalidId() && acquire != null)
			{
				handle = acquire();
			}
			countdown = ((countdown > 0f) ? (countdown - Time.deltaTime) : countdown);
			if (Time.frameCount > lastFrameCount)
			{
				countdownCustom = ((countdownCustom > 0f) ? (countdownCustom - Time.deltaTime) : countdownCustom);
			}
		}
		else
		{
			shouldHigh = false;
			Stop();
		}
		lastFrameCount = Time.frameCount;
	}

	private void Stop()
	{
		countdown = 0f;
		countdownCustom = 0f;
		if (handle != invalidId() && free != null)
		{
			handle = free(handle);
		}
	}
}

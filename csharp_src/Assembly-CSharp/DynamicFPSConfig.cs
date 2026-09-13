using System;
using System.Collections.Generic;
using UnityEngine;

public static class DynamicFPSConfig
{
	public const int INVALID_ID = -1;

	private static int _normalFPS = 30;

	private static int _highFPS = 60;

	private static int _targetFPS = -1;

	private static bool _enabled = false;

	private static int _fpsLockerIDInc = 0;

	private static readonly Stack<int> _freeFPSLockerList = new Stack<int>(2);

	private static ulong _activeFPSLockerFlag = 0uL;

	public static bool enabled => _enabled;

	private static int targetFPS
	{
		get
		{
			if (_targetFPS < 0)
			{
				_targetFPS = Application.targetFrameRate;
			}
			return _targetFPS;
		}
		set
		{
			if (value > 0 && value != _targetFPS)
			{
				_targetFPS = value;
				if (_enabled)
				{
					Application.targetFrameRate = value;
				}
			}
		}
	}

	public static int nLocker { get; private set; }

	public static void Initialize(int normalFPS, int highFPS, bool defaultHighFPS)
	{
		if (GrayUtils.isGrayServer || Debug.isDebugBuild)
		{
			normalFPS = 45;
			highFPS = Mathf.Max(normalFPS, highFPS);
		}
		if (normalFPS < 0 || highFPS < 0 || highFPS < normalFPS)
		{
			throw new ArgumentException("Need assure normalFPS > 0 && highFPS > 0 && highFPS >= normalFPS");
		}
		_normalFPS = normalFPS;
		_highFPS = highFPS;
		if (normalFPS != highFPS)
		{
			QualitySettings.vSyncCount = 0;
			_enabled = true;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = normalFPS;
			_enabled = false;
		}
		targetFPS = (defaultHighFPS ? _highFPS : _normalFPS);
		DynamicFPSAutoHighBehaviour.Initialize();
	}

	public static int AcquireHighFPSLocker()
	{
		int num = ((_freeFPSLockerList.Count <= 0) ? _fpsLockerIDInc++ : _freeFPSLockerList.Pop());
		_activeFPSLockerFlag |= (ulong)(1L << num);
		if (_activeFPSLockerFlag != 0L)
		{
			targetFPS = _highFPS;
		}
		nLocker++;
		return num;
	}

	public static int FreeHighFPSLocker(int lockerId)
	{
		if (lockerId >= 0)
		{
			nLocker--;
			_activeFPSLockerFlag &= (ulong)(~(1L << lockerId));
			if (_activeFPSLockerFlag == 0L)
			{
				targetFPS = _normalFPS;
			}
			_freeFPSLockerList.Push(lockerId);
			return -1;
		}
		return lockerId;
	}

	public static void AcquireHighFPSLockerForSeconds(float seconds)
	{
		DynamicFPSAutoHighBehaviour.AcquireHighFPSLockerForSeconds(seconds);
	}

	public static void AcquireHighFPSLockerForChildrenScrollComponents(GameObject go)
	{
		UIScrollComponentValueChangedCheckStrategy.AcquireHighFPSLockerForChildren(go);
	}

	public static void FreeHighFPSLockerForChildrenScrollComponents(GameObject go)
	{
		UIScrollComponentValueChangedCheckStrategy.FreeHighFPSLockerForChildren(go);
	}

	public static void AcquireHighFPSLockerGameObject(GameObject go)
	{
		GameObjectNeedHighFPSCheckStrategy.AcquireHighFPSLockerForGameObject(go);
	}

	public static void FreeHighFPSLockerForGameObject(GameObject go)
	{
		GameObjectNeedHighFPSCheckStrategy.FreeHighFPSLockerForGameObject(go);
	}
}

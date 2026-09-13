using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine;

public sealed class CinemachineCore
{
	public enum Stage
	{
		Body,
		Aim,
		Noise,
		Finalize
	}

	public delegate float AxisInputDelegate(string axisName);

	private class UpdateStatus
	{
		public int lastUpdateFrame;

		public int lastUpdateFixedFrame;

		public UpdateTracker.UpdateClock lastUpdateMode;

		public UpdateStatus()
		{
			lastUpdateFrame = -2;
			lastUpdateFixedFrame = 0;
			lastUpdateMode = UpdateTracker.UpdateClock.Late;
		}
	}

	internal enum UpdateFilter
	{
		Fixed = 0,
		Late = 1,
		Smart = 8,
		SmartFixed = 8,
		SmartLate = 9
	}

	public static readonly int kStreamingVersion = 20170927;

	public static readonly string kVersionString = "2.2.8";

	private static CinemachineCore sInstance = null;

	public static bool sShowHiddenObjects = false;

	public static AxisInputDelegate GetInputAxis = Input.GetAxis;

	public static CinemachineBrain.BrainEvent CameraUpdatedEvent = new CinemachineBrain.BrainEvent();

	private List<CinemachineBrain> mActiveBrains = new List<CinemachineBrain>();

	private List<CinemachineVirtualCameraBase> mActiveCameras = new List<CinemachineVirtualCameraBase>();

	private List<List<CinemachineVirtualCameraBase>> mAllCameras = new List<List<CinemachineVirtualCameraBase>>();

	private CinemachineVirtualCameraBase mRoundRobinVcamLastFrame;

	private static float mLastUpdateTime;

	private static Dictionary<CinemachineVirtualCameraBase, UpdateStatus> mUpdateStatus;

	public static CinemachineCore Instance
	{
		get
		{
			if (sInstance == null)
			{
				sInstance = new CinemachineCore();
			}
			return sInstance;
		}
	}

	public int BrainCount => mActiveBrains.Count;

	public int VirtualCameraCount => mActiveCameras.Count;

	private static int FixedFrameCount { get; set; }

	internal UpdateFilter CurrentUpdateFilter { get; set; }

	public CinemachineBrain GetActiveBrain(int index)
	{
		return mActiveBrains[index];
	}

	internal void AddActiveBrain(CinemachineBrain brain)
	{
		RemoveActiveBrain(brain);
		mActiveBrains.Insert(0, brain);
	}

	internal void RemoveActiveBrain(CinemachineBrain brain)
	{
		mActiveBrains.Remove(brain);
	}

	public CinemachineVirtualCameraBase GetVirtualCamera(int index)
	{
		return mActiveCameras[index];
	}

	internal void AddActiveCamera(CinemachineVirtualCameraBase vcam)
	{
		RemoveActiveCamera(vcam);
		int i;
		for (i = 0; i < mActiveCameras.Count && vcam.Priority < mActiveCameras[i].Priority; i++)
		{
		}
		mActiveCameras.Insert(i, vcam);
	}

	internal void RemoveActiveCamera(CinemachineVirtualCameraBase vcam)
	{
		mActiveCameras.Remove(vcam);
	}

	internal void CameraAwakened(CinemachineVirtualCameraBase vcam)
	{
		int num = 0;
		for (ICinemachineCamera parentCamera = vcam.ParentCamera; parentCamera != null; parentCamera = parentCamera.ParentCamera)
		{
			num++;
		}
		while (mAllCameras.Count <= num)
		{
			mAllCameras.Add(new List<CinemachineVirtualCameraBase>());
		}
		mAllCameras[num].Add(vcam);
	}

	internal void CameraDestroyed(CinemachineVirtualCameraBase vcam)
	{
		for (int i = 0; i < mAllCameras.Count; i++)
		{
			mAllCameras[i].Remove(vcam);
		}
	}

	internal void UpdateAllActiveVirtualCameras(int layerMask, Vector3 worldUp, float deltaTime)
	{
		UpdateFilter currentUpdateFilter = CurrentUpdateFilter;
		bool flag = currentUpdateFilter != UpdateFilter.Smart;
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = mRoundRobinVcamLastFrame;
		float time = Time.time;
		if (time != mLastUpdateTime)
		{
			mLastUpdateTime = time;
			if ((currentUpdateFilter & (UpdateFilter)(-9)) == 0)
			{
				FixedFrameCount++;
			}
		}
		for (int num = mAllCameras.Count - 1; num >= 0; num--)
		{
			List<CinemachineVirtualCameraBase> list = mAllCameras[num];
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				CinemachineVirtualCameraBase cinemachineVirtualCameraBase2 = list[num2];
				if (flag && cinemachineVirtualCameraBase2 == mRoundRobinVcamLastFrame)
				{
					cinemachineVirtualCameraBase = null;
				}
				if (cinemachineVirtualCameraBase2 == null)
				{
					list.RemoveAt(num2);
				}
				else if (cinemachineVirtualCameraBase2.m_StandbyUpdate == CinemachineVirtualCameraBase.StandbyUpdateMode.Always || IsLive(cinemachineVirtualCameraBase2))
				{
					if (((1 << cinemachineVirtualCameraBase2.gameObject.layer) & layerMask) != 0)
					{
						UpdateVirtualCamera(cinemachineVirtualCameraBase2, worldUp, deltaTime);
					}
				}
				else if (cinemachineVirtualCameraBase == null && mRoundRobinVcamLastFrame != cinemachineVirtualCameraBase2 && flag && cinemachineVirtualCameraBase2.m_StandbyUpdate != CinemachineVirtualCameraBase.StandbyUpdateMode.Never && cinemachineVirtualCameraBase2.isActiveAndEnabled)
				{
					CurrentUpdateFilter &= (UpdateFilter)(-9);
					UpdateVirtualCamera(cinemachineVirtualCameraBase2, worldUp, deltaTime);
					CurrentUpdateFilter = currentUpdateFilter;
					cinemachineVirtualCameraBase = cinemachineVirtualCameraBase2;
				}
			}
		}
		if (flag)
		{
			if (cinemachineVirtualCameraBase == mRoundRobinVcamLastFrame)
			{
				cinemachineVirtualCameraBase = null;
			}
			mRoundRobinVcamLastFrame = cinemachineVirtualCameraBase;
		}
	}

	internal void UpdateVirtualCamera(CinemachineVirtualCameraBase vcam, Vector3 worldUp, float deltaTime)
	{
		if (vcam == null)
		{
			return;
		}
		bool num = (CurrentUpdateFilter & UpdateFilter.Smart) == UpdateFilter.Smart;
		UpdateTracker.UpdateClock updateClock = (UpdateTracker.UpdateClock)(CurrentUpdateFilter & (UpdateFilter)(-9));
		if (num)
		{
			Transform updateTarget = GetUpdateTarget(vcam);
			if (updateTarget == null || UpdateTracker.GetPreferredUpdate(updateTarget) != updateClock)
			{
				return;
			}
		}
		if (mUpdateStatus == null)
		{
			mUpdateStatus = new Dictionary<CinemachineVirtualCameraBase, UpdateStatus>();
		}
		if (!mUpdateStatus.TryGetValue(vcam, out var value))
		{
			value = new UpdateStatus();
			mUpdateStatus.Add(vcam, value);
		}
		int num2 = ((updateClock == UpdateTracker.UpdateClock.Late) ? (Time.frameCount - value.lastUpdateFrame) : (FixedFrameCount - value.lastUpdateFixedFrame));
		if (!(deltaTime >= 0f) || num2 != 0 || value.lastUpdateMode != updateClock)
		{
			if (num2 != 1)
			{
				deltaTime = -1f;
			}
			vcam.InternalUpdateCameraState(worldUp, deltaTime);
			value.lastUpdateFrame = Time.frameCount;
			value.lastUpdateFixedFrame = FixedFrameCount;
			value.lastUpdateMode = updateClock;
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void InitializeModule()
	{
		mUpdateStatus = new Dictionary<CinemachineVirtualCameraBase, UpdateStatus>();
	}

	private static Transform GetUpdateTarget(CinemachineVirtualCameraBase vcam)
	{
		if (vcam == null || vcam.gameObject == null)
		{
			return null;
		}
		Transform lookAt = vcam.LookAt;
		if (lookAt != null)
		{
			return lookAt;
		}
		lookAt = vcam.Follow;
		if (lookAt != null)
		{
			return lookAt;
		}
		return vcam.transform;
	}

	internal UpdateTracker.UpdateClock GetVcamUpdateStatus(CinemachineVirtualCameraBase vcam)
	{
		if (mUpdateStatus == null || !mUpdateStatus.TryGetValue(vcam, out var value))
		{
			return UpdateTracker.UpdateClock.Late;
		}
		return value.lastUpdateMode;
	}

	public bool IsLive(ICinemachineCamera vcam)
	{
		if (vcam != null)
		{
			for (int i = 0; i < BrainCount; i++)
			{
				CinemachineBrain activeBrain = GetActiveBrain(i);
				if (activeBrain != null && activeBrain.IsLive(vcam))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void GenerateCameraActivationEvent(ICinemachineCamera vcam, ICinemachineCamera vcamFrom)
	{
		if (vcam == null)
		{
			return;
		}
		for (int i = 0; i < BrainCount; i++)
		{
			CinemachineBrain activeBrain = GetActiveBrain(i);
			if (activeBrain != null && activeBrain.IsLive(vcam))
			{
				activeBrain.m_CameraActivatedEvent.Invoke(vcam, vcamFrom);
			}
		}
	}

	public void GenerateCameraCutEvent(ICinemachineCamera vcam)
	{
		if (vcam == null)
		{
			return;
		}
		for (int i = 0; i < BrainCount; i++)
		{
			CinemachineBrain activeBrain = GetActiveBrain(i);
			if (activeBrain != null && activeBrain.IsLive(vcam))
			{
				activeBrain.m_CameraCutEvent.Invoke(activeBrain);
			}
		}
	}

	public CinemachineBrain FindPotentialTargetBrain(CinemachineVirtualCameraBase vcam)
	{
		if (vcam != null)
		{
			int brainCount = BrainCount;
			for (int i = 0; i < brainCount; i++)
			{
				CinemachineBrain activeBrain = GetActiveBrain(i);
				if (activeBrain != null && activeBrain.OutputCamera != null && activeBrain.IsLive(vcam))
				{
					return activeBrain;
				}
			}
			int num = 1 << vcam.gameObject.layer;
			for (int j = 0; j < brainCount; j++)
			{
				CinemachineBrain activeBrain2 = GetActiveBrain(j);
				if (activeBrain2 != null && activeBrain2.OutputCamera != null && (activeBrain2.OutputCamera.cullingMask & num) != 0)
				{
					return activeBrain2;
				}
			}
		}
		return null;
	}
}

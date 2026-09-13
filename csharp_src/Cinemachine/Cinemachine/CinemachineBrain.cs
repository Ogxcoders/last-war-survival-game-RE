using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[DisallowMultipleComponent]
[ExecuteAlways]
[AddComponentMenu("Cinemachine/CinemachineBrain")]
[SaveDuringPlay]
public class CinemachineBrain : MonoBehaviour
{
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	public enum UpdateMethod
	{
		FixedUpdate,
		LateUpdate,
		SmartUpdate
	}

	[Serializable]
	public class BrainEvent : UnityEvent<CinemachineBrain>
	{
	}

	[Serializable]
	public class VcamActivatedEvent : UnityEvent<ICinemachineCamera, ICinemachineCamera>
	{
	}

	private class BrainFrame
	{
		public int id;

		public CinemachineBlend blend = new CinemachineBlend(null, null, null, 0f, 0f);

		public CinemachineBlend workingBlend = new CinemachineBlend(null, null, null, 0f, 0f);

		public BlendSourceVirtualCamera workingBlendSource = new BlendSourceVirtualCamera(null);

		public float deltaTimeOverride;

		public float timeOfOverride;

		public bool Active => blend.IsValid;

		public bool TimeOverrideExpired => Time.realtimeSinceStartup - timeOfOverride > Time.maximumDeltaTime;
	}

	[Tooltip("When enabled, the current camera and blend will be indicated in the game window, for debugging")]
	public bool m_ShowDebugText;

	[Tooltip("When enabled, the camera's frustum will be shown at all times in the scene view")]
	public bool m_ShowCameraFrustum = true;

	[Tooltip("When enabled, the cameras will always respond in real-time to user input and damping, even if the game is running in slow motion")]
	public bool m_IgnoreTimeScale;

	[Tooltip("If set, this object's Y axis will define the worldspace Up vector for all the virtual cameras.  This is useful for instance in top-down game environments.  If not set, Up is worldspace Y.  Setting this appropriately is important, because Virtual Cameras don't like looking straight up or straight down.")]
	public Transform m_WorldUpOverride;

	[Tooltip("Use FixedUpdate if all your targets are animated during FixedUpdate (e.g. RigidBodies), LateUpdate if all your targets are animated during the normal Update loop, and SmartUpdate if you want Cinemachine to do the appropriate thing on a per-target basis.  SmartUpdate is the recommended setting")]
	public UpdateMethod m_UpdateMethod = UpdateMethod.SmartUpdate;

	[CinemachineBlendDefinitionProperty]
	[Tooltip("The blend that is used in cases where you haven't explicitly defined a blend between two Virtual Cameras")]
	public CinemachineBlendDefinition m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);

	[Tooltip("This is the asset that contains custom settings for blends between specific virtual cameras in your scene")]
	public CinemachineBlenderSettings m_CustomBlends;

	private Camera m_OutputCamera;

	[Tooltip("This event will fire whenever a virtual camera goes live and there is no blend")]
	public BrainEvent m_CameraCutEvent = new BrainEvent();

	[Tooltip("This event will fire whenever a virtual camera goes live.  If a blend is involved, then the event will fire on the first frame of the blend.")]
	public VcamActivatedEvent m_CameraActivatedEvent = new VcamActivatedEvent();

	private static ICinemachineCamera mSoloCamera;

	private Coroutine mPhysicsCoroutine;

	private WaitForFixedUpdate mWaitForFixedUpdate = new WaitForFixedUpdate();

	private List<BrainFrame> mFrameStack = new List<BrainFrame>();

	private int mNextFrameId = 1;

	private CinemachineBlend mCurrentLiveCameras = new CinemachineBlend(null, null, null, 0f, 0f);

	private ICinemachineCamera mActiveCameraPreviousFrame;

	public Camera OutputCamera
	{
		get
		{
			if (m_OutputCamera == null && !Application.isPlaying)
			{
				m_OutputCamera = GetComponent<Camera>();
			}
			return m_OutputCamera;
		}
	}

	public static ICinemachineCamera SoloCamera
	{
		get
		{
			return mSoloCamera;
		}
		set
		{
			if (value != null && !CinemachineCore.Instance.IsLive(value))
			{
				value.OnTransitionFromCamera(null, Vector3.up, Time.deltaTime);
			}
			mSoloCamera = value;
		}
	}

	public Vector3 DefaultWorldUp
	{
		get
		{
			if (!(m_WorldUpOverride != null))
			{
				return Vector3.up;
			}
			return m_WorldUpOverride.transform.up;
		}
	}

	public ICinemachineCamera ActiveVirtualCamera
	{
		get
		{
			if (SoloCamera != null)
			{
				return SoloCamera;
			}
			return DeepCamBFromBlend(mCurrentLiveCameras);
		}
	}

	public bool IsBlending => ActiveBlend != null;

	public CinemachineBlend ActiveBlend
	{
		get
		{
			if (SoloCamera != null)
			{
				return null;
			}
			if (mCurrentLiveCameras.CamA == null || mCurrentLiveCameras.IsComplete)
			{
				return null;
			}
			return mCurrentLiveCameras;
		}
	}

	public CameraState CurrentCameraState { get; private set; }

	public static Color GetSoloGUIColor()
	{
		return Color.Lerp(Color.red, Color.yellow, 0.8f);
	}

	private void OnEnable()
	{
		if (mFrameStack.Count == 0)
		{
			mFrameStack.Add(new BrainFrame());
		}
		m_OutputCamera = GetComponent<Camera>();
		CinemachineCore.Instance.AddActiveBrain(this);
		CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(OnGuiHandler));
		CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Combine(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(OnGuiHandler));
		mPhysicsCoroutine = StartCoroutine(AfterPhysics());
	}

	private void OnDisable()
	{
		CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(OnGuiHandler));
		CinemachineCore.Instance.RemoveActiveBrain(this);
		mFrameStack.Clear();
		StopCoroutine(mPhysicsCoroutine);
	}

	private void Start()
	{
		UpdateVirtualCameras(CinemachineCore.UpdateFilter.Late, -1f);
	}

	private void OnGuiHandler()
	{
		if (!m_ShowDebugText)
		{
			CinemachineDebug.ReleaseScreenPos(this);
			return;
		}
		StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
		Color color = GUI.color;
		stringBuilder.Length = 0;
		stringBuilder.Append("CM ");
		stringBuilder.Append(base.gameObject.name);
		stringBuilder.Append(": ");
		if (SoloCamera != null)
		{
			stringBuilder.Append("SOLO ");
			GUI.color = GetSoloGUIColor();
		}
		if (IsBlending)
		{
			stringBuilder.Append(ActiveBlend.Description);
		}
		else
		{
			ICinemachineCamera activeVirtualCamera = ActiveVirtualCamera;
			if (activeVirtualCamera == null)
			{
				stringBuilder.Append("(none)");
			}
			else
			{
				stringBuilder.Append("[");
				stringBuilder.Append(activeVirtualCamera.Name);
				stringBuilder.Append("]");
			}
		}
		string text = stringBuilder.ToString();
		GUI.Label(CinemachineDebug.GetScreenPos(this, text, GUI.skin.box), text, GUI.skin.box);
		GUI.color = color;
		CinemachineDebug.ReturnToPool(stringBuilder);
	}

	private IEnumerator AfterPhysics()
	{
		while (true)
		{
			yield return mWaitForFixedUpdate;
			if (m_UpdateMethod != UpdateMethod.LateUpdate)
			{
				CinemachineCore.UpdateFilter updateFilter = CinemachineCore.UpdateFilter.Fixed;
				if (m_UpdateMethod == UpdateMethod.SmartUpdate)
				{
					UpdateTracker.OnUpdate(UpdateTracker.UpdateClock.Fixed);
					updateFilter = CinemachineCore.UpdateFilter.Smart;
				}
				UpdateVirtualCameras(updateFilter, GetEffectiveDeltaTime(fixedDelta: true));
			}
		}
	}

	private void LateUpdate()
	{
		float effectiveDeltaTime = GetEffectiveDeltaTime(fixedDelta: false);
		UpdateFrame0(effectiveDeltaTime);
		UpdateCurrentLiveCameras();
		if (m_UpdateMethod == UpdateMethod.FixedUpdate)
		{
			CinemachineCore.Instance.CurrentUpdateFilter = CinemachineCore.UpdateFilter.Fixed;
			if (SoloCamera != null)
			{
				SoloCamera.UpdateCameraState(DefaultWorldUp, effectiveDeltaTime);
			}
			mCurrentLiveCameras.UpdateCameraState(DefaultWorldUp, effectiveDeltaTime);
		}
		else
		{
			CinemachineCore.UpdateFilter updateFilter = CinemachineCore.UpdateFilter.Late;
			if (m_UpdateMethod == UpdateMethod.SmartUpdate)
			{
				UpdateTracker.OnUpdate(UpdateTracker.UpdateClock.Late);
				updateFilter = CinemachineCore.UpdateFilter.SmartLate;
			}
			UpdateVirtualCameras(updateFilter, effectiveDeltaTime);
		}
		ProcessActiveCamera(effectiveDeltaTime);
	}

	private float GetEffectiveDeltaTime(bool fixedDelta)
	{
		if (SoloCamera != null)
		{
			return Time.unscaledDeltaTime;
		}
		if (!Application.isPlaying)
		{
			for (int num = mFrameStack.Count - 1; num > 0; num--)
			{
				BrainFrame brainFrame = mFrameStack[num];
				if (brainFrame.Active)
				{
					if (!brainFrame.TimeOverrideExpired)
					{
						return brainFrame.deltaTimeOverride;
					}
					return -1f;
				}
			}
			return -1f;
		}
		if (m_IgnoreTimeScale)
		{
			if (!fixedDelta)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.fixedDeltaTime;
		}
		if (!fixedDelta)
		{
			return Time.deltaTime;
		}
		return Time.fixedDeltaTime * Time.timeScale;
	}

	private void UpdateVirtualCameras(CinemachineCore.UpdateFilter updateFilter, float deltaTime)
	{
		CinemachineCore.Instance.CurrentUpdateFilter = updateFilter;
		Camera outputCamera = OutputCamera;
		CinemachineCore.Instance.UpdateAllActiveVirtualCameras((outputCamera == null) ? (-1) : outputCamera.cullingMask, DefaultWorldUp, deltaTime);
		if (SoloCamera != null)
		{
			SoloCamera.UpdateCameraState(DefaultWorldUp, deltaTime);
		}
		mCurrentLiveCameras.UpdateCameraState(DefaultWorldUp, deltaTime);
		updateFilter = CinemachineCore.UpdateFilter.Late;
		if (Application.isPlaying)
		{
			if (m_UpdateMethod == UpdateMethod.SmartUpdate)
			{
				updateFilter |= CinemachineCore.UpdateFilter.Smart;
			}
			else if (m_UpdateMethod == UpdateMethod.FixedUpdate)
			{
				updateFilter = CinemachineCore.UpdateFilter.Fixed;
			}
		}
		CinemachineCore.Instance.CurrentUpdateFilter = updateFilter;
	}

	private static ICinemachineCamera DeepCamBFromBlend(CinemachineBlend blend)
	{
		ICinemachineCamera camB;
		for (camB = blend.CamB; camB != null; camB = blendSourceVirtualCamera.Blend.CamB)
		{
			if (!camB.IsValid)
			{
				return null;
			}
			if (!(camB is BlendSourceVirtualCamera blendSourceVirtualCamera))
			{
				break;
			}
		}
		return camB;
	}

	private int GetBrainFrame(int withId)
	{
		_ = mFrameStack.Count;
		for (int num = mFrameStack.Count - 1; num > 0; num--)
		{
			if (mFrameStack[num].id == withId)
			{
				return num;
			}
		}
		mFrameStack.Add(new BrainFrame
		{
			id = withId
		});
		return mFrameStack.Count - 1;
	}

	internal int SetCameraOverride(int overrideId, ICinemachineCamera camA, ICinemachineCamera camB, float weightB, float deltaTime)
	{
		if (overrideId < 0)
		{
			overrideId = mNextFrameId++;
		}
		BrainFrame brainFrame = mFrameStack[GetBrainFrame(overrideId)];
		brainFrame.deltaTimeOverride = deltaTime;
		brainFrame.timeOfOverride = Time.realtimeSinceStartup;
		brainFrame.blend.CamA = camA;
		brainFrame.blend.CamB = camB;
		brainFrame.blend.BlendCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		brainFrame.blend.Duration = 1f;
		brainFrame.blend.TimeInBlend = weightB;
		return overrideId;
	}

	internal void ReleaseCameraOverride(int overrideId)
	{
		for (int num = mFrameStack.Count - 1; num > 0; num--)
		{
			if (mFrameStack[num].id == overrideId)
			{
				mFrameStack.RemoveAt(num);
				break;
			}
		}
	}

	private void ProcessActiveCamera(float deltaTime)
	{
		ICinemachineCamera activeVirtualCamera = ActiveVirtualCamera;
		if (activeVirtualCamera != null)
		{
			if (activeVirtualCamera != mActiveCameraPreviousFrame)
			{
				activeVirtualCamera.OnTransitionFromCamera(mActiveCameraPreviousFrame, DefaultWorldUp, deltaTime);
				if (m_CameraActivatedEvent != null)
				{
					m_CameraActivatedEvent.Invoke(activeVirtualCamera, mActiveCameraPreviousFrame);
				}
				if (m_CameraCutEvent != null && !IsBlending)
				{
					m_CameraCutEvent.Invoke(this);
				}
			}
			PushStateToUnityCamera((SoloCamera != null) ? SoloCamera.State : mCurrentLiveCameras.State);
		}
		mActiveCameraPreviousFrame = activeVirtualCamera;
	}

	private void UpdateFrame0(float deltaTime)
	{
		BrainFrame brainFrame = mFrameStack[0];
		ICinemachineCamera cinemachineCamera = TopCameraFromPriorityQueue();
		ICinemachineCamera camB = brainFrame.blend.CamB;
		if (cinemachineCamera != camB)
		{
			if ((UnityEngine.Object)cinemachineCamera != null && (UnityEngine.Object)camB != null && deltaTime >= 0f)
			{
				CinemachineBlendDefinition cinemachineBlendDefinition = LookupBlend(camB, cinemachineCamera);
				if (cinemachineBlendDefinition.BlendCurve != null && cinemachineBlendDefinition.m_Time > 0f)
				{
					if (brainFrame.blend.IsComplete)
					{
						brainFrame.blend.CamA = camB;
					}
					else
					{
						brainFrame.blend.CamA = new BlendSourceVirtualCamera(new CinemachineBlend(brainFrame.blend.CamA, brainFrame.blend.CamB, brainFrame.blend.BlendCurve, brainFrame.blend.Duration, brainFrame.blend.TimeInBlend));
					}
					brainFrame.blend.BlendCurve = cinemachineBlendDefinition.BlendCurve;
					brainFrame.blend.Duration = cinemachineBlendDefinition.m_Time;
					brainFrame.blend.TimeInBlend = 0f;
				}
			}
			brainFrame.blend.CamB = cinemachineCamera;
		}
		if (brainFrame.blend.CamA != null)
		{
			brainFrame.blend.TimeInBlend += ((deltaTime >= 0f) ? deltaTime : brainFrame.blend.Duration);
			if (brainFrame.blend.IsComplete)
			{
				brainFrame.blend.CamA = null;
				brainFrame.blend.BlendCurve = null;
				brainFrame.blend.Duration = 0f;
				brainFrame.blend.TimeInBlend = 0f;
			}
		}
	}

	private void UpdateCurrentLiveCameras()
	{
		int index = 0;
		for (int i = 0; i < mFrameStack.Count; i++)
		{
			BrainFrame brainFrame = mFrameStack[i];
			if (i != 0 && !brainFrame.Active)
			{
				continue;
			}
			brainFrame.workingBlend.CamA = brainFrame.blend.CamA;
			brainFrame.workingBlend.CamB = brainFrame.blend.CamB;
			brainFrame.workingBlend.BlendCurve = brainFrame.blend.BlendCurve;
			brainFrame.workingBlend.Duration = brainFrame.blend.Duration;
			brainFrame.workingBlend.TimeInBlend = brainFrame.blend.TimeInBlend;
			if (i > 0 && !brainFrame.blend.IsComplete)
			{
				if (brainFrame.workingBlend.CamA == null)
				{
					if (mFrameStack[index].blend.IsComplete)
					{
						brainFrame.workingBlend.CamA = mFrameStack[index].blend.CamB;
					}
					else
					{
						brainFrame.workingBlendSource.Blend = mFrameStack[index].workingBlend;
						brainFrame.workingBlend.CamA = brainFrame.workingBlendSource;
					}
				}
				else if (brainFrame.workingBlend.CamB == null)
				{
					if (mFrameStack[index].blend.IsComplete)
					{
						brainFrame.workingBlend.CamB = mFrameStack[index].blend.CamB;
					}
					else
					{
						brainFrame.workingBlendSource.Blend = mFrameStack[index].workingBlend;
						brainFrame.workingBlend.CamB = brainFrame.workingBlendSource;
					}
				}
			}
			index = i;
		}
		CinemachineBlend workingBlend = mFrameStack[index].workingBlend;
		mCurrentLiveCameras.CamA = workingBlend.CamA;
		mCurrentLiveCameras.CamB = workingBlend.CamB;
		mCurrentLiveCameras.BlendCurve = workingBlend.BlendCurve;
		mCurrentLiveCameras.Duration = workingBlend.Duration;
		mCurrentLiveCameras.TimeInBlend = workingBlend.TimeInBlend;
	}

	public bool IsLive(ICinemachineCamera vcam)
	{
		if (SoloCamera == vcam)
		{
			return true;
		}
		if (mCurrentLiveCameras.Uses(vcam))
		{
			return true;
		}
		ICinemachineCamera parentCamera = vcam.ParentCamera;
		while (parentCamera != null && parentCamera.IsLiveChild(vcam))
		{
			if (mCurrentLiveCameras.Uses(parentCamera))
			{
				return true;
			}
			vcam = parentCamera;
			parentCamera = vcam.ParentCamera;
		}
		return false;
	}

	private ICinemachineCamera TopCameraFromPriorityQueue()
	{
		CinemachineCore instance = CinemachineCore.Instance;
		Camera outputCamera = OutputCamera;
		int num = ((outputCamera == null) ? (-1) : outputCamera.cullingMask);
		int virtualCameraCount = instance.VirtualCameraCount;
		for (int i = 0; i < virtualCameraCount; i++)
		{
			CinemachineVirtualCameraBase virtualCamera = instance.GetVirtualCamera(i);
			GameObject gameObject = ((virtualCamera != null) ? virtualCamera.gameObject : null);
			if (gameObject != null && (num & (1 << gameObject.layer)) != 0)
			{
				return virtualCamera;
			}
		}
		return null;
	}

	private CinemachineBlendDefinition LookupBlend(ICinemachineCamera fromKey, ICinemachineCamera toKey)
	{
		CinemachineBlendDefinition cinemachineBlendDefinition = m_DefaultBlend;
		if (m_CustomBlends != null)
		{
			string fromCameraName = ((fromKey != null) ? fromKey.Name : string.Empty);
			string toCameraName = ((toKey != null) ? toKey.Name : string.Empty);
			cinemachineBlendDefinition = m_CustomBlends.GetBlendForVirtualCameras(fromCameraName, toCameraName, cinemachineBlendDefinition);
		}
		return cinemachineBlendDefinition;
	}

	private void PushStateToUnityCamera(CameraState state)
	{
		CurrentCameraState = state;
		if ((state.BlendHint & CameraState.BlendHintValue.NoPosition) == 0)
		{
			base.transform.position = state.FinalPosition;
		}
		if ((state.BlendHint & CameraState.BlendHintValue.NoOrientation) == 0)
		{
			base.transform.rotation = state.FinalOrientation;
		}
		if ((state.BlendHint & CameraState.BlendHintValue.NoLens) == 0)
		{
			Camera outputCamera = OutputCamera;
			if (outputCamera != null)
			{
				outputCamera.nearClipPlane = state.Lens.NearClipPlane;
				outputCamera.farClipPlane = state.Lens.FarClipPlane;
				outputCamera.fieldOfView = state.Lens.FieldOfView;
				if (outputCamera.orthographic)
				{
					outputCamera.orthographicSize = state.Lens.OrthographicSize;
				}
				else
				{
					outputCamera.usePhysicalProperties = state.Lens.IsPhysicalCamera;
					outputCamera.lensShift = state.Lens.LensShift;
				}
			}
		}
		if (CinemachineCore.CameraUpdatedEvent != null)
		{
			CinemachineCore.CameraUpdatedEvent.Invoke(this);
		}
	}
}

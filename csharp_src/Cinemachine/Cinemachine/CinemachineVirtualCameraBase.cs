using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[SaveDuringPlay]
public abstract class CinemachineVirtualCameraBase : MonoBehaviour, ICinemachineCamera
{
	public enum StandbyUpdateMode
	{
		Never,
		Always,
		RoundRobin
	}

	public enum BlendHint
	{
		None,
		SphericalPosition,
		CylindricalPosition,
		ScreenSpaceAimWhenTargetsDiffer
	}

	[Serializable]
	public struct TransitionParams
	{
		[Tooltip("Hint for blending positions to and from this virtual camera")]
		[FormerlySerializedAs("m_PositionBlending")]
		public BlendHint m_BlendHint;

		[Tooltip("When this virtual camera goes Live, attempt to force the position to be the same as the current position of the Unity Camera")]
		public bool m_InheritPosition;

		[Tooltip("This event fires when the virtual camera goes Live")]
		public CinemachineBrain.VcamActivatedEvent m_OnCameraLive;
	}

	[HideInInspector]
	[SerializeField]
	[NoSaveDuringPlay]
	public string[] m_ExcludedPropertiesInInspector = new string[1] { "m_Script" };

	[HideInInspector]
	[SerializeField]
	[NoSaveDuringPlay]
	public CinemachineCore.Stage[] m_LockStageInInspector;

	private int m_ValidatingStreamVersion;

	private bool m_OnValidateCalled;

	[HideInInspector]
	[SerializeField]
	[NoSaveDuringPlay]
	private int m_StreamingVersion;

	[NoSaveDuringPlay]
	[Tooltip("The priority will determine which camera becomes active based on the state of other cameras and this camera.  Higher numbers have greater priority.")]
	public int m_Priority = 10;

	[Tooltip("When the virtual camera is not live, this is how often the virtual camera will be updated.  Set this to tune for performance. Most of the time Never is fine, unless the virtual camera is doing shot evaluation.")]
	public StandbyUpdateMode m_StandbyUpdate = StandbyUpdateMode.RoundRobin;

	private List<CinemachineExtension> mExtensions;

	private bool m_previousStateIsValid;

	private Transform m_previousLookAtTarget;

	private Transform m_previousFollowTarget;

	private bool mSlaveStatusUpdated;

	private CinemachineVirtualCameraBase m_parentVcam;

	private int m_QueuePriority = int.MaxValue;

	public int ValidatingStreamVersion
	{
		get
		{
			if (!m_OnValidateCalled)
			{
				return CinemachineCore.kStreamingVersion;
			}
			return m_ValidatingStreamVersion;
		}
		private set
		{
			m_ValidatingStreamVersion = value;
		}
	}

	public string Name => base.name;

	public virtual string Description => "";

	public int Priority
	{
		get
		{
			return m_Priority;
		}
		set
		{
			m_Priority = value;
		}
	}

	public GameObject VirtualCameraGameObject
	{
		get
		{
			if (this == null)
			{
				return null;
			}
			return base.gameObject;
		}
	}

	public bool IsValid => !(this == null);

	public abstract CameraState State { get; }

	public ICinemachineCamera ParentCamera
	{
		get
		{
			if (!mSlaveStatusUpdated || !Application.isPlaying)
			{
				UpdateSlaveStatus();
			}
			return m_parentVcam;
		}
	}

	public abstract Transform LookAt { get; set; }

	public abstract Transform Follow { get; set; }

	public bool PreviousStateIsValid
	{
		get
		{
			if (LookAt != m_previousLookAtTarget)
			{
				m_previousLookAtTarget = LookAt;
				m_previousStateIsValid = false;
			}
			if (Follow != m_previousFollowTarget)
			{
				m_previousFollowTarget = Follow;
				m_previousStateIsValid = false;
			}
			return m_previousStateIsValid;
		}
		set
		{
			m_previousStateIsValid = value;
		}
	}

	public virtual void AddExtension(CinemachineExtension extension)
	{
		if (mExtensions == null)
		{
			mExtensions = new List<CinemachineExtension>();
		}
		else
		{
			mExtensions.Remove(extension);
		}
		mExtensions.Add(extension);
	}

	public virtual void RemoveExtension(CinemachineExtension extension)
	{
		if (mExtensions != null)
		{
			mExtensions.Remove(extension);
		}
	}

	protected void InvokePostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState newState, float deltaTime)
	{
		if (mExtensions != null)
		{
			for (int i = 0; i < mExtensions.Count; i++)
			{
				CinemachineExtension cinemachineExtension = mExtensions[i];
				if (cinemachineExtension == null)
				{
					mExtensions.RemoveAt(i);
					i--;
				}
				else if (cinemachineExtension.enabled)
				{
					cinemachineExtension.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
				}
			}
		}
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = ParentCamera as CinemachineVirtualCameraBase;
		if (cinemachineVirtualCameraBase != null)
		{
			cinemachineVirtualCameraBase.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
		}
	}

	protected void ApplyPositionBlendMethod(ref CameraState state, BlendHint hint)
	{
		switch (hint)
		{
		case BlendHint.SphericalPosition:
			state.BlendHint |= CameraState.BlendHintValue.SphericalPositionBlend;
			break;
		case BlendHint.CylindricalPosition:
			state.BlendHint |= CameraState.BlendHintValue.CylindricalPositionBlend;
			break;
		case BlendHint.ScreenSpaceAimWhenTargetsDiffer:
			state.BlendHint |= CameraState.BlendHintValue.RadialAimBlend;
			break;
		}
	}

	public virtual bool IsLiveChild(ICinemachineCamera vcam)
	{
		return false;
	}

	public void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		CinemachineCore.Instance.UpdateVirtualCamera(this, worldUp, deltaTime);
	}

	public abstract void InternalUpdateCameraState(Vector3 worldUp, float deltaTime);

	public virtual void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			PreviousStateIsValid = false;
		}
	}

	protected virtual void OnDestroy()
	{
		CinemachineCore.Instance.RemoveActiveCamera(this);
	}

	protected virtual void OnTransformParentChanged()
	{
		CinemachineCore.Instance.CameraDestroyed(this);
		CinemachineCore.Instance.CameraAwakened(this);
		UpdateSlaveStatus();
		UpdateVcamPoolStatus();
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnValidate()
	{
		m_OnValidateCalled = true;
		ValidatingStreamVersion = m_StreamingVersion;
		m_StreamingVersion = CinemachineCore.kStreamingVersion;
	}

	protected virtual void OnEnable()
	{
		CinemachineVirtualCameraBase[] components = GetComponents<CinemachineVirtualCameraBase>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].enabled && components[i] != this)
			{
				Debug.LogError(Name + " has multiple CinemachineVirtualCameraBase-derived components.  Disabling " + GetType().Name + ".");
				base.enabled = false;
			}
		}
		UpdateSlaveStatus();
		UpdateVcamPoolStatus();
		if (!CinemachineCore.Instance.IsLive(this))
		{
			PreviousStateIsValid = false;
		}
		CinemachineCore.Instance.CameraAwakened(this);
	}

	protected virtual void OnDisable()
	{
		UpdateVcamPoolStatus();
		CinemachineCore.Instance.CameraDestroyed(this);
	}

	protected virtual void Update()
	{
		if (m_Priority != m_QueuePriority)
		{
			UpdateVcamPoolStatus();
		}
	}

	private void UpdateSlaveStatus()
	{
		mSlaveStatusUpdated = true;
		m_parentVcam = null;
		Transform parent = base.transform.parent;
		if (parent != null)
		{
			m_parentVcam = parent.GetComponent<CinemachineVirtualCameraBase>();
		}
	}

	protected Transform ResolveLookAt(Transform localLookAt)
	{
		Transform transform = localLookAt;
		if (transform == null && ParentCamera != null)
		{
			transform = ParentCamera.LookAt;
		}
		return transform;
	}

	protected Transform ResolveFollow(Transform localFollow)
	{
		Transform transform = localFollow;
		if (transform == null && ParentCamera != null)
		{
			transform = ParentCamera.Follow;
		}
		return transform;
	}

	private void UpdateVcamPoolStatus()
	{
		CinemachineCore.Instance.RemoveActiveCamera(this);
		if (m_parentVcam == null && base.isActiveAndEnabled)
		{
			CinemachineCore.Instance.AddActiveCamera(this);
		}
		m_QueuePriority = m_Priority;
	}

	public void MoveToTopOfPrioritySubqueue()
	{
		UpdateVcamPoolStatus();
	}

	public virtual void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
	{
		if (mExtensions != null)
		{
			for (int i = 0; i < mExtensions.Count; i++)
			{
				mExtensions[i].OnTargetObjectWarped(target, positionDelta);
			}
		}
	}

	protected CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, CinemachineBlendDefinition blendDef, CinemachineBlend activeBlend)
	{
		if (blendDef.BlendCurve == null || blendDef.m_Time <= 0f || (camA == null && camB == null))
		{
			return null;
		}
		if (activeBlend != null)
		{
			camA = new BlendSourceVirtualCamera(activeBlend);
		}
		else if (camA == null)
		{
			camA = new StaticPointVirtualCamera(State, "(none)");
		}
		return new CinemachineBlend(camA, camB, blendDef.BlendCurve, blendDef.m_Time, 0f);
	}

	protected CameraState PullStateFromVirtualCamera(Vector3 worldUp, ref LensSettings lens)
	{
		CameraState result = CameraState.Default;
		result.RawPosition = base.transform.position;
		result.RawOrientation = base.transform.rotation;
		result.ReferenceUp = worldUp;
		CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this);
		if (cinemachineBrain != null)
		{
			lens.SnapshotCameraReadOnlyProperties(cinemachineBrain.OutputCamera);
		}
		result.Lens = lens;
		return result;
	}
}

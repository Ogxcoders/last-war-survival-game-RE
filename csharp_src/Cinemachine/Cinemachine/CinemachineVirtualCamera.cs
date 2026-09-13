using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[DisallowMultipleComponent]
[ExecuteAlways]
[AddComponentMenu("Cinemachine/CinemachineVirtualCamera")]
public class CinemachineVirtualCamera : CinemachineVirtualCameraBase
{
	public delegate Transform CreatePipelineDelegate(CinemachineVirtualCamera vcam, string name, CinemachineComponentBase[] copyFrom);

	public delegate void DestroyPipelineDelegate(GameObject pipeline);

	[Tooltip("The object that the camera wants to look at (the Aim target).  If this is null, then the vcam's Transform orientation will define the camera's orientation.")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("The object that the camera wants to move with (the Body target).  If this is null, then the vcam's Transform position will define the camera's position.")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[FormerlySerializedAs("m_LensAttributes")]
	[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active.")]
	[LensSettingsProperty]
	public LensSettings m_Lens = LensSettings.Default;

	public TransitionParams m_Transitions;

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_BlendHint")]
	[FormerlySerializedAs("m_PositionBlending")]
	private BlendHint m_LegacyBlendHint;

	public const string PipelineName = "cm";

	public static CreatePipelineDelegate CreatePipelineOverride;

	public static DestroyPipelineDelegate DestroyPipelineOverride;

	private CameraState m_State = CameraState.Default;

	private CinemachineComponentBase[] m_ComponentPipeline;

	[SerializeField]
	[HideInInspector]
	private Transform m_ComponentOwner;

	private Transform mCachedLookAtTarget;

	private CinemachineVirtualCameraBase mCachedLookAtTargetVcam;

	public override CameraState State => m_State;

	public override Transform LookAt
	{
		get
		{
			return ResolveLookAt(m_LookAt);
		}
		set
		{
			m_LookAt = value;
		}
	}

	public override Transform Follow
	{
		get
		{
			return ResolveFollow(m_Follow);
		}
		set
		{
			m_Follow = value;
		}
	}

	public bool UserIsDragging { get; set; }

	public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!base.PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		m_State = CalculateNewState(worldUp, deltaTime);
		ApplyPositionBlendMethod(ref m_State, m_Transitions.m_BlendHint);
		if (!UserIsDragging)
		{
			if (Follow != null)
			{
				base.transform.position = State.RawPosition;
			}
			if (LookAt != null)
			{
				base.transform.rotation = State.RawOrientation;
			}
		}
		base.PreviousStateIsValid = true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		m_State = PullStateFromVirtualCamera(Vector3.up, ref m_Lens);
		InvalidateComponentPipeline();
		if (base.ValidatingStreamVersion < 20170927)
		{
			if (Follow != null && GetCinemachineComponent(CinemachineCore.Stage.Body) == null)
			{
				AddCinemachineComponent<CinemachineHardLockToTarget>();
			}
			if (LookAt != null && GetCinemachineComponent(CinemachineCore.Stage.Aim) == null)
			{
				AddCinemachineComponent<CinemachineHardLookAt>();
			}
		}
	}

	protected override void OnDestroy()
	{
		foreach (Transform item in base.transform)
		{
			if (item.GetComponent<CinemachinePipeline>() != null)
			{
				item.gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			}
		}
		base.OnDestroy();
	}

	protected override void OnValidate()
	{
		base.OnValidate();
		m_Lens.Validate();
		if (m_LegacyBlendHint != BlendHint.None)
		{
			m_Transitions.m_BlendHint = m_LegacyBlendHint;
			m_LegacyBlendHint = BlendHint.None;
		}
	}

	private void OnTransformChildrenChanged()
	{
		InvalidateComponentPipeline();
	}

	private void Reset()
	{
		DestroyPipeline();
	}

	private void DestroyPipeline()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in base.transform)
		{
			if (item.GetComponent<CinemachinePipeline>() != null)
			{
				list.Add(item);
			}
		}
		foreach (Transform item2 in list)
		{
			if (DestroyPipelineOverride != null)
			{
				DestroyPipelineOverride(item2.gameObject);
			}
			else
			{
				Object.Destroy(item2.gameObject);
			}
		}
		m_ComponentOwner = null;
		base.PreviousStateIsValid = false;
	}

	private Transform CreatePipeline(CinemachineVirtualCamera copyFrom)
	{
		CinemachineComponentBase[] copyFrom2 = null;
		if (copyFrom != null)
		{
			copyFrom.InvalidateComponentPipeline();
			copyFrom2 = copyFrom.GetComponentPipeline();
		}
		Transform transform = null;
		if (CreatePipelineOverride != null)
		{
			transform = CreatePipelineOverride(this, "cm", copyFrom2);
		}
		else
		{
			GameObject obj = new GameObject("cm");
			obj.transform.parent = base.transform;
			obj.AddComponent<CinemachinePipeline>();
			transform = obj.transform;
		}
		base.PreviousStateIsValid = false;
		return transform;
	}

	public void InvalidateComponentPipeline()
	{
		m_ComponentPipeline = null;
	}

	public Transform GetComponentOwner()
	{
		UpdateComponentPipeline();
		return m_ComponentOwner;
	}

	public CinemachineComponentBase[] GetComponentPipeline()
	{
		UpdateComponentPipeline();
		return m_ComponentPipeline;
	}

	public CinemachineComponentBase GetCinemachineComponent(CinemachineCore.Stage stage)
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline != null)
		{
			CinemachineComponentBase[] array = componentPipeline;
			foreach (CinemachineComponentBase cinemachineComponentBase in array)
			{
				if (cinemachineComponentBase.Stage == stage)
				{
					return cinemachineComponentBase;
				}
			}
		}
		return null;
	}

	public T GetCinemachineComponent<T>() where T : CinemachineComponentBase
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline != null)
		{
			CinemachineComponentBase[] array = componentPipeline;
			foreach (CinemachineComponentBase cinemachineComponentBase in array)
			{
				if (cinemachineComponentBase is T)
				{
					return cinemachineComponentBase as T;
				}
			}
		}
		return null;
	}

	public T AddCinemachineComponent<T>() where T : CinemachineComponentBase
	{
		Transform componentOwner = GetComponentOwner();
		CinemachineComponentBase[] components = componentOwner.GetComponents<CinemachineComponentBase>();
		T val = componentOwner.gameObject.AddComponent<T>();
		if (val != null && components != null)
		{
			CinemachineCore.Stage stage = val.Stage;
			for (int num = components.Length - 1; num >= 0; num--)
			{
				if (components[num].Stage == stage)
				{
					components[num].enabled = false;
					RuntimeUtility.DestroyObject(components[num]);
				}
			}
		}
		InvalidateComponentPipeline();
		return val;
	}

	public void DestroyCinemachineComponent<T>() where T : CinemachineComponentBase
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline == null)
		{
			return;
		}
		CinemachineComponentBase[] array = componentPipeline;
		foreach (CinemachineComponentBase cinemachineComponentBase in array)
		{
			if (cinemachineComponentBase is T)
			{
				cinemachineComponentBase.enabled = false;
				RuntimeUtility.DestroyObject(cinemachineComponentBase);
				InvalidateComponentPipeline();
			}
		}
	}

	private void UpdateComponentPipeline()
	{
		bool flag = false;
		if (m_ComponentOwner != null && m_ComponentPipeline != null)
		{
			return;
		}
		m_ComponentOwner = null;
		List<CinemachineComponentBase> list = new List<CinemachineComponentBase>();
		foreach (Transform item in base.transform)
		{
			if (!(item.GetComponent<CinemachinePipeline>() != null))
			{
				continue;
			}
			m_ComponentOwner = item;
			CinemachineComponentBase[] components = item.GetComponents<CinemachineComponentBase>();
			foreach (CinemachineComponentBase cinemachineComponentBase in components)
			{
				if (cinemachineComponentBase.enabled)
				{
					list.Add(cinemachineComponentBase);
				}
			}
		}
		flag = base.gameObject.scene.name == null;
		if (m_ComponentOwner == null && !flag)
		{
			m_ComponentOwner = CreatePipeline(null);
		}
		if (m_ComponentOwner != null)
		{
			SetFlagsForHiddenChild(m_ComponentOwner.gameObject);
		}
		if (m_ComponentOwner != null && m_ComponentOwner.gameObject != null)
		{
			list.Sort((CinemachineComponentBase c1, CinemachineComponentBase c2) => c1.Stage - c2.Stage);
			m_ComponentPipeline = list.ToArray();
		}
	}

	internal static void SetFlagsForHiddenChild(GameObject child)
	{
		if (child != null)
		{
			if (CinemachineCore.sShowHiddenObjects)
			{
				child.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			}
			else
			{
				child.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
			}
		}
	}

	private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
	{
		CameraState curState = PullStateFromVirtualCamera(worldUp, ref m_Lens);
		Transform lookAt = LookAt;
		if (lookAt != mCachedLookAtTarget)
		{
			mCachedLookAtTarget = lookAt;
			mCachedLookAtTargetVcam = null;
			if (lookAt != null)
			{
				mCachedLookAtTargetVcam = lookAt.GetComponent<CinemachineVirtualCameraBase>();
			}
		}
		if (lookAt != null)
		{
			if (mCachedLookAtTargetVcam != null)
			{
				curState.ReferenceLookAt = mCachedLookAtTargetVcam.State.FinalPosition;
			}
			else
			{
				curState.ReferenceLookAt = lookAt.position;
			}
		}
		CinemachineCore.Stage curStage = CinemachineCore.Stage.Body;
		UpdateComponentPipeline();
		bool hasAim = false;
		if (m_ComponentPipeline != null)
		{
			for (int i = 0; i < m_ComponentPipeline.Length; i++)
			{
				m_ComponentPipeline[i].PrePipelineMutateCameraState(ref curState);
				if (m_ComponentPipeline[i].Stage == CinemachineCore.Stage.Aim)
				{
					hasAim = true;
				}
			}
			for (int j = 0; j < m_ComponentPipeline.Length; j++)
			{
				curStage = AdvancePipelineStage(ref curState, deltaTime, curStage, (int)m_ComponentPipeline[j].Stage, hasAim);
				m_ComponentPipeline[j].MutateCameraState(ref curState, deltaTime);
			}
		}
		AdvancePipelineStage(ref curState, deltaTime, curStage, 4, hasAim);
		return curState;
	}

	private CinemachineCore.Stage AdvancePipelineStage(ref CameraState state, float deltaTime, CinemachineCore.Stage curStage, int maxStage, bool hasAim)
	{
		while ((int)curStage < maxStage)
		{
			if (!hasAim && curStage == CinemachineCore.Stage.Aim)
			{
				state.BlendHint |= CameraState.BlendHintValue.IgnoreLookAtTarget;
			}
			InvokePostPipelineStageCallback(this, curStage, ref state, deltaTime);
			curStage++;
		}
		return curStage;
	}

	internal void SetStateRawPosition(Vector3 pos)
	{
		m_State.RawPosition = pos;
	}

	public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
	{
		if (target == Follow)
		{
			base.transform.position += positionDelta;
			m_State.RawPosition += positionDelta;
		}
		UpdateComponentPipeline();
		if (m_ComponentPipeline != null)
		{
			for (int i = 0; i < m_ComponentPipeline.Length; i++)
			{
				m_ComponentPipeline[i].OnTargetObjectWarped(target, positionDelta);
			}
		}
		base.OnTargetObjectWarped(target, positionDelta);
	}

	public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		bool flag = false;
		if (m_Transitions.m_InheritPosition && fromCam != null)
		{
			base.transform.position = fromCam.State.RawPosition;
			base.PreviousStateIsValid = false;
			flag = true;
		}
		UpdateComponentPipeline();
		if (m_ComponentPipeline != null)
		{
			for (int i = 0; i < m_ComponentPipeline.Length; i++)
			{
				if (m_ComponentPipeline[i].OnTransitionFromCamera(fromCam, worldUp, deltaTime, ref m_Transitions))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			InternalUpdateCameraState(worldUp, deltaTime);
		}
		else
		{
			UpdateCameraState(worldUp, deltaTime);
		}
		if (m_Transitions.m_OnCameraLive != null)
		{
			m_Transitions.m_OnCameraLive.Invoke(this, fromCam);
		}
	}
}

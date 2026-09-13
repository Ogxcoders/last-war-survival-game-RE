using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[DisallowMultipleComponent]
[ExecuteAlways]
[AddComponentMenu("Cinemachine/CinemachineFreeLook")]
public class CinemachineFreeLook : CinemachineVirtualCameraBase
{
	[Serializable]
	public struct Orbit
	{
		public float m_Height;

		public float m_Radius;

		public Orbit(float h, float r)
		{
			m_Height = h;
			m_Radius = r;
		}
	}

	public delegate CinemachineVirtualCamera CreateRigDelegate(CinemachineFreeLook vcam, string name, CinemachineVirtualCamera copyFrom);

	public delegate void DestroyRigDelegate(GameObject rig);

	[Tooltip("Object for the camera children to look at (the aim target).")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("Object for the camera children wants to move with (the body target).")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[Tooltip("If enabled, this lens setting will apply to all three child rigs, otherwise the child rig lens settings will be used")]
	[FormerlySerializedAs("m_UseCommonLensSetting")]
	public bool m_CommonLens = true;

	[FormerlySerializedAs("m_LensAttributes")]
	[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active")]
	[LensSettingsProperty]
	public LensSettings m_Lens = LensSettings.Default;

	public TransitionParams m_Transitions;

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_BlendHint")]
	[FormerlySerializedAs("m_PositionBlending")]
	private BlendHint m_LegacyBlendHint;

	[Header("Axis Control")]
	[Tooltip("The Vertical axis.  Value is 0..1.  Chooses how to blend the child rigs")]
	[AxisStateProperty]
	public AxisState m_YAxis = new AxisState(0f, 1f, wrap: false, rangeLocked: true, 2f, 0.2f, 0.1f, "Mouse Y", invert: false);

	[Tooltip("Controls how automatic recentering of the Y axis is accomplished")]
	public AxisState.Recentering m_YAxisRecentering = new AxisState.Recentering(enabled: false, 1f, 2f);

	[Tooltip("The Horizontal axis.  Value is -180...180.  This is passed on to the rigs' OrbitalTransposer component")]
	[AxisStateProperty]
	public AxisState m_XAxis = new AxisState(-180f, 180f, wrap: true, rangeLocked: false, 300f, 0.1f, 0.1f, "Mouse X", invert: true);

	[OrbitalTransposerHeadingProperty]
	[Tooltip("The definition of Forward.  Camera will follow behind.")]
	public CinemachineOrbitalTransposer.Heading m_Heading = new CinemachineOrbitalTransposer.Heading(CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward, 4, 0f);

	[Tooltip("Controls how automatic recentering of the X axis is accomplished")]
	public AxisState.Recentering m_RecenterToTargetHeading = new AxisState.Recentering(enabled: false, 1f, 2f);

	[Header("Orbits")]
	[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
	public CinemachineTransposer.BindingMode m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

	[Tooltip("Controls how taut is the line that connects the rigs' orbits, which determines final placement on the Y axis")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("m_SplineTension")]
	public float m_SplineCurvature = 0.2f;

	[Tooltip("The radius and height of the three orbiting rigs.")]
	public Orbit[] m_Orbits = new Orbit[3]
	{
		new Orbit(4.5f, 1.75f),
		new Orbit(2.5f, 3f),
		new Orbit(0.4f, 1.3f)
	};

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_HeadingBias")]
	private float m_LegacyHeadingBias = float.MaxValue;

	private bool mUseLegacyRigDefinitions;

	private bool mIsDestroyed;

	private CameraState m_State = CameraState.Default;

	[SerializeField]
	[HideInInspector]
	[NoSaveDuringPlay]
	private CinemachineVirtualCamera[] m_Rigs = new CinemachineVirtualCamera[3];

	private CinemachineOrbitalTransposer[] mOrbitals;

	private CinemachineBlend mBlendA;

	private CinemachineBlend mBlendB;

	public static CreateRigDelegate CreateRigOverride;

	public static DestroyRigDelegate DestroyRigOverride;

	private Orbit[] m_CachedOrbits;

	private float m_CachedTension;

	private Vector4[] m_CachedKnots;

	private Vector4[] m_CachedCtrl1;

	private Vector4[] m_CachedCtrl2;

	public static string[] RigNames => new string[3] { "TopRig", "MiddleRig", "BottomRig" };

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

	protected override void OnValidate()
	{
		base.OnValidate();
		if (m_LegacyHeadingBias != float.MaxValue)
		{
			m_Heading.m_Bias = m_LegacyHeadingBias;
			m_LegacyHeadingBias = float.MaxValue;
			int heading = (int)m_Heading.m_Definition;
			if (m_RecenterToTargetHeading.LegacyUpgrade(ref heading, ref m_Heading.m_VelocityFilterStrength))
			{
				m_Heading.m_Definition = (CinemachineOrbitalTransposer.Heading.HeadingDefinition)heading;
			}
			mUseLegacyRigDefinitions = true;
		}
		if (m_LegacyBlendHint != BlendHint.None)
		{
			m_Transitions.m_BlendHint = m_LegacyBlendHint;
			m_LegacyBlendHint = BlendHint.None;
		}
		m_YAxis.Validate();
		m_XAxis.Validate();
		m_RecenterToTargetHeading.Validate();
		m_YAxisRecentering.Validate();
		m_Lens.Validate();
		InvalidateRigCache();
	}

	public CinemachineVirtualCamera GetRig(int i)
	{
		UpdateRigCache();
		if (i >= 0 && i <= 2)
		{
			return m_Rigs[i];
		}
		return null;
	}

	protected override void OnEnable()
	{
		mIsDestroyed = false;
		base.OnEnable();
		InvalidateRigCache();
	}

	protected override void OnDestroy()
	{
		if (m_Rigs != null)
		{
			CinemachineVirtualCamera[] rigs = m_Rigs;
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in rigs)
			{
				if (cinemachineVirtualCamera != null && cinemachineVirtualCamera.gameObject != null)
				{
					cinemachineVirtualCamera.gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
				}
			}
		}
		mIsDestroyed = true;
		base.OnDestroy();
	}

	private void OnTransformChildrenChanged()
	{
		InvalidateRigCache();
	}

	private void Reset()
	{
		DestroyRigs();
	}

	public override bool IsLiveChild(ICinemachineCamera vcam)
	{
		if (m_Rigs == null || m_Rigs.Length != 3)
		{
			return false;
		}
		if (vcam == m_Rigs[1])
		{
			return true;
		}
		if (GetYAxisValue() < 0.5f)
		{
			return vcam == m_Rigs[2];
		}
		return vcam == m_Rigs[0];
	}

	public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
	{
		UpdateRigCache();
		if (m_Rigs != null)
		{
			CinemachineVirtualCamera[] rigs = m_Rigs;
			for (int i = 0; i < rigs.Length; i++)
			{
				rigs[i].OnTargetObjectWarped(target, positionDelta);
			}
		}
		base.OnTargetObjectWarped(target, positionDelta);
	}

	public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!base.PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		UpdateRigCache();
		m_State = CalculateNewState(worldUp, deltaTime);
		ApplyPositionBlendMethod(ref m_State, m_Transitions.m_BlendHint);
		if (Follow != null)
		{
			Vector3 vector = State.RawPosition - base.transform.position;
			base.transform.position = State.RawPosition;
			m_Rigs[0].transform.position -= vector;
			m_Rigs[1].transform.position -= vector;
			m_Rigs[2].transform.position -= vector;
		}
		InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref m_State, deltaTime);
		base.PreviousStateIsValid = true;
		if (deltaTime >= 0f && CinemachineCore.Instance.IsLive(this) && m_YAxis.Update(deltaTime))
		{
			m_YAxisRecentering.CancelRecentering();
		}
		PushSettingsToRigs();
	}

	public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		bool flag = false;
		if (fromCam != null && m_Transitions.m_InheritPosition)
		{
			Vector3 vector = fromCam.State.RawPosition;
			if (fromCam is CinemachineFreeLook)
			{
				CinemachineFreeLook cinemachineFreeLook = fromCam as CinemachineFreeLook;
				CinemachineOrbitalTransposer cinemachineOrbitalTransposer = ((cinemachineFreeLook.mOrbitals != null) ? cinemachineFreeLook.mOrbitals[1] : null);
				if (cinemachineOrbitalTransposer != null)
				{
					vector = cinemachineOrbitalTransposer.GetTargetCameraPosition(worldUp);
				}
			}
			UpdateRigCache();
			if (m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				m_XAxis.Value = mOrbitals[1].GetAxisClosestValue(vector, worldUp);
			}
			m_YAxis.Value = GetYAxisClosestValue(vector, worldUp);
			base.transform.position = vector;
			m_State = PullStateFromVirtualCamera(worldUp, ref m_Lens);
			base.PreviousStateIsValid = false;
			PushSettingsToRigs();
			flag = true;
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

	private float GetYAxisClosestValue(Vector3 cameraPos, Vector3 up)
	{
		if (Follow != null)
		{
			Vector3 vector = Quaternion.FromToRotation(up, Vector3.up) * (cameraPos - Follow.position);
			Vector3 vector2 = vector;
			vector2.y = 0f;
			if (!vector2.AlmostZero())
			{
				vector = Quaternion.AngleAxis(Vector3.SignedAngle(vector2, Vector3.back, Vector3.up), Vector3.up) * vector;
			}
			vector.x = 0f;
			int num = 0;
			int num2 = 0;
			float num3 = 0f;
			float f = 0f;
			float num4 = 1f / 12f;
			for (int i = 0; i < 13; i++)
			{
				float num5 = Vector3.SignedAngle(vector, GetLocalPositionForCameraFromInput((float)i * num4), Vector3.right);
				if (i == 0)
				{
					num3 = (f = num5);
				}
				else if (Mathf.Abs(num5) < Mathf.Abs(num3))
				{
					f = num3;
					num2 = num;
					num3 = num5;
					num = i;
				}
				else if (Mathf.Abs(num5) < Mathf.Abs(f))
				{
					f = num5;
					num2 = i;
				}
			}
			if (Mathf.Sign(num3) == Mathf.Sign(f))
			{
				return (float)num * num4;
			}
			float t = Mathf.Abs(num3) / (Mathf.Abs(num3) + Mathf.Abs(f));
			return Mathf.Lerp((float)num * num4, (float)num2 * num4, t);
		}
		return m_YAxis.Value;
	}

	private void InvalidateRigCache()
	{
		mOrbitals = null;
	}

	private void DestroyRigs()
	{
		CinemachineVirtualCamera[] array = new CinemachineVirtualCamera[RigNames.Length];
		for (int i = 0; i < RigNames.Length; i++)
		{
			foreach (Transform item in base.transform)
			{
				if (item.gameObject.name == RigNames[i])
				{
					array[i] = item.GetComponent<CinemachineVirtualCamera>();
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] != null)
			{
				if (DestroyRigOverride != null)
				{
					DestroyRigOverride(array[j].gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(array[j].gameObject);
				}
			}
		}
		m_Rigs = null;
		mOrbitals = null;
	}

	private CinemachineVirtualCamera[] CreateRigs(CinemachineVirtualCamera[] copyFrom)
	{
		mOrbitals = null;
		float[] array = new float[3] { 0.5f, 0.55f, 0.6f };
		CinemachineVirtualCamera[] array2 = new CinemachineVirtualCamera[3];
		for (int i = 0; i < RigNames.Length; i++)
		{
			CinemachineVirtualCamera cinemachineVirtualCamera = null;
			if (copyFrom != null && copyFrom.Length > i)
			{
				cinemachineVirtualCamera = copyFrom[i];
			}
			if (CreateRigOverride != null)
			{
				array2[i] = CreateRigOverride(this, RigNames[i], cinemachineVirtualCamera);
			}
			else
			{
				GameObject gameObject = new GameObject(RigNames[i]);
				gameObject.transform.parent = base.transform;
				array2[i] = gameObject.AddComponent<CinemachineVirtualCamera>();
				gameObject = array2[i].GetComponentOwner().gameObject;
				gameObject.AddComponent<CinemachineOrbitalTransposer>();
				gameObject.AddComponent<CinemachineComposer>();
			}
			array2[i].InvalidateComponentPipeline();
			CinemachineOrbitalTransposer cinemachineOrbitalTransposer = array2[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
			if (cinemachineOrbitalTransposer == null)
			{
				cinemachineOrbitalTransposer = array2[i].AddCinemachineComponent<CinemachineOrbitalTransposer>();
			}
			if (cinemachineVirtualCamera == null)
			{
				cinemachineOrbitalTransposer.m_YawDamping = 0f;
				CinemachineComposer cinemachineComponent = array2[i].GetCinemachineComponent<CinemachineComposer>();
				if (cinemachineComponent != null)
				{
					cinemachineComponent.m_HorizontalDamping = (cinemachineComponent.m_VerticalDamping = 0f);
					cinemachineComponent.m_ScreenX = 0.5f;
					cinemachineComponent.m_ScreenY = array[i];
					cinemachineComponent.m_DeadZoneWidth = (cinemachineComponent.m_DeadZoneHeight = 0f);
					cinemachineComponent.m_SoftZoneWidth = (cinemachineComponent.m_SoftZoneHeight = 0.8f);
					cinemachineComponent.m_BiasX = (cinemachineComponent.m_BiasY = 0f);
				}
			}
		}
		return array2;
	}

	private void UpdateRigCache()
	{
		if (mIsDestroyed)
		{
			return;
		}
		bool flag = false;
		if (mOrbitals == null || mOrbitals.Length != 3)
		{
			flag = base.gameObject.scene.name == null;
			if (LocateExistingRigs(RigNames, forceOrbital: false) != 3 && !flag)
			{
				DestroyRigs();
				m_Rigs = CreateRigs(null);
				LocateExistingRigs(RigNames, forceOrbital: true);
			}
			CinemachineVirtualCamera[] rigs = m_Rigs;
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in rigs)
			{
				cinemachineVirtualCamera.m_ExcludedPropertiesInInspector = (m_CommonLens ? new string[8] { "m_Script", "Header", "Extensions", "m_Priority", "m_Transitions", "m_Follow", "m_StandbyUpdate", "m_Lens" } : new string[7] { "m_Script", "Header", "Extensions", "m_Priority", "m_Transitions", "m_Follow", "m_StandbyUpdate" });
				cinemachineVirtualCamera.m_LockStageInInspector = new CinemachineCore.Stage[1];
			}
			mBlendA = new CinemachineBlend(m_Rigs[1], m_Rigs[0], AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0f);
			mBlendB = new CinemachineBlend(m_Rigs[2], m_Rigs[1], AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0f);
		}
	}

	private int LocateExistingRigs(string[] rigNames, bool forceOrbital)
	{
		mOrbitals = new CinemachineOrbitalTransposer[rigNames.Length];
		m_Rigs = new CinemachineVirtualCamera[rigNames.Length];
		int num = 0;
		foreach (Transform item in base.transform)
		{
			CinemachineVirtualCamera component = item.GetComponent<CinemachineVirtualCamera>();
			if (!(component != null))
			{
				continue;
			}
			GameObject gameObject = item.gameObject;
			for (int i = 0; i < rigNames.Length; i++)
			{
				if (!(mOrbitals[i] == null) || !(gameObject.name == rigNames[i]))
				{
					continue;
				}
				mOrbitals[i] = component.GetCinemachineComponent<CinemachineOrbitalTransposer>();
				if (mOrbitals[i] == null && forceOrbital)
				{
					mOrbitals[i] = component.AddCinemachineComponent<CinemachineOrbitalTransposer>();
				}
				if (!(mOrbitals[i] != null))
				{
					continue;
				}
				mOrbitals[i].m_HeadingIsSlave = true;
				if (i == 1)
				{
					mOrbitals[i].HeadingUpdater = (CinemachineOrbitalTransposer orbital, float deltaTime, Vector3 up) => orbital.UpdateHeading(deltaTime, up, ref m_XAxis);
				}
				m_Rigs[i] = component;
				m_Rigs[i].m_StandbyUpdate = m_StandbyUpdate;
				num++;
			}
		}
		return num;
	}

	private void PushSettingsToRigs()
	{
		UpdateRigCache();
		for (int i = 0; i < m_Rigs.Length; i++)
		{
			if (m_Rigs[i] == null)
			{
				continue;
			}
			if (m_CommonLens)
			{
				m_Rigs[i].m_Lens = m_Lens;
			}
			if (mUseLegacyRigDefinitions)
			{
				mUseLegacyRigDefinitions = false;
				m_Orbits[i].m_Height = mOrbitals[i].m_FollowOffset.y;
				m_Orbits[i].m_Radius = 0f - mOrbitals[i].m_FollowOffset.z;
				if (m_Rigs[i].Follow != null)
				{
					Follow = m_Rigs[i].Follow;
				}
			}
			m_Rigs[i].Follow = null;
			m_Rigs[i].m_StandbyUpdate = m_StandbyUpdate;
			if (!base.PreviousStateIsValid)
			{
				m_Rigs[i].PreviousStateIsValid = false;
				m_Rigs[i].transform.position = base.transform.position;
				m_Rigs[i].transform.rotation = base.transform.rotation;
			}
			CinemachineVirtualCamera.SetFlagsForHiddenChild(m_Rigs[i].gameObject);
			mOrbitals[i].m_FollowOffset = GetLocalPositionForCameraFromInput(GetYAxisValue());
			mOrbitals[i].m_BindingMode = m_BindingMode;
			mOrbitals[i].m_Heading = m_Heading;
			mOrbitals[i].m_XAxis = m_XAxis;
			mOrbitals[i].m_RecenterToTargetHeading.m_enabled = i == 1 && m_RecenterToTargetHeading.m_enabled;
			mOrbitals[i].m_RecenterToTargetHeading.m_WaitTime = m_RecenterToTargetHeading.m_WaitTime;
			mOrbitals[i].m_RecenterToTargetHeading.m_RecenteringTime = m_RecenterToTargetHeading.m_RecenteringTime;
			if (m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				m_Rigs[i].SetStateRawPosition(State.RawPosition);
			}
		}
	}

	private float GetYAxisValue()
	{
		float num = m_YAxis.m_MaxValue - m_YAxis.m_MinValue;
		if (!(num > 0.0001f))
		{
			return 0.5f;
		}
		return m_YAxis.Value / num;
	}

	private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
	{
		CameraState result = PullStateFromVirtualCamera(worldUp, ref m_Lens);
		m_YAxisRecentering.DoRecentering(ref m_YAxis, deltaTime, 0.5f);
		float yAxisValue = GetYAxisValue();
		if (yAxisValue > 0.5f)
		{
			if (mBlendA != null)
			{
				mBlendA.TimeInBlend = (yAxisValue - 0.5f) * 2f;
				mBlendA.UpdateCameraState(worldUp, deltaTime);
				result = mBlendA.State;
			}
		}
		else if (mBlendB != null)
		{
			mBlendB.TimeInBlend = yAxisValue * 2f;
			mBlendB.UpdateCameraState(worldUp, deltaTime);
			result = mBlendB.State;
		}
		return result;
	}

	public Vector3 GetLocalPositionForCameraFromInput(float t)
	{
		if (mOrbitals == null)
		{
			return Vector3.zero;
		}
		UpdateCachedSpline();
		int num = 1;
		if (t > 0.5f)
		{
			t -= 0.5f;
			num = 2;
		}
		return SplineHelpers.Bezier3(t * 2f, m_CachedKnots[num], m_CachedCtrl1[num], m_CachedCtrl2[num], m_CachedKnots[num + 1]);
	}

	private void UpdateCachedSpline()
	{
		bool flag = m_CachedOrbits != null && m_CachedTension == m_SplineCurvature;
		for (int i = 0; i < 3 && flag; i++)
		{
			flag = m_CachedOrbits[i].m_Height == m_Orbits[i].m_Height && m_CachedOrbits[i].m_Radius == m_Orbits[i].m_Radius;
		}
		if (!flag)
		{
			float splineCurvature = m_SplineCurvature;
			m_CachedKnots = new Vector4[5];
			m_CachedCtrl1 = new Vector4[5];
			m_CachedCtrl2 = new Vector4[5];
			m_CachedKnots[1] = new Vector4(0f, m_Orbits[2].m_Height, 0f - m_Orbits[2].m_Radius, 0f);
			m_CachedKnots[2] = new Vector4(0f, m_Orbits[1].m_Height, 0f - m_Orbits[1].m_Radius, 0f);
			m_CachedKnots[3] = new Vector4(0f, m_Orbits[0].m_Height, 0f - m_Orbits[0].m_Radius, 0f);
			m_CachedKnots[0] = Vector4.Lerp(m_CachedKnots[1], Vector4.zero, splineCurvature);
			m_CachedKnots[4] = Vector4.Lerp(m_CachedKnots[3], Vector4.zero, splineCurvature);
			SplineHelpers.ComputeSmoothControlPoints(ref m_CachedKnots, ref m_CachedCtrl1, ref m_CachedCtrl2);
			m_CachedOrbits = new Orbit[3];
			for (int j = 0; j < 3; j++)
			{
				m_CachedOrbits[j] = m_Orbits[j];
			}
			m_CachedTension = m_SplineCurvature;
		}
	}
}

using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")]
[SaveDuringPlay]
public class CinemachineTransposer : CinemachineComponentBase
{
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	public enum BindingMode
	{
		LockToTargetOnAssign,
		LockToTargetWithWorldUp,
		LockToTargetNoRoll,
		LockToTarget,
		WorldSpace,
		SimpleFollowWithWorldUp
	}

	public enum AngularDampingMode
	{
		Euler,
		Quaternion
	}

	[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
	public BindingMode m_BindingMode = BindingMode.LockToTargetWithWorldUp;

	[Tooltip("The distance vector that the transposer will attempt to maintain from the Follow target")]
	public Vector3 m_FollowOffset = Vector3.back * 10f;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to maintain the offset in the X-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's x-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
	public float m_XDamping = 1f;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to maintain the offset in the Y-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's y-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
	public float m_YDamping = 1f;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to maintain the offset in the Z-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's z-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
	public float m_ZDamping = 1f;

	public AngularDampingMode m_AngularDampingMode;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to track the target rotation's X angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
	public float m_PitchDamping;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to track the target rotation's Y angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
	public float m_YawDamping;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to track the target rotation's Z angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
	public float m_RollDamping;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to track the target's orientation.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
	public float m_AngularDamping;

	private Vector3 m_PreviousTargetPosition = Vector3.zero;

	private Quaternion m_PreviousReferenceOrientation = Quaternion.identity;

	private Quaternion m_targetOrientationOnAssign = Quaternion.identity;

	private Transform m_previousTarget;

	public bool HideOffsetInInspector { get; set; }

	public Vector3 EffectiveOffset
	{
		get
		{
			Vector3 followOffset = m_FollowOffset;
			if (m_BindingMode == BindingMode.SimpleFollowWithWorldUp)
			{
				followOffset.x = 0f;
				followOffset.z = 0f - Mathf.Abs(followOffset.z);
			}
			return followOffset;
		}
	}

	public override bool IsValid
	{
		get
		{
			if (base.enabled)
			{
				return base.FollowTarget != null;
			}
			return false;
		}
	}

	public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

	protected Vector3 Damping
	{
		get
		{
			BindingMode bindingMode = m_BindingMode;
			if (bindingMode == BindingMode.SimpleFollowWithWorldUp)
			{
				return new Vector3(0f, m_YDamping, m_ZDamping);
			}
			return new Vector3(m_XDamping, m_YDamping, m_ZDamping);
		}
	}

	protected Vector3 AngularDamping
	{
		get
		{
			switch (m_BindingMode)
			{
			case BindingMode.LockToTargetNoRoll:
				return new Vector3(m_PitchDamping, m_YawDamping, 0f);
			case BindingMode.LockToTargetWithWorldUp:
				return new Vector3(0f, m_YawDamping, 0f);
			case BindingMode.LockToTargetOnAssign:
			case BindingMode.WorldSpace:
			case BindingMode.SimpleFollowWithWorldUp:
				return Vector3.zero;
			default:
				return new Vector3(m_PitchDamping, m_YawDamping, m_RollDamping);
			}
		}
	}

	protected virtual void OnValidate()
	{
		m_FollowOffset = EffectiveOffset;
	}

	public override void MutateCameraState(ref CameraState curState, float deltaTime)
	{
		InitPrevFrameStateInfo(ref curState, deltaTime);
		if (IsValid)
		{
			Vector3 effectiveOffset = EffectiveOffset;
			TrackTarget(deltaTime, curState.ReferenceUp, effectiveOffset, out var outTargetPosition, out var outTargetOrient);
			curState.RawPosition = outTargetPosition + outTargetOrient * effectiveOffset;
			curState.ReferenceUp = outTargetOrient * Vector3.up;
		}
	}

	public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
	{
		base.OnTargetObjectWarped(target, positionDelta);
		if (target == base.FollowTarget)
		{
			m_PreviousTargetPosition += positionDelta;
		}
	}

	protected void InitPrevFrameStateInfo(ref CameraState curState, float deltaTime)
	{
		if (m_previousTarget != base.FollowTarget || deltaTime < 0f)
		{
			m_previousTarget = base.FollowTarget;
			m_targetOrientationOnAssign = ((m_previousTarget == null) ? Quaternion.identity : base.FollowTargetRotation);
		}
		if (deltaTime < 0f)
		{
			m_PreviousTargetPosition = base.FollowTargetPosition;
			m_PreviousReferenceOrientation = GetReferenceOrientation(curState.ReferenceUp);
		}
	}

	protected void TrackTarget(float deltaTime, Vector3 up, Vector3 desiredCameraOffset, out Vector3 outTargetPosition, out Quaternion outTargetOrient)
	{
		Quaternion referenceOrientation = GetReferenceOrientation(up);
		Quaternion quaternion = referenceOrientation;
		if (deltaTime >= 0f)
		{
			if (m_AngularDampingMode == AngularDampingMode.Quaternion)
			{
				float t = Damper.Damp(1f, m_AngularDamping, deltaTime);
				quaternion = Quaternion.Slerp(m_PreviousReferenceOrientation, referenceOrientation, t);
			}
			else
			{
				Vector3 eulerAngles = (Quaternion.Inverse(m_PreviousReferenceOrientation) * referenceOrientation).eulerAngles;
				for (int i = 0; i < 3; i++)
				{
					if (eulerAngles[i] > 180f)
					{
						eulerAngles[i] -= 360f;
					}
				}
				eulerAngles = Damper.Damp(eulerAngles, AngularDamping, deltaTime);
				quaternion = m_PreviousReferenceOrientation * Quaternion.Euler(eulerAngles);
			}
		}
		m_PreviousReferenceOrientation = quaternion;
		Vector3 followTargetPosition = base.FollowTargetPosition;
		Vector3 previousTargetPosition = m_PreviousTargetPosition;
		Vector3 vector = followTargetPosition - previousTargetPosition;
		if (deltaTime >= 0f)
		{
			Quaternion quaternion2 = ((!desiredCameraOffset.AlmostZero()) ? Quaternion.LookRotation(quaternion * desiredCameraOffset.normalized, up) : base.VcamState.RawOrientation);
			Vector3 initial = Quaternion.Inverse(quaternion2) * vector;
			initial = Damper.Damp(initial, Damping, deltaTime);
			vector = quaternion2 * initial;
		}
		outTargetPosition = (m_PreviousTargetPosition = previousTargetPosition + vector);
		outTargetOrient = quaternion;
	}

	public Vector3 GetTargetCameraPosition(Vector3 worldUp)
	{
		if (!IsValid)
		{
			return Vector3.zero;
		}
		return base.FollowTargetPosition + GetReferenceOrientation(worldUp) * EffectiveOffset;
	}

	public Quaternion GetReferenceOrientation(Vector3 worldUp)
	{
		if (base.FollowTarget != null)
		{
			Quaternion rotation = base.FollowTarget.rotation;
			switch (m_BindingMode)
			{
			case BindingMode.LockToTargetOnAssign:
				return m_targetOrientationOnAssign;
			case BindingMode.LockToTargetWithWorldUp:
				return Uppify(rotation, worldUp);
			case BindingMode.LockToTargetNoRoll:
				return Quaternion.LookRotation(rotation * Vector3.forward, worldUp);
			case BindingMode.LockToTarget:
				return rotation;
			case BindingMode.SimpleFollowWithWorldUp:
			{
				Vector3 vector = base.FollowTargetPosition - base.VcamState.RawPosition;
				vector = vector.ProjectOntoPlane(worldUp);
				if (!vector.AlmostZero())
				{
					return Quaternion.LookRotation(vector.normalized, worldUp);
				}
				break;
			}
			}
		}
		return Quaternion.identity;
	}

	private static Quaternion Uppify(Quaternion q, Vector3 up)
	{
		return Quaternion.FromToRotation(q * Vector3.up, up) * q;
	}
}

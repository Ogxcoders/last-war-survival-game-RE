using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")]
[SaveDuringPlay]
public class CinemachinePOV : CinemachineComponentBase
{
	[Tooltip("The Vertical axis.  Value is -90..90. Controls the vertical orientation")]
	[AxisStateProperty]
	public AxisState m_VerticalAxis = new AxisState(-70f, 70f, wrap: false, rangeLocked: false, 300f, 0.1f, 0.1f, "Mouse Y", invert: true);

	[Tooltip("Controls how automatic recentering of the Vertical axis is accomplished")]
	public AxisState.Recentering m_VerticalRecentering = new AxisState.Recentering(enabled: false, 1f, 2f);

	[Tooltip("The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation")]
	[AxisStateProperty]
	public AxisState m_HorizontalAxis = new AxisState(-180f, 180f, wrap: true, rangeLocked: false, 300f, 0.1f, 0.1f, "Mouse X", invert: false);

	[Tooltip("Controls how automatic recentering of the Horizontal axis is accomplished")]
	public AxisState.Recentering m_HorizontalRecentering = new AxisState.Recentering(enabled: false, 1f, 2f);

	public override bool IsValid => base.enabled;

	public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Aim;

	private void OnValidate()
	{
		m_VerticalAxis.Validate();
		m_VerticalRecentering.Validate();
		m_HorizontalAxis.Validate();
		m_HorizontalRecentering.Validate();
	}

	public override void MutateCameraState(ref CameraState curState, float deltaTime)
	{
		if (!IsValid)
		{
			return;
		}
		if (deltaTime >= 0f && CinemachineCore.Instance.IsLive(base.VirtualCamera))
		{
			if (m_HorizontalAxis.Update(deltaTime))
			{
				m_HorizontalRecentering.CancelRecentering();
			}
			if (m_VerticalAxis.Update(deltaTime))
			{
				m_VerticalRecentering.CancelRecentering();
			}
		}
		m_HorizontalRecentering.DoRecentering(ref m_HorizontalAxis, deltaTime, 0f);
		m_VerticalRecentering.DoRecentering(ref m_VerticalAxis, deltaTime, 0f);
		Quaternion quaternion = Quaternion.Euler(m_VerticalAxis.Value, m_HorizontalAxis.Value, 0f);
		Transform parent = base.VirtualCamera.transform.parent;
		if (parent != null)
		{
			quaternion = parent.rotation * quaternion;
		}
		else
		{
			quaternion *= Quaternion.FromToRotation(Vector3.up, curState.ReferenceUp);
		}
		curState.RawOrientation = quaternion;
	}

	public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime, ref CinemachineVirtualCameraBase.TransitionParams transitionParams)
	{
		if (fromCam != null && transitionParams.m_InheritPosition)
		{
			Vector3 referenceUp = base.VcamState.ReferenceUp;
			Quaternion rawOrientation = fromCam.State.RawOrientation;
			Vector3 vector = Vector3.forward;
			Transform parent = base.VirtualCamera.transform.parent;
			if (parent != null)
			{
				vector = parent.rotation * vector;
			}
			m_HorizontalAxis.Value = 0f;
			m_HorizontalAxis.Reset();
			Vector3 vector2 = rawOrientation * Vector3.forward;
			Vector3 vector3 = vector.ProjectOntoPlane(referenceUp);
			Vector3 vector4 = vector2.ProjectOntoPlane(referenceUp);
			if (!vector3.AlmostZero() && !vector4.AlmostZero())
			{
				m_HorizontalAxis.Value = Vector3.SignedAngle(vector3, vector4, referenceUp);
			}
			m_VerticalAxis.Value = 0f;
			m_VerticalAxis.Reset();
			vector = Quaternion.AngleAxis(m_HorizontalAxis.Value, referenceUp) * vector;
			Vector3 vector5 = Vector3.Cross(referenceUp, vector);
			if (!vector5.AlmostZero())
			{
				m_VerticalAxis.Value = Vector3.SignedAngle(vector, vector2, vector5);
			}
			return true;
		}
		return false;
	}
}

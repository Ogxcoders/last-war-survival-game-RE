using Cinemachine;
using UnityEngine;

[AddComponentMenu("")]
public class CinemachineCameraOffset : CinemachineExtension
{
	[Tooltip("Offset the camera's position by this much (camera space)")]
	public Vector3 m_Offset = Vector3.zero;

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == CinemachineCore.Stage.Aim)
		{
			Vector3 vector = state.FinalOrientation * m_Offset;
			state.ReferenceLookAt += vector;
			state.PositionCorrection += vector;
		}
	}
}

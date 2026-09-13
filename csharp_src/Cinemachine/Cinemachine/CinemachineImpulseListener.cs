using UnityEngine;

namespace Cinemachine;

[SaveDuringPlay]
[AddComponentMenu("")]
[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[ExecuteAlways]
public class CinemachineImpulseListener : CinemachineExtension
{
	[Tooltip("Impulse events on channels not included in the mask will be ignored.")]
	[CinemachineImpulseChannelProperty]
	public int m_ChannelMask = 1;

	[Tooltip("Gain to apply to the Impulse signal.  1 is normal strength.  Setting this to 0 completely mutes the signal.")]
	public float m_Gain = 1f;

	[Tooltip("Enable this to perform distance calculation in 2D (ignore Z)")]
	public bool m_Use2DDistance;

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == CinemachineCore.Stage.Aim)
		{
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			if (CinemachineImpulseManager.Instance.GetImpulseAt(state.FinalPosition, m_Use2DDistance, m_ChannelMask, out pos, out rot))
			{
				state.PositionCorrection += pos * (0f - m_Gain);
				rot = Quaternion.SlerpUnclamped(Quaternion.identity, rot, 0f - m_Gain);
				state.OrientationCorrection *= rot;
			}
		}
	}
}

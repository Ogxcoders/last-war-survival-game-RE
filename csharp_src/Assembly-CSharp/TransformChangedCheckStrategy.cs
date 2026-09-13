using UnityEngine;

public class TransformChangedCheckStrategy : NeedHighFPSCheckStrategyBase
{
	private Transform m_Target;

	private Vector3 m_PositionLast;

	private Vector3 m_PositionCurr;

	private Vector3 m_RotationLast;

	private Vector3 m_RotationCurr;

	private float m_SqrMagnitude;

	private int m_FrameCountLast = -1;

	public TransformChangedCheckStrategy(Transform transform, float magnitude = 0.001f)
	{
		m_Target = transform;
		m_SqrMagnitude = magnitude * magnitude;
	}

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		TryUpdate();
		pass = false;
		pass |= (m_PositionCurr - m_PositionLast).sqrMagnitude > m_SqrMagnitude;
		pass |= (m_RotationCurr - m_RotationLast).sqrMagnitude > m_SqrMagnitude;
		keep = 0f;
	}

	private void TryUpdate()
	{
		if (Time.frameCount > m_FrameCountLast)
		{
			m_FrameCountLast = Time.frameCount;
			m_PositionLast = m_PositionCurr;
			m_RotationLast = m_RotationCurr;
			m_PositionCurr = m_Target.position;
			m_RotationCurr = m_Target.eulerAngles;
		}
	}
}

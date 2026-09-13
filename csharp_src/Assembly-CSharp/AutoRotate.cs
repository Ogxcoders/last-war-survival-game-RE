using System;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public Vector3 m_rotateSpeed = Vector3.zero;

	public Vector3 m_startScale;

	public Vector3 m_endScale;

	public float m_scaleTime;

	private float m_scaleTimer;

	private void Update()
	{
		try
		{
			if (m_rotateSpeed != Vector3.zero)
			{
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				if (m_rotateSpeed.x != 0f)
				{
					localEulerAngles.x += m_rotateSpeed.x * Time.deltaTime;
				}
				if (m_rotateSpeed.y != 0f)
				{
					localEulerAngles.y += m_rotateSpeed.y * Time.deltaTime;
				}
				if (m_rotateSpeed.z != 0f)
				{
					localEulerAngles.z += m_rotateSpeed.z * Time.deltaTime;
				}
				base.transform.localEulerAngles = localEulerAngles;
			}
			if (m_scaleTime > 0f)
			{
				m_scaleTimer += Time.deltaTime;
				float t = Mathf.PingPong(m_scaleTimer, m_scaleTime) / m_scaleTime;
				base.transform.localScale = Vector3.Lerp(m_startScale, m_endScale, t);
			}
		}
		catch (Exception)
		{
		}
	}
}

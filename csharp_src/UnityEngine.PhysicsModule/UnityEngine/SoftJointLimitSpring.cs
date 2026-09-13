namespace UnityEngine;

public struct SoftJointLimitSpring
{
	private float m_Spring;

	private float m_Damper;

	public float spring
	{
		get
		{
			return m_Spring;
		}
		set
		{
			m_Spring = value;
		}
	}

	public float damper
	{
		get
		{
			return m_Damper;
		}
		set
		{
			m_Damper = value;
		}
	}
}

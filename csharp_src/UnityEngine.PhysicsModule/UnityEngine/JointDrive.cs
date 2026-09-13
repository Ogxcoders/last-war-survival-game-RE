using System;

namespace UnityEngine;

public struct JointDrive
{
	private float m_PositionSpring;

	private float m_PositionDamper;

	private float m_MaximumForce;

	[Obsolete("JointDriveMode is obsolete")]
	public JointDriveMode mode
	{
		get
		{
			return JointDriveMode.None;
		}
		set
		{
		}
	}

	public float positionSpring
	{
		get
		{
			return m_PositionSpring;
		}
		set
		{
			m_PositionSpring = value;
		}
	}

	public float positionDamper
	{
		get
		{
			return m_PositionDamper;
		}
		set
		{
			m_PositionDamper = value;
		}
	}

	public float maximumForce
	{
		get
		{
			return m_MaximumForce;
		}
		set
		{
			m_MaximumForce = value;
		}
	}
}

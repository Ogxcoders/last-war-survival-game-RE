using System;
using System.ComponentModel;

namespace UnityEngine;

public struct JointLimits
{
	private float m_Min;

	private float m_Max;

	private float m_Bounciness;

	private float m_BounceMinVelocity;

	private float m_ContactDistance;

	[Obsolete("minBounce and maxBounce are replaced by a single JointLimits.bounciness for both limit ends.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public float minBounce;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("minBounce and maxBounce are replaced by a single JointLimits.bounciness for both limit ends.", true)]
	public float maxBounce;

	public float min
	{
		get
		{
			return m_Min;
		}
		set
		{
			m_Min = value;
		}
	}

	public float max
	{
		get
		{
			return m_Max;
		}
		set
		{
			m_Max = value;
		}
	}

	public float bounciness
	{
		get
		{
			return m_Bounciness;
		}
		set
		{
			m_Bounciness = value;
		}
	}

	public float bounceMinVelocity
	{
		get
		{
			return m_BounceMinVelocity;
		}
		set
		{
			m_BounceMinVelocity = value;
		}
	}

	public float contactDistance
	{
		get
		{
			return m_ContactDistance;
		}
		set
		{
			m_ContactDistance = value;
		}
	}
}

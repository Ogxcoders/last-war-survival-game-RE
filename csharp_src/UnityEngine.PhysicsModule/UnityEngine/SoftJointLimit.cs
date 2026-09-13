using System;
using System.ComponentModel;

namespace UnityEngine;

public struct SoftJointLimit
{
	private float m_Limit;

	private float m_Bounciness;

	private float m_ContactDistance;

	public float limit
	{
		get
		{
			return m_Limit;
		}
		set
		{
			m_Limit = value;
		}
	}

	[Obsolete("Spring has been moved to SoftJointLimitSpring class in Unity 5", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public float spring
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Damper has been moved to SoftJointLimitSpring class in Unity 5", true)]
	public float damper
	{
		get
		{
			return 0f;
		}
		set
		{
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use SoftJointLimit.bounciness instead", true)]
	public float bouncyness
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
}

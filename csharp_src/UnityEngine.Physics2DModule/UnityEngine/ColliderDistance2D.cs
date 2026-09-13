namespace UnityEngine;

public struct ColliderDistance2D
{
	private Vector2 m_PointA;

	private Vector2 m_PointB;

	private Vector2 m_Normal;

	private float m_Distance;

	private int m_IsValid;

	public Vector2 pointA
	{
		get
		{
			return m_PointA;
		}
		set
		{
			m_PointA = value;
		}
	}

	public Vector2 pointB
	{
		get
		{
			return m_PointB;
		}
		set
		{
			m_PointB = value;
		}
	}

	public Vector2 normal => m_Normal;

	public float distance
	{
		get
		{
			return m_Distance;
		}
		set
		{
			m_Distance = value;
		}
	}

	public bool isOverlapped => m_Distance < 0f;

	public bool isValid
	{
		get
		{
			return m_IsValid != 0;
		}
		set
		{
			m_IsValid = (value ? 1 : 0);
		}
	}
}

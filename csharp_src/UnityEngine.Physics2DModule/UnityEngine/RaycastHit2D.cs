using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Runtime/Interfaces/IPhysics2D.h")]
[NativeClass("RaycastHit2D", "struct RaycastHit2D;")]
[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
public struct RaycastHit2D
{
	[NativeName("centroid")]
	private Vector2 m_Centroid;

	[NativeName("point")]
	private Vector2 m_Point;

	[NativeName("normal")]
	private Vector2 m_Normal;

	[NativeName("distance")]
	private float m_Distance;

	[NativeName("fraction")]
	private float m_Fraction;

	[NativeName("collider")]
	private int m_Collider;

	public Vector2 centroid
	{
		get
		{
			return m_Centroid;
		}
		set
		{
			m_Centroid = value;
		}
	}

	public Vector2 point
	{
		get
		{
			return m_Point;
		}
		set
		{
			m_Point = value;
		}
	}

	public Vector2 normal
	{
		get
		{
			return m_Normal;
		}
		set
		{
			m_Normal = value;
		}
	}

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

	public float fraction
	{
		get
		{
			return m_Fraction;
		}
		set
		{
			m_Fraction = value;
		}
	}

	public Collider2D collider => Object.FindObjectFromInstanceID(m_Collider) as Collider2D;

	public Rigidbody2D rigidbody => (collider != null) ? collider.attachedRigidbody : null;

	public Transform transform
	{
		get
		{
			Rigidbody2D rigidbody2D = rigidbody;
			if (rigidbody2D != null)
			{
				return rigidbody2D.transform;
			}
			if (collider != null)
			{
				return collider.transform;
			}
			return null;
		}
	}

	public static implicit operator bool(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	public int CompareTo(RaycastHit2D other)
	{
		if (collider == null)
		{
			return 1;
		}
		if (other.collider == null)
		{
			return -1;
		}
		return fraction.CompareTo(other.fraction);
	}
}

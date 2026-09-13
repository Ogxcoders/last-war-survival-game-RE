using System;

namespace UnityEngine.Rendering;

public struct LODParameters : IEquatable<LODParameters>
{
	private int m_IsOrthographic;

	private Vector3 m_CameraPosition;

	private float m_FieldOfView;

	private float m_OrthoSize;

	private int m_CameraPixelHeight;

	public bool isOrthographic
	{
		get
		{
			return Convert.ToBoolean(m_IsOrthographic);
		}
		set
		{
			m_IsOrthographic = Convert.ToInt32(value);
		}
	}

	public Vector3 cameraPosition
	{
		get
		{
			return m_CameraPosition;
		}
		set
		{
			m_CameraPosition = value;
		}
	}

	public float fieldOfView
	{
		get
		{
			return m_FieldOfView;
		}
		set
		{
			m_FieldOfView = value;
		}
	}

	public float orthoSize
	{
		get
		{
			return m_OrthoSize;
		}
		set
		{
			m_OrthoSize = value;
		}
	}

	public int cameraPixelHeight
	{
		get
		{
			return m_CameraPixelHeight;
		}
		set
		{
			m_CameraPixelHeight = value;
		}
	}

	public bool Equals(LODParameters other)
	{
		return m_IsOrthographic == other.m_IsOrthographic && m_CameraPosition.Equals(other.m_CameraPosition) && m_FieldOfView.Equals(other.m_FieldOfView) && m_OrthoSize.Equals(other.m_OrthoSize) && m_CameraPixelHeight == other.m_CameraPixelHeight;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is LODParameters && Equals((LODParameters)obj);
	}

	public override int GetHashCode()
	{
		int num = m_IsOrthographic;
		num = (num * 397) ^ m_CameraPosition.GetHashCode();
		num = (num * 397) ^ m_FieldOfView.GetHashCode();
		num = (num * 397) ^ m_OrthoSize.GetHashCode();
		return (num * 397) ^ m_CameraPixelHeight;
	}

	public static bool operator ==(LODParameters left, LODParameters right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(LODParameters left, LODParameters right)
	{
		return !left.Equals(right);
	}
}

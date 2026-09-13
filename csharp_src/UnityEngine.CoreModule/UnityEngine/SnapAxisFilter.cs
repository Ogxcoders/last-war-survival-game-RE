using System;

namespace UnityEngine;

internal struct SnapAxisFilter : IEquatable<SnapAxisFilter>
{
	private const SnapAxis X = SnapAxis.X;

	private const SnapAxis Y = SnapAxis.Y;

	private const SnapAxis Z = SnapAxis.Z;

	public static readonly SnapAxisFilter all = new SnapAxisFilter(SnapAxis.All);

	private SnapAxis m_Mask;

	public float x => ((m_Mask & SnapAxis.X) == SnapAxis.X) ? 1f : 0f;

	public float y => ((m_Mask & SnapAxis.Y) == SnapAxis.Y) ? 1f : 0f;

	public float z => ((m_Mask & SnapAxis.Z) == SnapAxis.Z) ? 1f : 0f;

	public int active
	{
		get
		{
			int num = 0;
			if ((int)(m_Mask & SnapAxis.X) > 0)
			{
				num++;
			}
			if ((int)(m_Mask & SnapAxis.Y) > 0)
			{
				num++;
			}
			if ((int)(m_Mask & SnapAxis.Z) > 0)
			{
				num++;
			}
			return num;
		}
	}

	public float this[int i]
	{
		get
		{
			if (i < 0 || i > 2)
			{
				throw new IndexOutOfRangeException();
			}
			return (float)(1 & ((int)m_Mask >> i)) * 1f;
		}
		set
		{
			if (i < 0 || i > 2)
			{
				throw new IndexOutOfRangeException();
			}
			m_Mask &= (SnapAxis)(byte)(~(1 << i));
			m_Mask |= (SnapAxis)(byte)(((value > 0f) ? 1u : 0u) << i);
		}
	}

	public SnapAxisFilter(Vector3 v)
	{
		m_Mask = SnapAxis.None;
		float num = 1E-06f;
		if (Mathf.Abs(v.x) > num)
		{
			m_Mask |= SnapAxis.X;
		}
		if (Mathf.Abs(v.y) > num)
		{
			m_Mask |= SnapAxis.Y;
		}
		if (Mathf.Abs(v.z) > num)
		{
			m_Mask |= SnapAxis.Z;
		}
	}

	public SnapAxisFilter(SnapAxis axis)
	{
		m_Mask = SnapAxis.None;
		if ((axis & SnapAxis.X) == SnapAxis.X)
		{
			m_Mask |= SnapAxis.X;
		}
		if ((axis & SnapAxis.Y) == SnapAxis.Y)
		{
			m_Mask |= SnapAxis.Y;
		}
		if ((axis & SnapAxis.Z) == SnapAxis.Z)
		{
			m_Mask |= SnapAxis.Z;
		}
	}

	public override string ToString()
	{
		return $"{{{x}, {y}, {z}}}";
	}

	public static implicit operator Vector3(SnapAxisFilter mask)
	{
		return new Vector3(mask.x, mask.y, mask.z);
	}

	public static explicit operator SnapAxisFilter(Vector3 v)
	{
		return new SnapAxisFilter(v);
	}

	public static explicit operator SnapAxis(SnapAxisFilter mask)
	{
		return mask.m_Mask;
	}

	public static SnapAxisFilter operator |(SnapAxisFilter left, SnapAxisFilter right)
	{
		return new SnapAxisFilter(left.m_Mask | right.m_Mask);
	}

	public static SnapAxisFilter operator &(SnapAxisFilter left, SnapAxisFilter right)
	{
		return new SnapAxisFilter(left.m_Mask & right.m_Mask);
	}

	public static SnapAxisFilter operator ^(SnapAxisFilter left, SnapAxisFilter right)
	{
		return new SnapAxisFilter(left.m_Mask ^ right.m_Mask);
	}

	public static SnapAxisFilter operator ~(SnapAxisFilter left)
	{
		return new SnapAxisFilter((SnapAxis)(~(uint)left.m_Mask));
	}

	public static Vector3 operator *(SnapAxisFilter mask, float value)
	{
		return new Vector3(mask.x * value, mask.y * value, mask.z * value);
	}

	public static Vector3 operator *(SnapAxisFilter mask, Vector3 right)
	{
		return new Vector3(mask.x * right.x, mask.y * right.y, mask.z * right.z);
	}

	public static Vector3 operator *(Quaternion rotation, SnapAxisFilter mask)
	{
		int num = mask.active;
		if (num > 2)
		{
			return mask;
		}
		Vector3 vector = rotation * (Vector3)mask;
		vector = new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		if (num > 1)
		{
			return new Vector3((vector.x > vector.y || vector.x > vector.z) ? 1 : 0, (vector.y > vector.x || vector.y > vector.z) ? 1 : 0, (vector.z > vector.x || vector.z > vector.y) ? 1 : 0);
		}
		return new Vector3((vector.x > vector.y && vector.x > vector.z) ? 1 : 0, (vector.y > vector.z && vector.y > vector.x) ? 1 : 0, (vector.z > vector.x && vector.z > vector.y) ? 1 : 0);
	}

	public static bool operator ==(SnapAxisFilter left, SnapAxisFilter right)
	{
		return left.m_Mask == right.m_Mask;
	}

	public static bool operator !=(SnapAxisFilter left, SnapAxisFilter right)
	{
		return !(left == right);
	}

	public bool Equals(SnapAxisFilter other)
	{
		return m_Mask == other.m_Mask;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is SnapAxisFilter && Equals((SnapAxisFilter)obj);
	}

	public override int GetHashCode()
	{
		return m_Mask.GetHashCode();
	}
}

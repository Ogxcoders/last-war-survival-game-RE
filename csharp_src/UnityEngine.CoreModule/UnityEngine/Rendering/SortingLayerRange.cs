using System;

namespace UnityEngine.Rendering;

public struct SortingLayerRange : IEquatable<SortingLayerRange>
{
	private short m_LowerBound;

	private short m_UpperBound;

	public short lowerBound
	{
		get
		{
			return m_LowerBound;
		}
		set
		{
			m_LowerBound = value;
		}
	}

	public short upperBound
	{
		get
		{
			return m_UpperBound;
		}
		set
		{
			m_UpperBound = value;
		}
	}

	public static SortingLayerRange all => new SortingLayerRange
	{
		m_LowerBound = short.MinValue,
		m_UpperBound = short.MaxValue
	};

	public SortingLayerRange(short lowerBound, short upperBound)
	{
		m_LowerBound = lowerBound;
		m_UpperBound = upperBound;
	}

	public bool Equals(SortingLayerRange other)
	{
		return m_LowerBound == other.m_LowerBound && m_UpperBound == other.m_UpperBound;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is SortingLayerRange))
		{
			return false;
		}
		return Equals((SortingLayerRange)obj);
	}

	public static bool operator !=(SortingLayerRange lhs, SortingLayerRange rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator ==(SortingLayerRange lhs, SortingLayerRange rhs)
	{
		return lhs.Equals(rhs);
	}

	public override int GetHashCode()
	{
		return (m_UpperBound << 16) | (m_LowerBound & 0xFFFF);
	}
}

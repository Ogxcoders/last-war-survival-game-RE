using System;

namespace UnityEngine.Rendering;

public struct RasterState : IEquatable<RasterState>
{
	public static readonly RasterState defaultValue = new RasterState(CullMode.Back, 0, 0f, depthClip: true);

	private CullMode m_CullingMode;

	private int m_OffsetUnits;

	private float m_OffsetFactor;

	private byte m_DepthClip;

	private byte m_Padding1;

	private byte m_Padding2;

	private byte m_Padding3;

	public CullMode cullingMode
	{
		get
		{
			return m_CullingMode;
		}
		set
		{
			m_CullingMode = value;
		}
	}

	public bool depthClip
	{
		get
		{
			return Convert.ToBoolean(m_DepthClip);
		}
		set
		{
			m_DepthClip = Convert.ToByte(value);
		}
	}

	public int offsetUnits
	{
		get
		{
			return m_OffsetUnits;
		}
		set
		{
			m_OffsetUnits = value;
		}
	}

	public float offsetFactor
	{
		get
		{
			return m_OffsetFactor;
		}
		set
		{
			m_OffsetFactor = value;
		}
	}

	public RasterState(CullMode cullingMode = CullMode.Back, int offsetUnits = 0, float offsetFactor = 0f, bool depthClip = true)
	{
		m_CullingMode = cullingMode;
		m_OffsetUnits = offsetUnits;
		m_OffsetFactor = offsetFactor;
		m_DepthClip = Convert.ToByte(depthClip);
		m_Padding1 = 0;
		m_Padding2 = 0;
		m_Padding3 = 0;
	}

	public bool Equals(RasterState other)
	{
		return m_CullingMode == other.m_CullingMode && m_OffsetUnits == other.m_OffsetUnits && m_OffsetFactor.Equals(other.m_OffsetFactor) && m_DepthClip == other.m_DepthClip;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is RasterState && Equals((RasterState)obj);
	}

	public override int GetHashCode()
	{
		int num = (int)m_CullingMode;
		num = (num * 397) ^ m_OffsetUnits;
		num = (num * 397) ^ m_OffsetFactor.GetHashCode();
		return (num * 397) ^ m_DepthClip.GetHashCode();
	}

	public static bool operator ==(RasterState left, RasterState right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(RasterState left, RasterState right)
	{
		return !left.Equals(right);
	}
}

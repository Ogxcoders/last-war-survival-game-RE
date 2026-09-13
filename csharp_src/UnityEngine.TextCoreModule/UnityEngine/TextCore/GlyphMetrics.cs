using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore;

[Serializable]
[UsedByNativeCode]
public struct GlyphMetrics : IEquatable<GlyphMetrics>
{
	[SerializeField]
	[NativeName("width")]
	private float m_Width;

	[NativeName("height")]
	[SerializeField]
	private float m_Height;

	[SerializeField]
	[NativeName("horizontalBearingX")]
	private float m_HorizontalBearingX;

	[SerializeField]
	[NativeName("horizontalBearingY")]
	private float m_HorizontalBearingY;

	[SerializeField]
	[NativeName("horizontalAdvance")]
	private float m_HorizontalAdvance;

	public float width
	{
		get
		{
			return m_Width;
		}
		set
		{
			m_Width = value;
		}
	}

	public float height
	{
		get
		{
			return m_Height;
		}
		set
		{
			m_Height = value;
		}
	}

	public float horizontalBearingX
	{
		get
		{
			return m_HorizontalBearingX;
		}
		set
		{
			m_HorizontalBearingX = value;
		}
	}

	public float horizontalBearingY
	{
		get
		{
			return m_HorizontalBearingY;
		}
		set
		{
			m_HorizontalBearingY = value;
		}
	}

	public float horizontalAdvance
	{
		get
		{
			return m_HorizontalAdvance;
		}
		set
		{
			m_HorizontalAdvance = value;
		}
	}

	public GlyphMetrics(float width, float height, float bearingX, float bearingY, float advance)
	{
		m_Width = width;
		m_Height = height;
		m_HorizontalBearingX = bearingX;
		m_HorizontalBearingY = bearingY;
		m_HorizontalAdvance = advance;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public bool Equals(GlyphMetrics other)
	{
		return base.Equals((object)other);
	}

	public static bool operator ==(GlyphMetrics lhs, GlyphMetrics rhs)
	{
		return lhs.width == rhs.width && lhs.height == rhs.height && lhs.horizontalBearingX == rhs.horizontalBearingX && lhs.horizontalBearingY == rhs.horizontalBearingY && lhs.horizontalAdvance == rhs.horizontalAdvance;
	}

	public static bool operator !=(GlyphMetrics lhs, GlyphMetrics rhs)
	{
		return !(lhs == rhs);
	}
}

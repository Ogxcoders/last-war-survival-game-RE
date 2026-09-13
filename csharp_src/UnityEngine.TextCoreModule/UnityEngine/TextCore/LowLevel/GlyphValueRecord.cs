using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel;

[Serializable]
[UsedByNativeCode]
public struct GlyphValueRecord : IEquatable<GlyphValueRecord>
{
	[SerializeField]
	[NativeName("xPlacement")]
	private float m_XPlacement;

	[NativeName("yPlacement")]
	[SerializeField]
	private float m_YPlacement;

	[SerializeField]
	[NativeName("xAdvance")]
	private float m_XAdvance;

	[SerializeField]
	[NativeName("yAdvance")]
	private float m_YAdvance;

	public float xPlacement
	{
		get
		{
			return m_XPlacement;
		}
		set
		{
			m_XPlacement = value;
		}
	}

	public float yPlacement
	{
		get
		{
			return m_YPlacement;
		}
		set
		{
			m_YPlacement = value;
		}
	}

	public float xAdvance
	{
		get
		{
			return m_XAdvance;
		}
		set
		{
			m_XAdvance = value;
		}
	}

	public float yAdvance
	{
		get
		{
			return m_YAdvance;
		}
		set
		{
			m_YAdvance = value;
		}
	}

	public GlyphValueRecord(float xPlacement, float yPlacement, float xAdvance, float yAdvance)
	{
		m_XPlacement = xPlacement;
		m_YPlacement = yPlacement;
		m_XAdvance = xAdvance;
		m_YAdvance = yAdvance;
	}

	public static GlyphValueRecord operator +(GlyphValueRecord a, GlyphValueRecord b)
	{
		GlyphValueRecord result = default(GlyphValueRecord);
		result.m_XPlacement = a.xPlacement + b.xPlacement;
		result.m_YPlacement = a.yPlacement + b.yPlacement;
		result.m_XAdvance = a.xAdvance + b.xAdvance;
		result.m_YAdvance = a.yAdvance + b.yAdvance;
		return result;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public bool Equals(GlyphValueRecord other)
	{
		return base.Equals((object)other);
	}

	public static bool operator ==(GlyphValueRecord lhs, GlyphValueRecord rhs)
	{
		return lhs.m_XPlacement == rhs.m_XPlacement && lhs.m_YPlacement == rhs.m_YPlacement && lhs.m_XAdvance == rhs.m_XAdvance && lhs.m_YAdvance == rhs.m_YAdvance;
	}

	public static bool operator !=(GlyphValueRecord lhs, GlyphValueRecord rhs)
	{
		return !(lhs == rhs);
	}
}

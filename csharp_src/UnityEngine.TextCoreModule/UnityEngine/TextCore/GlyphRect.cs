using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore;

[Serializable]
[UsedByNativeCode]
public struct GlyphRect : IEquatable<GlyphRect>
{
	[NativeName("x")]
	[SerializeField]
	private int m_X;

	[SerializeField]
	[NativeName("y")]
	private int m_Y;

	[NativeName("width")]
	[SerializeField]
	private int m_Width;

	[SerializeField]
	[NativeName("height")]
	private int m_Height;

	private static readonly GlyphRect s_ZeroGlyphRect = new GlyphRect(0, 0, 0, 0);

	public int x
	{
		get
		{
			return m_X;
		}
		set
		{
			m_X = value;
		}
	}

	public int y
	{
		get
		{
			return m_Y;
		}
		set
		{
			m_Y = value;
		}
	}

	public int width
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

	public int height
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

	public static GlyphRect zero => s_ZeroGlyphRect;

	public GlyphRect(int x, int y, int width, int height)
	{
		m_X = x;
		m_Y = y;
		m_Width = width;
		m_Height = height;
	}

	public GlyphRect(Rect rect)
	{
		m_X = (int)rect.x;
		m_Y = (int)rect.y;
		m_Width = (int)rect.width;
		m_Height = (int)rect.height;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public bool Equals(GlyphRect other)
	{
		return base.Equals((object)other);
	}

	public static bool operator ==(GlyphRect lhs, GlyphRect rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
	}

	public static bool operator !=(GlyphRect lhs, GlyphRect rhs)
	{
		return !(lhs == rhs);
	}
}

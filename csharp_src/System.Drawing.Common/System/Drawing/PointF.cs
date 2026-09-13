using System.ComponentModel;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;

namespace System.Drawing;

[Serializable]
[TypeForwardedFrom("Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065")]
public struct PointF : IEquatable<PointF>
{
	public static readonly PointF Empty;

	private float x;

	private float y;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (x == 0f)
			{
				return y == 0f;
			}
			return false;
		}
	}

	public float X
	{
		get
		{
			return x;
		}
		set
		{
			x = value;
		}
	}

	public float Y
	{
		get
		{
			return y;
		}
		set
		{
			y = value;
		}
	}

	public PointF(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public static PointF operator +(PointF pt, Size sz)
	{
		return Add(pt, sz);
	}

	public static PointF operator -(PointF pt, Size sz)
	{
		return Subtract(pt, sz);
	}

	public static PointF operator +(PointF pt, SizeF sz)
	{
		return Add(pt, sz);
	}

	public static PointF operator -(PointF pt, SizeF sz)
	{
		return Subtract(pt, sz);
	}

	public static bool operator ==(PointF left, PointF right)
	{
		if (left.X == right.X)
		{
			return left.Y == right.Y;
		}
		return false;
	}

	public static bool operator !=(PointF left, PointF right)
	{
		return !(left == right);
	}

	public static PointF Add(PointF pt, Size sz)
	{
		return new PointF(pt.X + (float)sz.Width, pt.Y + (float)sz.Height);
	}

	public static PointF Subtract(PointF pt, Size sz)
	{
		return new PointF(pt.X - (float)sz.Width, pt.Y - (float)sz.Height);
	}

	public static PointF Add(PointF pt, SizeF sz)
	{
		return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
	}

	public static PointF Subtract(PointF pt, SizeF sz)
	{
		return new PointF(pt.X - sz.Width, pt.Y - sz.Height);
	}

	public override bool Equals(object obj)
	{
		if (obj is PointF)
		{
			return Equals((PointF)obj);
		}
		return false;
	}

	public bool Equals(PointF other)
	{
		return this == other;
	}

	public override int GetHashCode()
	{
		return System.Numerics.Hashing.HashHelpers.Combine(X.GetHashCode(), Y.GetHashCode());
	}

	public override string ToString()
	{
		return "{X=" + x + ", Y=" + y + "}";
	}
}

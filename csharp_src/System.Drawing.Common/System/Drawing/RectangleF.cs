using System.ComponentModel;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;

namespace System.Drawing;

[Serializable]
[TypeForwardedFrom("Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065")]
public struct RectangleF : IEquatable<RectangleF>
{
	public static readonly RectangleF Empty;

	private float x;

	private float y;

	private float width;

	private float height;

	[Browsable(false)]
	public PointF Location
	{
		get
		{
			return new PointF(X, Y);
		}
		set
		{
			X = value.X;
			Y = value.Y;
		}
	}

	[Browsable(false)]
	public SizeF Size
	{
		get
		{
			return new SizeF(Width, Height);
		}
		set
		{
			Width = value.Width;
			Height = value.Height;
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

	public float Width
	{
		get
		{
			return width;
		}
		set
		{
			width = value;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
		}
	}

	[Browsable(false)]
	public float Left => X;

	[Browsable(false)]
	public float Top => Y;

	[Browsable(false)]
	public float Right => X + Width;

	[Browsable(false)]
	public float Bottom => Y + Height;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (!(Width <= 0f))
			{
				return Height <= 0f;
			}
			return true;
		}
	}

	public RectangleF(float x, float y, float width, float height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	public RectangleF(PointF location, SizeF size)
	{
		x = location.X;
		y = location.Y;
		width = size.Width;
		height = size.Height;
	}

	public static RectangleF FromLTRB(float left, float top, float right, float bottom)
	{
		return new RectangleF(left, top, right - left, bottom - top);
	}

	public override bool Equals(object obj)
	{
		if (obj is RectangleF)
		{
			return Equals((RectangleF)obj);
		}
		return false;
	}

	public bool Equals(RectangleF other)
	{
		return this == other;
	}

	public static bool operator ==(RectangleF left, RectangleF right)
	{
		if (left.X == right.X && left.Y == right.Y && left.Width == right.Width)
		{
			return left.Height == right.Height;
		}
		return false;
	}

	public static bool operator !=(RectangleF left, RectangleF right)
	{
		return !(left == right);
	}

	public bool Contains(float x, float y)
	{
		if (X <= x && x < X + Width && Y <= y)
		{
			return y < Y + Height;
		}
		return false;
	}

	public bool Contains(PointF pt)
	{
		return Contains(pt.X, pt.Y);
	}

	public bool Contains(RectangleF rect)
	{
		if (X <= rect.X && rect.X + rect.Width <= X + Width && Y <= rect.Y)
		{
			return rect.Y + rect.Height <= Y + Height;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(X.GetHashCode(), Y.GetHashCode()), Width.GetHashCode()), Height.GetHashCode());
	}

	public void Inflate(float x, float y)
	{
		X -= x;
		Y -= y;
		Width += 2f * x;
		Height += 2f * y;
	}

	public void Inflate(SizeF size)
	{
		Inflate(size.Width, size.Height);
	}

	public static RectangleF Inflate(RectangleF rect, float x, float y)
	{
		RectangleF result = rect;
		result.Inflate(x, y);
		return result;
	}

	public void Intersect(RectangleF rect)
	{
		RectangleF rectangleF = Intersect(rect, this);
		X = rectangleF.X;
		Y = rectangleF.Y;
		Width = rectangleF.Width;
		Height = rectangleF.Height;
	}

	public static RectangleF Intersect(RectangleF a, RectangleF b)
	{
		float num = Math.Max(a.X, b.X);
		float num2 = Math.Min(a.X + a.Width, b.X + b.Width);
		float num3 = Math.Max(a.Y, b.Y);
		float num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
		if (num2 >= num && num4 >= num3)
		{
			return new RectangleF(num, num3, num2 - num, num4 - num3);
		}
		return Empty;
	}

	public bool IntersectsWith(RectangleF rect)
	{
		if (rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height)
		{
			return Y < rect.Y + rect.Height;
		}
		return false;
	}

	public static RectangleF Union(RectangleF a, RectangleF b)
	{
		float num = Math.Min(a.X, b.X);
		float num2 = Math.Max(a.X + a.Width, b.X + b.Width);
		float num3 = Math.Min(a.Y, b.Y);
		float num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
		return new RectangleF(num, num3, num2 - num, num4 - num3);
	}

	public void Offset(PointF pos)
	{
		Offset(pos.X, pos.Y);
	}

	public void Offset(float x, float y)
	{
		X += x;
		Y += y;
	}

	public static implicit operator RectangleF(Rectangle r)
	{
		return new RectangleF(r.X, r.Y, r.Width, r.Height);
	}

	public override string ToString()
	{
		return "{X=" + X + ",Y=" + Y + ",Width=" + Width + ",Height=" + Height + "}";
	}
}

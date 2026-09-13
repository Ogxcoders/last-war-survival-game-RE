using System.Globalization;

namespace RVO;

public struct Vector2
{
	internal float x_;

	internal float y_;

	public Vector2(float x, float y)
	{
		x_ = x;
		y_ = y;
	}

	public override string ToString()
	{
		return "(" + x_.ToString(new CultureInfo("").NumberFormat) + "," + y_.ToString(new CultureInfo("").NumberFormat) + ")";
	}

	public float x()
	{
		return x_;
	}

	public float y()
	{
		return y_;
	}

	public static float operator *(Vector2 vector1, Vector2 vector2)
	{
		return vector1.x_ * vector2.x_ + vector1.y_ * vector2.y_;
	}

	public static Vector2 operator *(float scalar, Vector2 vector)
	{
		return vector * scalar;
	}

	public static Vector2 operator *(Vector2 vector, float scalar)
	{
		return new Vector2(vector.x_ * scalar, vector.y_ * scalar);
	}

	public static Vector2 operator /(Vector2 vector, float scalar)
	{
		return new Vector2(vector.x_ / scalar, vector.y_ / scalar);
	}

	public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
	{
		return new Vector2(vector1.x_ + vector2.x_, vector1.y_ + vector2.y_);
	}

	public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
	{
		return new Vector2(vector1.x_ - vector2.x_, vector1.y_ - vector2.y_);
	}

	public static Vector2 operator -(Vector2 vector)
	{
		return new Vector2(0f - vector.x_, 0f - vector.y_);
	}
}

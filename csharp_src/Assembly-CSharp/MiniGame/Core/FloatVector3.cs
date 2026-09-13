using System;
using System.Runtime.CompilerServices;

namespace MiniGame.Core;

public struct FloatVector3
{
	public float X;

	public float Y;

	public float Z;

	public static FloatVector3 Zero => default(FloatVector3);

	public static FloatVector3 One => new FloatVector3(1f, 1f, 1f);

	public static FloatVector3 UnitX => new FloatVector3(1f, 0f, 0f);

	public static FloatVector3 UnitY => new FloatVector3(0f, 1f, 0f);

	public static FloatVector3 UnitZ => new FloatVector3(0f, 0f, 1f);

	public FloatVector3(float f)
	{
		X = f;
		Y = f;
		Z = f;
	}

	public FloatVector3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Length()
	{
		return (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LengthSquared()
	{
		return (float)((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector3 Normalize(FloatVector3 value)
	{
		float num = value.Length();
		return new FloatVector3(value.X / num, value.Y / num, value.Z / num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector3 operator *(FloatVector3 left, FloatVector3 right)
	{
		return new FloatVector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector3 operator *(FloatVector3 left, float right)
	{
		return left * new FloatVector3(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector3 operator +(FloatVector3 left, FloatVector3 right)
	{
		return new FloatVector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector3 operator -(FloatVector3 left, FloatVector3 right)
	{
		return new FloatVector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FloatVector3 Lerp(FloatVector3 right, float factor)
	{
		return new FloatVector3(X + (right.X - X) * factor, Y + (right.Y - Y) * factor, Z + (right.Z - Z) * factor);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Angle(FloatVector3 a, FloatVector3 b)
	{
		float num = a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		float num2 = a.Length();
		float num3 = b.Length();
		if (num2 == 0f || num3 == 0f)
		{
			return 0f;
		}
		return (float)(Math.Acos(Clamp(num / (num2 * num3), -1f, 1f)) * (180.0 / Math.PI));
	}

	private static float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(FloatVector3 a, FloatVector3 b)
	{
		float num = a.X - b.X;
		float num2 = a.Y - b.Y;
		float num3 = a.Z - b.Z;
		return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
	}
}

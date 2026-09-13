using System;
using System.Runtime.CompilerServices;

namespace MiniGame.Core;

public struct FloatVector2
{
	public float X;

	public float Y;

	public static FloatVector2 Zero => default(FloatVector2);

	public static FloatVector2 One => new FloatVector2(1f, 1f);

	public static FloatVector2 UnitX => new FloatVector2(1f, 0f);

	public static FloatVector2 UnitY => new FloatVector2(0f, 1f);

	public FloatVector2(float f)
	{
		X = f;
		Y = f;
	}

	public FloatVector2(float x, float y)
	{
		X = x;
		Y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Length()
	{
		return (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LengthSquared()
	{
		return (float)((double)X * (double)X + (double)Y * (double)Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector2 Normalize(FloatVector2 value)
	{
		float num = value.Length();
		return new FloatVector2(value.X / num, value.Y / num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector2 operator *(FloatVector2 left, FloatVector2 right)
	{
		return new FloatVector2(left.X * right.X, left.Y * right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector2 operator *(FloatVector2 left, float right)
	{
		return left * new FloatVector2(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector2 operator +(FloatVector2 left, FloatVector2 right)
	{
		return new FloatVector2(left.X + right.X, left.Y + right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FloatVector2 operator -(FloatVector2 left, FloatVector2 right)
	{
		return new FloatVector2(left.X - right.X, left.Y - right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FloatVector2 Lerp(FloatVector2 right, float factor)
	{
		return new FloatVector2(X + (right.X - X) * factor, Y + (right.Y - Y) * factor);
	}
}

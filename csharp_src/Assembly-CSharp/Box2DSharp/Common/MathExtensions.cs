using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public static class MathExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetZero(this ref FVector2 vector2)
	{
		vector2.X = 0f;
		vector2.Y = 0f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Set(this ref FVector2 vector2, FP x, FP y)
	{
		vector2.X = x;
		vector2.Y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetZero(this ref FVector3 vector3)
	{
		vector3.X = 0f;
		vector3.Y = 0f;
		vector3.Z = 0f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Set(this ref FVector3 vector3, FP x, FP y, FP z)
	{
		vector3.X = x;
		vector3.Y = y;
		vector3.Z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Skew(this ref FVector2 vector2)
	{
		return new FVector2(-vector2.Y, vector2.X);
	}
}

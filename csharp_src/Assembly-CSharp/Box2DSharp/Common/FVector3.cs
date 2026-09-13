using System;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public struct FVector3 : IEquatable<FVector3>
{
	private static FP ZeroEpsilonSq;

	internal static FVector3 InternalZero;

	internal static FVector3 Arbitrary;

	public static readonly FVector3 Zero;

	public static readonly FVector3 Left;

	public static readonly FVector3 Right;

	public static readonly FVector3 Up;

	public static readonly FVector3 Down;

	public static readonly FVector3 Back;

	public static readonly FVector3 Forward;

	public static readonly FVector3 One;

	public static readonly FVector3 MinValue;

	public static readonly FVector3 MaxValue;

	public FP X;

	public FP Y;

	public FP Z;

	static FVector3()
	{
		ZeroEpsilonSq = FMath.Epsilon;
		One = new FVector3(1, 1, 1);
		Zero = new FVector3(0, 0, 0);
		Left = new FVector3(-1, 0, 0);
		Right = new FVector3(1, 0, 0);
		Up = new FVector3(0, 1, 0);
		Down = new FVector3(0, -1, 0);
		Back = new FVector3(0, 0, -1);
		Forward = new FVector3(0, 0, 1);
		MinValue = new FVector3(FP.MinValue);
		MaxValue = new FVector3(FP.MaxValue);
		Arbitrary = new FVector3(1, 1, 1);
		InternalZero = Zero;
	}

	public FVector3(FP x, FP y, FP z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public FVector3(FP value)
	{
		X = value;
		Y = value;
		Z = value;
	}

	public void Set(FP x, FP y, FP z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public bool Equals(FVector3 other)
	{
		if (X.Equals(other.X) && Y.Equals(other.Y))
		{
			return Z.Equals(other.Z);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is FVector3 other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((X.GetHashCode() * 397) ^ Y.GetHashCode()) * 397) ^ Z.GetHashCode();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FP Dot(FVector3 vector1, FVector3 vector2)
	{
		return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 Cross(FVector3 vector1, FVector3 vector2)
	{
		return new FVector3(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
	}

	public static FVector3 Min(FVector3 value1, FVector3 value2)
	{
		return new FVector3((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y, (value1.Z < value2.Z) ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 Max(FVector3 value1, FVector3 value2)
	{
		return new FVector3((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y, (value1.Z > value2.Z) ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 Abs(FVector3 value)
	{
		return new FVector3(FP.Abs(value.X), FP.Abs(value.Y), FP.Abs(value.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 SquareRoot(FVector3 value)
	{
		return new FVector3(FP.Sqrt(value.X), FP.Sqrt(value.Y), FP.Sqrt(value.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator +(FVector3 left, FVector3 right)
	{
		return new FVector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator -(FVector3 left, FVector3 right)
	{
		return new FVector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator *(FVector3 left, FVector3 right)
	{
		return new FVector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator *(FVector3 left, FP right)
	{
		return left * new FVector3(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator *(FP left, FVector3 right)
	{
		return new FVector3(left) * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator /(FVector3 left, FVector3 right)
	{
		return new FVector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator /(FVector3 value1, FP value2)
	{
		return value1 / new FVector3(value2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 operator -(FVector3 value)
	{
		return Zero - value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FVector3 left, FVector3 right)
	{
		if (left.X == right.X && left.Y == right.Y)
		{
			return left.Z == right.Z;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FVector3 left, FVector3 right)
	{
		return !(left == right);
	}
}

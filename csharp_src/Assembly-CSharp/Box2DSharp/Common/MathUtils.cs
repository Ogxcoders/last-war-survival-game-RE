using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public static class MathUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FP Cross(in FVector2 a, in FVector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Cross(in FVector2 a, FP s)
	{
		return new FVector2(s * a.Y, -s * a.X);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Cross(FP s, in FVector2 a)
	{
		return new FVector2(-s * a.Y, s * a.X);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Mul(in Matrix2x2 m, in FVector2 v)
	{
		return new FVector2(m.Ex.X * v.X + m.Ey.X * v.Y, m.Ex.Y * v.X + m.Ey.Y * v.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 MulT(in Matrix2x2 m, in FVector2 v)
	{
		return new FVector2(FVector2.Dot(v, m.Ex), FVector2.Dot(v, m.Ey));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix2x2 Mul(in Matrix2x2 a, in Matrix2x2 b)
	{
		return new Matrix2x2(Mul(in a, in b.Ex), Mul(in a, in b.Ey));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix2x2 MulT(in Matrix2x2 a, in Matrix2x2 b)
	{
		return new Matrix2x2(new FVector2(FVector2.Dot(a.Ex, b.Ex), FVector2.Dot(a.Ey, b.Ex)), new FVector2(FVector2.Dot(a.Ex, b.Ey), FVector2.Dot(a.Ey, b.Ey)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector3 Mul(in Matrix3x3 m, in FVector3 v)
	{
		return v.X * m.Ex + v.Y * m.Ey + v.Z * m.Ez;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Mul22(in Matrix3x3 m, in FVector2 v)
	{
		return new FVector2(m.Ex.X * v.X + m.Ey.X * v.Y, m.Ex.Y * v.X + m.Ey.Y * v.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rotation Mul(in Rotation q, in Rotation r)
	{
		return new Rotation(q.Sin * r.Cos + q.Cos * r.Sin, q.Cos * r.Cos - q.Sin * r.Sin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rotation MulT(in Rotation q, in Rotation r)
	{
		return new Rotation(q.Cos * r.Sin - q.Sin * r.Cos, q.Cos * r.Cos + q.Sin * r.Sin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Mul(in Rotation q, in FVector2 v)
	{
		return new FVector2(q.Cos * v.X - q.Sin * v.Y, q.Sin * v.X + q.Cos * v.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 MulT(in Rotation q, in FVector2 v)
	{
		return new FVector2(q.Cos * v.X + q.Sin * v.Y, -q.Sin * v.X + q.Cos * v.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 Mul(in Transform T, in FVector2 v)
	{
		FP x = T.Rotation.Cos * v.X - T.Rotation.Sin * v.Y + T.Position.X;
		FP y = T.Rotation.Sin * v.X + T.Rotation.Cos * v.Y + T.Position.Y;
		return new FVector2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2 MulT(in Transform T, in FVector2 v)
	{
		FP y = v.X - T.Position.X;
		FP y2 = v.Y - T.Position.Y;
		return new FVector2(T.Rotation.Cos * y + T.Rotation.Sin * y2, -T.Rotation.Sin * y + T.Rotation.Cos * y2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Transform Mul(in Transform A, in Transform B)
	{
		return new Transform(Mul(in A.Rotation, in B.Position) + A.Position, Mul(in A.Rotation, in B.Rotation));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Transform MulT(in Transform A, in Transform B)
	{
		return new Transform(MulT(in A.Rotation, B.Position - A.Position), MulT(in A.Rotation, in B.Rotation));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FP Clamp(FP a, FP low, FP high)
	{
		if (!(a < low))
		{
			if (!(a > high))
			{
				return a;
			}
			return high;
		}
		return low;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FP SmothStep(FP left, FP right, FP value)
	{
		FP x = Clamp((value - left) / (right - left), 0, 1f);
		return x * x * ((FP)3 - (FP)2 * x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap<T>(ref T a, ref T b)
	{
		T val = a;
		a = b;
		b = val;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint NextPowerOfTwo(uint x)
	{
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsPowerOfTwo(uint x)
	{
		if (x != 0)
		{
			return (x & (x - 1)) == 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetArraySize(int capacity)
	{
		int num = capacity - 1;
		num |= num >> 1;
		num |= num >> 2;
		num |= num >> 4;
		num |= num >> 8;
		num |= num >> 16;
		if (num >= 0)
		{
			return num + 1;
		}
		return 128;
	}
}

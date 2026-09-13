using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public struct Matrix2x2
{
	public FVector2 Ex;

	public FVector2 Ey;

	public Matrix2x2(in FVector2 c1, in FVector2 c2)
	{
		Ex = c1;
		Ey = c2;
	}

	public Matrix2x2(FP a11, FP a12, FP a21, FP a22)
	{
		Ex.X = a11;
		Ex.Y = a21;
		Ey.X = a12;
		Ey.Y = a22;
	}

	public void Set(in FVector2 c1, in FVector2 c2)
	{
		Ex = c1;
		Ey = c2;
	}

	public void SetIdentity()
	{
		Ex.X = 1f;
		Ey.X = 0f;
		Ex.Y = 0f;
		Ey.Y = 1f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetZero()
	{
		Ex.X = 0f;
		Ey.X = 0f;
		Ex.Y = 0f;
		Ey.Y = 0f;
	}

	public Matrix2x2 GetInverse()
	{
		FP x = Ex.X;
		FP x2 = Ey.X;
		FP y = Ex.Y;
		FP y2 = Ey.Y;
		FP x3 = x * y2 - x2 * y;
		if (!x3.Equals(0f))
		{
			x3 = 1f / x3;
		}
		Matrix2x2 result = default(Matrix2x2);
		result.Ex.X = x3 * y2;
		result.Ey.X = -x3 * x2;
		result.Ex.Y = -x3 * y;
		result.Ey.Y = x3 * x;
		return result;
	}

	public FVector2 Solve(in FVector2 b)
	{
		FP x = Ex.X;
		FP x2 = Ey.X;
		FP y = Ex.Y;
		FP y2 = Ey.Y;
		FP x3 = x * y2 - x2 * y;
		if (x3 != 0)
		{
			x3 = 1f / x3;
		}
		return new FVector2
		{
			X = x3 * (y2 * b.X - x2 * b.Y),
			Y = x3 * (x * b.Y - y * b.X)
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix2x2 operator +(in Matrix2x2 A, in Matrix2x2 B)
	{
		return new Matrix2x2(A.Ex + B.Ex, A.Ey + B.Ey);
	}
}

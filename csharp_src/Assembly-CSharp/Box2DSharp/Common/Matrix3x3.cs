namespace Box2DSharp.Common;

public struct Matrix3x3
{
	public FVector3 Ex;

	public FVector3 Ey;

	public FVector3 Ez;

	public Matrix3x3(in FVector3 c1, in FVector3 c2, in FVector3 c3)
	{
		Ex = c1;
		Ey = c2;
		Ez = c3;
	}

	public void SetZero()
	{
		Ex.SetZero();
		Ey.SetZero();
		Ez.SetZero();
	}

	public FVector3 Solve33(in FVector3 b)
	{
		FP x = FVector3.Dot(Ex, FVector3.Cross(Ey, Ez));
		if (!x.Equals(0f))
		{
			x = 1f / x;
		}
		FVector3 result = default(FVector3);
		result.X = x * FVector3.Dot(b, FVector3.Cross(Ey, Ez));
		result.Y = x * FVector3.Dot(Ex, FVector3.Cross(b, Ez));
		result.Z = x * FVector3.Dot(Ex, FVector3.Cross(Ey, b));
		return result;
	}

	public FVector2 Solve22(in FVector2 b)
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
		FVector2 result = default(FVector2);
		result.X = x3 * (y2 * b.X - x2 * b.Y);
		result.Y = x3 * (x * b.Y - y * b.X);
		return result;
	}

	public void GetInverse22(ref Matrix3x3 matrix3X3)
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
		matrix3X3.Ex.X = x3 * y2;
		matrix3X3.Ey.X = -x3 * x2;
		matrix3X3.Ex.Z = 0f;
		matrix3X3.Ex.Y = -x3 * y;
		matrix3X3.Ey.Y = x3 * x;
		matrix3X3.Ey.Z = 0f;
		matrix3X3.Ez.X = 0f;
		matrix3X3.Ez.Y = 0f;
		matrix3X3.Ez.Z = 0f;
	}

	public void GetSymInverse33(ref Matrix3x3 matrix3X3)
	{
		FP x = FVector3.Dot(Ex, FVector3.Cross(Ey, Ez));
		if (!x.Equals(0f))
		{
			x = 1f / x;
		}
		FP x2 = Ex.X;
		FP x3 = Ey.X;
		FP x4 = Ez.X;
		FP x5 = Ey.Y;
		FP x6 = Ez.Y;
		FP y = Ez.Z;
		matrix3X3.Ex.X = x * (x5 * y - x6 * x6);
		matrix3X3.Ex.Y = x * (x4 * x6 - x3 * y);
		matrix3X3.Ex.Z = x * (x3 * x6 - x4 * x5);
		matrix3X3.Ey.X = matrix3X3.Ex.Y;
		matrix3X3.Ey.Y = x * (x2 * y - x4 * x4);
		matrix3X3.Ey.Z = x * (x4 * x3 - x2 * x6);
		matrix3X3.Ez.X = matrix3X3.Ex.Z;
		matrix3X3.Ez.Y = matrix3X3.Ey.Z;
		matrix3X3.Ez.Z = x * (x2 * x5 - x3 * x3);
	}
}

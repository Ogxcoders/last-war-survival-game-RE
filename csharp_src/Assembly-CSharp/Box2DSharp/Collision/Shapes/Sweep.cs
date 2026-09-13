using Box2DSharp.Common;

namespace Box2DSharp.Collision.Shapes;

public struct Sweep
{
	public FVector2 LocalCenter;

	public FVector2 C0;

	public FVector2 C;

	public FP A0;

	public FP A;

	public FP Alpha0;

	public void GetTransform(out Transform xf, FP beta)
	{
		FVector2 position = ((FP)1f - beta) * C0 + beta * C;
		FP angle = ((FP)1f - beta) * A0 + beta * A;
		xf = new Transform(in position, angle);
		xf.Position -= MathUtils.Mul(in xf.Rotation, in LocalCenter);
	}

	public void Advance(FP alpha)
	{
		FP x = (alpha - Alpha0) / ((FP)1f - Alpha0);
		C0 += x * (C - C0);
		A0 += x * (A - A0);
		Alpha0 = alpha;
	}

	public void Normalize()
	{
		FP y = FP.PiTimes2 * FP.Floor(A0 / FP.PiTimes2);
		A0 -= y;
		A -= y;
	}
}

using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public struct Rotation
{
	public FP Sin;

	public FP Cos;

	public FP Angle => FP.Atan2(Sin, Cos);

	public Rotation(FP sin, FP cos)
	{
		Sin = sin;
		Cos = cos;
	}

	public Rotation(FP angle)
	{
		Sin = FP.Sin(angle);
		Cos = FP.Cos(angle);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(FP angle)
	{
		Sin = FP.Sin(angle);
		Cos = FP.Cos(angle);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetIdentity()
	{
		Sin = 0f;
		Cos = 1f;
	}

	public FVector2 GetXAxis()
	{
		return new FVector2(Cos, Sin);
	}

	public FVector2 GetYAxis()
	{
		return new FVector2(-Sin, Cos);
	}
}

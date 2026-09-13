using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct DistanceOutput
{
	public FVector2 PointA;

	public FVector2 PointB;

	public FP Distance;

	public int Iterations;
}

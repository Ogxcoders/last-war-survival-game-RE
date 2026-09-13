using Box2DSharp.Common;

namespace Box2DSharp.Collision.Collider;

public struct RayCastInput
{
	public FVector2 P1;

	public FVector2 P2;

	public FP MaxFraction;
}

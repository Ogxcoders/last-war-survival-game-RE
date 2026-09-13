using Box2DSharp.Common;

namespace Box2DSharp.Collision.Collider;

public struct ManifoldPoint
{
	public FVector2 LocalPoint;

	public FP NormalImpulse;

	public FP TangentImpulse;

	public ContactId Id;
}

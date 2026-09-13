using Box2DSharp.Common;

namespace Box2DSharp.Collision.Collider;

public struct Manifold
{
	public FixedArray2<ManifoldPoint> Points;

	public FVector2 LocalNormal;

	public FVector2 LocalPoint;

	public ManifoldType Type;

	public int PointCount;
}

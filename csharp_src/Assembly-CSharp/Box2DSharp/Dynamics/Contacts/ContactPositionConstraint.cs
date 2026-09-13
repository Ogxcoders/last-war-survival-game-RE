using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public struct ContactPositionConstraint
{
	public FixedArray2<FVector2> LocalPoints;

	public int IndexA;

	public int IndexB;

	public FP InvIa;

	public FP InvIb;

	public FP InvMassA;

	public FP InvMassB;

	public FVector2 LocalCenterA;

	public FVector2 LocalCenterB;

	public FVector2 LocalNormal;

	public FVector2 LocalPoint;

	public int PointCount;

	public FP RadiusA;

	public FP RadiusB;

	public ManifoldType Type;
}

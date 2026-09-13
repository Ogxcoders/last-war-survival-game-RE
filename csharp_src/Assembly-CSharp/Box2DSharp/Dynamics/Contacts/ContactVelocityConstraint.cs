using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public struct ContactVelocityConstraint
{
	public FixedArray2<VelocityConstraintPoint> Points;

	public int ContactIndex;

	public FP Friction;

	public int IndexA;

	public int IndexB;

	public FP InvIa;

	public FP InvIb;

	public FP InvMassA;

	public FP InvMassB;

	public Matrix2x2 K;

	public FVector2 Normal;

	public Matrix2x2 NormalMass;

	public int PointCount;

	public FP Restitution;

	public FP Threshold;

	public FP TangentSpeed;
}

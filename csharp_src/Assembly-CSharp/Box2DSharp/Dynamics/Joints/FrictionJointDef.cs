using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class FrictionJointDef : JointDef
{
	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public FP MaxForce;

	public FP MaxTorque;

	public FrictionJointDef()
	{
		JointType = JointType.FrictionJoint;
		LocalAnchorA.SetZero();
		LocalAnchorB.SetZero();
		MaxForce = 0f;
		MaxTorque = 0f;
	}

	public void Initialize(Body bA, Body bB, in FVector2 anchor)
	{
		BodyA = bA;
		BodyB = bB;
		LocalAnchorA = BodyA.GetLocalPoint(in anchor);
		LocalAnchorB = BodyB.GetLocalPoint(in anchor);
	}
}

using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class WeldJointDef : JointDef
{
	public FP Stiffness;

	public FP Damping;

	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public FP ReferenceAngle;

	public WeldJointDef()
	{
		JointType = JointType.WeldJoint;
		LocalAnchorA.Set(0f, 0f);
		LocalAnchorB.Set(0f, 0f);
		ReferenceAngle = 0f;
		Stiffness = 0f;
		Damping = 0f;
	}

	public void Initialize(Body bA, Body bB, in FVector2 anchor)
	{
		BodyA = bA;
		BodyB = bB;
		LocalAnchorA = BodyA.GetLocalPoint(in anchor);
		LocalAnchorB = BodyB.GetLocalPoint(in anchor);
		ReferenceAngle = BodyB.GetAngle() - BodyA.GetAngle();
	}
}

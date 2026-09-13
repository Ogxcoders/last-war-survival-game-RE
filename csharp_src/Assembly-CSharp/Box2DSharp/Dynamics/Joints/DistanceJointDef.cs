using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class DistanceJointDef : JointDef
{
	public FP MinLength;

	public FP MaxLength;

	public FP Stiffness;

	public FP Damping;

	public FP Length;

	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public DistanceJointDef()
	{
		JointType = JointType.DistanceJoint;
		LocalAnchorA.Set(0f, 0f);
		LocalAnchorB.Set(0f, 0f);
		MinLength = 0f;
		MaxLength = Settings.MaxFloat;
		Length = 1f;
		Stiffness = 0f;
		Damping = 0f;
	}

	public void Initialize(Body b1, Body b2, in FVector2 anchor1, in FVector2 anchor2)
	{
		BodyA = b1;
		BodyB = b2;
		LocalAnchorA = BodyA.GetLocalPoint(in anchor1);
		LocalAnchorB = BodyB.GetLocalPoint(in anchor2);
		Length = FP.Max((anchor2 - anchor1).Length(), Settings.LinearSlop);
		MinLength = Length;
		MaxLength = Length;
	}
}

using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class PulleyJointDef : JointDef
{
	public FVector2 GroundAnchorA;

	public FVector2 GroundAnchorB;

	public FP LengthA;

	public FP LengthB;

	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public FP Ratio;

	public PulleyJointDef()
	{
		JointType = JointType.PulleyJoint;
		GroundAnchorA.Set(-1f, 1f);
		GroundAnchorB.Set(1f, 1f);
		LocalAnchorA.Set(-1f, 0f);
		LocalAnchorB.Set(1f, 0f);
		LengthA = 0f;
		LengthB = 0f;
		Ratio = 1f;
		CollideConnected = true;
	}

	public void Initialize(Body bA, Body bB, in FVector2 groundA, in FVector2 groundB, in FVector2 anchorA, in FVector2 anchorB, FP r)
	{
		BodyA = bA;
		BodyB = bB;
		GroundAnchorA = groundA;
		GroundAnchorB = groundB;
		LocalAnchorA = BodyA.GetLocalPoint(in anchorA);
		LocalAnchorB = BodyB.GetLocalPoint(in anchorB);
		LengthA = (anchorA - groundA).Length();
		LengthB = (anchorB - groundB).Length();
		Ratio = r;
	}
}

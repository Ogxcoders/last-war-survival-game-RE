using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class MouseJointDef : JointDef
{
	public FP Damping;

	public FP Stiffness;

	public FP MaxForce;

	public FVector2 Target;

	public MouseJointDef()
	{
		JointType = JointType.MouseJoint;
		Target.Set(0f, 0f);
		MaxForce = 0f;
		Stiffness = 5f;
		Damping = 0.7f;
	}
}

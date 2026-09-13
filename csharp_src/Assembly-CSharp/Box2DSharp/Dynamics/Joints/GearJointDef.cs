using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class GearJointDef : JointDef
{
	public Joint Joint1;

	public Joint Joint2;

	public FP Ratio;

	public GearJointDef()
	{
		JointType = JointType.GearJoint;
		Joint1 = null;
		Joint2 = null;
		Ratio = 1f;
	}
}

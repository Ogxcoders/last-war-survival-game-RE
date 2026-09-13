using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class MotorJointDef : JointDef
{
	public FP AngularOffset;

	public FP CorrectionFactor;

	public FVector2 LinearOffset;

	public FP MaxForce;

	public FP MaxTorque;

	public MotorJointDef()
	{
		JointType = JointType.MotorJoint;
		LinearOffset.SetZero();
		AngularOffset = 0f;
		MaxForce = 1f;
		MaxTorque = 1f;
		CorrectionFactor = 0.3f;
	}

	public void Initialize(Body bA, Body bB)
	{
		BodyA = bA;
		BodyB = bB;
		FVector2 worldPoint = BodyB.GetPosition();
		LinearOffset = BodyA.GetLocalPoint(in worldPoint);
		FP y = BodyA.GetAngle();
		AngularOffset = BodyB.GetAngle() - y;
	}
}

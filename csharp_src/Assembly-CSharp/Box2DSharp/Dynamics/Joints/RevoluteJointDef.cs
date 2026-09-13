using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class RevoluteJointDef : JointDef
{
	public bool EnableLimit;

	public bool EnableMotor;

	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public FP LowerAngle;

	public FP MaxMotorTorque;

	public FP MotorSpeed;

	public FP ReferenceAngle;

	public FP UpperAngle;

	public RevoluteJointDef()
	{
		JointType = JointType.RevoluteJoint;
		LocalAnchorA.Set(0f, 0f);
		LocalAnchorB.Set(0f, 0f);
		ReferenceAngle = 0f;
		LowerAngle = 0f;
		UpperAngle = 0f;
		MaxMotorTorque = 0f;
		MotorSpeed = 0f;
		EnableLimit = false;
		EnableMotor = false;
	}

	public void Initialize(Body bA, Body bB, FVector2 anchor)
	{
		BodyA = bA;
		BodyB = bB;
		LocalAnchorA = BodyA.GetLocalPoint(in anchor);
		LocalAnchorB = BodyB.GetLocalPoint(in anchor);
		ReferenceAngle = BodyB.GetAngle() - BodyA.GetAngle();
	}
}

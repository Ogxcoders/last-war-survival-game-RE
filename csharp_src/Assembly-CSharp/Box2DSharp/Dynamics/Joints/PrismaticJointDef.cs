using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class PrismaticJointDef : JointDef
{
	public bool EnableLimit;

	public bool EnableMotor;

	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public FVector2 LocalAxisA;

	public FP LowerTranslation;

	public FP MaxMotorForce;

	public FP MotorSpeed;

	public FP ReferenceAngle;

	public FP UpperTranslation;

	public PrismaticJointDef()
	{
		JointType = JointType.PrismaticJoint;
		LocalAnchorA.SetZero();
		LocalAnchorB.SetZero();
		LocalAxisA.Set(1f, 0f);
		ReferenceAngle = 0f;
		EnableLimit = false;
		LowerTranslation = 0f;
		UpperTranslation = 0f;
		EnableMotor = false;
		MaxMotorForce = 0f;
		MotorSpeed = 0f;
	}

	public void Initialize(Body bA, Body bB, in FVector2 anchor, in FVector2 axis)
	{
		BodyA = bA;
		BodyB = bB;
		LocalAnchorA = BodyA.GetLocalPoint(in anchor);
		LocalAnchorB = BodyB.GetLocalPoint(in anchor);
		LocalAxisA = BodyA.GetLocalVector(in axis);
		ReferenceAngle = BodyB.GetAngle() - BodyA.GetAngle();
	}
}

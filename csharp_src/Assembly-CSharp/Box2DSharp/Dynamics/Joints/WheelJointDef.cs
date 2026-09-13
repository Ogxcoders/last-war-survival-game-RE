using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class WheelJointDef : JointDef
{
	public FVector2 LocalAnchorA;

	public FVector2 LocalAnchorB;

	public FVector2 LocalAxisA;

	public bool EnableLimit;

	public FP LowerTranslation;

	public FP UpperTranslation;

	public bool EnableMotor;

	public FP MaxMotorTorque;

	public FP MotorSpeed;

	public FP Stiffness;

	public FP Damping;

	public WheelJointDef()
	{
		JointType = JointType.WheelJoint;
		LocalAnchorA.SetZero();
		LocalAnchorB.SetZero();
		LocalAxisA.Set(1f, 0f);
		EnableLimit = false;
		LowerTranslation = 0f;
		UpperTranslation = 0f;
		EnableMotor = false;
		MaxMotorTorque = 0f;
		MotorSpeed = 0f;
		Stiffness = 0f;
		Damping = 0f;
	}

	public void Initialize(Body bA, Body bB, in FVector2 anchor, in FVector2 axis)
	{
		BodyA = bA;
		BodyB = bB;
		LocalAnchorA = BodyA.GetLocalPoint(in anchor);
		LocalAnchorB = BodyB.GetLocalPoint(in anchor);
		LocalAxisA = BodyA.GetLocalVector(in axis);
	}
}

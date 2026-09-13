using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class GearJoint : Joint
{
	private readonly Body _bodyC;

	private readonly Body _bodyD;

	private readonly FP _constant;

	private readonly Joint _joint1;

	private readonly Joint _joint2;

	private readonly FVector2 _localAnchorA;

	private readonly FVector2 _localAnchorB;

	private readonly FVector2 _localAnchorC;

	private readonly FVector2 _localAnchorD;

	private readonly FVector2 _localAxisC;

	private readonly FVector2 _localAxisD;

	private readonly FP _referenceAngleA;

	private readonly FP _referenceAngleB;

	private readonly JointType _typeA;

	private readonly JointType _typeB;

	private FP _iA;

	private FP _iB;

	private FP _iC;

	private FP _iD;

	private FP _impulse;

	private int _indexA;

	private int _indexB;

	private int _indexC;

	private int _indexD;

	private FVector2 _jvAc;

	private FVector2 _jvBd;

	private FP _jwA;

	private FP _jwB;

	private FP _jwC;

	private FP _jwD;

	private FVector2 _lcA;

	private FVector2 _lcB;

	private FVector2 _lcC;

	private FVector2 _lcD;

	private FP _mA;

	private FP _mB;

	private FP _mC;

	private FP _mD;

	private FP _mass;

	private FP _ratio;

	private FP _tolerance;

	public GearJoint(GearJointDef def)
		: base(def)
	{
		_joint1 = def.Joint1;
		_joint2 = def.Joint2;
		_typeA = _joint1.JointType;
		_typeB = _joint2.JointType;
		_bodyC = _joint1.BodyA;
		BodyA = _joint1.BodyB;
		Transform transform = BodyA.Transform;
		FP x = BodyA.Sweep.A;
		Transform transform2 = _bodyC.Transform;
		FP y = _bodyC.Sweep.A;
		FP x2;
		if (_typeA == JointType.RevoluteJoint)
		{
			RevoluteJoint revoluteJoint = (RevoluteJoint)def.Joint1;
			_localAnchorC = revoluteJoint.LocalAnchorA;
			_localAnchorA = revoluteJoint.LocalAnchorB;
			_referenceAngleA = revoluteJoint.ReferenceAngle;
			_localAxisC.SetZero();
			x2 = x - y - _referenceAngleA;
			_tolerance = Settings.AngularSlop;
		}
		else
		{
			PrismaticJoint prismaticJoint = (PrismaticJoint)def.Joint1;
			_localAnchorC = prismaticJoint.LocalAnchorA;
			_localAnchorA = prismaticJoint.LocalAnchorB;
			_referenceAngleA = prismaticJoint.ReferenceAngle;
			_localAxisC = prismaticJoint.LocalXAxisA;
			FVector2 localAnchorC = _localAnchorC;
			x2 = FVector2.Dot(MathUtils.MulT(in transform2.Rotation, MathUtils.Mul(in transform.Rotation, in _localAnchorA) + (transform.Position - transform2.Position)) - localAnchorC, _localAxisC);
			_tolerance = Settings.LinearSlop;
		}
		_bodyD = _joint2.BodyA;
		BodyB = _joint2.BodyB;
		Transform transform3 = BodyB.Transform;
		FP x3 = BodyB.Sweep.A;
		Transform transform4 = _bodyD.Transform;
		FP y2 = _bodyD.Sweep.A;
		FP y3;
		if (_typeB == JointType.RevoluteJoint)
		{
			RevoluteJoint revoluteJoint2 = (RevoluteJoint)def.Joint2;
			_localAnchorD = revoluteJoint2.LocalAnchorA;
			_localAnchorB = revoluteJoint2.LocalAnchorB;
			_referenceAngleB = revoluteJoint2.ReferenceAngle;
			_localAxisD.SetZero();
			y3 = x3 - y2 - _referenceAngleB;
		}
		else
		{
			PrismaticJoint prismaticJoint2 = (PrismaticJoint)def.Joint2;
			_localAnchorD = prismaticJoint2.LocalAnchorA;
			_localAnchorB = prismaticJoint2.LocalAnchorB;
			_referenceAngleB = prismaticJoint2.ReferenceAngle;
			_localAxisD = prismaticJoint2.LocalXAxisA;
			FVector2 localAnchorD = _localAnchorD;
			y3 = FVector2.Dot(MathUtils.MulT(in transform4.Rotation, MathUtils.Mul(in transform3.Rotation, in _localAnchorB) + (transform3.Position - transform4.Position)) - localAnchorD, _localAxisD);
		}
		_ratio = def.Ratio;
		_constant = x2 + _ratio * y3;
		_impulse = 0f;
	}

	public Joint GetJoint1()
	{
		return _joint1;
	}

	public Joint GetJoint2()
	{
		return _joint2;
	}

	public void SetRatio(FP ratio)
	{
		_ratio = ratio;
	}

	public FP GetRatio()
	{
		return _ratio;
	}

	public override void Dump()
	{
		int islandIndex = BodyA.IslandIndex;
		int islandIndex2 = BodyB.IslandIndex;
		_ = _joint1.Index;
		int index = _joint2.Index;
		DumpLogger.Log("  b2GearJointDef jd;");
		DumpLogger.Log($"  jd.bodyA = bodies[{islandIndex}];");
		DumpLogger.Log($"  jd.bodyB = bodies[{islandIndex2}];");
		DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
		DumpLogger.Log("  jd.joint1 = joints[index1];");
		DumpLogger.Log($"  jd.joint2 = joints[{index}];");
		DumpLogger.Log($"  jd.ratio = {_ratio};");
		DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
	}

	public override FVector2 GetAnchorA()
	{
		return BodyA.GetWorldPoint(in _localAnchorA);
	}

	public override FVector2 GetAnchorB()
	{
		return BodyB.GetWorldPoint(in _localAnchorB);
	}

	public override FVector2 GetReactionForce(FP inv_dt)
	{
		FVector2 fVector = _impulse * _jvAc;
		return inv_dt * fVector;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * (_impulse * _jwA);
	}

	internal override void InitVelocityConstraints(in SolverData data)
	{
		_indexA = BodyA.IslandIndex;
		_indexB = BodyB.IslandIndex;
		_indexC = _bodyC.IslandIndex;
		_indexD = _bodyD.IslandIndex;
		_lcA = BodyA.Sweep.LocalCenter;
		_lcB = BodyB.Sweep.LocalCenter;
		_lcC = _bodyC.Sweep.LocalCenter;
		_lcD = _bodyD.Sweep.LocalCenter;
		_mA = BodyA.InvMass;
		_mB = BodyB.InvMass;
		_mC = _bodyC.InvMass;
		_mD = _bodyD.InvMass;
		_iA = BodyA.InverseInertia;
		_iB = BodyB.InverseInertia;
		_iC = _bodyC.InverseInertia;
		_iD = _bodyD.InverseInertia;
		FP angle = data.Positions[_indexA].Angle;
		FVector2 v = data.Velocities[_indexA].V;
		FP x = data.Velocities[_indexA].W;
		FP angle2 = data.Positions[_indexB].Angle;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x2 = data.Velocities[_indexB].W;
		FP angle3 = data.Positions[_indexC].Angle;
		FVector2 v3 = data.Velocities[_indexC].V;
		FP x3 = data.Velocities[_indexC].W;
		FP angle4 = data.Positions[_indexD].Angle;
		FVector2 v4 = data.Velocities[_indexD].V;
		FP x4 = data.Velocities[_indexD].W;
		Rotation q = new Rotation(angle);
		Rotation q2 = new Rotation(angle2);
		Rotation q3 = new Rotation(angle3);
		Rotation q4 = new Rotation(angle4);
		_mass = 0f;
		if (_typeA == JointType.RevoluteJoint)
		{
			_jvAc.SetZero();
			_jwA = 1f;
			_jwC = 1f;
			_mass += _iA + _iC;
		}
		else
		{
			FVector2 b = MathUtils.Mul(in q3, in _localAxisC);
			FVector2 a = MathUtils.Mul(in q3, _localAnchorC - _lcC);
			FVector2 a2 = MathUtils.Mul(in q, _localAnchorA - _lcA);
			_jvAc = b;
			_jwC = MathUtils.Cross(in a, in b);
			_jwA = MathUtils.Cross(in a2, in b);
			_mass += _mC + _mA + _iC * _jwC * _jwC + _iA * _jwA * _jwA;
		}
		if (_typeB == JointType.RevoluteJoint)
		{
			_jvBd.SetZero();
			_jwB = _ratio;
			_jwD = _ratio;
			_mass += _ratio * _ratio * (_iB + _iD);
		}
		else
		{
			FVector2 b2 = MathUtils.Mul(in q4, in _localAxisD);
			FVector2 a3 = MathUtils.Mul(in q4, _localAnchorD - _lcD);
			FVector2 a4 = MathUtils.Mul(in q2, _localAnchorB - _lcB);
			_jvBd = _ratio * b2;
			_jwD = _ratio * MathUtils.Cross(in a3, in b2);
			_jwB = _ratio * MathUtils.Cross(in a4, in b2);
			_mass += _ratio * _ratio * (_mD + _mB) + _iD * _jwD * _jwD + _iB * _jwB * _jwB;
		}
		_mass = ((_mass > FP.Zero) ? (FP.One / _mass) : FP.Zero);
		if (data.Step.WarmStarting)
		{
			v += _mA * _impulse * _jvAc;
			x += _iA * _impulse * _jwA;
			v2 += _mB * _impulse * _jvBd;
			x2 += _iB * _impulse * _jwB;
			v3 -= _mC * _impulse * _jvAc;
			x3 -= _iC * _impulse * _jwC;
			v4 -= _mD * _impulse * _jvBd;
			x4 -= _iD * _impulse * _jwD;
		}
		else
		{
			_impulse = 0f;
		}
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = x;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x2;
		data.Velocities[_indexC].V = v3;
		data.Velocities[_indexC].W = x3;
		data.Velocities[_indexD].V = v4;
		data.Velocities[_indexD].W = x4;
	}

	internal override void SolveVelocityConstraints(in SolverData data)
	{
		FVector2 v = data.Velocities[_indexA].V;
		FP y = data.Velocities[_indexA].W;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP y2 = data.Velocities[_indexB].W;
		FVector2 v3 = data.Velocities[_indexC].V;
		FP y3 = data.Velocities[_indexC].W;
		FVector2 v4 = data.Velocities[_indexD].V;
		FP y4 = data.Velocities[_indexD].W;
		FP y5 = FVector2.Dot(_jvAc, v - v3) + FVector2.Dot(_jvBd, v2 - v4) + (_jwA * y - _jwC * y3 + (_jwB * y2 - _jwD * y4));
		FP y6 = -_mass * y5;
		_impulse += y6;
		v += _mA * y6 * _jvAc;
		y += _iA * y6 * _jwA;
		v2 += _mB * y6 * _jvBd;
		y2 += _iB * y6 * _jwB;
		v3 -= _mC * y6 * _jvAc;
		y3 -= _iC * y6 * _jwC;
		v4 -= _mD * y6 * _jvBd;
		y4 -= _iD * y6 * _jwD;
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = y;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = y2;
		data.Velocities[_indexC].V = v3;
		data.Velocities[_indexC].W = y3;
		data.Velocities[_indexD].V = v4;
		data.Velocities[_indexD].W = y4;
	}

	internal override bool SolvePositionConstraints(in SolverData data)
	{
		FVector2 center = data.Positions[_indexA].Center;
		FP x = data.Positions[_indexA].Angle;
		FVector2 center2 = data.Positions[_indexB].Center;
		FP x2 = data.Positions[_indexB].Angle;
		FVector2 center3 = data.Positions[_indexC].Center;
		FP y = data.Positions[_indexC].Angle;
		FVector2 center4 = data.Positions[_indexD].Center;
		FP y2 = data.Positions[_indexD].Angle;
		Rotation q = new Rotation(x);
		Rotation q2 = new Rotation(x2);
		Rotation q3 = new Rotation(y);
		Rotation q4 = new Rotation(y2);
		FVector2 vector = default(FVector2);
		FVector2 vector2 = default(FVector2);
		FP x3 = FP.Zero;
		FP y3;
		FP y4;
		FP x4;
		if (_typeA == JointType.RevoluteJoint)
		{
			vector.SetZero();
			y3 = 1f;
			y4 = 1f;
			x3 += _iA + _iC;
			x4 = x - y - _referenceAngleA;
		}
		else
		{
			FVector2 b = MathUtils.Mul(in q3, in _localAxisC);
			FVector2 a = MathUtils.Mul(in q3, _localAnchorC - _lcC);
			FVector2 a2 = MathUtils.Mul(in q, _localAnchorA - _lcA);
			vector = b;
			y4 = MathUtils.Cross(in a, in b);
			y3 = MathUtils.Cross(in a2, in b);
			x3 += _mC + _mA + _iC * y4 * y4 + _iA * y3 * y3;
			FVector2 fVector = _localAnchorC - _lcC;
			x4 = FVector2.Dot(MathUtils.MulT(in q3, a2 + (center - center3)) - fVector, _localAxisC);
		}
		FP y5;
		FP y6;
		FP y7;
		if (_typeB == JointType.RevoluteJoint)
		{
			vector2.SetZero();
			y5 = _ratio;
			y6 = _ratio;
			x3 += _ratio * _ratio * (_iB + _iD);
			y7 = x2 - y2 - _referenceAngleB;
		}
		else
		{
			FVector2 b2 = MathUtils.Mul(in q4, in _localAxisD);
			FVector2 a3 = MathUtils.Mul(in q4, _localAnchorD - _lcD);
			FVector2 a4 = MathUtils.Mul(in q2, _localAnchorB - _lcB);
			vector2 = _ratio * b2;
			y6 = _ratio * MathUtils.Cross(in a3, in b2);
			y5 = _ratio * MathUtils.Cross(in a4, in b2);
			x3 += _ratio * _ratio * (_mD + _mB) + _iD * y6 * y6 + _iB * y5 * y5;
			FVector2 fVector2 = _localAnchorD - _lcD;
			y7 = FVector2.Dot(MathUtils.MulT(in q4, a4 + (center2 - center4)) - fVector2, _localAxisD);
		}
		FP fP = x4 + _ratio * y7 - _constant;
		FP y8 = FP.Zero;
		if (x3 > 0f)
		{
			y8 = -fP / x3;
		}
		center += _mA * y8 * vector;
		x += _iA * y8 * y3;
		center2 += _mB * y8 * vector2;
		x2 += _iB * y8 * y5;
		center3 -= _mC * y8 * vector;
		y -= _iC * y8 * y4;
		center4 -= _mD * y8 * vector2;
		y2 -= _iD * y8 * y6;
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = x;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x2;
		data.Positions[_indexC].Center = center3;
		data.Positions[_indexC].Angle = y;
		data.Positions[_indexD].Center = center4;
		data.Positions[_indexD].Angle = y2;
		return FMath.Abs(fP) < _tolerance;
	}
}

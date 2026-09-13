using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class PulleyJoint : Joint
{
	private readonly FP _constant;

	private readonly FP _lengthA;

	private readonly FP _lengthB;

	private readonly FVector2 _localAnchorA;

	private readonly FVector2 _localAnchorB;

	private readonly FP _ratio;

	private FVector2 _groundAnchorA;

	private FVector2 _groundAnchorB;

	private FP _impulse;

	private int _indexA;

	private int _indexB;

	private FP _invIa;

	private FP _invIb;

	private FP _invMassA;

	private FP _invMassB;

	private FVector2 _localCenterA;

	private FVector2 _localCenterB;

	private FP _mass;

	private FVector2 _rA;

	private FVector2 _rB;

	private FVector2 _uA;

	private FVector2 _uB;

	public PulleyJoint(PulleyJointDef def)
		: base(def)
	{
		_groundAnchorA = def.GroundAnchorA;
		_groundAnchorB = def.GroundAnchorB;
		_localAnchorA = def.LocalAnchorA;
		_localAnchorB = def.LocalAnchorB;
		_lengthA = def.LengthA;
		_lengthB = def.LengthB;
		_ratio = def.Ratio;
		_constant = def.LengthA + _ratio * def.LengthB;
		_impulse = 0f;
	}

	public FVector2 GetGroundAnchorA()
	{
		return _groundAnchorA;
	}

	public FVector2 GetGroundAnchorB()
	{
		return _groundAnchorB;
	}

	public FP GetLengthA()
	{
		return _lengthA;
	}

	public FP GetLengthB()
	{
		return _lengthB;
	}

	public FP GetRatio()
	{
		return _ratio;
	}

	public FP GetCurrentLengthA()
	{
		FVector2 worldPoint = BodyA.GetWorldPoint(in _localAnchorA);
		FVector2 groundAnchorA = _groundAnchorA;
		return (worldPoint - groundAnchorA).Length();
	}

	public FP GetCurrentLengthB()
	{
		FVector2 worldPoint = BodyB.GetWorldPoint(in _localAnchorB);
		FVector2 groundAnchorB = _groundAnchorB;
		return (worldPoint - groundAnchorB).Length();
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
		FVector2 fVector = _impulse * _uB;
		return inv_dt * fVector;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return 0f;
	}

	internal override void InitVelocityConstraints(in SolverData data)
	{
		_indexA = BodyA.IslandIndex;
		_indexB = BodyB.IslandIndex;
		_localCenterA = BodyA.Sweep.LocalCenter;
		_localCenterB = BodyB.Sweep.LocalCenter;
		_invMassA = BodyA.InvMass;
		_invMassB = BodyB.InvMass;
		_invIa = BodyA.InverseInertia;
		_invIb = BodyB.InverseInertia;
		FVector2 center = data.Positions[_indexA].Center;
		FP angle = data.Positions[_indexA].Angle;
		FVector2 v = data.Velocities[_indexA].V;
		FP x = data.Velocities[_indexA].W;
		FVector2 center2 = data.Positions[_indexB].Center;
		FP angle2 = data.Positions[_indexB].Angle;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x2 = data.Velocities[_indexB].W;
		Rotation q = new Rotation(angle);
		Rotation q2 = new Rotation(angle2);
		_rA = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
		_rB = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
		_uA = center + _rA - _groundAnchorA;
		_uB = center2 + _rB - _groundAnchorB;
		FP fP = _uA.Length();
		FP fP2 = _uB.Length();
		if (fP > (FP)10f * Settings.LinearSlop)
		{
			_uA *= 1f / fP;
		}
		else
		{
			_uA.SetZero();
		}
		if (fP2 > (FP)10f * Settings.LinearSlop)
		{
			_uB *= 1f / fP2;
		}
		else
		{
			_uB.SetZero();
		}
		FP y = MathUtils.Cross(in _rA, in _uA);
		FP y2 = MathUtils.Cross(in _rB, in _uB);
		FP x3 = _invMassA + _invIa * y * y;
		FP y3 = _invMassB + _invIb * y2 * y2;
		_mass = x3 + _ratio * _ratio * y3;
		if (_mass > 0f)
		{
			_mass = 1f / _mass;
		}
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			FVector2 b = -_impulse * _uA;
			FVector2 b2 = -_ratio * _impulse * _uB;
			v += _invMassA * b;
			x += _invIa * MathUtils.Cross(in _rA, in b);
			v2 += _invMassB * b2;
			x2 += _invIb * MathUtils.Cross(in _rB, in b2);
		}
		else
		{
			_impulse = 0f;
		}
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = x;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x2;
	}

	internal override void SolveVelocityConstraints(in SolverData data)
	{
		FVector2 v = data.Velocities[_indexA].V;
		FP x = data.Velocities[_indexA].W;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x2 = data.Velocities[_indexB].W;
		FVector2 value = v + MathUtils.Cross(x, in _rA);
		FVector2 value2 = v2 + MathUtils.Cross(x2, in _rB);
		FP y = -FVector2.Dot(_uA, value) - _ratio * FVector2.Dot(_uB, value2);
		FP y2 = -_mass * y;
		_impulse += y2;
		FVector2 b = -y2 * _uA;
		FVector2 b2 = -_ratio * y2 * _uB;
		v += _invMassA * b;
		x += _invIa * MathUtils.Cross(in _rA, in b);
		v2 += _invMassB * b2;
		x2 += _invIb * MathUtils.Cross(in _rB, in b2);
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = x;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x2;
	}

	internal override bool SolvePositionConstraints(in SolverData data)
	{
		FVector2 center = data.Positions[_indexA].Center;
		FP x = data.Positions[_indexA].Angle;
		FVector2 center2 = data.Positions[_indexB].Center;
		FP x2 = data.Positions[_indexB].Angle;
		Rotation q = new Rotation(x);
		Rotation q2 = new Rotation(x2);
		FVector2 a = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
		FVector2 a2 = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
		FVector2 vector = center + a - _groundAnchorA;
		FVector2 vector2 = center2 + a2 - _groundAnchorB;
		FP y = vector.Length();
		FP y2 = vector2.Length();
		if (y > (FP)10f * Settings.LinearSlop)
		{
			vector *= 1f / y;
		}
		else
		{
			vector.SetZero();
		}
		if (y2 > (FP)10f * Settings.LinearSlop)
		{
			vector2 *= 1f / y2;
		}
		else
		{
			vector2.SetZero();
		}
		FP y3 = MathUtils.Cross(in a, in vector);
		FP y4 = MathUtils.Cross(in a2, in vector2);
		FP x3 = _invMassA + _invIa * y3 * y3;
		FP y5 = _invMassB + _invIb * y4 * y4;
		FP fP = x3 + _ratio * _ratio * y5;
		if (fP > 0f)
		{
			fP = 1f / fP;
		}
		FP y6 = _constant - y - _ratio * y2;
		FP fP2 = FP.Abs(y6);
		FP y7 = -fP * y6;
		FVector2 b = -y7 * vector;
		FVector2 b2 = -_ratio * y7 * vector2;
		center += _invMassA * b;
		x += _invIa * MathUtils.Cross(in a, in b);
		center2 += _invMassB * b2;
		x2 += _invIb * MathUtils.Cross(in a2, in b2);
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = x;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x2;
		return fP2 < Settings.LinearSlop;
	}

	public override void Dump()
	{
		int islandIndex = BodyA.IslandIndex;
		int islandIndex2 = BodyB.IslandIndex;
		DumpLogger.Log("  b2PulleyJointDef jd;");
		DumpLogger.Log($"  jd.bodyA = bodies[{islandIndex}];");
		DumpLogger.Log($"  jd.bodyB = bodies[{islandIndex2}];");
		DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
		DumpLogger.Log($"  jd.groundAnchorA.Set({_groundAnchorA.X}, {_groundAnchorA.Y});");
		DumpLogger.Log($"  jd.groundAnchorB.Set({_groundAnchorB.X}, {_groundAnchorB.Y});");
		DumpLogger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
		DumpLogger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
		DumpLogger.Log($"  jd.lengthA = {_lengthA};");
		DumpLogger.Log($"  jd.lengthB = {_lengthB};");
		DumpLogger.Log($"  jd.ratio = {_ratio};");
		DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
	}

	public override void ShiftOrigin(in FVector2 newOrigin)
	{
		_groundAnchorA -= newOrigin;
		_groundAnchorB -= newOrigin;
	}
}

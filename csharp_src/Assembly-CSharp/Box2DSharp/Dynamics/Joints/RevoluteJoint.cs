using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class RevoluteJoint : Joint
{
	internal readonly FP ReferenceAngle;

	private bool _enableLimit;

	private bool _enableMotor;

	private FVector2 _impulse;

	private int _indexA;

	private int _indexB;

	private FP _invIa;

	private FP _invIb;

	private Matrix2x2 _K;

	private FP _angle;

	private FP _axialMass;

	private FP _invMassA;

	private FP _invMassB;

	private FVector2 _localCenterA;

	private FVector2 _localCenterB;

	private FP _lowerAngle;

	private FP _maxMotorTorque;

	private FP _motorImpulse;

	private FP _lowerImpulse;

	private FP _upperImpulse;

	private FP _motorSpeed;

	private FVector2 _rA;

	private FVector2 _rB;

	private FP _upperAngle;

	internal FVector2 LocalAnchorA;

	internal FVector2 LocalAnchorB;

	internal RevoluteJoint(RevoluteJointDef def)
		: base(def)
	{
		LocalAnchorA = def.LocalAnchorA;
		LocalAnchorB = def.LocalAnchorB;
		ReferenceAngle = def.ReferenceAngle;
		_impulse.SetZero();
		_motorImpulse = 0f;
		_axialMass = 0f;
		_lowerImpulse = 0f;
		_upperImpulse = 0f;
		_lowerAngle = def.LowerAngle;
		_upperAngle = def.UpperAngle;
		_maxMotorTorque = def.MaxMotorTorque;
		_motorSpeed = def.MotorSpeed;
		_enableLimit = def.EnableLimit;
		_enableMotor = def.EnableMotor;
		_angle = 0f;
	}

	public FVector2 GetLocalAnchorA()
	{
		return LocalAnchorA;
	}

	public FVector2 GetLocalAnchorB()
	{
		return LocalAnchorB;
	}

	public FP GetReferenceAngle()
	{
		return ReferenceAngle;
	}

	public FP GetJointAngle()
	{
		Body bodyA = BodyA;
		return BodyB.Sweep.A - bodyA.Sweep.A - ReferenceAngle;
	}

	public FP GetJointSpeed()
	{
		Body bodyA = BodyA;
		return BodyB.AngularVelocity - bodyA.AngularVelocity;
	}

	public bool IsLimitEnabled()
	{
		return _enableLimit;
	}

	public void EnableLimit(bool flag)
	{
		if (flag != _enableLimit)
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_enableLimit = flag;
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
	}

	public FP GetLowerLimit()
	{
		return _lowerAngle;
	}

	public FP GetUpperLimit()
	{
		return _upperAngle;
	}

	public void SetLimits(FP lower, FP upper)
	{
		if (FP.Abs(lower - _lowerAngle) > Settings.Epsilon || FP.Abs(upper - _upperAngle) > Settings.Epsilon)
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
			_lowerAngle = lower;
			_upperAngle = upper;
		}
	}

	public bool IsMotorEnabled()
	{
		return _enableMotor;
	}

	public void EnableMotor(bool flag)
	{
		if (flag != _enableMotor)
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_enableMotor = flag;
		}
	}

	public void SetMotorSpeed(FP speed)
	{
		if (speed != _motorSpeed)
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_motorSpeed = speed;
		}
	}

	public FP GetMotorSpeed()
	{
		return _motorSpeed;
	}

	public void SetMaxMotorTorque(FP torque)
	{
		if (torque != _maxMotorTorque)
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_maxMotorTorque = torque;
		}
	}

	public FP GetMaxMotorTorque()
	{
		return _maxMotorTorque;
	}

	public FP GetMotorTorque(FP inv_dt)
	{
		return inv_dt * _motorImpulse;
	}

	public override FVector2 GetAnchorA()
	{
		return BodyA.GetWorldPoint(in LocalAnchorA);
	}

	public override FVector2 GetAnchorB()
	{
		return BodyB.GetWorldPoint(in LocalAnchorB);
	}

	public override FVector2 GetReactionForce(FP inv_dt)
	{
		FVector2 fVector = new FVector2(_impulse.X, _impulse.Y);
		return inv_dt * fVector;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * (_motorImpulse + _lowerImpulse - _upperImpulse);
	}

	public override void Dump()
	{
		int islandIndex = BodyA.IslandIndex;
		int islandIndex2 = BodyB.IslandIndex;
		DumpLogger.Log("  b2RevoluteJointDef jd;");
		DumpLogger.Log($"  jd.bodyA = bodies[{islandIndex}];");
		DumpLogger.Log($"  jd.bodyB = bodies[{islandIndex2}];");
		DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
		DumpLogger.Log($"  jd.localAnchorA.Set({LocalAnchorA.X}, {LocalAnchorA.Y});");
		DumpLogger.Log($"  jd.localAnchorB.Set({LocalAnchorB.X}, {LocalAnchorB.Y});");
		DumpLogger.Log($"  jd.referenceAngle = {ReferenceAngle};");
		DumpLogger.Log($"  jd.enableLimit = bool({_enableLimit});");
		DumpLogger.Log($"  jd.lowerAngle = {_lowerAngle};");
		DumpLogger.Log($"  jd.upperAngle = {_upperAngle};");
		DumpLogger.Log($"  jd.enableMotor = bool({_enableMotor});");
		DumpLogger.Log($"  jd.motorSpeed = {_motorSpeed};");
		DumpLogger.Log($"  jd.maxMotorTorque = {_maxMotorTorque};");
		DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
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
		FP y = data.Positions[_indexA].Angle;
		FVector2 v = data.Velocities[_indexA].V;
		FP x = data.Velocities[_indexA].W;
		FP x2 = data.Positions[_indexB].Angle;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x3 = data.Velocities[_indexB].W;
		Rotation q = new Rotation(y);
		Rotation q2 = new Rotation(x2);
		_rA = MathUtils.Mul(in q, LocalAnchorA - _localCenterA);
		_rB = MathUtils.Mul(in q2, LocalAnchorB - _localCenterB);
		FP x4 = _invMassA;
		FP y2 = _invMassB;
		FP y3 = _invIa;
		FP y4 = _invIb;
		_K.Ex.X = x4 + y2 + _rA.Y * _rA.Y * y3 + _rB.Y * _rB.Y * y4;
		_K.Ey.X = -_rA.Y * _rA.X * y3 - _rB.Y * _rB.X * y4;
		_K.Ex.Y = _K.Ey.X;
		_K.Ey.Y = x4 + y2 + _rA.X * _rA.X * y3 + _rB.X * _rB.X * y4;
		_axialMass = y3 + y4;
		bool flag;
		if (_axialMass > 0f)
		{
			_axialMass = 1f / _axialMass;
			flag = false;
		}
		else
		{
			flag = true;
		}
		_angle = x2 - y - ReferenceAngle;
		if (!_enableLimit || flag)
		{
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
		if (!_enableMotor || flag)
		{
			_motorImpulse = 0f;
		}
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			_motorImpulse *= data.Step.DtRatio;
			_lowerImpulse *= data.Step.DtRatio;
			_upperImpulse *= data.Step.DtRatio;
			FP y5 = _motorImpulse + _lowerImpulse - _upperImpulse;
			FVector2 b = new FVector2(_impulse.X, _impulse.Y);
			v -= x4 * b;
			x -= y3 * (MathUtils.Cross(in _rA, in b) + y5);
			v2 += y2 * b;
			x3 += y4 * (MathUtils.Cross(in _rB, in b) + y5);
		}
		else
		{
			_impulse.SetZero();
			_motorImpulse = 0f;
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = x;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x3;
	}

	internal override void SolveVelocityConstraints(in SolverData data)
	{
		FVector2 v = data.Velocities[_indexA].V;
		FP y = data.Velocities[_indexA].W;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x = data.Velocities[_indexB].W;
		FP invMassA = _invMassA;
		FP invMassB = _invMassB;
		FP x2 = _invIa;
		FP y2 = _invIb;
		bool flag = (x2 + y2).Equals(0f);
		if (_enableMotor && !flag)
		{
			FP y3 = x - y - _motorSpeed;
			FP y4 = -_axialMass * y3;
			FP y5 = _motorImpulse;
			FP fP = data.Step.Dt * _maxMotorTorque;
			_motorImpulse = MathUtils.Clamp(_motorImpulse + y4, -fP, fP);
			y4 = _motorImpulse - y5;
			y -= x2 * y4;
			x += y2 * y4;
		}
		if (_enableLimit && !flag)
		{
			FP left = _angle - _lowerAngle;
			FP x3 = x - y;
			FP y6 = -_axialMass * (x3 + FP.Max(left, 0f) * data.Step.InvDt);
			FP y7 = _lowerImpulse;
			_lowerImpulse = FP.Max(_lowerImpulse + y6, 0f);
			y6 = _lowerImpulse - y7;
			y -= x2 * y6;
			x += y2 * y6;
			FP left2 = _upperAngle - _angle;
			FP x4 = y - x;
			FP y8 = -_axialMass * (x4 + FP.Max(left2, 0f) * data.Step.InvDt);
			FP y9 = _upperImpulse;
			_upperImpulse = FP.Max(_upperImpulse + y8, 0f);
			y8 = _upperImpulse - y9;
			y += x2 * y8;
			x -= y2 * y8;
		}
		FVector2 fVector = v2 + MathUtils.Cross(x, in _rB) - v - MathUtils.Cross(y, in _rA);
		FVector2 b = _K.Solve(-fVector);
		ref FP x5 = ref _impulse.X;
		x5 += b.X;
		ref FP y10 = ref _impulse.Y;
		y10 += b.Y;
		v -= invMassA * b;
		y -= x2 * MathUtils.Cross(in _rA, in b);
		v2 += invMassB * b;
		x += y2 * MathUtils.Cross(in _rB, in b);
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = y;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x;
	}

	internal override bool SolvePositionConstraints(in SolverData data)
	{
		FVector2 center = data.Positions[_indexA].Center;
		FP y = data.Positions[_indexA].Angle;
		FVector2 center2 = data.Positions[_indexB].Center;
		FP x = data.Positions[_indexB].Angle;
		Rotation q = new Rotation(y);
		Rotation q2 = new Rotation(x);
		FP fP = FP.Zero;
		_ = FP.Zero;
		bool flag = (_invIa + _invIb).Equals(0f);
		if (_enableLimit && !flag)
		{
			FP x2 = x - y - ReferenceAngle;
			FP y2 = 0f;
			if (FP.Abs(_upperAngle - _lowerAngle) < (FP)2f * Settings.AngularSlop)
			{
				y2 = MathUtils.Clamp(x2 - _lowerAngle, -Settings.MaxAngularCorrection, Settings.MaxAngularCorrection);
			}
			else if (x2 <= _lowerAngle)
			{
				y2 = MathUtils.Clamp(x2 - _lowerAngle + Settings.AngularSlop, -Settings.MaxAngularCorrection, 0f);
			}
			else if (x2 >= _upperAngle)
			{
				y2 = MathUtils.Clamp(x2 - _upperAngle - Settings.AngularSlop, 0f, Settings.MaxAngularCorrection);
			}
			FP y3 = -_axialMass * y2;
			y -= _invIa * y3;
			x += _invIb * y3;
			fP = FP.Abs(y2);
		}
		q.Set(y);
		q2.Set(x);
		FVector2 a = MathUtils.Mul(in q, LocalAnchorA - _localCenterA);
		FVector2 a2 = MathUtils.Mul(in q2, LocalAnchorB - _localCenterB);
		FVector2 b = center2 + a2 - center - a;
		FP fP2 = b.Length();
		FP x3 = _invMassA;
		FP y4 = _invMassB;
		FP x4 = _invIa;
		FP x5 = _invIb;
		Matrix2x2 matrix2x = default(Matrix2x2);
		matrix2x.Ex.X = x3 + y4 + x4 * a.Y * a.Y + x5 * a2.Y * a2.Y;
		matrix2x.Ex.Y = -x4 * a.X * a.Y - x5 * a2.X * a2.Y;
		matrix2x.Ey.X = matrix2x.Ex.Y;
		matrix2x.Ey.Y = x3 + y4 + x4 * a.X * a.X + x5 * a2.X * a2.X;
		FVector2 b2 = -matrix2x.Solve(in b);
		center -= x3 * b2;
		y -= x4 * MathUtils.Cross(in a, in b2);
		center2 += y4 * b2;
		x += x5 * MathUtils.Cross(in a2, in b2);
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = y;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x;
		if (fP2 <= Settings.LinearSlop)
		{
			return fP <= Settings.AngularSlop;
		}
		return false;
	}

	public override void Draw(IDraw draw)
	{
		Transform T = BodyA.GetTransform();
		Transform T2 = BodyB.GetTransform();
		FVector2 p = MathUtils.Mul(in T, in LocalAnchorA);
		FVector2 p2 = MathUtils.Mul(in T2, in LocalAnchorB);
		Color color = Color.FromArgb(0.7f, 0.7f, 0.7f);
		Color color2 = Color.FromArgb(0.3f, 0.9f, 0.3f);
		Color color3 = Color.FromArgb(0.9f, 0.3f, 0.3f);
		Color color4 = Color.FromArgb(0.3f, 0.3f, 0.9f);
		Color color5 = Color.FromArgb(0.4f, 0.4f, 0.4f);
		draw.DrawPoint(in p, 5f, in color4);
		draw.DrawPoint(in p2, 5f, in color5);
		FP y = BodyA.GetAngle();
		FP x = BodyB.GetAngle() - y - ReferenceAngle;
		float num = 0.5f;
		FVector2 fVector = num * new FVector2(FP.Cos(x), FP.Sin(x));
		draw.DrawSegment(in p2, p2 + fVector, in color);
		draw.DrawCircle(in p2, num, in color);
		if (_enableLimit)
		{
			FVector2 fVector2 = num * new FVector2(FP.Cos(_lowerAngle), FP.Cos(_lowerAngle));
			FVector2 fVector3 = num * new FVector2(FP.Cos(_upperAngle), FP.Cos(_upperAngle));
			draw.DrawSegment(in p2, p2 + fVector2, in color2);
			draw.DrawSegment(in p2, p2 + fVector3, in color3);
		}
		Color color6 = Color.FromArgb(0.5f, 0.8f, 0.8f);
		draw.DrawSegment(in T.Position, in p, in color6);
		draw.DrawSegment(in p, in p2, in color6);
		draw.DrawSegment(in T2.Position, in p2, in color6);
	}
}

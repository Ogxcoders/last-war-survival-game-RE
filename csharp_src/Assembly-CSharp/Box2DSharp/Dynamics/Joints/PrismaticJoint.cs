using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class PrismaticJoint : Joint
{
	internal readonly FVector2 LocalAnchorA;

	internal readonly FVector2 LocalAnchorB;

	internal readonly FVector2 LocalXAxisA;

	internal readonly FVector2 LocalYAxisA;

	internal readonly FP ReferenceAngle;

	private FVector2 _impulse;

	private FP _motorImpulse;

	private FP _lowerImpulse;

	private FP _upperImpulse;

	private FP _lowerTranslation;

	private FP _upperTranslation;

	private FP _maxMotorForce;

	private FP _motorSpeed;

	private bool _enableLimit;

	private bool _enableMotor;

	private int _indexA;

	private int _indexB;

	private FVector2 _localCenterA;

	private FVector2 _localCenterB;

	private FP _invMassA;

	private FP _invMassB;

	private FP _invIA;

	private FP _invIB;

	private FVector2 _axis;

	private FVector2 _perp;

	private FP _s1;

	private FP _s2;

	private FP _a1;

	private FP _a2;

	private Matrix2x2 _k;

	private FP _translation;

	private FP _axialMass;

	internal PrismaticJoint(PrismaticJointDef def)
		: base(def)
	{
		LocalAnchorA = def.LocalAnchorA;
		LocalAnchorB = def.LocalAnchorB;
		LocalXAxisA = def.LocalAxisA;
		LocalXAxisA.Normalize();
		LocalYAxisA = MathUtils.Cross(1f, in LocalXAxisA);
		ReferenceAngle = def.ReferenceAngle;
		_impulse.SetZero();
		_axialMass = 0f;
		_motorImpulse = 0f;
		_lowerImpulse = 0f;
		_upperImpulse = 0f;
		_lowerTranslation = def.LowerTranslation;
		_upperTranslation = def.UpperTranslation;
		_maxMotorForce = def.MaxMotorForce;
		_motorSpeed = def.MotorSpeed;
		_enableLimit = def.EnableLimit;
		_enableMotor = def.EnableMotor;
		_translation = 0f;
		_axis.SetZero();
		_perp.SetZero();
	}

	public FVector2 GetLocalAnchorA()
	{
		return LocalAnchorA;
	}

	public FVector2 GetLocalAnchorB()
	{
		return LocalAnchorB;
	}

	public FVector2 GetLocalAxisA()
	{
		return LocalXAxisA;
	}

	public FP GetReferenceAngle()
	{
		return ReferenceAngle;
	}

	public FP GetJointTranslation()
	{
		FVector2 worldPoint = BodyA.GetWorldPoint(in LocalAnchorA);
		FVector2 value = BodyB.GetWorldPoint(in LocalAnchorB) - worldPoint;
		FVector2 worldVector = BodyA.GetWorldVector(in LocalXAxisA);
		return FVector2.Dot(value, worldVector);
	}

	public FP GetJointSpeed()
	{
		Body bodyA = BodyA;
		Body bodyB = BodyB;
		FVector2 a = MathUtils.Mul(in bodyA.Transform.Rotation, LocalAnchorA - bodyA.Sweep.LocalCenter);
		FVector2 a2 = MathUtils.Mul(in bodyB.Transform.Rotation, LocalAnchorB - bodyB.Sweep.LocalCenter);
		FVector2 fVector = bodyA.Sweep.C + a;
		FVector2 value = bodyB.Sweep.C + a2 - fVector;
		FVector2 a3 = MathUtils.Mul(in bodyA.Transform.Rotation, in LocalXAxisA);
		FVector2 linearVelocity = bodyA.LinearVelocity;
		FVector2 linearVelocity2 = bodyB.LinearVelocity;
		FP angularVelocity = bodyA.AngularVelocity;
		FP angularVelocity2 = bodyB.AngularVelocity;
		return FVector2.Dot(value, MathUtils.Cross(angularVelocity, in a3)) + FVector2.Dot(a3, linearVelocity2 + MathUtils.Cross(angularVelocity2, in a2) - linearVelocity - MathUtils.Cross(angularVelocity, in a));
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
		return _lowerTranslation;
	}

	public FP GetUpperLimit()
	{
		return _upperTranslation;
	}

	public void SetLimits(FP lower, FP upper)
	{
		if (!lower.Equals(_lowerTranslation) || !upper.Equals(_upperTranslation))
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_lowerTranslation = lower;
			_upperTranslation = upper;
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
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

	public void SetMaxMotorForce(FP force)
	{
		if (FP.Abs(force - _maxMotorForce) > 1E-06f)
		{
			BodyA.IsAwake = true;
			BodyB.IsAwake = true;
			_maxMotorForce = force;
		}
	}

	public FP GetMaxMotorForce()
	{
		return _maxMotorForce;
	}

	public FP GetMotorForce(FP inv_dt)
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
		return inv_dt * (_impulse.X * _perp + (_motorImpulse + _lowerImpulse - _upperImpulse) * _axis);
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * _impulse.Y;
	}

	public override void Dump()
	{
	}

	internal override void InitVelocityConstraints(in SolverData data)
	{
		_indexA = BodyA.IslandIndex;
		_indexB = BodyB.IslandIndex;
		_localCenterA = BodyA.Sweep.LocalCenter;
		_localCenterB = BodyB.Sweep.LocalCenter;
		_invMassA = BodyA.InvMass;
		_invMassB = BodyB.InvMass;
		_invIA = BodyA.InverseInertia;
		_invIB = BodyB.InverseInertia;
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
		FVector2 fVector = MathUtils.Mul(in q, LocalAnchorA - _localCenterA);
		FVector2 a = MathUtils.Mul(in q2, LocalAnchorB - _localCenterB);
		FVector2 fVector2 = center2 - center + a - fVector;
		FP x3 = _invMassA;
		FP y = _invMassB;
		FP x4 = _invIA;
		FP x5 = _invIB;
		_axis = MathUtils.Mul(in q, in LocalXAxisA);
		_a1 = MathUtils.Cross(fVector2 + fVector, in _axis);
		_a2 = MathUtils.Cross(in a, in _axis);
		_axialMass = x3 + y + x4 * _a1 * _a1 + x5 * _a2 * _a2;
		if (_axialMass > 0f)
		{
			_axialMass = 1f / _axialMass;
		}
		_perp = MathUtils.Mul(in q, in LocalYAxisA);
		_s1 = MathUtils.Cross(fVector2 + fVector, in _perp);
		_s2 = MathUtils.Cross(in a, in _perp);
		FP x6 = x3 + y + x4 * _s1 * _s1 + x5 * _s2 * _s2;
		FP fP = x4 * _s1 + x5 * _s2;
		FP y2 = x4 + x5;
		if (y2.Equals(0f))
		{
			y2 = 1f;
		}
		_k.Ex.Set(x6, fP);
		_k.Ey.Set(fP, y2);
		if (_enableLimit)
		{
			_translation = FVector2.Dot(_axis, fVector2);
		}
		else
		{
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
		if (!_enableMotor)
		{
			_motorImpulse = 0f;
		}
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			_motorImpulse *= data.Step.DtRatio;
			_lowerImpulse = data.Step.DtRatio;
			_upperImpulse = data.Step.DtRatio;
			FP x7 = _motorImpulse + _lowerImpulse - _upperImpulse;
			FVector2 fVector3 = _impulse.X * _perp + x7 * _axis;
			FP y3 = _impulse.X * _s1 + _impulse.Y + x7 * _a1;
			FP y4 = _impulse.X * _s2 + _impulse.Y + x7 * _a2;
			v -= x3 * fVector3;
			x -= x4 * y3;
			v2 += y * fVector3;
			x2 += x5 * y4;
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
		data.Velocities[_indexB].W = x2;
	}

	internal override void SolveVelocityConstraints(in SolverData data)
	{
		FVector2 v = data.Velocities[_indexA].V;
		FP y = data.Velocities[_indexA].W;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP y2 = data.Velocities[_indexB].W;
		FP invMassA = _invMassA;
		FP invMassB = _invMassB;
		FP x = _invIA;
		FP x2 = _invIB;
		if (_enableMotor)
		{
			FP y3 = FVector2.Dot(_axis, v2 - v) + _a2 * y2 - _a1 * y;
			FP y4 = _axialMass * (_motorSpeed - y3);
			FP y5 = _motorImpulse;
			FP fP = data.Step.Dt * _maxMotorForce;
			_motorImpulse = MathUtils.Clamp(_motorImpulse + y4, -fP, fP);
			y4 = _motorImpulse - y5;
			FVector2 fVector = y4 * _axis;
			FP y6 = y4 * _a1;
			FP y7 = y4 * _a2;
			v -= invMassA * fVector;
			y -= x * y6;
			v2 += invMassB * fVector;
			y2 += x2 * y7;
		}
		FVector2 fVector2 = default(FVector2);
		fVector2.X = FVector2.Dot(_perp, v2 - v) + _s2 * y2 - _s1 * y;
		fVector2.Y = y2 - y;
		if (_enableLimit)
		{
			FP left = _translation - _lowerTranslation;
			FP x3 = FVector2.Dot(_axis, v2 - v) + _a2 * y2 - _a1 * y;
			FP y8 = -_axialMass * (x3 + FP.Max(left, 0f) * data.Step.InvDt);
			FP y9 = _lowerImpulse;
			_lowerImpulse = FP.Max(_lowerImpulse + y8, 0f);
			y8 = _lowerImpulse - y9;
			FVector2 fVector3 = y8 * _axis;
			FP y10 = y8 * _a1;
			FP y11 = y8 * _a2;
			v -= invMassA * fVector3;
			y -= x * y10;
			v2 += invMassB * fVector3;
			y2 += x2 * y11;
			FP left2 = _upperTranslation - _translation;
			FP x4 = FVector2.Dot(_axis, v - v2) + _a1 * y - _a2 * y2;
			FP y12 = -_axialMass * (x4 + FP.Max(left2, 0f) * data.Step.InvDt);
			FP y13 = _upperImpulse;
			_upperImpulse = FP.Max(_upperImpulse + y12, 0f);
			y12 = _upperImpulse - y13;
			FVector2 fVector4 = y12 * _axis;
			FP y14 = y12 * _a1;
			FP y15 = y12 * _a2;
			v += invMassA * fVector4;
			y += x * y14;
			v2 -= invMassB * fVector4;
			y2 -= x2 * y15;
		}
		FVector2 fVector5 = new FVector2
		{
			X = FVector2.Dot(_perp, v2 - v) + _s2 * y2 - _s1 * y,
			Y = y2 - y
		};
		FVector2 fVector6 = _k.Solve(-fVector5);
		_impulse += fVector6;
		FVector2 fVector7 = fVector6.X * _perp;
		FP y16 = fVector6.X * _s1 + fVector6.Y;
		FP y17 = fVector6.X * _s2 + fVector6.Y;
		v -= invMassA * fVector7;
		y -= x * y16;
		v2 += invMassB * fVector7;
		y2 += x2 * y17;
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = y;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = y2;
	}

	internal override bool SolvePositionConstraints(in SolverData data)
	{
		FVector2 center = data.Positions[_indexA].Center;
		FP x = data.Positions[_indexA].Angle;
		FVector2 center2 = data.Positions[_indexB].Center;
		FP x2 = data.Positions[_indexB].Angle;
		Rotation q = new Rotation(x);
		Rotation q2 = new Rotation(x2);
		FP x3 = _invMassA;
		FP y = _invMassB;
		FP x4 = _invIA;
		FP x5 = _invIB;
		FVector2 fVector = MathUtils.Mul(in q, LocalAnchorA - _localCenterA);
		FVector2 a = MathUtils.Mul(in q2, LocalAnchorB - _localCenterB);
		FVector2 fVector2 = center2 + a - center - fVector;
		FVector2 b = MathUtils.Mul(in q, in LocalXAxisA);
		FP y2 = MathUtils.Cross(fVector2 + fVector, in b);
		FP y3 = MathUtils.Cross(in a, in b);
		FVector2 b2 = MathUtils.Mul(in q, in LocalYAxisA);
		FP y4 = MathUtils.Cross(fVector2 + fVector, in b2);
		FP y5 = MathUtils.Cross(in a, in b2);
		FVector3 fVector3 = default(FVector3);
		FVector2 fVector4 = new FVector2
		{
			X = FVector2.Dot(b2, fVector2),
			Y = x2 - x - ReferenceAngle
		};
		FP fP = FP.Abs(fVector4.X);
		FP fP2 = FP.Abs(fVector4.Y);
		bool flag = false;
		FP z = FP.Zero;
		if (_enableLimit)
		{
			FP x6 = FVector2.Dot(b, fVector2);
			if (FP.Abs(_upperTranslation - _lowerTranslation) < (FP)2f * Settings.LinearSlop)
			{
				z = x6;
				fP = FP.Max(fP, FP.Abs(x6));
				flag = true;
			}
			else if (x6 <= _lowerTranslation)
			{
				z = FP.Min(x6 - _lowerTranslation, 0f);
				fP = FP.Max(fP, _lowerTranslation - x6);
				flag = true;
			}
			else if (x6 >= _upperTranslation)
			{
				z = FP.Max(x6 - _upperTranslation, 0f);
				fP = FP.Max(fP, x6 - _upperTranslation);
				flag = true;
			}
		}
		if (flag)
		{
			FP x7 = x3 + y + x4 * y4 * y4 + x5 * y5 * y5;
			FP fP3 = x4 * y4 + x5 * y5;
			FP fP4 = x4 * y4 * y2 + x5 * y5 * y3;
			FP y6 = x4 + x5;
			if (y6.Equals(0f))
			{
				y6 = 1f;
			}
			FP fP5 = x4 * y2 + x5 * y3;
			FP z2 = x3 + y + x4 * y2 * y2 + x5 * y3 * y3;
			Matrix3x3 matrix3x = default(Matrix3x3);
			matrix3x.Ex.Set(x7, fP3, fP4);
			matrix3x.Ey.Set(fP3, y6, fP5);
			matrix3x.Ez.Set(fP4, fP5, z2);
			fVector3 = matrix3x.Solve33(-new FVector3
			{
				X = fVector4.X,
				Y = fVector4.Y,
				Z = z
			});
		}
		else
		{
			FP x8 = x3 + y + x4 * y4 * y4 + x5 * y5 * y5;
			FP fP6 = x4 * y4 + x5 * y5;
			FP y7 = x4 + x5;
			if (y7.Equals(0f))
			{
				y7 = 1f;
			}
			Matrix2x2 matrix2x = default(Matrix2x2);
			matrix2x.Ex.Set(x8, fP6);
			matrix2x.Ey.Set(fP6, y7);
			FVector2 fVector5 = matrix2x.Solve(-fVector4);
			fVector3.X = fVector5.X;
			fVector3.Y = fVector5.Y;
			fVector3.Z = 0f;
		}
		FVector2 fVector6 = fVector3.X * b2 + fVector3.Z * b;
		FP y8 = fVector3.X * y4 + fVector3.Y + fVector3.Z * y2;
		FP y9 = fVector3.X * y5 + fVector3.Y + fVector3.Z * y3;
		center -= x3 * fVector6;
		x -= x4 * y8;
		center2 += y * fVector6;
		x2 += x5 * y9;
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = x;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x2;
		if (fP <= Settings.LinearSlop)
		{
			return fP2 <= Settings.AngularSlop;
		}
		return false;
	}

	public override void Draw(IDraw draw)
	{
		Transform T = BodyA.GetTransform();
		Transform T2 = BodyB.GetTransform();
		FVector2 p = MathUtils.Mul(in T, in LocalAnchorA);
		FVector2 p2 = MathUtils.Mul(in T2, in LocalAnchorB);
		FVector2 fVector = MathUtils.Mul(in T.Rotation, in LocalXAxisA);
		Color color = Color.FromArgb(0.7f, 0.7f, 0.7f);
		Color color2 = Color.FromArgb(0.3f, 0.9f, 0.3f);
		Color color3 = Color.FromArgb(0.9f, 0.3f, 0.3f);
		Color color4 = Color.FromArgb(0.3f, 0.3f, 0.9f);
		draw.DrawSegment(in p, in p2, Color.FromArgb(0.4f, 0.4f, 0.4f));
		if (_enableLimit)
		{
			FVector2 p3 = p + _lowerTranslation * fVector;
			FVector2 p4 = p + _upperTranslation * fVector;
			FVector2 fVector2 = MathUtils.Mul(in T.Rotation, in LocalYAxisA);
			draw.DrawSegment(in p3, in p4, in color);
			draw.DrawSegment(p3 - 0.5f * fVector2, p3 + 0.5f * fVector2, in color2);
			draw.DrawSegment(p4 - 0.5f * fVector2, p4 + 0.5f * fVector2, in color3);
		}
		else
		{
			draw.DrawSegment(p - 1f * fVector, p + 1f * fVector, in color);
		}
		draw.DrawPoint(in p, 5f, in color);
		draw.DrawPoint(in p2, 5f, in color4);
	}
}

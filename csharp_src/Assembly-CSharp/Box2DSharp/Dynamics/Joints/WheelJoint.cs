using System;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class WheelJoint : Joint
{
	private readonly FVector2 _localAnchorA;

	private readonly FVector2 _localAnchorB;

	private readonly FVector2 _localXAxisA;

	private readonly FVector2 _localYAxisA;

	private FP _impulse;

	private FP _motorImpulse;

	private FP _springImpulse;

	private FP _lowerImpulse;

	private FP _upperImpulse;

	private FP _translation;

	private FP _lowerTranslation;

	private FP _upperTranslation;

	private FP _maxMotorTorque;

	private FP _motorSpeed;

	private bool _enableLimit;

	private bool _enableMotor;

	private FP _stiffness;

	private FP _damping;

	private int _indexA;

	private int _indexB;

	private FVector2 _localCenterA;

	private FVector2 _localCenterB;

	private FP _invMassA;

	private FP _invMassB;

	private FP _invIA;

	private FP _invIB;

	private FVector2 _ax;

	private FVector2 _ay;

	private FP _sAx;

	private FP _sBx;

	private FP _sAy;

	private FP _sBy;

	private FP _mass;

	private FP _motorMass;

	private FP _axialMass;

	private FP _springMass;

	private FP _bias;

	private FP _gamma;

	internal WheelJoint(WheelJointDef def)
		: base(def)
	{
		_localAnchorA = def.LocalAnchorA;
		_localAnchorB = def.LocalAnchorB;
		_localXAxisA = def.LocalAxisA;
		_localYAxisA = MathUtils.Cross(1f, in _localXAxisA);
		_mass = 0f;
		_impulse = 0f;
		_motorMass = 0f;
		_motorImpulse = 0f;
		_springMass = 0f;
		_springImpulse = 0f;
		_axialMass = 0f;
		_lowerImpulse = 0f;
		_upperImpulse = 0f;
		_lowerTranslation = def.LowerTranslation;
		_upperTranslation = def.UpperTranslation;
		_enableLimit = def.EnableLimit;
		_maxMotorTorque = def.MaxMotorTorque;
		_motorSpeed = def.MotorSpeed;
		_enableMotor = def.EnableMotor;
		_bias = 0f;
		_gamma = 0f;
		_ax.SetZero();
		_ay.SetZero();
		_stiffness = def.Stiffness;
		_damping = def.Damping;
	}

	public FVector2 GetLocalAnchorA()
	{
		return _localAnchorA;
	}

	public FVector2 GetLocalAnchorB()
	{
		return _localAnchorB;
	}

	public FVector2 GetLocalAxisA()
	{
		return _localXAxisA;
	}

	public FP GetJointTranslation()
	{
		Body bodyA = BodyA;
		Body bodyB = BodyB;
		FVector2 worldPoint = bodyA.GetWorldPoint(in _localAnchorA);
		FVector2 value = bodyB.GetWorldPoint(in _localAnchorB) - worldPoint;
		FVector2 worldVector = bodyA.GetWorldVector(in _localXAxisA);
		return FVector2.Dot(value, worldVector);
	}

	public FP GetJointLinearSpeed()
	{
		Body bodyA = BodyA;
		Body bodyB = BodyB;
		FVector2 a = MathUtils.Mul(in bodyA.Transform.Rotation, _localAnchorA - bodyA.Sweep.LocalCenter);
		FVector2 a2 = MathUtils.Mul(in bodyB.Transform.Rotation, _localAnchorB - bodyB.Sweep.LocalCenter);
		FVector2 fVector = bodyA.Sweep.C + a;
		FVector2 value = bodyB.Sweep.C + a2 - fVector;
		FVector2 a3 = MathUtils.Mul(in bodyA.Transform.Rotation, in _localXAxisA);
		FVector2 linearVelocity = bodyA.LinearVelocity;
		FVector2 linearVelocity2 = bodyB.LinearVelocity;
		FP angularVelocity = bodyA.AngularVelocity;
		FP angularVelocity2 = bodyB.AngularVelocity;
		return FVector2.Dot(value, MathUtils.Cross(angularVelocity, in a3)) + FVector2.Dot(a3, linearVelocity2 + MathUtils.Cross(angularVelocity2, in a2) - linearVelocity - MathUtils.Cross(angularVelocity, in a));
	}

	public FP GetJointAngle()
	{
		Body bodyA = BodyA;
		return BodyB.Sweep.A - bodyA.Sweep.A;
	}

	public FP GetJointAngularSpeed()
	{
		FP y = BodyA.AngularVelocity;
		return BodyB.AngularVelocity - y;
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
		if (!speed.Equals(_motorSpeed))
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
		if (!torque.Equals(_maxMotorTorque))
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

	public void SetStiffness(FP stiffness)
	{
		_stiffness = stiffness;
	}

	public FP GetStiffness()
	{
		return _stiffness;
	}

	public void SetDamping(FP damping)
	{
		_damping = damping;
	}

	public FP GetDamping()
	{
		return _damping;
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
		return inv_dt * (_impulse * _ay + (_springImpulse + _lowerImpulse - _upperImpulse) * _ax);
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * _motorImpulse;
	}

	public override void Dump()
	{
		throw new NotImplementedException();
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
		FP x = _invMassA;
		FP y = _invMassB;
		FP x2 = _invIA;
		FP x3 = _invIB;
		FVector2 center = data.Positions[_indexA].Center;
		FP angle = data.Positions[_indexA].Angle;
		FVector2 v = data.Velocities[_indexA].V;
		FP x4 = data.Velocities[_indexA].W;
		FVector2 center2 = data.Positions[_indexB].Center;
		FP angle2 = data.Positions[_indexB].Angle;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x5 = data.Velocities[_indexB].W;
		Rotation q = new Rotation(angle);
		Rotation q2 = new Rotation(angle2);
		FVector2 fVector = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
		FVector2 a = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
		FVector2 fVector2 = center2 + a - center - fVector;
		_ay = MathUtils.Mul(in q, in _localYAxisA);
		_sAy = MathUtils.Cross(fVector2 + fVector, in _ay);
		_sBy = MathUtils.Cross(in a, in _ay);
		_mass = x + y + x2 * _sAy * _sAy + x3 * _sBy * _sBy;
		if (_mass > 0f)
		{
			_mass = 1f / _mass;
		}
		_ax = MathUtils.Mul(in q, in _localXAxisA);
		_sAx = MathUtils.Cross(fVector2 + fVector, in _ax);
		_sBx = MathUtils.Cross(in a, in _ax);
		FP x6 = x + y + x2 * _sAx * _sAx + x3 * _sBx * _sBx;
		if (x6 > 0f)
		{
			_axialMass = 1f / x6;
		}
		else
		{
			_axialMass = 0f;
		}
		_springMass = 0f;
		_bias = 0f;
		_gamma = 0f;
		if (_stiffness > 0f && x6 > 0f)
		{
			_springMass = 1f / x6;
			FP x7 = FVector2.Dot(fVector2, _ax);
			FP x8 = data.Step.Dt;
			_gamma = x8 * (_damping + x8 * _stiffness);
			if (_gamma > 0f)
			{
				_gamma = 1f / _gamma;
			}
			_bias = x7 * x8 * _stiffness * _gamma;
			_springMass = x6 + _gamma;
			if (_springMass > 0f)
			{
				_springMass = 1f / _springMass;
			}
		}
		else
		{
			_springImpulse = 0f;
		}
		if (_enableLimit)
		{
			_translation = FVector2.Dot(_ax, fVector2);
		}
		else
		{
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
		if (_enableMotor)
		{
			_motorMass = x2 + x3;
			if (_motorMass > 0f)
			{
				_motorMass = 1f / _motorMass;
			}
		}
		else
		{
			_motorMass = 0f;
			_motorImpulse = 0f;
		}
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			_springImpulse *= data.Step.DtRatio;
			_motorImpulse *= data.Step.DtRatio;
			FP x9 = _springImpulse + _lowerImpulse - _upperImpulse;
			FVector2 fVector3 = _impulse * _ay + x9 * _ax;
			FP y2 = _impulse * _sAy + x9 * _sAx + _motorImpulse;
			FP y3 = _impulse * _sBy + x9 * _sBx + _motorImpulse;
			v -= _invMassA * fVector3;
			x4 -= _invIA * y2;
			v2 += _invMassB * fVector3;
			x5 += _invIB * y3;
		}
		else
		{
			_impulse = 0f;
			_springImpulse = 0f;
			_motorImpulse = 0f;
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = x4;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x5;
	}

	internal override void SolveVelocityConstraints(in SolverData data)
	{
		FP invMassA = _invMassA;
		FP invMassB = _invMassB;
		FP x = _invIA;
		FP x2 = _invIB;
		FVector2 v = data.Velocities[_indexA].V;
		FP y = data.Velocities[_indexA].W;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP y2 = data.Velocities[_indexB].W;
		FP x3 = FVector2.Dot(_ax, v2 - v) + _sBx * y2 - _sAx * y;
		FP y3 = -_springMass * (x3 + _bias + _gamma * _springImpulse);
		_springImpulse += y3;
		FVector2 fVector = y3 * _ax;
		FP y4 = y3 * _sAx;
		FP y5 = y3 * _sBx;
		v -= invMassA * fVector;
		y -= x * y4;
		v2 += invMassB * fVector;
		y2 += x2 * y5;
		FP y6 = y2 - y - _motorSpeed;
		FP y7 = -_motorMass * y6;
		FP y8 = _motorImpulse;
		FP fP = data.Step.Dt * _maxMotorTorque;
		_motorImpulse = MathUtils.Clamp(_motorImpulse + y7, -fP, fP);
		y7 = _motorImpulse - y8;
		y -= x * y7;
		y2 += x2 * y7;
		if (_enableLimit)
		{
			FP left = _translation - _lowerTranslation;
			FP x4 = FVector2.Dot(_ax, v2 - v) + _sBx * y2 - _sAx * y;
			FP y9 = -_axialMass * (x4 + FP.Max(left, 0f) * data.Step.InvDt);
			FP y10 = _lowerImpulse;
			_lowerImpulse = FP.Max(_lowerImpulse + y9, 0f);
			y9 = _lowerImpulse - y10;
			FVector2 fVector2 = y9 * _ax;
			FP y11 = y9 * _sAx;
			FP y12 = y9 * _sBx;
			v -= invMassA * fVector2;
			y -= x * y11;
			v2 += invMassB * fVector2;
			y2 += x2 * y12;
			FP left2 = _upperTranslation - _translation;
			FP x5 = FVector2.Dot(_ax, v - v2) + _sAx * y - _sBx * y2;
			FP y13 = -_axialMass * (x5 + FP.Max(left2, 0f) * data.Step.InvDt);
			FP y14 = _upperImpulse;
			_upperImpulse = FP.Max(_upperImpulse + y13, 0f);
			y13 = _upperImpulse - y14;
			FVector2 fVector3 = y13 * _ax;
			FP y15 = y13 * _sAx;
			FP y16 = y13 * _sBx;
			v += invMassA * fVector3;
			y += x * y15;
			v2 -= invMassB * fVector3;
			y2 -= x2 * y16;
		}
		FP y17 = FVector2.Dot(_ay, v2 - v) + _sBy * y2 - _sAy * y;
		FP y18 = -_mass * y17;
		_impulse += y18;
		FVector2 fVector4 = y18 * _ay;
		FP y19 = y18 * _sAy;
		FP y20 = y18 * _sBy;
		v -= invMassA * fVector4;
		y -= x * y19;
		v2 += invMassB * fVector4;
		y2 += x2 * y20;
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
		FP left = FP.Zero;
		if (_enableLimit)
		{
			Rotation q = new Rotation(x);
			Rotation q2 = new Rotation(x2);
			FVector2 fVector = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
			FVector2 a = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
			FVector2 fVector2 = center2 - center + a - fVector;
			FVector2 fVector3 = MathUtils.Mul(in q, in _localXAxisA);
			FP y = MathUtils.Cross(fVector2 + fVector, in _ax);
			FP y2 = MathUtils.Cross(in a, in _ax);
			FP fP = FP.Zero;
			FP x3 = FVector2.Dot(fVector3, fVector2);
			if (FP.Abs(_upperTranslation - _lowerTranslation) < (FP)2f * Settings.LinearSlop)
			{
				fP = x3;
			}
			else if (x3 <= _lowerTranslation)
			{
				fP = FP.Min(x3 - _lowerTranslation, 0f);
			}
			else if (x3 >= _upperTranslation)
			{
				fP = FP.Max(x3 - _upperTranslation, 0f);
			}
			if (fP != FP.Zero)
			{
				FP fP2 = _invMassA + _invMassB + _invIA * y * y + _invIB * y2 * y2;
				FP x4 = FP.Zero;
				if (!fP2.Equals(0))
				{
					x4 = -fP / fP2;
				}
				FVector2 fVector4 = x4 * fVector3;
				FP y3 = x4 * y;
				FP y4 = x4 * y2;
				center -= _invMassA * fVector4;
				x -= _invIA * y3;
				center2 += _invMassB * fVector4;
				x2 += _invIB * y4;
				left = FP.Abs(fP);
			}
		}
		Rotation q3 = new Rotation(x);
		Rotation q4 = new Rotation(x2);
		FVector2 fVector5 = MathUtils.Mul(in q3, _localAnchorA - _localCenterA);
		FVector2 a2 = MathUtils.Mul(in q4, _localAnchorB - _localCenterB);
		FVector2 fVector6 = center2 - center + a2 - fVector5;
		FVector2 b = MathUtils.Mul(in q3, in _localYAxisA);
		FP y5 = MathUtils.Cross(fVector6 + fVector5, in b);
		FP y6 = MathUtils.Cross(in a2, in b);
		FP fP3 = FVector2.Dot(fVector6, b);
		FP fP4 = _invMassA + _invMassB + _invIA * _sAy * _sAy + _invIB * _sBy * _sBy;
		FP x5 = FP.Zero;
		if (fP4 != FP.Zero)
		{
			x5 = -fP3 / fP4;
		}
		FVector2 fVector7 = x5 * b;
		FP y7 = x5 * y5;
		FP y8 = x5 * y6;
		center -= _invMassA * fVector7;
		x -= _invIA * y7;
		center2 += _invMassB * fVector7;
		x2 += _invIB * y8;
		left = FP.Max(left, FP.Abs(fP3));
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = x;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x2;
		return left <= Settings.LinearSlop;
	}

	public override void Draw(IDraw draw)
	{
		Transform T = BodyA.GetTransform();
		Transform T2 = BodyB.GetTransform();
		FVector2 p = MathUtils.Mul(in T, in _localAnchorA);
		FVector2 p2 = MathUtils.Mul(in T2, in _localAnchorB);
		FVector2 fVector = MathUtils.Mul(in T.Rotation, in _localXAxisA);
		Color color = Color.FromArgb(0.7f, 0.7f, 0.7f);
		Color color2 = Color.FromArgb(0.3f, 0.9f, 0.3f);
		Color color3 = Color.FromArgb(0.9f, 0.3f, 0.3f);
		Color color4 = Color.FromArgb(0.3f, 0.3f, 0.9f);
		draw.DrawSegment(in p, in p2, Color.FromArgb(0.4f, 0.4f, 0.4f));
		if (_enableLimit)
		{
			FVector2 p3 = p + _lowerTranslation * fVector;
			FVector2 p4 = p + _upperTranslation * fVector;
			FVector2 fVector2 = MathUtils.Mul(in T.Rotation, in _localYAxisA);
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

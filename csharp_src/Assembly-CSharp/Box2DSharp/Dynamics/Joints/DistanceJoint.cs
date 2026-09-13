using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class DistanceJoint : Joint
{
	private readonly FVector2 _localAnchorA;

	private readonly FVector2 _localAnchorB;

	private FP _bias;

	private FP _gamma;

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

	private FVector2 _u;

	private FP _length;

	private FP _minLength;

	private FP _maxLength;

	private FP _currentLength;

	private FP _lowerImpulse;

	private FP _upperImpulse;

	public FP Stiffness { get; set; }

	public FP Damping { get; set; }

	public FP SoftMass { get; set; }

	internal DistanceJoint(DistanceJointDef def)
		: base(def)
	{
		_localAnchorA = def.LocalAnchorA;
		_localAnchorB = def.LocalAnchorB;
		_length = FP.Max(def.Length, Settings.LinearSlop);
		_minLength = FP.Max(def.MinLength, Settings.LinearSlop);
		_maxLength = FP.Max(def.MaxLength, _minLength);
		Stiffness = def.Stiffness;
		Damping = def.Damping;
		_impulse = 0f;
		_gamma = 0f;
		_bias = 0f;
		_impulse = 0f;
		_lowerImpulse = 0f;
		_upperImpulse = 0f;
		_currentLength = 0f;
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
		return inv_dt * (_impulse + _lowerImpulse - _upperImpulse) * _u;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return 0f;
	}

	public FVector2 GetLocalAnchorA()
	{
		return _localAnchorA;
	}

	public FVector2 GetLocalAnchorB()
	{
		return _localAnchorB;
	}

	public FP SetLength(FP length)
	{
		_impulse = 0f;
		_length = FP.Max(Settings.LinearSlop, length);
		return _length;
	}

	public FP SetMinLength(FP minLength)
	{
		_lowerImpulse = 0f;
		_minLength = MathUtils.Clamp(minLength, Settings.LinearSlop, _maxLength);
		return _minLength;
	}

	public FP SetMaxLength(FP maxLength)
	{
		_upperImpulse = 0f;
		_maxLength = FP.Max(maxLength, _minLength);
		return _maxLength;
	}

	public FP GetCurrentLength()
	{
		FVector2 worldPoint = BodyA.GetWorldPoint(in _localAnchorA);
		return (BodyB.GetWorldPoint(in _localAnchorB) - worldPoint).Length();
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
		_u = center2 + _rB - center - _rA;
		_currentLength = _u.Length();
		if (_currentLength > Settings.LinearSlop)
		{
			_u *= 1f / _currentLength;
		}
		else
		{
			_u.Set(0f, 0f);
			_mass = 0f;
			_impulse = 0f;
			_lowerImpulse = 0f;
			_upperImpulse = 0f;
		}
		FP y = MathUtils.Cross(in _rA, in _u);
		FP y2 = MathUtils.Cross(in _rB, in _u);
		FP x3 = _invMassA + _invIa * y * y + _invMassB + _invIb * y2 * y2;
		_mass = ((x3 != FP.Zero) ? (FP.One / x3) : FP.Zero);
		if (Stiffness > 0f && _minLength < _maxLength)
		{
			FP x4 = _currentLength - _length;
			FP x5 = Damping;
			FP y3 = Stiffness;
			FP x6 = data.Step.Dt;
			_gamma = x6 * (x5 + x6 * y3);
			_gamma = ((_gamma != FP.Zero) ? (FP.One / _gamma) : FP.Zero);
			_bias = x4 * x6 * y3 * _gamma;
			x3 += _gamma;
			SoftMass = ((FP.Abs(x3) > Settings.Epsilon) ? (FP.One / x3) : FP.Zero);
		}
		else
		{
			_gamma = 0f;
			_bias = 0f;
			_mass = ((x3 != FP.Zero) ? (FP.One / x3) : FP.Zero);
			SoftMass = _mass;
		}
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			_lowerImpulse *= data.Step.DtRatio;
			_upperImpulse *= data.Step.DtRatio;
			FVector2 b = (_impulse + _lowerImpulse - _upperImpulse) * _u;
			v -= _invMassA * b;
			x -= _invIa * MathUtils.Cross(in _rA, in b);
			v2 += _invMassB * b;
			x2 += _invIb * MathUtils.Cross(in _rB, in b);
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
		if (_minLength < _maxLength)
		{
			if (Stiffness > 0f)
			{
				FVector2 fVector = v + MathUtils.Cross(x, in _rA);
				FVector2 fVector2 = v2 + MathUtils.Cross(x2, in _rB);
				FP x3 = FVector2.Dot(_u, fVector2 - fVector);
				FP y = -SoftMass * (x3 + _bias + _gamma * _impulse);
				_impulse += y;
				FVector2 b = y * _u;
				v -= _invMassA * b;
				x -= _invIa * MathUtils.Cross(in _rA, in b);
				v2 += _invMassB * b;
				x2 += _invIb * MathUtils.Cross(in _rB, in b);
			}
			FP righ = _currentLength - _minLength;
			FP y2 = FP.Max(0f, righ) * data.Step.InvDt;
			FVector2 fVector3 = v + MathUtils.Cross(x, in _rA);
			FVector2 fVector4 = v2 + MathUtils.Cross(x2, in _rB);
			FP x4 = FVector2.Dot(_u, fVector4 - fVector3);
			FP y3 = -_mass * (x4 + y2);
			FP y4 = _lowerImpulse;
			_lowerImpulse = FP.Max(0f, _lowerImpulse + y3);
			y3 = _lowerImpulse - y4;
			FVector2 b2 = y3 * _u;
			v -= _invMassA * b2;
			x -= _invIa * MathUtils.Cross(in _rA, in b2);
			v2 += _invMassB * b2;
			x2 += _invIb * MathUtils.Cross(in _rB, in b2);
			FP righ2 = _maxLength - _currentLength;
			FP y5 = FP.Max(0f, righ2).SafeMul(in data.Step.InvDt);
			FVector2 fVector5 = v + MathUtils.Cross(x, in _rA);
			FVector2 fVector6 = v2 + MathUtils.Cross(x2, in _rB);
			FP fP = FVector2.Dot(_u, fVector5 - fVector6);
			FP y6 = -_mass.SafeMul(fP.SafeAdd(in y5));
			FP y7 = _upperImpulse;
			_upperImpulse = FP.Max(0f, _upperImpulse + y6);
			y6 = _upperImpulse - y7;
			FVector2 b3 = -y6 * _u;
			v -= _invMassA * b3;
			x -= _invIa * MathUtils.Cross(in _rA, in b3);
			v2 += _invMassB * b3;
			x2 += _invIb * MathUtils.Cross(in _rB, in b3);
		}
		else
		{
			FVector2 fVector7 = v + MathUtils.Cross(x, in _rA);
			FVector2 fVector8 = v2 + MathUtils.Cross(x2, in _rB);
			FP y8 = FVector2.Dot(_u, fVector8 - fVector7);
			FP y9 = -_mass * y8;
			_impulse += y9;
			FVector2 b4 = y9 * _u;
			v -= _invMassA * b4;
			x -= _invIa * MathUtils.Cross(in _rA, in b4);
			v2 += _invMassB * b4;
			x2 += _invIb * MathUtils.Cross(in _rB, in b4);
		}
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
		FVector2 fVector = center2 + a2 - center - a;
		FP x3 = fVector.Normalize();
		FP zero = FP.Zero;
		if (FP.Abs(_minLength - _maxLength) < Settings.Epsilon)
		{
			zero = x3 - _minLength;
		}
		else if (x3 < _minLength)
		{
			zero = x3 - _minLength;
		}
		else
		{
			if (!(_maxLength < x3))
			{
				return true;
			}
			zero = x3 - _maxLength;
		}
		FVector2 b = -_mass * zero * fVector;
		center -= _invMassA * b;
		x -= _invIa * MathUtils.Cross(in a, in b);
		center2 += _invMassB * b;
		x2 += _invIb * MathUtils.Cross(in a2, in b);
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = x;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x2;
		return FP.Abs(zero) < Settings.LinearSlop;
	}

	public override void Draw(IDraw draw)
	{
		Transform T = BodyA.GetTransform();
		Transform T2 = BodyB.GetTransform();
		FVector2 p = MathUtils.Mul(in T, in _localAnchorA);
		FVector2 p2 = MathUtils.Mul(in T2, in _localAnchorB);
		FVector2 fVector = p2 - p;
		fVector.Normalize();
		Color color = Color.FromArgb(0.7f, 0.7f, 0.7f);
		Color color2 = Color.FromArgb(0.3f, 0.9f, 0.3f);
		Color color3 = Color.FromArgb(0.9f, 0.3f, 0.3f);
		draw.DrawSegment(in p, in p2, Color.FromArgb(0.4f, 0.4f, 0.4f));
		draw.DrawPoint(p + _length * fVector, 8f, in color);
		if (FP.Abs(_minLength - _maxLength) > Settings.Epsilon)
		{
			if (_minLength > Settings.LinearSlop)
			{
				draw.DrawPoint(p + _minLength * fVector, 4f, in color2);
			}
			if (_maxLength < Settings.MaxFloat)
			{
				draw.DrawPoint(p + _maxLength * fVector, 4f, in color3);
			}
		}
	}
}

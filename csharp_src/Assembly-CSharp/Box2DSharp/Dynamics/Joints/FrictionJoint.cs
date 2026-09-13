using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class FrictionJoint : Joint
{
	private FP _angularImpulse;

	private FP _angularMass;

	private int _indexA;

	private int _indexB;

	private FP _invIa;

	private FP _invIb;

	private FP _invMassA;

	private FP _invMassB;

	private FVector2 _linearImpulse;

	private Matrix2x2 _linearMass;

	private FVector2 _localAnchorA;

	private FVector2 _localAnchorB;

	private FVector2 _localCenterA;

	private FVector2 _localCenterB;

	private FP _maxForce;

	private FP _maxTorque;

	private FVector2 _rA;

	private FVector2 _rB;

	public FP MaxForce
	{
		get
		{
			return _maxForce;
		}
		set
		{
			_maxForce = value;
		}
	}

	public FP MaxTorque
	{
		get
		{
			return _maxTorque;
		}
		set
		{
			_maxTorque = value;
		}
	}

	internal FrictionJoint(FrictionJointDef def)
		: base(def)
	{
		_localAnchorA = def.LocalAnchorA;
		_localAnchorB = def.LocalAnchorB;
		_linearImpulse.SetZero();
		_angularImpulse = 0f;
		_maxForce = def.MaxForce;
		_maxTorque = def.MaxTorque;
	}

	public FVector2 GetLocalAnchorA()
	{
		return _localAnchorA;
	}

	public FVector2 GetLocalAnchorB()
	{
		return _localAnchorB;
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
		return inv_dt * _linearImpulse;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * _angularImpulse;
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
		FP angle = data.Positions[_indexA].Angle;
		FVector2 v = data.Velocities[_indexA].V;
		FP x = data.Velocities[_indexA].W;
		FP angle2 = data.Positions[_indexB].Angle;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x2 = data.Velocities[_indexB].W;
		Rotation q = new Rotation(angle);
		Rotation q2 = new Rotation(angle2);
		_rA = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
		_rB = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
		FP x3 = _invMassA;
		FP y = _invMassB;
		FP x4 = _invIa;
		FP x5 = _invIb;
		Matrix2x2 matrix2x = default(Matrix2x2);
		matrix2x.Ex.X = x3 + y + x4 * _rA.Y * _rA.Y + x5 * _rB.Y * _rB.Y;
		matrix2x.Ex.Y = -x4 * _rA.X * _rA.Y - x5 * _rB.X * _rB.Y;
		matrix2x.Ey.X = matrix2x.Ex.Y;
		matrix2x.Ey.Y = x3 + y + x4 * _rA.X * _rA.X + x5 * _rB.X * _rB.X;
		_linearMass = matrix2x.GetInverse();
		_angularMass = x4 + x5;
		if (_angularMass > 0f)
		{
			_angularMass = 1f / _angularMass;
		}
		if (data.Step.WarmStarting)
		{
			_linearImpulse *= data.Step.DtRatio;
			_angularImpulse *= data.Step.DtRatio;
			FVector2 b = new FVector2(_linearImpulse.X, _linearImpulse.Y);
			v -= x3 * b;
			x -= x4 * (MathUtils.Cross(in _rA, in b) + _angularImpulse);
			v2 += y * b;
			x2 += x5 * (MathUtils.Cross(in _rB, in b) + _angularImpulse);
		}
		else
		{
			_linearImpulse.SetZero();
			_angularImpulse = 0f;
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
		FP x = data.Velocities[_indexB].W;
		FP invMassA = _invMassA;
		FP invMassB = _invMassB;
		FP x2 = _invIa;
		FP x3 = _invIb;
		FP x4 = data.Step.Dt;
		FP y2 = x - y;
		FP y3 = -_angularMass * y2;
		FP y4 = _angularImpulse;
		FP fP = x4 * _maxTorque;
		_angularImpulse = MathUtils.Clamp(_angularImpulse + y3, -fP, fP);
		y3 = _angularImpulse - y4;
		y -= x2 * y3;
		x += x3 * y3;
		FVector2 v3 = v2 + MathUtils.Cross(x, in _rB) - v - MathUtils.Cross(y, in _rA);
		FVector2 fVector = -MathUtils.Mul(in _linearMass, in v3);
		FVector2 linearImpulse = _linearImpulse;
		_linearImpulse += fVector;
		FP x5 = x4 * _maxForce;
		if (_linearImpulse.LengthSquared() > x5 * x5)
		{
			_linearImpulse.Normalize();
			_linearImpulse *= x5;
		}
		fVector = _linearImpulse - linearImpulse;
		v -= invMassA * fVector;
		y -= x2 * MathUtils.Cross(in _rA, in fVector);
		v2 += invMassB * fVector;
		x += x3 * MathUtils.Cross(in _rB, in fVector);
		data.Velocities[_indexA].V = v;
		data.Velocities[_indexA].W = y;
		data.Velocities[_indexB].V = v2;
		data.Velocities[_indexB].W = x;
	}

	internal override bool SolvePositionConstraints(in SolverData data)
	{
		return true;
	}
}

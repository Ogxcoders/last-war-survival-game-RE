using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class MouseJoint : Joint
{
	private readonly FVector2 _localAnchorB;

	private FP _beta;

	private FVector2 _C;

	public FP Damping;

	public FP Stiffness;

	private FP _gamma;

	private FVector2 _impulse;

	private int _indexB;

	private FP _invIb;

	private FP _invMassB;

	private FVector2 _localCenterB;

	private Matrix2x2 _mass;

	public FP MaxForce;

	private FVector2 _rB;

	public FVector2 Target;

	internal MouseJoint(MouseJointDef def)
		: base(def)
	{
		Target = def.Target;
		_localAnchorB = MathUtils.MulT(BodyB.GetTransform(), in Target);
		MaxForce = def.MaxForce;
		Stiffness = def.Stiffness;
		Damping = def.Damping;
		_impulse.SetZero();
		_beta = 0f;
		_gamma = 0f;
	}

	public void SetTarget(in FVector2 target)
	{
		if (target != Target)
		{
			BodyB.IsAwake = true;
			Target = target;
		}
	}

	public override void ShiftOrigin(in FVector2 newOrigin)
	{
		Target -= newOrigin;
	}

	public override FVector2 GetAnchorA()
	{
		return Target;
	}

	public override FVector2 GetAnchorB()
	{
		return BodyB.GetWorldPoint(in _localAnchorB);
	}

	public override FVector2 GetReactionForce(FP inv_dt)
	{
		return inv_dt * _impulse;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * (FP)0f;
	}

	public override void Dump()
	{
		DumpLogger.Log("Mouse joint dumping is not supported.");
	}

	internal override void InitVelocityConstraints(in SolverData data)
	{
		_indexB = BodyB.IslandIndex;
		_localCenterB = BodyB.Sweep.LocalCenter;
		_invMassB = BodyB.InvMass;
		_invIb = BodyB.InverseInertia;
		FVector2 center = data.Positions[_indexB].Center;
		FP angle = data.Positions[_indexB].Angle;
		FVector2 v = data.Velocities[_indexB].V;
		FP x = data.Velocities[_indexB].W;
		Rotation q = new Rotation(angle);
		FP x2 = Damping;
		FP y = Stiffness;
		FP x3 = data.Step.Dt;
		_gamma = x3 * (x2 + x3 * y);
		if (!_gamma.Equals(0f))
		{
			_gamma = 1f / _gamma;
		}
		_beta = x3 * y * _gamma;
		_rB = MathUtils.Mul(in q, _localAnchorB - _localCenterB);
		Matrix2x2 matrix2x = default(Matrix2x2);
		matrix2x.Ex.X = _invMassB + _invIb * _rB.Y * _rB.Y + _gamma;
		matrix2x.Ex.Y = -_invIb * _rB.X * _rB.Y;
		matrix2x.Ey.X = matrix2x.Ex.Y;
		matrix2x.Ey.Y = _invMassB + _invIb * _rB.X * _rB.X + _gamma;
		_mass = matrix2x.GetInverse();
		_C = center + _rB - Target;
		_C *= _beta;
		x *= (FP)0.98f;
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			v += _invMassB * _impulse;
			x += _invIb * MathUtils.Cross(in _rB, in _impulse);
		}
		else
		{
			_impulse.SetZero();
		}
		data.Velocities[_indexB].V = v;
		data.Velocities[_indexB].W = x;
	}

	internal override void SolveVelocityConstraints(in SolverData data)
	{
		FVector2 v = data.Velocities[_indexB].V;
		FP x = data.Velocities[_indexB].W;
		FVector2 fVector = v + MathUtils.Cross(x, in _rB);
		FVector2 fVector2 = MathUtils.Mul(in _mass, -(fVector + _C + _gamma * _impulse));
		FVector2 impulse = _impulse;
		_impulse += fVector2;
		FP x2 = data.Step.Dt * MaxForce;
		if (_impulse.LengthSquared() > x2 * x2)
		{
			_impulse *= x2 / _impulse.Length();
		}
		fVector2 = _impulse - impulse;
		v += _invMassB * fVector2;
		x += _invIb * MathUtils.Cross(in _rB, in fVector2);
		data.Velocities[_indexB].V = v;
		data.Velocities[_indexB].W = x;
	}

	internal override bool SolvePositionConstraints(in SolverData data)
	{
		return true;
	}
}

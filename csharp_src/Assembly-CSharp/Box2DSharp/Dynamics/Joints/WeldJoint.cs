using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public class WeldJoint : Joint
{
	private readonly FVector2 _localAnchorA;

	private readonly FVector2 _localAnchorB;

	private readonly FP _referenceAngle;

	private FP _bias;

	public FP Stiffness = 0f;

	public FP Damping = 0f;

	private FP _gamma;

	private FVector3 _impulse;

	private int _indexA;

	private int _indexB;

	private FP _invIa;

	private FP _invIb;

	private FP _invMassA;

	private FP _invMassB;

	private FVector2 _localCenterA;

	private FVector2 _localCenterB;

	private Matrix3x3 _mass;

	private FVector2 _rA;

	private FVector2 _rB;

	internal WeldJoint(WeldJointDef def)
		: base(def)
	{
		_localAnchorA = def.LocalAnchorA;
		_localAnchorB = def.LocalAnchorB;
		_referenceAngle = def.ReferenceAngle;
		Damping = def.Damping;
		Stiffness = def.Stiffness;
		_impulse.SetZero();
	}

	public FVector2 GetLocalAnchorA()
	{
		return _localAnchorA;
	}

	public FVector2 GetLocalAnchorB()
	{
		return _localAnchorB;
	}

	public FP GetReferenceAngle()
	{
		return _referenceAngle;
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
		FVector2 fVector = new FVector2(_impulse.X, _impulse.Y);
		return inv_dt * fVector;
	}

	public override FP GetReactionTorque(FP inv_dt)
	{
		return inv_dt * _impulse.Z;
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
		FP y = data.Positions[_indexA].Angle;
		FVector2 v = data.Velocities[_indexA].V;
		FP x = data.Velocities[_indexA].W;
		FP x2 = data.Positions[_indexB].Angle;
		FVector2 v2 = data.Velocities[_indexB].V;
		FP x3 = data.Velocities[_indexB].W;
		Rotation q = new Rotation(y);
		Rotation q2 = new Rotation(x2);
		_rA = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
		_rB = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
		FP x4 = _invMassA;
		FP y2 = _invMassB;
		FP y3 = _invIa;
		FP y4 = _invIb;
		Matrix3x3 matrix3x = default(Matrix3x3);
		matrix3x.Ex.X = x4 + y2 + _rA.Y * _rA.Y * y3 + _rB.Y * _rB.Y * y4;
		matrix3x.Ey.X = -_rA.Y * _rA.X * y3 - _rB.Y * _rB.X * y4;
		matrix3x.Ez.X = -_rA.Y * y3 - _rB.Y * y4;
		matrix3x.Ex.Y = matrix3x.Ey.X;
		matrix3x.Ey.Y = x4 + y2 + _rA.X * _rA.X * y3 + _rB.X * _rB.X * y4;
		matrix3x.Ez.Y = _rA.X * y3 + _rB.X * y4;
		matrix3x.Ex.Z = matrix3x.Ez.X;
		matrix3x.Ey.Z = matrix3x.Ez.Y;
		matrix3x.Ez.Z = y3 + y4;
		if (Stiffness > 0f)
		{
			matrix3x.GetInverse22(ref _mass);
			FP x5 = y3 + y4;
			FP x6 = x2 - y - _referenceAngle;
			FP x7 = Damping;
			FP y5 = Stiffness;
			FP x8 = data.Step.Dt;
			_gamma = x8 * (x7 + x8 * y5);
			_gamma = ((_gamma != FP.Zero) ? (FP.One / _gamma) : FP.Zero);
			_bias = x6 * x8 * y5 * _gamma;
			x5 += _gamma;
			_mass.Ez.Z = ((x5 != FP.Zero) ? (FP.One / x5) : FP.Zero);
		}
		else if (matrix3x.Ez.Z.Equals(0f))
		{
			matrix3x.GetInverse22(ref _mass);
			_gamma = 0f;
			_bias = 0f;
		}
		else
		{
			matrix3x.GetSymInverse33(ref _mass);
			_gamma = 0f;
			_bias = 0f;
		}
		if (data.Step.WarmStarting)
		{
			_impulse *= data.Step.DtRatio;
			FVector2 b = new FVector2(_impulse.X, _impulse.Y);
			v -= x4 * b;
			x -= y3 * (MathUtils.Cross(in _rA, in b) + _impulse.Z);
			v2 += y2 * b;
			x3 += y4 * (MathUtils.Cross(in _rB, in b) + _impulse.Z);
		}
		else
		{
			_impulse.SetZero();
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
		FP x3 = _invIb;
		if (Stiffness > 0f)
		{
			FP x4 = x - y;
			FP y2 = -_mass.Ez.Z * (x4 + _bias + _gamma * _impulse.Z);
			ref FP z = ref _impulse.Z;
			z += y2;
			y -= x2 * y2;
			x += x3 * y2;
			FVector2 v3 = v2 + MathUtils.Cross(x, in _rB) - v - MathUtils.Cross(y, in _rA);
			FVector2 fVector = -MathUtils.Mul22(in _mass, in v3);
			ref FP x5 = ref _impulse.X;
			x5 += fVector.X;
			ref FP y3 = ref _impulse.Y;
			y3 += fVector.Y;
			FVector2 b = fVector;
			v -= invMassA * b;
			y -= x2 * MathUtils.Cross(in _rA, in b);
			v2 += invMassB * b;
			x += x3 * MathUtils.Cross(in _rB, in b);
		}
		else
		{
			FVector2 fVector2 = v2 + MathUtils.Cross(x, in _rB) - v - MathUtils.Cross(y, in _rA);
			FP z2 = x - y;
			FVector3 v4 = new FVector3(fVector2.X, fVector2.Y, z2);
			FVector3 fVector3 = -MathUtils.Mul(in _mass, in v4);
			_impulse += fVector3;
			FVector2 b2 = new FVector2(fVector3.X, fVector3.Y);
			v -= invMassA * b2;
			y -= x2 * (MathUtils.Cross(in _rA, in b2) + fVector3.Z);
			v2 += invMassB * b2;
			x += x3 * (MathUtils.Cross(in _rB, in b2) + fVector3.Z);
		}
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
		FP x2 = _invMassA;
		FP y2 = _invMassB;
		FP y3 = _invIa;
		FP y4 = _invIb;
		FVector2 a = MathUtils.Mul(in q, _localAnchorA - _localCenterA);
		FVector2 a2 = MathUtils.Mul(in q2, _localAnchorB - _localCenterB);
		Matrix3x3 matrix3x = default(Matrix3x3);
		matrix3x.Ex.X = x2 + y2 + a.Y * a.Y * y3 + a2.Y * a2.Y * y4;
		matrix3x.Ey.X = -a.Y * a.X * y3 - a2.Y * a2.X * y4;
		matrix3x.Ez.X = -a.Y * y3 - a2.Y * y4;
		matrix3x.Ex.Y = matrix3x.Ey.X;
		matrix3x.Ey.Y = x2 + y2 + a.X * a.X * y3 + a2.X * a2.X * y4;
		matrix3x.Ez.Y = a.X * y3 + a2.X * y4;
		matrix3x.Ex.Z = matrix3x.Ez.X;
		matrix3x.Ey.Z = matrix3x.Ez.Y;
		matrix3x.Ez.Z = y3 + y4;
		FP fP;
		FP fP2;
		if (Stiffness > 0f)
		{
			FVector2 b = center2 + a2 - center - a;
			fP = b.Length();
			fP2 = 0f;
			FVector2 b2 = -matrix3x.Solve22(in b);
			center -= x2 * b2;
			y -= y3 * MathUtils.Cross(in a, in b2);
			center2 += y2 * b2;
			x += y4 * MathUtils.Cross(in a2, in b2);
		}
		else
		{
			FVector2 b3 = center2 + a2 - center - a;
			FP fP3 = x - y - _referenceAngle;
			fP = b3.Length();
			fP2 = FP.Abs(fP3);
			FVector3 b4 = new FVector3(b3.X, b3.Y, fP3);
			FVector3 fVector = default(FVector3);
			if (matrix3x.Ez.Z > 0f)
			{
				fVector = -matrix3x.Solve33(in b4);
			}
			else
			{
				FVector2 fVector2 = -matrix3x.Solve22(in b3);
				fVector.Set(fVector2.X, fVector2.Y, 0f);
			}
			FVector2 b5 = new FVector2(fVector.X, fVector.Y);
			center -= x2 * b5;
			y -= y3 * (MathUtils.Cross(in a, in b5) + fVector.Z);
			center2 += y2 * b5;
			x += y4 * (MathUtils.Cross(in a2, in b5) + fVector.Z);
		}
		data.Positions[_indexA].Center = center;
		data.Positions[_indexA].Angle = y;
		data.Positions[_indexB].Center = center2;
		data.Positions[_indexB].Angle = x;
		if (fP <= Settings.LinearSlop)
		{
			return fP2 <= Settings.AngularSlop;
		}
		return false;
	}
}

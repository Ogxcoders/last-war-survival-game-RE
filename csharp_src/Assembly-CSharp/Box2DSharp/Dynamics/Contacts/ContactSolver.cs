using System;
using System.Buffers;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public class ContactSolver
{
	internal ContactPositionConstraint[] PositionConstraints;

	internal ContactVelocityConstraint[] VelocityConstraints;

	private int _contactCount;

	private Contact[] _contacts;

	private Position[] _positions;

	private Velocity[] _velocities;

	private readonly ArrayPool<ContactPositionConstraint> _contactPositionConstraintPool = ArrayPool<ContactPositionConstraint>.Create();

	private readonly ArrayPool<ContactVelocityConstraint> _contactVelocityConstraintPool = ArrayPool<ContactVelocityConstraint>.Create();

	public void Setup(in ContactSolverDef def)
	{
		TimeStep step = def.Step;
		_contactCount = def.ContactCount;
		PositionConstraints = _contactPositionConstraintPool.Rent(_contactCount);
		VelocityConstraints = _contactVelocityConstraintPool.Rent(_contactCount);
		_positions = def.Positions;
		_velocities = def.Velocities;
		_contacts = def.Contacts;
		Span<Contact> span = _contacts;
		Span<ContactVelocityConstraint> span2 = VelocityConstraints;
		Span<ContactPositionConstraint> span3 = PositionConstraints;
		for (int i = 0; i < _contactCount; i++)
		{
			Contact contact = span[i];
			Fixture fixtureA = contact.FixtureA;
			Fixture fixtureB = contact.FixtureB;
			Shape shape = fixtureA.Shape;
			Shape shape2 = fixtureB.Shape;
			FP radius = shape.Radius;
			FP radius2 = shape2.Radius;
			Body body = fixtureA.Body;
			Body body2 = fixtureB.Body;
			ref Manifold manifold = ref contact.Manifold;
			int pointCount = manifold.PointCount;
			ref ContactVelocityConstraint reference = ref span2[i];
			reference.Friction = contact.Friction;
			reference.Restitution = contact.Restitution;
			reference.Threshold = contact.RestitutionThreshold;
			reference.TangentSpeed = contact.TangentSpeed;
			reference.IndexA = body.IslandIndex;
			reference.IndexB = body2.IslandIndex;
			reference.InvMassA = body.InvMass;
			reference.InvMassB = body2.InvMass;
			reference.InvIa = body.InverseInertia;
			reference.InvIb = body2.InverseInertia;
			reference.ContactIndex = i;
			reference.PointCount = pointCount;
			reference.K.SetZero();
			reference.NormalMass.SetZero();
			ref ContactPositionConstraint reference2 = ref span3[i];
			reference2.IndexA = body.IslandIndex;
			reference2.IndexB = body2.IslandIndex;
			reference2.InvMassA = body.InvMass;
			reference2.InvMassB = body2.InvMass;
			reference2.LocalCenterA = body.Sweep.LocalCenter;
			reference2.LocalCenterB = body2.Sweep.LocalCenter;
			reference2.InvIa = body.InverseInertia;
			reference2.InvIb = body2.InverseInertia;
			reference2.LocalNormal = manifold.LocalNormal;
			reference2.LocalPoint = manifold.LocalPoint;
			reference2.PointCount = pointCount;
			reference2.RadiusA = radius;
			reference2.RadiusB = radius2;
			reference2.Type = manifold.Type;
			for (int j = 0; j < pointCount; j++)
			{
				ref ManifoldPoint reference3 = ref j == 0 ? ref manifold.Points.Value0 : ref manifold.Points.Value1;
				ref VelocityConstraintPoint reference4 = ref j == 0 ? ref reference.Points.Value0 : ref reference.Points.Value1;
				if (step.WarmStarting)
				{
					reference4.NormalImpulse = step.DtRatio * reference3.NormalImpulse;
					reference4.TangentImpulse = step.DtRatio * reference3.TangentImpulse;
				}
				else
				{
					reference4.NormalImpulse = 0f;
					reference4.TangentImpulse = 0f;
				}
				reference4.Ra = default(FVector2);
				reference4.Rb = default(FVector2);
				reference4.NormalMass = 0f;
				reference4.TangentMass = 0f;
				reference4.VelocityBias = 0f;
				reference2.LocalPoints[j] = reference3.LocalPoint;
			}
		}
	}

	public void Reset()
	{
		_contactPositionConstraintPool.Return(PositionConstraints, clearArray: true);
		PositionConstraints = null;
		_contactVelocityConstraintPool.Return(VelocityConstraints, clearArray: true);
		VelocityConstraints = null;
		_positions = null;
		_contacts = null;
		_velocities = null;
		_contactCount = 0;
	}

	public void InitializeVelocityConstraints()
	{
		Span<Position> span = _positions;
		Span<Velocity> span2 = _velocities;
		for (int i = 0; i < _contactCount; i++)
		{
			ref ContactVelocityConstraint reference = ref VelocityConstraints[i];
			ref ContactPositionConstraint reference2 = ref PositionConstraints[i];
			FP radiusA = reference2.RadiusA;
			FP radiusB = reference2.RadiusB;
			ref Manifold manifold = ref _contacts[reference.ContactIndex].Manifold;
			int indexA = reference.IndexA;
			int indexB = reference.IndexB;
			FP x = reference.InvMassA;
			FP y = reference.InvMassB;
			FP x2 = reference.InvIa;
			FP x3 = reference.InvIb;
			FVector2 v = reference2.LocalCenterA;
			FVector2 v2 = reference2.LocalCenterB;
			FVector2 center = span[indexA].Center;
			FP angle = span[indexA].Angle;
			FVector2 v3 = span2[indexA].V;
			FP x4 = span2[indexA].W;
			FVector2 center2 = span[indexB].Center;
			FP angle2 = span[indexB].Angle;
			FVector2 v4 = span2[indexB].V;
			FP x5 = span2[indexB].W;
			Transform xfA = default(Transform);
			Transform xfB = default(Transform);
			xfA.Rotation.Set(angle);
			xfB.Rotation.Set(angle2);
			xfA.Position = center - MathUtils.Mul(in xfA.Rotation, in v);
			xfB.Position = center2 - MathUtils.Mul(in xfB.Rotation, in v2);
			WorldManifold worldManifold = default(WorldManifold);
			worldManifold.Initialize(in manifold, in xfA, radiusA, in xfB, radiusB);
			reference.Normal = worldManifold.Normal;
			for (int j = 0; j < reference.PointCount; j++)
			{
				ref VelocityConstraintPoint reference3 = ref j == 0 ? ref reference.Points.Value0 : ref reference.Points.Value1;
				ref FVector2 reference4 = ref j == 0 ? ref worldManifold.Points.Value0 : ref worldManifold.Points.Value1;
				reference3.Ra = reference4 - center;
				reference3.Rb = reference4 - center2;
				FP y2 = MathUtils.Cross(in reference3.Ra, in reference.Normal);
				FP y3 = MathUtils.Cross(in reference3.Rb, in reference.Normal);
				FP fP = x + y + x2 * y2 * y2 + x3 * y3 * y3;
				reference3.NormalMass = ((fP > FP.Zero) ? (FP.One / fP) : FP.Zero);
				FVector2 b = MathUtils.Cross(in reference.Normal, 1f);
				FP y4 = MathUtils.Cross(in reference3.Ra, in b);
				FP y5 = MathUtils.Cross(in reference3.Rb, in b);
				FP fP2 = x + y + x2 * y4 * y4 + x3 * y5 * y5;
				reference3.TangentMass = ((fP2 > FP.Zero) ? (FP.One / fP2) : FP.Zero);
				reference3.VelocityBias = FP.Zero;
				FP y6 = FVector2.Dot(reference.Normal, new FVector2(v4.X - x5 * reference3.Rb.Y - v3.X + x4 * reference3.Ra.Y, v4.Y + x5 * reference3.Rb.X - v3.Y - x4 * reference3.Ra.X));
				if (y6 < -reference.Threshold)
				{
					reference3.VelocityBias = -reference.Restitution * y6;
				}
			}
			if (reference.PointCount == 2)
			{
				ref VelocityConstraintPoint value = ref reference.Points.Value0;
				ref VelocityConstraintPoint value2 = ref reference.Points.Value1;
				FP y7 = value.Ra.X * reference.Normal.Y - value.Ra.Y * reference.Normal.X;
				FP y8 = value.Rb.X * reference.Normal.Y - value.Rb.Y * reference.Normal.X;
				FP y9 = value2.Ra.X * reference.Normal.Y - value2.Ra.Y * reference.Normal.X;
				FP y10 = value2.Rb.X * reference.Normal.Y - value2.Rb.Y * reference.Normal.X;
				FP x6 = x + y + x2 * y7 * y7 + x3 * y8 * y8;
				FP y11 = x + y + x2 * y9 * y9 + x3 * y10 * y10;
				FP x7 = x + y + x2 * y7 * y9 + x3 * y8 * y10;
				FP x8 = 1000;
				if (x6 * x6 < x8 * (x6 * y11 - x7 * x7))
				{
					reference.K.Ex.Set(x6, x7);
					reference.K.Ey.Set(x7, y11);
					reference.NormalMass = reference.K.GetInverse();
				}
				else
				{
					reference.PointCount = 1;
				}
			}
		}
	}

	public void WarmStart()
	{
		Span<ContactVelocityConstraint> span = VelocityConstraints;
		Span<Velocity> span2 = _velocities;
		for (int i = 0; i < _contactCount; i++)
		{
			ref ContactVelocityConstraint reference = ref span[i];
			int indexA = reference.IndexA;
			int indexB = reference.IndexB;
			FP invMassA = reference.InvMassA;
			FP x = reference.InvIa;
			FP invMassB = reference.InvMassB;
			FP x2 = reference.InvIb;
			int pointCount = reference.PointCount;
			FVector2 v = span2[indexA].V;
			FP x3 = span2[indexA].W;
			FVector2 v2 = span2[indexB].V;
			FP x4 = span2[indexB].W;
			FVector2 a = reference.Normal;
			FVector2 fVector = MathUtils.Cross(in a, 1f);
			for (int j = 0; j < pointCount; j++)
			{
				ref VelocityConstraintPoint reference2 = ref j == 0 ? ref reference.Points.Value0 : ref reference.Points.Value1;
				FVector2 b = reference2.NormalImpulse * a + reference2.TangentImpulse * fVector;
				x3 -= x * MathUtils.Cross(in reference2.Ra, in b);
				v -= invMassA * b;
				x4 += x2 * MathUtils.Cross(in reference2.Rb, in b);
				v2 += invMassB * b;
			}
			span2[indexA].V = v;
			span2[indexA].W = x3;
			span2[indexB].V = v2;
			span2[indexB].W = x4;
		}
	}

	public void SolveVelocityConstraints()
	{
		Span<ContactVelocityConstraint> span = VelocityConstraints;
		Span<Velocity> span2 = _velocities;
		for (int i = 0; i < _contactCount; i++)
		{
			ref ContactVelocityConstraint reference = ref span[i];
			int indexA = reference.IndexA;
			int indexB = reference.IndexB;
			FP x = reference.InvMassA;
			FP x2 = reference.InvIa;
			FP x3 = reference.InvMassB;
			FP x4 = reference.InvIb;
			int pointCount = reference.PointCount;
			ref Velocity reference2 = ref span2[indexA];
			ref Velocity reference3 = ref span2[indexB];
			FP y = reference2.V.X;
			FP y2 = reference2.V.Y;
			FP x5 = reference2.W;
			FP x6 = reference3.V.X;
			FP x7 = reference3.V.Y;
			FP x8 = reference3.W;
			FP y3 = reference.Normal.X;
			FP y4 = reference.Normal.Y;
			FP y5 = y4;
			FP y6 = -y3;
			FP x9 = reference.Friction;
			for (int j = 0; j < pointCount; j++)
			{
				ref VelocityConstraintPoint reference4 = ref j == 0 ? ref reference.Points.Value0 : ref reference.Points.Value1;
				FP x10 = x6 - x8 * reference4.Rb.Y - y + x5 * reference4.Ra.Y;
				FP x11 = x7 + x8 * reference4.Rb.X - y2 - x5 * reference4.Ra.X;
				FP fP = x10 * y5 + x11 * y6 - reference.TangentSpeed;
				FP y7 = reference4.TangentMass * -fP;
				FP fP2 = x9 * reference4.NormalImpulse;
				FP fP3 = reference4.TangentImpulse + y7;
				fP3 = ((fP3 < -fP2) ? (-fP2) : ((fP3 > fP2) ? fP2 : fP3));
				y7 = fP3 - reference4.TangentImpulse;
				reference4.TangentImpulse = fP3;
				FP y8 = y7 * y5;
				FP y9 = y7 * y6;
				y -= x * y8;
				y2 -= x * y9;
				x5 -= x2 * (reference4.Ra.X * y9 - reference4.Ra.Y * y8);
				x6 += x3 * y8;
				x7 += x3 * y9;
				x8 += x4 * (reference4.Rb.X * y9 - reference4.Rb.Y * y8);
			}
			if (pointCount == 1)
			{
				ref VelocityConstraintPoint value = ref reference.Points.Value0;
				FP x10 = x6 - x8 * value.Rb.Y - y + x5 * value.Ra.Y;
				FP x11 = x7 + x8 * value.Rb.X - y2 - x5 * value.Ra.X;
				FP x12 = x10 * y3 + x11 * y4;
				FP y10 = -value.NormalMass * (x12 - value.VelocityBias);
				FP x13 = FP.Max(value.NormalImpulse + y10, 0f);
				y10 = x13 - value.NormalImpulse;
				value.NormalImpulse = x13;
				FP y8 = y10 * y3;
				FP y9 = y10 * y4;
				y -= x * y8;
				y2 -= x * y9;
				x5 -= x2 * (value.Ra.X * y9 - value.Ra.Y * y8);
				x6 += x3 * y8;
				x7 += x3 * y9;
				x8 += x4 * (value.Rb.X * y9 - value.Rb.Y * y8);
			}
			else
			{
				FP y11 = reference.Points.Value0.VelocityBias;
				FP y12 = reference.Points.Value1.VelocityBias;
				FP normalMass = reference.Points.Value0.NormalMass;
				FP normalMass2 = reference.Points.Value1.NormalMass;
				FP y13 = reference.Points.Value0.Ra.X;
				FP y14 = reference.Points.Value0.Ra.Y;
				FP y15 = reference.Points.Value0.Rb.X;
				FP y16 = reference.Points.Value0.Rb.Y;
				FP y17 = reference.Points.Value1.Ra.X;
				FP y18 = reference.Points.Value1.Ra.Y;
				FP y19 = reference.Points.Value1.Rb.X;
				FP y20 = reference.Points.Value1.Rb.Y;
				ref FP normalImpulse = ref reference.Points.Value0.NormalImpulse;
				ref FP normalImpulse2 = ref reference.Points.Value1.NormalImpulse;
				FVector2 fVector = new FVector2(normalImpulse, normalImpulse2);
				FP x14 = x6 - x8 * y16 - y + x5 * y14;
				FP x15 = x7 + x8 * y15 - y2 - x5 * y13;
				FP x16 = x6 - x8 * y20 - y + x5 * y18;
				FP x17 = x7 + x8 * y19 - y2 - x5 * y17;
				FP x18 = x14 * y3 + x15 * y4;
				FP x19 = x16 * y3 + x17 * y4;
				FVector2 fVector2 = new FVector2(x18 - y11 - (reference.K.Ex.X * fVector.X + reference.K.Ey.X * fVector.Y), x19 - y12 - (reference.K.Ex.Y * fVector.X + reference.K.Ey.Y * fVector.Y));
				FVector2 fVector3 = new FVector2(-(reference.NormalMass.Ex.X * fVector2.X + reference.NormalMass.Ey.X * fVector2.Y), -(reference.NormalMass.Ex.Y * fVector2.X + reference.NormalMass.Ey.Y * fVector2.Y));
				if (fVector3.X >= 0f && fVector3.Y >= 0f)
				{
					FP x20 = fVector3.X - fVector.X;
					FP x21 = fVector3.Y - fVector.Y;
					FP x22 = x20 * y3;
					FP x23 = x20 * y4;
					FP y21 = x21 * y3;
					FP y22 = x21 * y4;
					y -= x * (x22 + y21);
					y2 -= x * (x23 + y22);
					x5 -= x2 * (y13 * x23 - y14 * x22 + (y17 * y22 - y18 * y21));
					x6 += x3 * (x22 + y21);
					x7 += x3 * (x23 + y22);
					x8 += x4 * (y15 * x23 - y16 * x22 + (y19 * y22 - y20 * y21));
					normalImpulse = fVector3.X;
					normalImpulse2 = fVector3.Y;
				}
				else
				{
					fVector3.X = -normalMass * fVector2.X;
					fVector3.Y = 0f;
					x18 = 0f;
					x19 = reference.K.Ex.Y * fVector3.X + fVector2.Y;
					if (fVector3.X >= 0f && x19 >= 0f)
					{
						FP x24 = fVector3.X - fVector.X;
						FP x25 = fVector3.Y - fVector.Y;
						FP x22 = x24 * y3;
						FP x23 = x24 * y4;
						FP y21 = x25 * y3;
						FP y22 = x25 * y4;
						y -= x * (x22 + y21);
						y2 -= x * (x23 + y22);
						x5 -= x2 * (y13 * x23 - y14 * x22 + (y17 * y22 - y18 * y21));
						x6 += x3 * (x22 + y21);
						x7 += x3 * (x23 + y22);
						x8 += x4 * (y15 * x23 - y16 * x22 + (y19 * y22 - y20 * y21));
						normalImpulse = fVector3.X;
						normalImpulse2 = fVector3.Y;
					}
					else
					{
						fVector3.X = 0f;
						fVector3.Y = -normalMass2 * fVector2.Y;
						x18 = reference.K.Ey.X * fVector3.Y + fVector2.X;
						x19 = 0f;
						if (fVector3.Y >= 0f && x18 >= 0f)
						{
							FP x26 = fVector3.X - fVector.X;
							FP x27 = fVector3.Y - fVector.Y;
							FP x22 = x26 * y3;
							FP x23 = x26 * y4;
							FP y21 = x27 * y3;
							FP y22 = x27 * y4;
							y -= x * (x22 + y21);
							y2 -= x * (x23 + y22);
							x5 -= x2 * (y13 * x23 - y14 * x22 + (y17 * y22 - y18 * y21));
							x6 += x3 * (x22 + y21);
							x7 += x3 * (x23 + y22);
							x8 += x4 * (y15 * x23 - y16 * x22 + (y19 * y22 - y20 * y21));
							normalImpulse = fVector3.X;
							normalImpulse2 = fVector3.Y;
						}
						else
						{
							fVector3.X = 0f;
							fVector3.Y = 0f;
							x18 = fVector2.X;
							x19 = fVector2.Y;
							if (x18 >= 0f && x19 >= 0f)
							{
								FP x28 = fVector3.X - fVector.X;
								FP x29 = fVector3.Y - fVector.Y;
								FP x22 = x28 * y3;
								FP x23 = x28 * y4;
								FP y21 = x29 * y3;
								FP y22 = x29 * y4;
								y -= x * (x22 + y21);
								y2 -= x * (x23 + y22);
								x5 -= x2 * (y13 * x23 - y14 * x22 + (y17 * y22 - y18 * y21));
								x6 += x3 * (x22 + y21);
								x7 += x3 * (x23 + y22);
								x8 += x4 * (y15 * x23 - y16 * x22 + (y19 * y22 - y20 * y21));
								normalImpulse = fVector3.X;
								normalImpulse2 = fVector3.Y;
							}
						}
					}
				}
			}
			span2[indexA].V.X = y;
			span2[indexA].V.Y = y2;
			span2[indexA].W = x5;
			span2[indexB].V.X = x6;
			span2[indexB].V.Y = x7;
			span2[indexB].W = x8;
		}
	}

	public void StoreImpulses()
	{
		Span<ContactVelocityConstraint> span = VelocityConstraints;
		Span<Contact> span2 = _contacts;
		for (int i = 0; i < _contactCount; i++)
		{
			ref ContactVelocityConstraint reference = ref span[i];
			ref Manifold manifold = ref span2[reference.ContactIndex].Manifold;
			if (reference.PointCount == 1)
			{
				manifold.Points.Value0.NormalImpulse = reference.Points.Value0.NormalImpulse;
				manifold.Points.Value0.TangentImpulse = reference.Points.Value0.TangentImpulse;
			}
			else if (reference.PointCount == 2)
			{
				manifold.Points.Value0.NormalImpulse = reference.Points.Value0.NormalImpulse;
				manifold.Points.Value0.TangentImpulse = reference.Points.Value0.TangentImpulse;
				manifold.Points.Value1.NormalImpulse = reference.Points.Value1.NormalImpulse;
				manifold.Points.Value1.TangentImpulse = reference.Points.Value1.TangentImpulse;
			}
		}
	}

	public bool SolvePositionConstraints()
	{
		FP fP = FP.Zero;
		Span<ContactPositionConstraint> span = PositionConstraints;
		Span<Position> span2 = _positions;
		for (int i = 0; i < _contactCount; i++)
		{
			ref ContactPositionConstraint reference = ref span[i];
			int indexA = reference.IndexA;
			int indexB = reference.IndexB;
			FVector2 v = reference.LocalCenterA;
			FP x = reference.InvMassA;
			FP x2 = reference.InvIa;
			FVector2 v2 = reference.LocalCenterB;
			FP y = reference.InvMassB;
			FP x3 = reference.InvIb;
			int pointCount = reference.PointCount;
			FVector2 center = span2[indexA].Center;
			FP x4 = span2[indexA].Angle;
			FVector2 center2 = span2[indexB].Center;
			FP x5 = span2[indexB].Angle;
			for (int j = 0; j < pointCount; j++)
			{
				Transform xfA = default(Transform);
				Transform xfB = xfA;
				xfA.Rotation.Set(x4);
				xfB.Rotation.Set(x5);
				xfA.Position = center - MathUtils.Mul(in xfA.Rotation, in v);
				xfB.Position = center2 - MathUtils.Mul(in xfB.Rotation, in v2);
				PositionSolverManifold positionSolverManifold = default(PositionSolverManifold);
				positionSolverManifold.Initialize(in reference, in xfA, in xfB, j);
				FVector2 b = positionSolverManifold.Normal;
				FVector2 point = positionSolverManifold.Point;
				FP x6 = positionSolverManifold.Separation;
				FVector2 a = point - center;
				FVector2 a2 = point - center2;
				fP = FP.Min(fP, x6);
				FP fP2 = MathUtils.Clamp(Settings.Baumgarte * (x6 + Settings.LinearSlop), -Settings.MaxLinearCorrection, 0f);
				FP y2 = MathUtils.Cross(in a, in b);
				FP y3 = MathUtils.Cross(in a2, in b);
				FP fP3 = x + y + x2 * y2 * y2 + x3 * y3 * y3;
				FVector2 b2 = ((fP3 > FP.Zero) ? (-fP2 / fP3) : FP.Zero) * b;
				center -= x * b2;
				x4 -= x2 * MathUtils.Cross(in a, in b2);
				center2 += y * b2;
				x5 += x3 * MathUtils.Cross(in a2, in b2);
			}
			span2[indexA].Center = center;
			span2[indexA].Angle = x4;
			span2[indexB].Center = center2;
			span2[indexB].Angle = x5;
		}
		return fP >= (FP)(-3f) * Settings.LinearSlop;
	}

	public bool SolveTOIPositionConstraints(int toiIndexA, int toiIndexB)
	{
		FP fP = FP.Zero;
		Span<ContactPositionConstraint> span = PositionConstraints;
		Span<Position> span2 = _positions;
		for (int i = 0; i < _contactCount; i++)
		{
			ref ContactPositionConstraint reference = ref span[i];
			int indexA = reference.IndexA;
			int indexB = reference.IndexB;
			FVector2 v = reference.LocalCenterA;
			FVector2 v2 = reference.LocalCenterB;
			int pointCount = reference.PointCount;
			FP x = FP.Zero;
			FP x2 = FP.Zero;
			if (indexA == toiIndexA || indexA == toiIndexB)
			{
				x = reference.InvMassA;
				x2 = reference.InvIa;
			}
			FP y = FP.Zero;
			FP x3 = FP.Zero;
			if (indexB == toiIndexA || indexB == toiIndexB)
			{
				y = reference.InvMassB;
				x3 = reference.InvIb;
			}
			FVector2 center = span2[indexA].Center;
			FP x4 = span2[indexA].Angle;
			FVector2 center2 = span2[indexB].Center;
			FP x5 = span2[indexB].Angle;
			for (int j = 0; j < pointCount; j++)
			{
				Transform xfA = default(Transform);
				Transform xfB = default(Transform);
				xfA.Rotation.Set(x4);
				xfB.Rotation.Set(x5);
				xfA.Position = center - MathUtils.Mul(in xfA.Rotation, in v);
				xfB.Position = center2 - MathUtils.Mul(in xfB.Rotation, in v2);
				PositionSolverManifold positionSolverManifold = default(PositionSolverManifold);
				positionSolverManifold.Initialize(in reference, in xfA, in xfB, j);
				FVector2 b = positionSolverManifold.Normal;
				FVector2 point = positionSolverManifold.Point;
				FP x6 = positionSolverManifold.Separation;
				FVector2 a = point - center;
				FVector2 a2 = point - center2;
				fP = FP.Min(fP, x6);
				FP fP2 = MathUtils.Clamp(Settings.ToiBaumgarte * (x6 + Settings.LinearSlop), -Settings.MaxLinearCorrection, 0f);
				FP y2 = MathUtils.Cross(in a, in b);
				FP y3 = MathUtils.Cross(in a2, in b);
				FP fP3 = x + y + x2 * y2 * y2 + x3 * y3 * y3;
				FVector2 b2 = ((fP3 > FP.Zero) ? (-fP2 / fP3) : FP.Zero) * b;
				center -= x * b2;
				x4 -= x2 * MathUtils.Cross(in a, in b2);
				center2 += y * b2;
				x5 += x3 * MathUtils.Cross(in a2, in b2);
			}
			_positions[indexA].Center = center;
			_positions[indexA].Angle = x4;
			_positions[indexB].Center = center2;
			_positions[indexB].Angle = x5;
		}
		return fP >= (FP)(-1.5f) * Settings.LinearSlop;
	}
}

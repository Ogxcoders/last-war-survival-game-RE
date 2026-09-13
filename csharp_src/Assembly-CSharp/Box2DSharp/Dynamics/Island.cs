using System.Buffers;
using System.Diagnostics;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Dynamics;

public class Island
{
	internal Body[] Bodies;

	internal int BodyCount;

	internal int ContactCount;

	internal IContactListener ContactListener;

	internal Contact[] Contacts;

	internal int JointCount;

	internal Joint[] Joints;

	internal Position[] Positions;

	internal Velocity[] Velocities;

	private readonly ArrayPool<Body> _bodyPool = ArrayPool<Body>.Create();

	private readonly ArrayPool<Contact> _contactPool = ArrayPool<Contact>.Create();

	private readonly ArrayPool<Joint> _jointPool = ArrayPool<Joint>.Create();

	private readonly ArrayPool<Position> _positionPool = ArrayPool<Position>.Create();

	private readonly ArrayPool<Velocity> _velocityPool = ArrayPool<Velocity>.Create();

	private readonly ContactSolver _solveContactSolver = new ContactSolver();

	private readonly Stopwatch _solveTimer = new Stopwatch();

	private static readonly FP linTolSqr = Settings.LinearSleepTolerance * Settings.LinearSleepTolerance;

	private static readonly FP angTolSqr = Settings.AngularSleepTolerance * Settings.AngularSleepTolerance;

	private readonly ContactSolver _solveToiContactSolver = new ContactSolver();

	public void Setup(int bodyCapacity, int contactCapacity, int jointCapacity, IContactListener contactListener)
	{
		BodyCount = 0;
		ContactCount = 0;
		JointCount = 0;
		ContactListener = contactListener;
		Bodies = _bodyPool.Rent(bodyCapacity);
		Contacts = _contactPool.Rent(contactCapacity);
		Joints = _jointPool.Rent(jointCapacity);
		Positions = _positionPool.Rent(bodyCapacity);
		Velocities = _velocityPool.Rent(bodyCapacity);
	}

	internal void Reset()
	{
		BodyCount = 0;
		ContactCount = 0;
		JointCount = 0;
		_bodyPool.Return(Bodies, clearArray: true);
		Bodies = null;
		_contactPool.Return(Contacts, clearArray: true);
		Contacts = null;
		_jointPool.Return(Joints, clearArray: true);
		Joints = null;
		_positionPool.Return(Positions, clearArray: true);
		Positions = null;
		_velocityPool.Return(Velocities, clearArray: true);
		Velocities = null;
	}

	internal void Clear()
	{
		BodyCount = 0;
		ContactCount = 0;
		JointCount = 0;
	}

	internal void Solve(out Profile profile, in TimeStep step, in FVector2 gravity, bool allowSleep)
	{
		profile = default(Profile);
		FP x = step.Dt;
		for (int i = 0; i < BodyCount; i++)
		{
			Body body = Bodies[i];
			FVector2 c = body.Sweep.C;
			FP a = body.Sweep.A;
			FVector2 linearVelocity = body.LinearVelocity;
			FP x2 = body.AngularVelocity;
			body.Sweep.C0 = body.Sweep.C;
			body.Sweep.A0 = body.Sweep.A;
			if (body.BodyType == BodyType.DynamicBody)
			{
				linearVelocity += x * body.InvMass * (body.GravityScale * body.Mass * gravity + body.Force);
				x2 += x * body.InverseInertia * body.Torque;
				linearVelocity *= 1f / ((FP)1f + x * body.LinearDamping);
				x2 *= 1f / ((FP)1f + x * body.AngularDamping);
			}
			Positions[i].Center = c;
			Positions[i].Angle = a;
			Velocities[i].V = linearVelocity;
			Velocities[i].W = x2;
		}
		_solveTimer.Restart();
		SolverData data = new SolverData(in step, Positions, Velocities);
		ContactSolverDef def = new ContactSolverDef(in step, ContactCount, Contacts, Positions, Velocities);
		ContactSolver solveContactSolver = _solveContactSolver;
		solveContactSolver.Setup(in def);
		solveContactSolver.InitializeVelocityConstraints();
		if (step.WarmStarting)
		{
			solveContactSolver.WarmStart();
		}
		for (int j = 0; j < JointCount; j++)
		{
			Joints[j].InitVelocityConstraints(in data);
		}
		profile.SolveInit = _solveTimer.ElapsedMilliseconds;
		_solveTimer.Restart();
		for (int k = 0; k < step.VelocityIterations; k++)
		{
			for (int l = 0; l < JointCount; l++)
			{
				Joints[l].SolveVelocityConstraints(in data);
			}
			solveContactSolver.SolveVelocityConstraints();
		}
		solveContactSolver.StoreImpulses();
		_solveTimer.Stop();
		profile.SolveVelocity = _solveTimer.ElapsedMilliseconds;
		for (int m = 0; m < BodyCount; m++)
		{
			FVector2 center = Positions[m].Center;
			FP x3 = Positions[m].Angle;
			FVector2 v = Velocities[m].V;
			FP y = Velocities[m].W;
			FVector2 fVector = x * v;
			if (FVector2.Dot(fVector, fVector) > Settings.MaxTranslationSquared)
			{
				FP fP = Settings.MaxTranslation / fVector.Length();
				v *= fP;
			}
			FP x4 = x * y;
			if (x4 * x4 > Settings.MaxRotationSquared)
			{
				y *= Settings.MaxRotation / FP.Abs(x4);
			}
			center += x * v;
			x3 += x * y;
			Positions[m].Center = center;
			Positions[m].Angle = x3;
			Velocities[m].V = v;
			Velocities[m].W = y;
		}
		_solveTimer.Restart();
		bool flag = false;
		for (int n = 0; n < step.PositionIterations; n++)
		{
			bool flag2 = solveContactSolver.SolvePositionConstraints();
			bool flag3 = true;
			for (int num = 0; num < JointCount; num++)
			{
				bool flag4 = Joints[num].SolvePositionConstraints(in data);
				flag3 = flag3 && flag4;
			}
			if (flag2 && flag3)
			{
				flag = true;
				break;
			}
		}
		for (int num2 = 0; num2 < BodyCount; num2++)
		{
			Body obj = Bodies[num2];
			obj.Sweep.C = Positions[num2].Center;
			obj.Sweep.A = Positions[num2].Angle;
			obj.LinearVelocity = Velocities[num2].V;
			obj.AngularVelocity = Velocities[num2].W;
			obj.SynchronizeTransform();
		}
		_solveTimer.Stop();
		profile.SolvePosition = _solveTimer.ElapsedMilliseconds;
		Report(solveContactSolver.VelocityConstraints);
		if (allowSleep)
		{
			FP fP2 = Settings.MaxFloat;
			for (int num3 = 0; num3 < BodyCount; num3++)
			{
				Body body2 = Bodies[num3];
				if (body2.BodyType != BodyType.StaticBody)
				{
					if (!body2.Flags.HasSetFlag(BodyFlags.AutoSleep) || body2.AngularVelocity * body2.AngularVelocity > angTolSqr || FVector2.Dot(body2.LinearVelocity, body2.LinearVelocity) > linTolSqr)
					{
						body2.SleepTime = 0f;
						fP2 = 0f;
					}
					else
					{
						body2.SleepTime += x;
						fP2 = FP.Min(fP2, body2.SleepTime);
					}
				}
			}
			if (fP2 >= Settings.TimeToSleep && flag)
			{
				for (int num4 = 0; num4 < BodyCount; num4++)
				{
					Bodies[num4].IsAwake = false;
				}
			}
		}
		solveContactSolver.Reset();
	}

	internal void SolveTOI(in TimeStep subStep, int toiIndexA, int toiIndexB)
	{
		for (int i = 0; i < BodyCount; i++)
		{
			Body body = Bodies[i];
			Positions[i].Center = body.Sweep.C;
			Positions[i].Angle = body.Sweep.A;
			Velocities[i].V = body.LinearVelocity;
			Velocities[i].W = body.AngularVelocity;
		}
		ContactSolverDef def = new ContactSolverDef(in subStep, ContactCount, Contacts, Positions, Velocities);
		ContactSolver solveToiContactSolver = _solveToiContactSolver;
		solveToiContactSolver.Setup(in def);
		for (int j = 0; j < subStep.PositionIterations; j++)
		{
			if (solveToiContactSolver.SolveTOIPositionConstraints(toiIndexA, toiIndexB))
			{
				break;
			}
		}
		Bodies[toiIndexA].Sweep.C0 = Positions[toiIndexA].Center;
		Bodies[toiIndexA].Sweep.A0 = Positions[toiIndexA].Angle;
		Bodies[toiIndexB].Sweep.C0 = Positions[toiIndexB].Center;
		Bodies[toiIndexB].Sweep.A0 = Positions[toiIndexB].Angle;
		solveToiContactSolver.InitializeVelocityConstraints();
		for (int k = 0; k < subStep.VelocityIterations; k++)
		{
			solveToiContactSolver.SolveVelocityConstraints();
		}
		FP x = subStep.Dt;
		for (int l = 0; l < BodyCount; l++)
		{
			FVector2 center = Positions[l].Center;
			FP x2 = Positions[l].Angle;
			FVector2 v = Velocities[l].V;
			FP y = Velocities[l].W;
			FVector2 fVector = x * v;
			if (FVector2.Dot(fVector, fVector) > Settings.MaxTranslationSquared)
			{
				FP fP = Settings.MaxTranslation / fVector.Length();
				v *= fP;
			}
			FP x3 = x * y;
			if (x3 * x3 > Settings.MaxRotationSquared)
			{
				y *= Settings.MaxRotation / FP.Abs(x3);
			}
			center += x * v;
			x2 += x * y;
			Positions[l].Center = center;
			Positions[l].Angle = x2;
			Velocities[l].V = v;
			Velocities[l].W = y;
			Body obj = Bodies[l];
			obj.Sweep.C = center;
			obj.Sweep.A = x2;
			obj.LinearVelocity = v;
			obj.AngularVelocity = y;
			obj.SynchronizeTransform();
		}
		Report(solveToiContactSolver.VelocityConstraints);
		solveToiContactSolver.Reset();
	}

	internal void Add(Body body)
	{
		body.IslandIndex = BodyCount;
		Bodies[BodyCount] = body;
		BodyCount++;
	}

	internal void Add(Contact contact)
	{
		Contacts[ContactCount++] = contact;
	}

	internal void Add(Joint joint)
	{
		Joints[JointCount++] = joint;
	}

	private void Report(ContactVelocityConstraint[] constraints)
	{
		if (ContactListener == null)
		{
			return;
		}
		for (int i = 0; i < ContactCount; i++)
		{
			Contact contact = Contacts[i];
			ContactVelocityConstraint contactVelocityConstraint = constraints[i];
			ContactImpulse impulse = new ContactImpulse
			{
				Count = contactVelocityConstraint.PointCount
			};
			for (int j = 0; j < contactVelocityConstraint.PointCount; j++)
			{
				impulse.NormalImpulses[j] = contactVelocityConstraint.Points[j].NormalImpulse;
				impulse.TangentImpulses[j] = contactVelocityConstraint.Points[j].TangentImpulse;
			}
			ContactListener.PostSolve(contact, in impulse);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Internal;
using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Dynamics;

public class World : IDisposable
{
	private class TreeQueryCallback : ITreeQueryCallback
	{
		public ContactManager ContactManager { get; private set; }

		public IQueryCallback Callback { get; private set; }

		public void Set(ContactManager contactManager, in IQueryCallback callback)
		{
			ContactManager = contactManager;
			Callback = callback;
		}

		public void Reset()
		{
			ContactManager = null;
			Callback = null;
		}

		public bool QueryCallback(int proxyId)
		{
			FixtureProxy fixtureProxy = (FixtureProxy)ContactManager.BroadPhase.GetUserData(proxyId);
			return Callback.QueryCallback(fixtureProxy.Fixture);
		}
	}

	private class InternalRayCastCallback : ITreeRayCastCallback
	{
		public ContactManager ContactManager { get; private set; }

		public IRayCastCallback Callback { get; private set; }

		public void Set(ContactManager contactManager, in IRayCastCallback callback)
		{
			ContactManager = contactManager;
			Callback = callback;
		}

		public void Reset()
		{
			ContactManager = null;
			Callback = null;
		}

		public FP RayCastCallback(in RayCastInput input, int proxyId)
		{
			FixtureProxy obj = (FixtureProxy)ContactManager.BroadPhase.GetUserData(proxyId);
			Fixture fixture = obj.Fixture;
			int childIndex = obj.ChildIndex;
			if (!fixture.RayCast(out var output, in input, childIndex))
			{
				return input.MaxFraction;
			}
			FP y = output.Fraction;
			FVector2 point = ((FP)1f - y) * input.P1 + y * input.P2;
			return Callback.RayCastCallback(fixture, in point, in output.Normal, y);
		}

		FP ITreeRayCastCallback.RayCastCallback(in RayCastInput input, int proxyId)
		{
			return RayCastCallback(in input, proxyId);
		}
	}

	private FP _invDt0;

	private bool _stepComplete;

	public bool HasNewContacts;

	private bool _allowSleep;

	public Profile Profile;

	private const int DisposedFalse = 0;

	private const int DisposedTrue = 1;

	private int _disposed;

	private readonly Stopwatch _stepTimer = new Stopwatch();

	private readonly Stopwatch _timer = new Stopwatch();

	private readonly TreeQueryCallback _treeQueryCallback = new TreeQueryCallback();

	private readonly InternalRayCastCallback _rayCastCallback = new InternalRayCastCallback();

	private readonly Stack<Body> _solveStack = new Stack<Body>(256);

	private readonly Stopwatch _solveTimer = new Stopwatch();

	private readonly Island _solveIsland = new Island();

	private readonly Island _solveToiIsland = new Island();

	public IDestructionListener DestructionListener { get; set; }

	public IDraw Draw { get; set; }

	public bool ContinuousPhysics { get; set; }

	public FVector2 Gravity { get; set; }

	public bool IsAutoClearForces { get; set; }

	public bool IsLocked { get; private set; }

	public bool AllowSleep
	{
		get
		{
			return _allowSleep;
		}
		set
		{
			if (_allowSleep == value)
			{
				return;
			}
			_allowSleep = value;
			if (!_allowSleep)
			{
				for (LinkedListNode<Body> linkedListNode = BodyList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Value.IsAwake = true;
				}
			}
		}
	}

	public bool SubStepping { get; set; }

	public bool WarmStarting { get; set; }

	public ToiProfile ToiProfile { get; set; }

	public GJkProfile GJkProfile { get; set; }

	public ContactManager ContactManager { get; private set; } = new ContactManager();

	public LinkedList<Body> BodyList { get; private set; } = new LinkedList<Body>();

	public LinkedList<Joint> JointList { get; private set; } = new LinkedList<Joint>();

	public int ProxyCount => ContactManager.BroadPhase.GetProxyCount();

	public int BodyCount => BodyList.Count;

	public int JointCount => JointList.Count;

	public int ContactCount => ContactManager.ContactList.Count;

	public int TreeHeight => ContactManager.BroadPhase.GetTreeHeight();

	public int TreeBalance => ContactManager.BroadPhase.GetTreeBalance();

	public FP TreeQuality => ContactManager.BroadPhase.GetTreeQuality();

	public World()
		: this(new FVector2(0, -10))
	{
	}

	public World(in FVector2 gravity)
	{
		Gravity = gravity;
		WarmStarting = true;
		ContinuousPhysics = true;
		SubStepping = false;
		_stepComplete = true;
		AllowSleep = true;
		IsAutoClearForces = true;
		_invDt0 = 0f;
		Profile = default(Profile);
	}

	~World()
	{
		Dispose();
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref _disposed, 1) != 1)
		{
			BodyList?.Clear();
			BodyList = null;
			JointList?.Clear();
			JointList = null;
			ContactManager?.Dispose();
			ContactManager = null;
			DestructionListener = null;
			Draw = null;
			Profile = default(Profile);
			ToiProfile = null;
			GJkProfile = null;
		}
	}

	public void SetContactFilter(IContactFilter filter)
	{
		ContactManager.ContactFilter = filter;
	}

	public void SetContactListener(IContactListener listener)
	{
		ContactManager.ContactListener = listener;
	}

	public Body CreateBody(in BodyDef def)
	{
		if (IsLocked)
		{
			return null;
		}
		Body body = new Body(in def, this);
		body.Node = BodyList.AddFirst(body);
		return body;
	}

	public bool DestroyBody(Body body)
	{
		if (IsLocked)
		{
			return false;
		}
		LinkedListNode<JointEdge> linkedListNode = body.JointEdges.First;
		while (linkedListNode != null)
		{
			LinkedListNode<JointEdge> next = linkedListNode.Next;
			DestructionListener?.SayGoodbye(linkedListNode.Value.Joint);
			DestroyJoint(linkedListNode.Value.Joint);
			linkedListNode = next;
		}
		LinkedListNode<ContactEdge> linkedListNode2 = body.ContactEdges.First;
		while (linkedListNode2 != null)
		{
			LinkedListNode<ContactEdge> next2 = linkedListNode2.Next;
			ContactManager.Destroy(linkedListNode2.Value.Contact);
			linkedListNode2 = next2;
		}
		foreach (Fixture fixture in body.Fixtures)
		{
			DestructionListener?.SayGoodbye(fixture);
			fixture.DestroyProxies(ContactManager.BroadPhase);
		}
		BodyList.Remove(body.Node);
		body.Dispose();
		return true;
	}

	public Joint CreateJoint(JointDef def)
	{
		if (IsLocked)
		{
			return null;
		}
		Joint joint = Joint.Create(def);
		joint.Node = JointList.AddFirst(joint);
		joint.EdgeA.Joint = joint;
		joint.EdgeA.Other = joint.BodyB;
		joint.EdgeA.Node = joint.BodyA.JointEdges.AddFirst(joint.EdgeA);
		joint.EdgeB.Joint = joint;
		joint.EdgeB.Other = joint.BodyA;
		joint.EdgeB.Node = joint.BodyB.JointEdges.AddFirst(joint.EdgeB);
		Body bodyA = def.BodyA;
		Body bodyB = def.BodyB;
		if (!def.CollideConnected)
		{
			LinkedListNode<ContactEdge> linkedListNode = bodyB.ContactEdges.First;
			while (linkedListNode != null)
			{
				ContactEdge value = linkedListNode.Value;
				linkedListNode = linkedListNode.Next;
				if (value.Other == bodyA)
				{
					value.Contact.FlagForFiltering();
				}
			}
		}
		return joint;
	}

	public void DestroyJoint(Joint joint)
	{
		if (IsLocked)
		{
			return;
		}
		bool collideConnected = joint.CollideConnected;
		JointList.Remove(joint.Node);
		Body bodyA = joint.BodyA;
		bodyA.IsAwake = true;
		bodyA.JointEdges.Remove(joint.EdgeA.Node);
		joint.EdgeA.Dispose();
		Body bodyB = joint.BodyB;
		bodyB.IsAwake = true;
		bodyB.JointEdges.Remove(joint.EdgeB.Node);
		joint.EdgeB.Dispose();
		if (collideConnected)
		{
			return;
		}
		LinkedListNode<ContactEdge> linkedListNode = bodyB.ContactEdges.First;
		while (linkedListNode != null)
		{
			ContactEdge value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if (value.Other == bodyA)
			{
				value.Contact.FlagForFiltering();
			}
		}
	}

	public void Step(FP timeStep, int velocityIterations, int positionIterations)
	{
		_stepTimer.Restart();
		if (HasNewContacts)
		{
			ContactManager.FindNewContacts();
			HasNewContacts = false;
		}
		IsLocked = true;
		TimeStep step = new TimeStep
		{
			Dt = timeStep,
			VelocityIterations = velocityIterations,
			PositionIterations = positionIterations
		};
		if (timeStep > 0f)
		{
			step.InvDt = 1f / timeStep;
		}
		else
		{
			step.InvDt = 0f;
		}
		step.DtRatio = _invDt0 * timeStep;
		step.WarmStarting = WarmStarting;
		_timer.Restart();
		ContactManager.Collide();
		_timer.Stop();
		Profile.Collide = _timer.ElapsedMilliseconds;
		if (_stepComplete && step.Dt > 0f)
		{
			_timer.Restart();
			Solve(in step);
			_timer.Stop();
			Profile.Solve = _timer.ElapsedMilliseconds;
		}
		if (ContinuousPhysics && step.Dt > 0f)
		{
			_timer.Restart();
			SolveTOI(in step);
			_timer.Stop();
			Profile.SolveTOI = _timer.ElapsedMilliseconds;
		}
		if (step.Dt > 0f)
		{
			_invDt0 = step.InvDt;
		}
		if (IsAutoClearForces)
		{
			ClearForces();
		}
		IsLocked = false;
		_stepTimer.Stop();
		Profile.Step = _stepTimer.ElapsedMilliseconds;
	}

	public void ClearForces()
	{
		LinkedListNode<Body> linkedListNode = BodyList.First;
		while (linkedListNode != null)
		{
			Body value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			value.Force.SetZero();
			value.Torque = 0f;
		}
	}

	public void QueryAABB(in IQueryCallback callback, in Box2DSharp.Collision.AABB aabb)
	{
		_treeQueryCallback.Set(ContactManager, in callback);
		BroadPhase broadPhase = ContactManager.BroadPhase;
		ITreeQueryCallback callback2 = _treeQueryCallback;
		broadPhase.Query(in callback2, in aabb);
	}

	public void RayCast(in IRayCastCallback callback, in FVector2 point1, in FVector2 point2)
	{
		RayCastInput input = new RayCastInput
		{
			MaxFraction = 1f,
			P1 = point1,
			P2 = point2
		};
		_rayCastCallback.Set(ContactManager, in callback);
		BroadPhase broadPhase = ContactManager.BroadPhase;
		ITreeRayCastCallback callback2 = _rayCastCallback;
		broadPhase.RayCast(in callback2, in input);
		_rayCastCallback.Reset();
	}

	public void ShiftOrigin(in FVector2 newOrigin)
	{
		if (!IsLocked)
		{
			LinkedListNode<Body> linkedListNode = BodyList.First;
			while (linkedListNode != null)
			{
				Body value = linkedListNode.Value;
				linkedListNode = linkedListNode.Next;
				value.Transform.Position -= newOrigin;
				value.Sweep.C0 -= newOrigin;
				value.Sweep.C -= newOrigin;
			}
			for (LinkedListNode<Joint> linkedListNode2 = JointList.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
			{
				linkedListNode2.Value.ShiftOrigin(in newOrigin);
			}
			ContactManager.BroadPhase.ShiftOrigin(in newOrigin);
		}
	}

	private void Solve(in TimeStep step)
	{
		Profile.SolveInit = 0f;
		Profile.SolveVelocity = 0f;
		Profile.SolvePosition = 0f;
		Island solveIsland = _solveIsland;
		solveIsland.Setup(BodyList.Count, ContactManager.ContactList.Count, JointList.Count, ContactManager.ContactListener);
		LinkedListNode<Body> linkedListNode;
		for (linkedListNode = BodyList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.UnsetFlag(BodyFlags.Island);
		}
		for (LinkedListNode<Contact> linkedListNode2 = ContactManager.ContactList.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
		{
			linkedListNode2.Value.Flags &= ~Contact.ContactFlag.IslandFlag;
		}
		for (LinkedListNode<Joint> linkedListNode3 = JointList.First; linkedListNode3 != null; linkedListNode3 = linkedListNode3.Next)
		{
			linkedListNode3.Value.IslandFlag = false;
		}
		Stack<Body> solveStack = _solveStack;
		solveStack.Clear();
		linkedListNode = BodyList.First;
		while (linkedListNode != null)
		{
			Body value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if (value.Flags.HasSetFlag(BodyFlags.Island) || !value.IsAwake || !value.IsEnabled || value.BodyType == BodyType.StaticBody)
			{
				continue;
			}
			solveIsland.Clear();
			solveStack.Push(value);
			value.SetFlag(BodyFlags.Island);
			while (solveStack.Count > 0)
			{
				Body body = solveStack.Pop();
				solveIsland.Add(body);
				if (body.BodyType == BodyType.StaticBody)
				{
					continue;
				}
				body.SetFlag(BodyFlags.IsAwake);
				LinkedListNode<ContactEdge> linkedListNode4 = body.ContactEdges.First;
				while (linkedListNode4 != null)
				{
					ContactEdge value2 = linkedListNode4.Value;
					linkedListNode4 = linkedListNode4.Next;
					Contact contact = value2.Contact;
					if (!contact.Flags.HasSetFlag(Contact.ContactFlag.IslandFlag) && contact.IsEnabled && contact.IsTouching && !contact.FixtureA.IsSensor && !contact.FixtureB.IsSensor)
					{
						solveIsland.Add(contact);
						contact.Flags |= Contact.ContactFlag.IslandFlag;
						Body other = value2.Other;
						if (!other.Flags.HasSetFlag(BodyFlags.Island))
						{
							solveStack.Push(other);
							other.SetFlag(BodyFlags.Island);
						}
					}
				}
				LinkedListNode<JointEdge> linkedListNode5 = body.JointEdges.First;
				while (linkedListNode5 != null)
				{
					JointEdge value3 = linkedListNode5.Value;
					linkedListNode5 = linkedListNode5.Next;
					if (value3.Joint.IslandFlag)
					{
						continue;
					}
					Body other2 = value3.Other;
					if (other2.IsEnabled)
					{
						solveIsland.Add(value3.Joint);
						value3.Joint.IslandFlag = true;
						if (!other2.Flags.HasSetFlag(BodyFlags.Island))
						{
							solveStack.Push(other2);
							other2.SetFlag(BodyFlags.Island);
						}
					}
				}
			}
			solveIsland.Solve(out var profile, in step, Gravity, AllowSleep);
			Profile.SolveInit += profile.SolveInit;
			Profile.SolveVelocity += profile.SolveVelocity;
			Profile.SolvePosition += profile.SolvePosition;
			for (int i = 0; i < solveIsland.BodyCount; i++)
			{
				Body body2 = solveIsland.Bodies[i];
				if (body2.BodyType == BodyType.StaticBody)
				{
					body2.UnsetFlag(BodyFlags.Island);
				}
			}
		}
		_solveTimer.Restart();
		linkedListNode = BodyList.First;
		while (linkedListNode != null)
		{
			Body value4 = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if (value4.Flags.HasSetFlag(BodyFlags.Island) && value4.BodyType != BodyType.StaticBody)
			{
				value4.SynchronizeFixtures();
			}
		}
		ContactManager.FindNewContacts();
		_solveTimer.Stop();
		Profile.Broadphase = _solveTimer.ElapsedMilliseconds;
		solveIsland.Reset();
	}

	private void SolveTOI(in TimeStep step)
	{
		Island solveToiIsland = _solveToiIsland;
		solveToiIsland.Setup(64, 32, 0, ContactManager.ContactListener);
		if (_stepComplete)
		{
			LinkedListNode<Body> linkedListNode = BodyList.First;
			while (linkedListNode != null)
			{
				Body value = linkedListNode.Value;
				linkedListNode = linkedListNode.Next;
				value.UnsetFlag(BodyFlags.Island);
				value.Sweep.Alpha0 = 0f;
			}
			for (LinkedListNode<Contact> linkedListNode2 = ContactManager.ContactList.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
			{
				Contact value2 = linkedListNode2.Value;
				value2.Flags &= ~(Contact.ContactFlag.IslandFlag | Contact.ContactFlag.ToiFlag);
				value2.ToiCount = 0;
				value2.Toi = 1f;
			}
		}
		while (true)
		{
			Contact contact = null;
			FP y = FP.One;
			LinkedListNode<Contact> linkedListNode3 = ContactManager.ContactList.First;
			while (linkedListNode3 != null)
			{
				Contact value3 = linkedListNode3.Value;
				linkedListNode3 = linkedListNode3.Next;
				if (!value3.IsEnabled || value3.ToiCount > 8)
				{
					continue;
				}
				FP one = FP.One;
				if (value3.Flags.HasSetFlag(Contact.ContactFlag.ToiFlag))
				{
					one = value3.Toi;
				}
				else
				{
					Fixture fixtureA = value3.FixtureA;
					Fixture fixtureB = value3.FixtureB;
					if (fixtureA.IsSensor || fixtureB.IsSensor)
					{
						continue;
					}
					Body body = fixtureA.Body;
					Body body2 = fixtureB.Body;
					BodyType bodyType = body.BodyType;
					BodyType bodyType2 = body2.BodyType;
					bool num = body.IsAwake && bodyType != BodyType.StaticBody;
					bool flag = body2.IsAwake && bodyType2 != BodyType.StaticBody;
					if (!num && !flag)
					{
						continue;
					}
					bool num2 = body.IsBullet || bodyType != BodyType.DynamicBody;
					bool flag2 = body2.IsBullet || bodyType2 != BodyType.DynamicBody;
					if (!num2 && !flag2)
					{
						continue;
					}
					FP x = body.Sweep.Alpha0;
					if (body.Sweep.Alpha0 < body2.Sweep.Alpha0)
					{
						x = body2.Sweep.Alpha0;
						body.Sweep.Advance(x);
					}
					else if (body2.Sweep.Alpha0 < body.Sweep.Alpha0)
					{
						x = body.Sweep.Alpha0;
						body2.Sweep.Advance(x);
					}
					int childIndexA = value3.ChildIndexA;
					int childIndexB = value3.ChildIndexB;
					ToiInput input = default(ToiInput);
					input.ProxyA.Set(fixtureA.Shape, childIndexA);
					input.ProxyB.Set(fixtureB.Shape, childIndexB);
					input.SweepA = body.Sweep;
					input.SweepB = body2.Sweep;
					input.Tmax = 1f;
					TimeOfImpact.ComputeTimeOfImpact(out var output, in input, ToiProfile, GJkProfile);
					FP y2 = output.Time;
					one = (value3.Toi = ((output.State == ToiOutput.ToiState.Touching) ? FP.Min(x + (FP.One - x) * y2, FP.One) : FP.One));
					value3.Flags |= Contact.ContactFlag.ToiFlag;
				}
				if (one < y)
				{
					contact = value3;
					y = one;
				}
			}
			if (contact == null || (FP)1f - (FP)10f * Settings.Epsilon < y)
			{
				_stepComplete = true;
				break;
			}
			Fixture fixtureA2 = contact.FixtureA;
			Fixture fixtureB2 = contact.FixtureB;
			Body body3 = fixtureA2.Body;
			Body body4 = fixtureB2.Body;
			Sweep sweep = body3.Sweep;
			Sweep sweep2 = body4.Sweep;
			body3.Advance(y);
			body4.Advance(y);
			contact.Update(ContactManager.ContactListener);
			contact.Flags &= ~Contact.ContactFlag.ToiFlag;
			contact.ToiCount++;
			if (!contact.IsEnabled || !contact.IsTouching)
			{
				contact.SetEnabled(flag: false);
				body3.Sweep = sweep;
				body4.Sweep = sweep2;
				body3.SynchronizeTransform();
				body4.SynchronizeTransform();
				continue;
			}
			body3.IsAwake = true;
			body4.IsAwake = true;
			solveToiIsland.Clear();
			solveToiIsland.Add(body3);
			solveToiIsland.Add(body4);
			solveToiIsland.Add(contact);
			body3.SetFlag(BodyFlags.Island);
			body4.SetFlag(BodyFlags.Island);
			contact.Flags |= Contact.ContactFlag.IslandFlag;
			Body body5 = body3;
			if (body5.BodyType == BodyType.DynamicBody)
			{
				LinkedListNode<ContactEdge> linkedListNode4 = body5.ContactEdges.First;
				while (linkedListNode4 != null)
				{
					ContactEdge value4 = linkedListNode4.Value;
					linkedListNode4 = linkedListNode4.Next;
					if (solveToiIsland.BodyCount == solveToiIsland.Bodies.Length || solveToiIsland.ContactCount == solveToiIsland.Contacts.Length)
					{
						break;
					}
					Contact contact2 = value4.Contact;
					if (contact2.Flags.HasSetFlag(Contact.ContactFlag.IslandFlag))
					{
						continue;
					}
					Body other = value4.Other;
					if (other.BodyType == BodyType.DynamicBody && !body5.IsBullet && !other.IsBullet)
					{
						continue;
					}
					bool isSensor = contact2.FixtureA.IsSensor;
					bool isSensor2 = contact2.FixtureB.IsSensor;
					if (isSensor || isSensor2)
					{
						continue;
					}
					Sweep sweep3 = other.Sweep;
					if (!other.Flags.HasSetFlag(BodyFlags.Island))
					{
						other.Advance(y);
					}
					contact2.Update(ContactManager.ContactListener);
					if (!contact2.IsEnabled)
					{
						other.Sweep = sweep3;
						other.SynchronizeTransform();
						continue;
					}
					if (!contact2.IsTouching)
					{
						other.Sweep = sweep3;
						other.SynchronizeTransform();
						continue;
					}
					contact2.Flags |= Contact.ContactFlag.IslandFlag;
					solveToiIsland.Add(contact2);
					if (!other.Flags.HasSetFlag(BodyFlags.Island))
					{
						other.SetFlag(BodyFlags.Island);
						if (other.BodyType != BodyType.StaticBody)
						{
							other.IsAwake = true;
						}
						solveToiIsland.Add(other);
					}
				}
			}
			Body body6 = body4;
			if (body6.BodyType == BodyType.DynamicBody)
			{
				LinkedListNode<ContactEdge> linkedListNode5 = body6.ContactEdges.First;
				while (linkedListNode5 != null)
				{
					ContactEdge value5 = linkedListNode5.Value;
					linkedListNode5 = linkedListNode5.Next;
					if (solveToiIsland.BodyCount == solveToiIsland.Bodies.Length || solveToiIsland.ContactCount == solveToiIsland.Contacts.Length)
					{
						break;
					}
					Contact contact3 = value5.Contact;
					if (contact3.Flags.HasSetFlag(Contact.ContactFlag.IslandFlag))
					{
						continue;
					}
					Body other2 = value5.Other;
					if (other2.BodyType == BodyType.DynamicBody && !body6.IsBullet && !other2.IsBullet)
					{
						continue;
					}
					bool isSensor3 = contact3.FixtureA.IsSensor;
					bool isSensor4 = contact3.FixtureB.IsSensor;
					if (isSensor3 || isSensor4)
					{
						continue;
					}
					Sweep sweep4 = other2.Sweep;
					if (!other2.Flags.HasSetFlag(BodyFlags.Island))
					{
						other2.Advance(y);
					}
					contact3.Update(ContactManager.ContactListener);
					if (!contact3.IsEnabled)
					{
						other2.Sweep = sweep4;
						other2.SynchronizeTransform();
						continue;
					}
					if (!contact3.IsTouching)
					{
						other2.Sweep = sweep4;
						other2.SynchronizeTransform();
						continue;
					}
					contact3.Flags |= Contact.ContactFlag.IslandFlag;
					solveToiIsland.Add(contact3);
					if (!other2.Flags.HasSetFlag(BodyFlags.Island))
					{
						other2.SetFlag(BodyFlags.Island);
						if (other2.BodyType != BodyType.StaticBody)
						{
							other2.IsAwake = true;
						}
						solveToiIsland.Add(other2);
					}
				}
			}
			FP fP = ((FP)1f - y) * step.Dt;
			solveToiIsland.SolveTOI(new TimeStep
			{
				Dt = fP,
				InvDt = 1f / fP,
				DtRatio = 1f,
				PositionIterations = 20,
				VelocityIterations = step.VelocityIterations,
				WarmStarting = false
			}, body3.IslandIndex, body4.IslandIndex);
			for (int i = 0; i < solveToiIsland.BodyCount; i++)
			{
				Body body7 = solveToiIsland.Bodies[i];
				body7.UnsetFlag(BodyFlags.Island);
				if (body7.BodyType == BodyType.DynamicBody)
				{
					body7.SynchronizeFixtures();
					for (LinkedListNode<ContactEdge> linkedListNode6 = body4.ContactEdges.First; linkedListNode6 != null; linkedListNode6 = linkedListNode6.Next)
					{
						linkedListNode6.Value.Contact.Flags &= ~(Contact.ContactFlag.IslandFlag | Contact.ContactFlag.ToiFlag);
					}
				}
			}
			ContactManager.FindNewContacts();
			if (!SubStepping)
			{
				continue;
			}
			_stepComplete = false;
			break;
		}
		solveToiIsland.Reset();
	}

	public void Dump()
	{
		if (IsLocked)
		{
			return;
		}
		DumpLogger.Log($"gravity = ({Gravity.X}, {Gravity.Y});");
		DumpLogger.Log($"bodies  = {BodyList.Count};");
		DumpLogger.Log($"joints  = {JointList.Count};");
		int num = 0;
		foreach (Body body in BodyList)
		{
			body.IslandIndex = num;
			body.Dump();
			num++;
		}
		num = 0;
		foreach (Joint joint in JointList)
		{
			joint.Index = num;
			num++;
		}
		foreach (Joint joint2 in JointList)
		{
			if (joint2.JointType != JointType.GearJoint)
			{
				DumpLogger.Log("{");
				joint2.Dump();
				DumpLogger.Log("}");
			}
		}
		foreach (Joint joint3 in JointList)
		{
			if (joint3.JointType == JointType.GearJoint)
			{
				DumpLogger.Log("{");
				joint3.Dump();
				DumpLogger.Log("}");
			}
		}
	}

	public void SetDebugDraw(IDraw draw)
	{
		Draw = draw;
	}

	public void DebugDraw()
	{
		if (Draw == null)
		{
			return;
		}
		Color color = Color.FromArgb(128, 128, 77);
		Color color2 = Color.FromArgb(127, 230, 127);
		Color color3 = Color.FromArgb(127, 127, 230);
		Color color4 = Color.FromArgb(153, 153, 153);
		Color color5 = Color.FromArgb(230, 179, 179);
		DrawFlag flags = Draw.Flags;
		if (flags.HasSetFlag(DrawFlag.DrawShape))
		{
			for (LinkedListNode<Body> linkedListNode = BodyList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				Body value = linkedListNode.Value;
				Transform xf = value.GetTransform();
				bool isEnabled = value.IsEnabled;
				bool isAwake = value.IsAwake;
				foreach (Fixture fixture in value.Fixtures)
				{
					if (value.BodyType == BodyType.DynamicBody && value.Mass.Equals(0))
					{
						DrawShape(fixture, in xf, Color.FromArgb(1f, 0f, 0f));
					}
					else if (!isEnabled)
					{
						DrawShape(fixture, in xf, in color);
					}
					else if (value.BodyType == BodyType.StaticBody)
					{
						DrawShape(fixture, in xf, in color2);
					}
					else if (value.BodyType == BodyType.KinematicBody)
					{
						DrawShape(fixture, in xf, in color3);
					}
					else if (!isAwake)
					{
						DrawShape(fixture, in xf, in color4);
					}
					else
					{
						DrawShape(fixture, in xf, in color5);
					}
				}
			}
		}
		if (flags.HasSetFlag(DrawFlag.DrawJoint))
		{
			for (LinkedListNode<Joint> linkedListNode2 = JointList.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
			{
				linkedListNode2.Value.Draw(Draw);
			}
		}
		if (flags.HasSetFlag(DrawFlag.DrawPair))
		{
			Color color6 = Color.FromArgb(77, 230, 230);
			for (LinkedListNode<Contact> linkedListNode3 = ContactManager.ContactList.First; linkedListNode3 != null; linkedListNode3 = linkedListNode3.Next)
			{
				Contact value2 = linkedListNode3.Value;
				Fixture fixtureA = value2.FixtureA;
				Fixture fixtureB = value2.FixtureB;
				FVector2 p = fixtureA.GetAABB(value2.ChildIndexA).GetCenter();
				FVector2 p2 = fixtureB.GetAABB(value2.ChildIndexB).GetCenter();
				Draw.DrawSegment(in p, in p2, in color6);
			}
		}
		if (flags.HasSetFlag(DrawFlag.DrawAABB))
		{
			Color color7 = Color.FromArgb(230, 77, 230);
			BroadPhase broadPhase = ContactManager.BroadPhase;
			LinkedListNode<Body> linkedListNode4 = BodyList.First;
			Span<FVector2> vertices = stackalloc FVector2[4];
			while (linkedListNode4 != null)
			{
				Body value3 = linkedListNode4.Value;
				linkedListNode4 = linkedListNode4.Next;
				if (!value3.IsEnabled)
				{
					continue;
				}
				foreach (Fixture fixture2 in value3.Fixtures)
				{
					FixtureProxy[] proxies = fixture2.Proxies;
					foreach (FixtureProxy fixtureProxy in proxies)
					{
						Box2DSharp.Collision.AABB fatAABB = broadPhase.GetFatAABB(fixtureProxy.ProxyId);
						vertices[0] = new FVector2(fatAABB.LowerBound.X, fatAABB.LowerBound.Y);
						vertices[1] = new FVector2(fatAABB.UpperBound.X, fatAABB.LowerBound.Y);
						vertices[2] = new FVector2(fatAABB.UpperBound.X, fatAABB.UpperBound.Y);
						vertices[3] = new FVector2(fatAABB.LowerBound.X, fatAABB.UpperBound.Y);
						Draw.DrawPolygon(vertices, 4, in color7);
					}
				}
			}
		}
		if (flags.HasSetFlag(DrawFlag.DrawCenterOfMass))
		{
			LinkedListNode<Body> linkedListNode5 = BodyList.First;
			while (linkedListNode5 != null)
			{
				Body value4 = linkedListNode5.Value;
				linkedListNode5 = linkedListNode5.Next;
				Transform xf2 = value4.GetTransform();
				xf2.Position = value4.GetWorldCenter();
				Draw.DrawTransform(in xf2);
			}
		}
	}

	private void DrawShape(Fixture fixture, in Transform xf, in Color color)
	{
		Shape shape = fixture.Shape;
		if (shape == null)
		{
			return;
		}
		if (!(shape is CircleShape circleShape))
		{
			if (!(shape is EdgeShape edgeShape))
			{
				if (!(shape is ChainShape chainShape))
				{
					if (shape is PolygonShape polygonShape)
					{
						PolygonShape polygonShape2 = polygonShape;
						int count = polygonShape2.Count;
						Span<FVector2> vertices = stackalloc FVector2[count];
						for (int i = 0; i < count; i++)
						{
							vertices[i] = MathUtils.Mul(in xf, in polygonShape2.Vertices[i]);
						}
						Draw.DrawSolidPolygon(vertices, count, in color);
					}
				}
				else
				{
					int count2 = chainShape.Count;
					FVector2[] vertices2 = chainShape.Vertices;
					FVector2 p = MathUtils.Mul(in xf, in vertices2[0]);
					for (int j = 1; j < count2; j++)
					{
						FVector2 p2 = MathUtils.Mul(in xf, in vertices2[j]);
						Draw.DrawSegment(in p, in p2, in color);
						p = p2;
					}
				}
			}
			else
			{
				EdgeShape edgeShape2 = edgeShape;
				FVector2 p3 = MathUtils.Mul(in xf, in edgeShape2.Vertex1);
				FVector2 p4 = MathUtils.Mul(in xf, in edgeShape2.Vertex2);
				Draw.DrawSegment(in p3, in p4, in color);
				if (!edgeShape2.OneSided)
				{
					Draw.DrawPoint(in p3, 4f, in color);
					Draw.DrawPoint(in p4, 4f, in color);
				}
			}
		}
		else
		{
			CircleShape circleShape2 = circleShape;
			FVector2 center = MathUtils.Mul(in xf, in circleShape2.Position);
			FP radius = circleShape2.Radius;
			FVector2 axis = MathUtils.Mul(in xf.Rotation, new FVector2(1f, 0f));
			Draw.DrawSolidCircle(in center, radius, in axis, in color);
		}
	}
}

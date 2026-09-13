using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Joints;
using Box2DSharp.Foreign;

namespace Box2DSharp.Dynamics;

public class Body : IDisposable
{
	internal readonly LinkedList<ContactEdge> ContactEdges;

	internal readonly List<Fixture> Fixtures;

	internal readonly LinkedList<JointEdge> JointEdges;

	private FP _angularDamping;

	private FP _inertia;

	private FP _linearDamping;

	private FP _mass;

	private BodyType _type;

	internal World _world;

	internal BodyFlags Flags;

	internal FVector2 Force;

	internal FP GravityScale;

	internal FP InverseInertia;

	internal FP InvMass;

	internal int IslandIndex;

	internal LinkedListNode<Body> Node;

	internal Sweep Sweep;

	internal FP Torque;

	internal Transform Transform;

	public IReadOnlyList<Fixture> FixtureList => Fixtures;

	public FP Gravity => GravityScale;

	public FP AngularDamping
	{
		get
		{
			return _angularDamping;
		}
		set
		{
			_angularDamping = value;
		}
	}

	public FP AngularVelocity { get; internal set; }

	public FP Inertia => _inertia + _mass * FVector2.Dot(Sweep.LocalCenter, Sweep.LocalCenter);

	public FP LinearDamping
	{
		get
		{
			return _linearDamping;
		}
		set
		{
			_linearDamping = value;
		}
	}

	public FVector2 LinearVelocity { get; internal set; }

	public FP Mass => _mass;

	internal FP SleepTime { get; set; }

	public BodyType BodyType
	{
		get
		{
			return _type;
		}
		set
		{
			if (_world.IsLocked || _type == value)
			{
				return;
			}
			_type = value;
			ResetMassData();
			if (_type == BodyType.StaticBody)
			{
				LinearVelocity = FVector2.Zero;
				AngularVelocity = 0f;
				Sweep.A0 = Sweep.A;
				Sweep.C0 = Sweep.C;
				UnsetFlag(BodyFlags.IsAwake);
				SynchronizeFixtures();
			}
			IsAwake = true;
			Force.SetZero();
			Torque = 0f;
			LinkedListNode<ContactEdge> linkedListNode = ContactEdges.First;
			while (linkedListNode != null)
			{
				ContactEdge value2 = linkedListNode.Value;
				linkedListNode = linkedListNode.Next;
				_world.ContactManager.Destroy(value2.Contact);
			}
			ContactEdges.Clear();
			BroadPhase broadPhase = _world.ContactManager.BroadPhase;
			foreach (Fixture fixture in Fixtures)
			{
				int proxyCount = fixture.ProxyCount;
				for (int i = 0; i < proxyCount; i++)
				{
					broadPhase.TouchProxy(fixture.Proxies[i].ProxyId);
				}
			}
		}
	}

	public bool IsBullet
	{
		get
		{
			return Flags.HasSetFlag(BodyFlags.IsBullet);
		}
		set
		{
			if (value)
			{
				Flags |= BodyFlags.IsBullet;
			}
			else
			{
				Flags &= ~BodyFlags.IsBullet;
			}
		}
	}

	public bool IsSleepingAllowed
	{
		get
		{
			return Flags.HasSetFlag(BodyFlags.AutoSleep);
		}
		set
		{
			if (value)
			{
				Flags |= BodyFlags.AutoSleep;
				return;
			}
			Flags &= ~BodyFlags.AutoSleep;
			IsAwake = true;
		}
	}

	public bool IsAwake
	{
		get
		{
			return Flags.HasSetFlag(BodyFlags.IsAwake);
		}
		set
		{
			if (BodyType != BodyType.StaticBody)
			{
				if (value)
				{
					Flags |= BodyFlags.IsAwake;
					SleepTime = 0f;
					return;
				}
				Flags &= ~BodyFlags.IsAwake;
				SleepTime = 0f;
				LinearVelocity = FVector2.Zero;
				AngularVelocity = 0f;
				Force.SetZero();
				Torque = 0f;
			}
		}
	}

	public bool IsEnabled
	{
		get
		{
			return Flags.HasSetFlag(BodyFlags.IsEnabled);
		}
		set
		{
			if (value == IsEnabled)
			{
				return;
			}
			if (value)
			{
				Flags |= BodyFlags.IsEnabled;
				BroadPhase broadPhase = _world.ContactManager.BroadPhase;
				foreach (Fixture fixture in Fixtures)
				{
					fixture.CreateProxies(in broadPhase, in Transform);
				}
				World.HasNewContacts = true;
				return;
			}
			Flags &= ~BodyFlags.IsEnabled;
			BroadPhase broadPhase2 = _world.ContactManager.BroadPhase;
			foreach (Fixture fixture2 in Fixtures)
			{
				fixture2.DestroyProxies(in broadPhase2);
			}
			LinkedListNode<ContactEdge> linkedListNode = ContactEdges.First;
			while (linkedListNode != null)
			{
				ContactEdge value2 = linkedListNode.Value;
				linkedListNode = linkedListNode.Next;
				_world.ContactManager.Destroy(value2.Contact);
			}
			ContactEdges.Clear();
		}
	}

	public bool IsFixedRotation
	{
		get
		{
			return Flags.HasSetFlag(BodyFlags.FixedRotation);
		}
		set
		{
			if (!(Flags.HasSetFlag(BodyFlags.FixedRotation) && value))
			{
				if (value)
				{
					Flags |= BodyFlags.FixedRotation;
				}
				else
				{
					Flags &= ~BodyFlags.FixedRotation;
				}
				AngularVelocity = 0f;
				ResetMassData();
			}
		}
	}

	public object UserData { get; set; }

	public World World => _world;

	internal Body(in BodyDef def, World world)
	{
		Flags = (BodyFlags)0;
		if (def.Bullet)
		{
			Flags |= BodyFlags.IsBullet;
		}
		if (def.FixedRotation)
		{
			Flags |= BodyFlags.FixedRotation;
		}
		if (def.AllowSleep)
		{
			Flags |= BodyFlags.AutoSleep;
		}
		if (def.Awake && def.BodyType != BodyType.StaticBody)
		{
			Flags |= BodyFlags.IsAwake;
		}
		if (def.Enabled)
		{
			Flags |= BodyFlags.IsEnabled;
		}
		_world = world;
		Transform.Position = def.Position;
		Transform.Rotation.Set(def.Angle);
		Sweep = new Sweep
		{
			LocalCenter = FVector2.Zero,
			C0 = Transform.Position,
			C = Transform.Position,
			A0 = def.Angle,
			A = def.Angle,
			Alpha0 = 0f
		};
		JointEdges = new LinkedList<JointEdge>();
		ContactEdges = new LinkedList<ContactEdge>();
		Fixtures = new List<Fixture>();
		Node = null;
		LinearVelocity = def.LinearVelocity;
		AngularVelocity = def.AngularVelocity;
		_linearDamping = def.LinearDamping;
		AngularDamping = def.AngularDamping;
		GravityScale = def.GravityScale;
		Force.SetZero();
		Torque = 0f;
		SleepTime = 0f;
		_type = def.BodyType;
		_mass = 0f;
		InvMass = 0f;
		_inertia = 0f;
		InverseInertia = 0f;
		UserData = def.UserData;
	}

	public void SetGravityScale(FP gFp)
	{
		GravityScale = gFp;
	}

	public void Dispose()
	{
		_world = null;
		ContactEdges?.Clear();
		JointEdges?.Clear();
		Fixtures?.Clear();
		GC.SuppressFinalize(this);
	}

	public void SetAngularVelocity(FP value)
	{
		if (_type != BodyType.StaticBody)
		{
			if (value * value > 0f)
			{
				IsAwake = true;
			}
			AngularVelocity = value;
		}
	}

	public void SetLinearVelocity(in FVector2 value)
	{
		if (_type != BodyType.StaticBody)
		{
			if (FVector2.Dot(value, value) > 0f)
			{
				IsAwake = true;
			}
			LinearVelocity = value;
		}
	}

	public Fixture CreateFixture(FixtureDef def)
	{
		if (_world.IsLocked)
		{
			return null;
		}
		Fixture fixture = Fixture.Create(this, in def);
		if (Flags.HasSetFlag(BodyFlags.IsEnabled))
		{
			fixture.CreateProxies(_world.ContactManager.BroadPhase, in Transform);
		}
		fixture.Body = this;
		Fixtures.Add(fixture);
		if (fixture.Density > 0f)
		{
			ResetMassData();
		}
		_world.HasNewContacts = true;
		return fixture;
	}

	public Fixture CreateFixture(Shape shape, FP density)
	{
		FixtureDef def = new FixtureDef
		{
			Shape = shape,
			Density = density
		};
		return CreateFixture(def);
	}

	public void DestroyFixture(Fixture fixture)
	{
		if (fixture == null || _world.IsLocked)
		{
			return;
		}
		FP density = fixture.Density;
		LinkedListNode<ContactEdge> linkedListNode = ContactEdges.First;
		while (linkedListNode != null)
		{
			ContactEdge value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if (value.Contact.FixtureA == fixture || value.Contact.FixtureB == fixture)
			{
				_world.ContactManager.Destroy(value.Contact);
			}
		}
		if (Flags.HasSetFlag(BodyFlags.IsEnabled))
		{
			fixture.DestroyProxies(_world.ContactManager.BroadPhase);
		}
		Fixtures.Remove(fixture);
		fixture.Body = null;
		Fixture.Destroy(fixture);
		if (density > FP.Zero)
		{
			ResetMassData();
		}
	}

	public void SetTransform(in FVector2 position, FP angle)
	{
		if (_world.IsLocked)
		{
			return;
		}
		Transform.Rotation.Set(angle);
		Transform.Position = position;
		Sweep.C = MathUtils.Mul(in Transform, in Sweep.LocalCenter);
		Sweep.A = angle;
		Sweep.C0 = Sweep.C;
		Sweep.A0 = angle;
		BroadPhase broadPhase = _world.ContactManager.BroadPhase;
		foreach (Fixture fixture in Fixtures)
		{
			fixture.Synchronize(in broadPhase, in Transform, in Transform);
		}
		World.HasNewContacts = true;
	}

	public Transform GetTransform()
	{
		return Transform;
	}

	public FVector2 GetPosition()
	{
		return Transform.Position;
	}

	public FP GetAngle()
	{
		return Sweep.A;
	}

	public FVector2 GetWorldCenter()
	{
		return Sweep.C;
	}

	public FVector2 GetLocalCenter()
	{
		return Sweep.LocalCenter;
	}

	public void ApplyForce(in FVector2 force, in FVector2 point, bool wake)
	{
		if (_type == BodyType.DynamicBody)
		{
			if (wake && !Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				IsAwake = true;
			}
			if (Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				Force += force;
				Torque += MathUtils.Cross(point - Sweep.C, in force);
			}
		}
	}

	public void ApplyForceToCenter(in FVector2 force, bool wake)
	{
		if (_type == BodyType.DynamicBody)
		{
			if (wake && !Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				IsAwake = true;
			}
			if (Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				Force += force;
			}
		}
	}

	public void ApplyTorque(FP torque, bool wake)
	{
		if (_type == BodyType.DynamicBody)
		{
			if (wake && !Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				IsAwake = true;
			}
			if (Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				Torque += torque;
			}
		}
	}

	public void ApplyLinearImpulse(in FVector2 impulse, in FVector2 point, bool wake)
	{
		if (_type == BodyType.DynamicBody)
		{
			if (wake && !Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				IsAwake = true;
			}
			if (Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				LinearVelocity += InvMass * impulse;
				AngularVelocity += InverseInertia * MathUtils.Cross(point - Sweep.C, in impulse);
			}
		}
	}

	public void ApplyLinearImpulseToCenter(in FVector2 impulse, bool wake)
	{
		if (_type == BodyType.DynamicBody)
		{
			if (wake && !Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				IsAwake = true;
			}
			if (Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				LinearVelocity += InvMass * impulse;
			}
		}
	}

	public void ApplyAngularImpulse(FP impulse, bool wake)
	{
		if (_type == BodyType.DynamicBody)
		{
			if (wake && !Flags.HasSetFlag(BodyFlags.IsAwake))
			{
				IsAwake = true;
			}
			if ((Flags & BodyFlags.IsAwake) != 0)
			{
				AngularVelocity += InverseInertia * impulse;
			}
		}
	}

	public MassData GetMassData()
	{
		return new MassData
		{
			Mass = _mass,
			RotationInertia = _inertia + _mass * FVector2.Dot(Sweep.LocalCenter, Sweep.LocalCenter),
			Center = Sweep.LocalCenter
		};
	}

	public void SetMassData(in MassData massData)
	{
		if (!_world.IsLocked && _type == BodyType.DynamicBody)
		{
			InvMass = 0f;
			_inertia = 0f;
			InverseInertia = 0f;
			_mass = massData.Mass;
			if (_mass <= 0f)
			{
				_mass = 1f;
			}
			InvMass = 1f / _mass;
			if (massData.RotationInertia > 0f && !Flags.HasSetFlag(BodyFlags.FixedRotation))
			{
				_inertia = massData.RotationInertia - _mass * FVector2.Dot(massData.Center, massData.Center);
				InverseInertia = 1f / _inertia;
			}
			FVector2 c = Sweep.C;
			Sweep.LocalCenter = massData.Center;
			Sweep.C0 = (Sweep.C = MathUtils.Mul(in Transform, in Sweep.LocalCenter));
			LinearVelocity += MathUtils.Cross(AngularVelocity, Sweep.C - c);
		}
	}

	private void ResetMassData()
	{
		_mass = 0f;
		InvMass = 0f;
		_inertia = 0f;
		InverseInertia = 0f;
		Sweep.LocalCenter.SetZero();
		if (_type == BodyType.StaticBody || _type == BodyType.KinematicBody)
		{
			Sweep.C0 = Transform.Position;
			Sweep.C = Transform.Position;
			Sweep.A0 = Sweep.A;
			return;
		}
		FVector2 zero = FVector2.Zero;
		foreach (Fixture fixture in Fixtures)
		{
			if (!fixture.Density.Equals(0f))
			{
				fixture.GetMassData(out var massData);
				_mass += massData.Mass;
				zero += massData.Mass * massData.Center;
				_inertia += massData.RotationInertia;
			}
		}
		if (_mass > 0f)
		{
			InvMass = 1f / _mass;
			zero *= InvMass;
		}
		if (_inertia > 0f && !Flags.HasSetFlag(BodyFlags.FixedRotation))
		{
			_inertia -= _mass * FVector2.Dot(zero, zero);
			InverseInertia = 1f / _inertia;
		}
		else
		{
			_inertia = 0f;
			InverseInertia = 0f;
		}
		FVector2 c = Sweep.C;
		Sweep.LocalCenter = zero;
		Sweep.C0 = (Sweep.C = MathUtils.Mul(in Transform, in Sweep.LocalCenter));
		LinearVelocity += MathUtils.Cross(AngularVelocity, Sweep.C - c);
	}

	public FVector2 GetWorldPoint(in FVector2 localPoint)
	{
		return MathUtils.Mul(in Transform, in localPoint);
	}

	public FVector2 GetWorldVector(in FVector2 localVector)
	{
		return MathUtils.Mul(in Transform.Rotation, in localVector);
	}

	public FVector2 GetLocalPoint(in FVector2 worldPoint)
	{
		return MathUtils.MulT(in Transform, in worldPoint);
	}

	public FVector2 GetLocalVector(in FVector2 worldVector)
	{
		return MathUtils.MulT(in Transform.Rotation, in worldVector);
	}

	public FVector2 GetLinearVelocityFromWorldPoint(in FVector2 worldPoint)
	{
		return LinearVelocity + MathUtils.Cross(AngularVelocity, worldPoint - Sweep.C);
	}

	public FVector2 GetLinearVelocityFromLocalPoint(in FVector2 localPoint)
	{
		return GetLinearVelocityFromWorldPoint(GetWorldPoint(in localPoint));
	}

	public void Dump()
	{
	}

	internal void SynchronizeFixtures()
	{
		BroadPhase broadPhase = World.ContactManager.BroadPhase;
		if (Flags.HasSetFlag(BodyFlags.IsAwake))
		{
			Transform transform = default(Transform);
			transform.Rotation.Set(Sweep.A0);
			transform.Position = Sweep.C0 - MathUtils.Mul(in transform.Rotation, in Sweep.LocalCenter);
			for (int i = 0; i < Fixtures.Count; i++)
			{
				Fixtures[i].Synchronize(in broadPhase, in transform, in Transform);
			}
		}
		else
		{
			for (int j = 0; j < Fixtures.Count; j++)
			{
				Fixtures[j].Synchronize(in broadPhase, in Transform, in Transform);
			}
		}
	}

	internal void SynchronizeTransform()
	{
		Transform.Rotation.Set(Sweep.A);
		Transform.Position = Sweep.C - MathUtils.Mul(in Transform.Rotation, in Sweep.LocalCenter);
	}

	internal bool ShouldCollide(Body other)
	{
		if (_type != BodyType.DynamicBody && other._type != BodyType.DynamicBody)
		{
			return false;
		}
		LinkedListNode<JointEdge> linkedListNode = JointEdges.First;
		while (linkedListNode != null)
		{
			JointEdge value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if (value.Other == other && !value.Joint.CollideConnected)
			{
				return false;
			}
		}
		return true;
	}

	internal void Advance(FP alpha)
	{
		Sweep.Advance(alpha);
		Sweep.C = Sweep.C0;
		Sweep.A = Sweep.A0;
		Transform.Rotation.Set(Sweep.A);
		Transform.Position = Sweep.C - MathUtils.Mul(in Transform.Rotation, in Sweep.LocalCenter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetFlag(BodyFlags flag)
	{
		Flags |= flag;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UnsetFlag(BodyFlags flag)
	{
		Flags &= ~flag;
	}

	public PhysicsSnapShot.ComponentPhysicsSnapshotData TakeSnapShot()
	{
		PhysicsSnapShot.ComponentPhysicsSnapshotData componentPhysicsSnapshotData = new PhysicsSnapShot.ComponentPhysicsSnapshotData();
		componentPhysicsSnapshotData.BodyDefData = new PhysicsSnapShot.ComponentPhysicsBodyDefData
		{
			BodyType = BodyType,
			Position = GetPosition(),
			Rotation = Transform.Rotation.Angle,
			Flags = Flags,
			GravityScale = GravityScale,
			Velocity = LinearVelocity,
			AngularDamping = AngularDamping,
			Inertia = _inertia,
			LinearDamping = LinearDamping,
			Mass = _mass,
			InverseInertia = InverseInertia,
			InvMass = InvMass,
			Torque = Torque,
			Force = Force,
			IslandIndex = IslandIndex,
			LocalCenter = Sweep.LocalCenter,
			Alpha0 = Sweep.Alpha0,
			UserData = UserData
		};
		componentPhysicsSnapshotData.FixtureDef = new PhysicsSnapShot.ComponentPhysicsFixtureDefData[Fixtures.Count];
		componentPhysicsSnapshotData.ShapeData = new PhysicsSnapShot.ComponentPhysicsShapeData[Fixtures.Count];
		for (int i = 0; i < Fixtures.Count; i++)
		{
			Fixture fixture = Fixtures[i];
			PhysicsSnapShot.ComponentPhysicsFixtureDefData componentPhysicsFixtureDefData = new PhysicsSnapShot.ComponentPhysicsFixtureDefData();
			componentPhysicsFixtureDefData.Density = fixture.Density;
			componentPhysicsFixtureDefData.Friction = fixture.Friction;
			componentPhysicsFixtureDefData.Restitution = fixture.Restitution;
			componentPhysicsFixtureDefData.RestitutionThreshold = fixture.RestitutionThreshold;
			componentPhysicsFixtureDefData.CategoryBits = fixture.Filter.CategoryBits;
			componentPhysicsFixtureDefData.MaskBits = fixture.Filter.MaskBits;
			componentPhysicsFixtureDefData.GroupIndex = fixture.Filter.GroupIndex;
			componentPhysicsFixtureDefData.UserData = fixture.UserData;
			componentPhysicsSnapshotData.FixtureDef[i] = componentPhysicsFixtureDefData;
			componentPhysicsSnapshotData.ShapeData[i] = fixture.Shape.TakeSnapShot();
		}
		return componentPhysicsSnapshotData;
	}

	public void RestoreSnapshot(PhysicsSnapShot.ComponentPhysicsSnapshotData snapshot)
	{
		if (Fixtures.Count > 0)
		{
			for (int num = Fixtures.Count - 1; num >= 0; num--)
			{
				DestroyFixture(Fixtures[num]);
			}
		}
		Fixtures.Clear();
		PhysicsSnapShot.ComponentPhysicsFixtureDefData[] fixtureDef = snapshot.FixtureDef;
		for (int i = 0; i < fixtureDef.Length; i++)
		{
			PhysicsSnapShot.ComponentPhysicsFixtureDefData componentPhysicsFixtureDefData = fixtureDef[i];
			FixtureDef def = new FixtureDef
			{
				Density = componentPhysicsFixtureDefData.Density,
				Friction = componentPhysicsFixtureDefData.Friction,
				Restitution = componentPhysicsFixtureDefData.Restitution,
				RestitutionThreshold = componentPhysicsFixtureDefData.RestitutionThreshold,
				UserData = componentPhysicsFixtureDefData.UserData,
				Filter = new Filter
				{
					GroupIndex = componentPhysicsFixtureDefData.GroupIndex,
					CategoryBits = componentPhysicsFixtureDefData.CategoryBits,
					MaskBits = componentPhysicsFixtureDefData.MaskBits
				}
			};
			PhysicsSnapShot.ComponentPhysicsShapeData componentPhysicsShapeData = snapshot.ShapeData[i];
			Shape shape = Activator.CreateInstance(componentPhysicsShapeData.Type) as Shape;
			shape.RestoreSnapshot(componentPhysicsShapeData);
			def.Shape = shape;
			CreateFixture(def);
		}
		PhysicsSnapShot.ComponentPhysicsBodyDefData bodyDefData = snapshot.BodyDefData;
		BodyType = bodyDefData.BodyType;
		Flags = bodyDefData.Flags;
		GravityScale = bodyDefData.GravityScale;
		LinearVelocity = bodyDefData.Velocity;
		AngularDamping = bodyDefData.AngularDamping;
		_inertia = bodyDefData.Inertia;
		LinearDamping = bodyDefData.LinearDamping;
		_mass = bodyDefData.Mass;
		InverseInertia = bodyDefData.InverseInertia;
		InvMass = bodyDefData.InvMass;
		Torque = bodyDefData.Torque;
		Force = bodyDefData.Force;
		IslandIndex = bodyDefData.IslandIndex;
		Sweep.LocalCenter = bodyDefData.LocalCenter;
		Sweep.Alpha0 = bodyDefData.Alpha0;
		UserData = bodyDefData.UserData;
		SetTransform(in bodyDefData.Position, bodyDefData.Rotation);
	}
}

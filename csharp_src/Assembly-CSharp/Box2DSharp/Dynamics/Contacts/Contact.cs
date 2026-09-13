using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public abstract class Contact
{
	[Flags]
	public enum ContactFlag
	{
		IslandFlag = 1,
		TouchingFlag = 2,
		EnabledFlag = 4,
		FilterFlag = 8,
		BulletHitFlag = 0x10,
		ToiFlag = 0x20
	}

	public Fixture FixtureA;

	public Fixture FixtureB;

	internal ContactFlag Flags;

	internal FP Friction;

	public int ChildIndexA;

	public int ChildIndexB;

	public Manifold Manifold;

	internal readonly LinkedListNode<Contact> Node = new LinkedListNode<Contact>(null);

	internal readonly ContactEdge NodeA = new ContactEdge();

	internal readonly ContactEdge NodeB = new ContactEdge();

	internal FP Restitution;

	internal FP RestitutionThreshold;

	internal FP TangentSpeed;

	internal FP Toi;

	internal int ToiCount;

	public bool IsTouching
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Flags.HasSetFlag(ContactFlag.TouchingFlag);
		}
	}

	public bool IsEnabled
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Flags.HasSetFlag(ContactFlag.EnabledFlag);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void Initialize(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		Flags = ContactFlag.EnabledFlag;
		FixtureA = fixtureA;
		FixtureB = fixtureB;
		ChildIndexA = indexA;
		ChildIndexB = indexB;
		ToiCount = 0;
		Friction = MixFriction(FixtureA.Friction, FixtureB.Friction);
		Restitution = MixRestitution(FixtureA.Restitution, FixtureB.Restitution);
		RestitutionThreshold = MixRestitutionThreshold(FixtureA.RestitutionThreshold, FixtureB.RestitutionThreshold);
		TangentSpeed = 0f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal virtual void Reset()
	{
		FixtureA = null;
		FixtureB = null;
		Flags = (ContactFlag)0;
		Friction = default(FP);
		ChildIndexA = 0;
		ChildIndexB = 0;
		Manifold = default(Manifold);
		Node.Value = null;
		NodeA.Node.Value = null;
		NodeA.Other = null;
		NodeB.Node.Value = null;
		NodeB.Other = null;
		Restitution = default(FP);
		TangentSpeed = default(FP);
		Toi = default(FP);
		ToiCount = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FP MixFriction(FP friction1, FP friction2)
	{
		return FP.Sqrt(friction1 * friction2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FP MixRestitution(FP restitution1, FP restitution2)
	{
		if (!(restitution1 > restitution2))
		{
			return restitution2;
		}
		return restitution1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FP MixRestitutionThreshold(FP threshold1, FP threshold2)
	{
		if (!(threshold1 < threshold2))
		{
			return threshold2;
		}
		return threshold1;
	}

	public void GetWorldManifold(out WorldManifold worldManifold)
	{
		Body body = FixtureA.Body;
		Body body2 = FixtureB.Body;
		Shape shape = FixtureA.Shape;
		Shape shape2 = FixtureB.Shape;
		worldManifold = default(WorldManifold);
		worldManifold.Initialize(in Manifold, body.GetTransform(), shape.Radius, body2.GetTransform(), shape2.Radius);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetEnabled(bool flag)
	{
		if (flag)
		{
			Flags |= ContactFlag.EnabledFlag;
		}
		else
		{
			Flags &= ~ContactFlag.EnabledFlag;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetFriction(FP friction)
	{
		Friction = friction;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP GetFriction()
	{
		return Friction;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ResetFriction()
	{
		Friction = MixFriction(FixtureA.Friction, FixtureB.Friction);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetRestitution(FP restitution)
	{
		Restitution = restitution;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP GetRestitution()
	{
		return Restitution;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ResetRestitution()
	{
		Restitution = MixRestitution(FixtureA.Restitution, FixtureB.Restitution);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetRestitutionThreshold(FP threshold)
	{
		RestitutionThreshold = threshold;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP GetRestitutionThreshold()
	{
		return RestitutionThreshold;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ResetRestitutionThreshold()
	{
		RestitutionThreshold = MixRestitutionThreshold(FixtureA.RestitutionThreshold, FixtureB.RestitutionThreshold);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetTangentSpeed(FP speed)
	{
		TangentSpeed = speed;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP GetTangentSpeed()
	{
		return TangentSpeed;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void FlagForFiltering()
	{
		Flags |= ContactFlag.FilterFlag;
	}

	internal void Update(IContactListener listener)
	{
		Manifold oldManifold = Manifold;
		Flags |= ContactFlag.EnabledFlag;
		bool flag = false;
		bool flag2 = Flags.HasSetFlag(ContactFlag.TouchingFlag);
		bool isSensor = FixtureA.IsSensor;
		bool isSensor2 = FixtureB.IsSensor;
		bool flag3 = isSensor || isSensor2;
		Body body = FixtureA.Body;
		Body body2 = FixtureB.Body;
		Transform xfA = body.GetTransform();
		Transform xfB = body2.GetTransform();
		if (flag3)
		{
			flag = CollisionUtils.TestOverlap(FixtureA.Shape, shapeB: FixtureB.Shape, indexA: ChildIndexA, indexB: ChildIndexB, xfA: in xfA, xfB: in xfB, gJkProfile: body.World.GJkProfile);
			Manifold.PointCount = 0;
		}
		else
		{
			Evaluate(ref Manifold, in xfA, xfB);
			flag = Manifold.PointCount > 0;
			for (int i = 0; i < Manifold.PointCount; i++)
			{
				ref ManifoldPoint reference = ref Manifold.Points[i];
				reference.NormalImpulse = 0f;
				reference.TangentImpulse = 0f;
				ContactId id = reference.Id;
				for (int j = 0; j < oldManifold.PointCount; j++)
				{
					ref ManifoldPoint reference2 = ref oldManifold.Points[j];
					if (reference2.Id.Key == id.Key)
					{
						reference.NormalImpulse = reference2.NormalImpulse;
						reference.TangentImpulse = reference2.TangentImpulse;
						break;
					}
				}
			}
			if (flag != flag2)
			{
				body.IsAwake = true;
				body2.IsAwake = true;
			}
		}
		if (flag)
		{
			Flags |= ContactFlag.TouchingFlag;
		}
		else
		{
			Flags &= ~ContactFlag.TouchingFlag;
		}
		if (listener != null)
		{
			if (!flag2 && flag)
			{
				listener.BeginContact(this);
			}
			if (flag2 && !flag)
			{
				listener.EndContact(this);
			}
			if (!flag3 && flag)
			{
				listener.PreSolve(this, in oldManifold);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetFlag(ContactFlag flag)
	{
		Flags |= flag;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UnsetFlag(ContactFlag flag)
	{
		Flags &= ~flag;
	}
}

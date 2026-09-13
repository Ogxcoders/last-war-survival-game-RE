using System;
using System.Collections.Generic;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;

namespace Box2DSharp.Dynamics;

public class Fixture : IDisposable
{
	public FP Restitution;

	public FP RestitutionThreshold;

	private FP _density;

	private Filter _filter;

	private bool _isSensor;

	public Body Body { get; set; }

	public FP Density
	{
		get
		{
			return _density;
		}
		set
		{
			_density = value;
		}
	}

	public Filter Filter
	{
		get
		{
			return _filter;
		}
		set
		{
			_filter = value;
			Refilter();
		}
	}

	public FP Friction { get; set; }

	public bool IsSensor
	{
		get
		{
			return _isSensor;
		}
		set
		{
			if (_isSensor != value)
			{
				Body.IsAwake = true;
				_isSensor = value;
			}
		}
	}

	public FixtureProxy[] Proxies { get; private set; }

	public int ProxyCount { get; internal set; }

	public Shape Shape { get; private set; }

	public object UserData { get; set; }

	public ShapeType ShapeType => Shape.ShapeType;

	internal static Fixture Create(Body body, in FixtureDef def)
	{
		int childCount = def.Shape.GetChildCount();
		Fixture fixture = new Fixture
		{
			UserData = def.UserData,
			Friction = def.Friction,
			Restitution = def.Restitution,
			RestitutionThreshold = def.RestitutionThreshold,
			Body = body,
			Filter = def.Filter,
			IsSensor = def.IsSensor,
			Shape = def.Shape.Clone(),
			ProxyCount = 0,
			Density = def.Density,
			Proxies = new FixtureProxy[childCount]
		};
		for (int i = 0; i < childCount; i++)
		{
			fixture.Proxies[i] = new FixtureProxy();
		}
		return fixture;
	}

	internal static void Destroy(Fixture fixture)
	{
		fixture.Dispose();
	}

	internal void CreateProxies(in BroadPhase broadPhase, in Transform xf)
	{
		ProxyCount = Shape.GetChildCount();
		for (int i = 0; i < ProxyCount; i++)
		{
			FixtureProxy fixtureProxy = Proxies[i];
			Shape.ComputeAABB(out fixtureProxy.AABB, in xf, i);
			fixtureProxy.Fixture = this;
			fixtureProxy.ChildIndex = i;
			fixtureProxy.ProxyId = broadPhase.CreateProxy(in fixtureProxy.AABB, fixtureProxy);
		}
	}

	internal void DestroyProxies(in BroadPhase broadPhase)
	{
		for (int i = 0; i < ProxyCount; i++)
		{
			FixtureProxy fixtureProxy = Proxies[i];
			broadPhase.DestroyProxy(fixtureProxy.ProxyId);
			fixtureProxy.ProxyId = -1;
		}
		ProxyCount = 0;
	}

	public void Refilter()
	{
		if (Body == null)
		{
			return;
		}
		LinkedListNode<ContactEdge> linkedListNode = Body.ContactEdges.First;
		while (linkedListNode != null)
		{
			Contact contact = linkedListNode.Value.Contact;
			linkedListNode = linkedListNode.Next;
			if (contact.FixtureA == this || contact.FixtureB == this)
			{
				contact.FlagForFiltering();
			}
		}
		if (Body._world != null)
		{
			BroadPhase broadPhase = Body._world.ContactManager.BroadPhase;
			for (int i = 0; i < ProxyCount; i++)
			{
				broadPhase.TouchProxy(Proxies[i].ProxyId);
			}
		}
	}

	public bool TestPoint(in FVector2 p)
	{
		return Shape.TestPoint(Body.GetTransform(), in p);
	}

	public bool RayCast(out RayCastOutput output, in RayCastInput input, int childIndex)
	{
		return Shape.RayCast(out output, in input, Body.GetTransform(), childIndex);
	}

	public void GetMassData(out MassData massData)
	{
		Shape.ComputeMass(out massData, Density);
	}

	public Box2DSharp.Collision.AABB GetAABB(int childIndex)
	{
		return Proxies[childIndex].AABB;
	}

	public void Dump(int bodyIndex)
	{
		throw new NotImplementedException();
	}

	internal void Synchronize(in BroadPhase broadPhase, in Transform transform1, in Transform transform2)
	{
		if (ProxyCount != 0)
		{
			for (int i = 0; i < ProxyCount; i++)
			{
				FixtureProxy fixtureProxy = Proxies[i];
				Shape.ComputeAABB(out var aabb, in transform1, fixtureProxy.ChildIndex);
				Shape.ComputeAABB(out var aabb2, in transform2, fixtureProxy.ChildIndex);
				fixtureProxy.AABB.Combine(in aabb, in aabb2);
				FVector2 displacement = aabb2.GetCenter() - aabb.GetCenter();
				broadPhase.MoveProxy(fixtureProxy.ProxyId, in fixtureProxy.AABB, in displacement);
			}
		}
	}

	public void Dispose()
	{
		Array.Clear(Proxies, 0, Proxies.Length);
		Proxies = null;
		Body = null;
	}
}

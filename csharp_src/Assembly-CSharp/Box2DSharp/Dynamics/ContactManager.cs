using System;
using System.Collections.Generic;
using System.Threading;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Internal;

namespace Box2DSharp.Dynamics;

public class ContactManager : IAddPairCallback, IDisposable
{
	public static readonly IContactFilter DefaultContactFilter;

	public IContactFilter ContactFilter = DefaultContactFilter;

	public IContactListener ContactListener;

	private static readonly ContactRegister[,] _registers;

	private const int DisposedFalse = 0;

	private const int DisposedTrue = 1;

	private int _disposed;

	public BroadPhase BroadPhase { get; private set; } = new BroadPhase();

	public LinkedList<Contact> ContactList { get; private set; } = new LinkedList<Contact>();

	public int ContactCount => ContactList.Count;

	static ContactManager()
	{
		DefaultContactFilter = new DefaultContactFilter();
		_registers = new ContactRegister[4, 4];
		Register(ShapeType.Circle, ShapeType.Circle, new CircleContactFactory());
		Register(ShapeType.Polygon, ShapeType.Circle, new PolygonAndCircleContactFactory());
		Register(ShapeType.Polygon, ShapeType.Polygon, new PolygonContactFactory());
		Register(ShapeType.Edge, ShapeType.Circle, new EdgeAndCircleContactFactory());
		Register(ShapeType.Edge, ShapeType.Polygon, new EdgeAndPolygonContactFactory());
		Register(ShapeType.Chain, ShapeType.Circle, new ChainAndCircleContactFactory());
		Register(ShapeType.Chain, ShapeType.Polygon, new ChainAndPolygonContactFactory());
		static void Register(ShapeType type1, ShapeType type2, IContactFactory factory)
		{
			_registers[(int)type1, (int)type2] = new ContactRegister(factory, primary: true);
			if (type1 != type2)
			{
				_registers[(int)type2, (int)type1] = new ContactRegister(factory, primary: false);
			}
		}
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref _disposed, 1) != 1)
		{
			BroadPhase = null;
			ContactList?.Clear();
			ContactList = null;
			ContactFilter = null;
			ContactListener = null;
		}
	}

	private Contact CreateContact(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		ShapeType shapeType = fixtureA.ShapeType;
		ShapeType shapeType2 = fixtureB.ShapeType;
		ContactRegister contactRegister = _registers[(int)shapeType, (int)shapeType2] ?? throw new NullReferenceException(shapeType.ToString() + " can not contact to " + shapeType2);
		if (contactRegister.Primary)
		{
			return contactRegister.Factory.Create(fixtureA, indexA, fixtureB, indexB);
		}
		return contactRegister.Factory.Create(fixtureB, indexB, fixtureA, indexA);
	}

	private void DestroyContact(Contact contact)
	{
		Fixture fixtureA = contact.FixtureA;
		Fixture fixtureB = contact.FixtureB;
		if (contact.Manifold.PointCount > 0 && !fixtureA.IsSensor && !fixtureB.IsSensor)
		{
			fixtureA.Body.IsAwake = true;
			fixtureB.Body.IsAwake = true;
		}
		ShapeType shapeType = fixtureA.ShapeType;
		ShapeType shapeType2 = fixtureB.ShapeType;
		_registers[(int)shapeType, (int)shapeType2].Factory.Destroy(contact);
	}

	public void AddPairCallback(object proxyUserDataA, object proxyUserDataB)
	{
		FixtureProxy obj = (FixtureProxy)proxyUserDataA;
		FixtureProxy fixtureProxy = (FixtureProxy)proxyUserDataB;
		Fixture fixture = obj.Fixture;
		Fixture fixture2 = fixtureProxy.Fixture;
		int childIndex = obj.ChildIndex;
		int childIndex2 = fixtureProxy.ChildIndex;
		Body body = fixture.Body;
		Body body2 = fixture2.Body;
		if (body == body2)
		{
			return;
		}
		LinkedListNode<ContactEdge> linkedListNode = body2.ContactEdges.First;
		while (linkedListNode != null)
		{
			ContactEdge value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if ((value.Contact.FixtureA == fixture && value.Contact.FixtureB == fixture2 && value.Contact.ChildIndexA == childIndex && value.Contact.ChildIndexB == childIndex2) || (value.Contact.FixtureA == fixture2 && value.Contact.FixtureB == fixture && value.Contact.ChildIndexA == childIndex2 && value.Contact.ChildIndexB == childIndex))
			{
				return;
			}
		}
		if (body2.ShouldCollide(body))
		{
			IContactFilter contactFilter = ContactFilter;
			if (contactFilter == null || contactFilter.ShouldCollide(fixture, fixture2))
			{
				Contact contact = CreateContact(fixture, childIndex, fixture2, childIndex2);
				fixture = contact.FixtureA;
				fixture2 = contact.FixtureB;
				body = fixture.Body;
				body2 = fixture2.Body;
				contact.Node.Value = contact;
				ContactList.AddFirst(contact.Node);
				contact.NodeA.Contact = contact;
				contact.NodeA.Other = body2;
				contact.NodeA.Node.Value = contact.NodeA;
				body.ContactEdges.AddFirst(contact.NodeA.Node);
				contact.NodeB.Contact = contact;
				contact.NodeB.Other = body;
				contact.NodeB.Node.Value = contact.NodeB;
				body2.ContactEdges.AddFirst(contact.NodeB.Node);
			}
		}
	}

	public void FindNewContacts()
	{
		BroadPhase.UpdatePairs(this);
	}

	public void Destroy(Contact c)
	{
		Fixture fixtureA = c.FixtureA;
		Fixture fixtureB = c.FixtureB;
		Body body = fixtureA.Body;
		Body body2 = fixtureB.Body;
		if (c.IsTouching)
		{
			ContactListener?.EndContact(c);
		}
		ContactList.Remove(c.Node);
		body.ContactEdges.Remove(c.NodeA.Node);
		body2.ContactEdges.Remove(c.NodeB.Node);
		DestroyContact(c);
	}

	public void Collide()
	{
		LinkedListNode<Contact> linkedListNode = ContactList.First;
		while (linkedListNode != null)
		{
			Contact value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			Fixture fixtureA = value.FixtureA;
			Fixture fixtureB = value.FixtureB;
			int childIndexA = value.ChildIndexA;
			int childIndexB = value.ChildIndexB;
			Body body = fixtureA.Body;
			Body body2 = fixtureB.Body;
			if (value.Flags.HasSetFlag(Contact.ContactFlag.FilterFlag))
			{
				if (!body2.ShouldCollide(body))
				{
					Destroy(value);
					continue;
				}
				IContactFilter contactFilter = ContactFilter;
				if (contactFilter != null && !contactFilter.ShouldCollide(fixtureA, fixtureB))
				{
					Destroy(value);
					continue;
				}
				value.Flags &= ~Contact.ContactFlag.FilterFlag;
			}
			bool num = body.IsAwake && body.BodyType != BodyType.StaticBody;
			bool flag = body2.IsAwake && body2.BodyType != BodyType.StaticBody;
			if (num || flag)
			{
				int proxyId = fixtureA.Proxies[childIndexA].ProxyId;
				int proxyId2 = fixtureB.Proxies[childIndexB].ProxyId;
				if (!BroadPhase.TestOverlap(proxyId, proxyId2))
				{
					Destroy(value);
				}
				else
				{
					value.Update(ContactListener);
				}
			}
		}
	}
}

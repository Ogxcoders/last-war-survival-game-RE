using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class EdgeAndPolygonContactFactory : IContactFactory
{
	private readonly ContactPool<EdgeAndPolygonContact> _pool = new ContactPool<EdgeAndPolygonContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		EdgeAndPolygonContact edgeAndPolygonContact = _pool.Get();
		edgeAndPolygonContact.Initialize(fixtureA, 0, fixtureB, 0);
		return edgeAndPolygonContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((EdgeAndPolygonContact)contact);
	}
}

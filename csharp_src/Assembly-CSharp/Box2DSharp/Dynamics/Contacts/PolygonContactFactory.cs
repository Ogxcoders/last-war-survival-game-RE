using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class PolygonContactFactory : IContactFactory
{
	private readonly ContactPool<PolygonContact> _pool = new ContactPool<PolygonContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		PolygonContact polygonContact = _pool.Get();
		polygonContact.Initialize(fixtureA, 0, fixtureB, 0);
		return polygonContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((PolygonContact)contact);
	}
}

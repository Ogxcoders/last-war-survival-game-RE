using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class ChainAndPolygonContactFactory : IContactFactory
{
	private readonly ContactPool<ChainAndPolygonContact> _pool = new ContactPool<ChainAndPolygonContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		ChainAndPolygonContact chainAndPolygonContact = _pool.Get();
		chainAndPolygonContact.Initialize(fixtureA, indexA, fixtureB, indexB);
		return chainAndPolygonContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((ChainAndPolygonContact)contact);
	}
}

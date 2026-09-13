using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class ChainAndCircleContactFactory : IContactFactory
{
	private readonly ContactPool<ChainAndCircleContact> _pool = new ContactPool<ChainAndCircleContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		ChainAndCircleContact chainAndCircleContact = _pool.Get();
		chainAndCircleContact.Initialize(fixtureA, indexA, fixtureB, indexB);
		return chainAndCircleContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((ChainAndCircleContact)contact);
	}
}

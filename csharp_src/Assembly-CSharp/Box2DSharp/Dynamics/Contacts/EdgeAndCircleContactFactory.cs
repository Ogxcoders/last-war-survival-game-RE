using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class EdgeAndCircleContactFactory : IContactFactory
{
	private readonly ContactPool<EdgeAndCircleContact> _pool = new ContactPool<EdgeAndCircleContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		EdgeAndCircleContact edgeAndCircleContact = _pool.Get();
		edgeAndCircleContact.Initialize(fixtureA, 0, fixtureB, 0);
		return edgeAndCircleContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((EdgeAndCircleContact)contact);
	}
}

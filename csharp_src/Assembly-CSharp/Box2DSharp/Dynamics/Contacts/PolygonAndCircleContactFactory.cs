using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class PolygonAndCircleContactFactory : IContactFactory
{
	private readonly ContactPool<PolygonAndCircleContact> _pool = new ContactPool<PolygonAndCircleContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		PolygonAndCircleContact polygonAndCircleContact = _pool.Get();
		polygonAndCircleContact.Initialize(fixtureA, 0, fixtureB, 0);
		return polygonAndCircleContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((PolygonAndCircleContact)contact);
	}
}

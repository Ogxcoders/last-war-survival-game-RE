using System.Runtime.CompilerServices;

namespace Box2DSharp.Dynamics.Contacts;

internal class CircleContactFactory : IContactFactory
{
	private readonly ContactPool<CircleContact> _pool = new ContactPool<CircleContact>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
	{
		CircleContact circleContact = _pool.Get();
		circleContact.Initialize(fixtureA, 0, fixtureB, 0);
		return circleContact;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Destroy(Contact contact)
	{
		_pool.Return((CircleContact)contact);
	}
}

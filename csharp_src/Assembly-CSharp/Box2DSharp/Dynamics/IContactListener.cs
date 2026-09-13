using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics.Contacts;

namespace Box2DSharp.Dynamics;

public interface IContactListener
{
	void BeginContact(Contact contact);

	void EndContact(Contact contact);

	void PreSolve(Contact contact, in Manifold oldManifold);

	void PostSolve(Contact contact, in ContactImpulse impulse);
}

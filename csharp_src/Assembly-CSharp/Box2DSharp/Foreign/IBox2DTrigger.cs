using Box2DSharp.Dynamics.Contacts;

namespace Box2DSharp.Foreign;

public interface IBox2DTrigger
{
	void OnCollider(Contact contact);

	void EndCollider(Contact contact);

	void OnSolve(Contact contact);
}

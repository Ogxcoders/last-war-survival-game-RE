using Box2DSharp.Dynamics.Contacts;

namespace Box2DSharp.Dynamics;

internal class ContactRegister
{
	public readonly IContactFactory Factory;

	public readonly bool Primary;

	public ContactRegister(IContactFactory factory, bool primary)
	{
		Primary = primary;
		Factory = factory;
	}
}

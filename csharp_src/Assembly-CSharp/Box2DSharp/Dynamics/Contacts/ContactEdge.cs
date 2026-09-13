using System.Collections.Generic;

namespace Box2DSharp.Dynamics.Contacts;

public class ContactEdge
{
	public Body Other;

	public Contact Contact;

	public readonly LinkedListNode<ContactEdge> Node = new LinkedListNode<ContactEdge>(null);
}

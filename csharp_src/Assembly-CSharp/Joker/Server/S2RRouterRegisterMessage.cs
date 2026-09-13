namespace Joker.Server;

[Message(0)]
public class S2RRouterRegisterMessage : IRouteInnerMessage, IRouteMessage, IMessage
{
	public int Node;

	public string Name;

	public long FromChannel;

	public long ToChannel;
}

namespace Joker.Server;

public class S2SRouteServerMessage : DecoMessage, IRouteServerMessage, IRouteMessage, IMessage, IDecoMessage
{
	public int From;

	public int To;

	public int GetFrom()
	{
		return From;
	}

	public int GetTo()
	{
		return To;
	}
}

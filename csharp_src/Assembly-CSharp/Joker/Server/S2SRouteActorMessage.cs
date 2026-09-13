namespace Joker.Server;

public class S2SRouteActorMessage : DecoMessage, IRouteActorMessage, IRouteMessage, IMessage, IDecoMessage
{
	public string AccountId;

	public string RoomId;

	public string GetAccountId()
	{
		return AccountId;
	}

	public string GetActorId()
	{
		return RoomId;
	}
}

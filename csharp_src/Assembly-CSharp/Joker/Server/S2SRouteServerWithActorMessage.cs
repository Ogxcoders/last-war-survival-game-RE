namespace Joker.Server;

public class S2SRouteServerWithActorMessage : DecoMessage, IRouteServerWithActorMessage, IRouteServerMessage, IRouteMessage, IMessage, IDecoMessage
{
	public int From;

	public int To;

	public string AccountId;

	public string TargetId;

	public int GetFrom()
	{
		return From;
	}

	public int GetTo()
	{
		return To;
	}

	public string GetAccountId()
	{
		return AccountId;
	}

	public string GetActorId()
	{
		return TargetId;
	}
}

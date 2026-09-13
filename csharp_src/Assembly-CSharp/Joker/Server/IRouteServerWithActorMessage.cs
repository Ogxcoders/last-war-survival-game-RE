namespace Joker.Server;

public interface IRouteServerWithActorMessage : IRouteServerMessage, IRouteMessage, IMessage, IDecoMessage
{
	string GetAccountId();

	string GetActorId();
}

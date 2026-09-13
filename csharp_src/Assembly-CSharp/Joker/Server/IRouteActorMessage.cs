namespace Joker.Server;

public interface IRouteActorMessage : IRouteMessage, IMessage, IDecoMessage
{
	string GetAccountId();

	string GetActorId();
}

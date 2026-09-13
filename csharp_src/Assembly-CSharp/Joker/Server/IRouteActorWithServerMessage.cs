namespace Joker.Server;

public interface IRouteActorWithServerMessage : IRouteActorMessage, IRouteMessage, IMessage, IDecoMessage
{
	int GetFrom();

	int GetTo();
}

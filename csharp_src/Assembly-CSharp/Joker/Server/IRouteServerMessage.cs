namespace Joker.Server;

public interface IRouteServerMessage : IRouteMessage, IMessage, IDecoMessage
{
	int GetFrom();

	int GetTo();
}

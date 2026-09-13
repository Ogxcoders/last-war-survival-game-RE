namespace Joker.Server;

public interface IGameActor
{
	int ClientWorld { get; }

	string AccountId { get; }

	string ActorId { get; }

	bool EnqueueMessage(IRouteActorMessage route, IMessage message, object source);

	bool ProcessMessages();

	void SendToClient(IMessage message);

	void SendToServer(int to, IMessage message);

	void SendToWorld(int to, IMessage message);
}

namespace MiniGame.Core.Server;

public interface IGameLogicSession : IGameLogicMessageSender
{
	string GetName();

	string GetUID();

	string GetSessionID();

	object GetProperty(string key);
}

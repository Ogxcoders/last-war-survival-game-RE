namespace MiniGame.Core.Server;

public interface IGameLogicMessageSender
{
	void SendRaw(int opCode, object data, object oriData);
}

namespace MiniGame.Core.Server;

public interface IGameLogicFactory
{
	void InitLogger(IGameLogicLogger logger);

	void InitWorkspace(string workspace);

	IGameLogic CreateGame(string type);
}

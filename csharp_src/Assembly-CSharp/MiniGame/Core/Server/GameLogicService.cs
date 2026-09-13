using System;

namespace MiniGame.Core.Server;

public abstract class GameLogicService
{
	public static GameLogicService Instance { get; private set; }

	protected GameLogicService()
	{
		if (Instance != null)
		{
			throw new Exception("IRuntimeService is already instantiated");
		}
		Instance = this;
	}

	public virtual bool PrepareGameResource(string type, string logicVersion, string resourceVersion, bool forceUpdate = false)
	{
		return true;
	}

	public abstract IGameLogic CreateGame(string type, string logicVersion, string resourceVersioon);
}

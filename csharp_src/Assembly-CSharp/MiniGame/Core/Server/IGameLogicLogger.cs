using System;

namespace MiniGame.Core.Server;

public interface IGameLogicLogger
{
	void Debug(string message);

	void Info(string message);

	void Warning(string message);

	void Error(string message);

	void Exception(Exception ex);
}

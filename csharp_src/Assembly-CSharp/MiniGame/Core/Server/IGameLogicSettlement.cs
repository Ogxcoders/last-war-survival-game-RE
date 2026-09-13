using System.Collections.Generic;

namespace MiniGame.Core.Server;

public interface IGameLogicSettlement
{
	Dictionary<string, string> Properties { get; }

	int GetWinnerCount();

	string GetWinner(int index = 0);

	int GetLoserCount();

	string GetLoser(int index = 0);

	string GetCode();

	string GetMessage();
}

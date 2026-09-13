using System.Collections.Generic;

namespace MiniGame.Core.Client;

public interface EditorEcsDriver
{
	void ToEdit(string levelPath, string gameLevelJson = null);

	void ToRun(string levelPath, string gameLevelJson = null);

	void ToRunMultiPlayer(string mode, string address, string room, string levelPath, string gameLevelJson);

	List<string> ListMultiPlayerRunMode();

	void ToConfig();
}

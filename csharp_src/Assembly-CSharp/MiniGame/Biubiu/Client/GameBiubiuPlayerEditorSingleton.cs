using System;

namespace MiniGame.Biubiu.Client;

public class GameBiubiuPlayerEditorSingleton : GameBiubiuPlayerEditor
{
	public static GameBiubiuPlayerEditorSingleton Instance { get; private set; }

	public GameBiubiuPlayerEditorSingleton()
		: base("127.0.0.1", Guid.NewGuid().ToString())
	{
		Instance = this;
	}

	~GameBiubiuPlayerEditorSingleton()
	{
		Instance = null;
	}
}

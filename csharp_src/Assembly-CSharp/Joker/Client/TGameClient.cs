namespace Joker.Client;

public class TGameClient<T> : GameClient
{
	public static TGameClient<T> Instance { get; private set; }

	public TGameClient()
	{
		Instance = this;
	}

	~TGameClient()
	{
		Instance = null;
	}
}

namespace MiniGame.Core.Server;

public interface IGameServer
{
	void ConnectPlayer(PlayerSession player);

	void DisconnectPlayer(PlayerSession player);

	void LoginPlayer(PlayerSession player);

	void LogoutPlayer(PlayerSession player);
}

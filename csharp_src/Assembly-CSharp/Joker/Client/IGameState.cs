namespace Joker.Client;

public interface IGameState
{
	int Id { get; }

	bool IsEndLoading { get; }

	void Exit(object param, IGameState state);

	void Enter(object param, IGameState state);
}

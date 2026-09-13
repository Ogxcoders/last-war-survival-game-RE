namespace MiniGame.Core;

public interface ISnapshot
{
	object TakeSnapshot();

	void RestoreSnapshot(object snapshot);
}

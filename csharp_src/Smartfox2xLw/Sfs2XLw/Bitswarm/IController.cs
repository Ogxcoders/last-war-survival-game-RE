namespace Sfs2XLw.Bitswarm;

public interface IController
{
	int Id { get; set; }

	void HandleMessage(IMessage message);
}

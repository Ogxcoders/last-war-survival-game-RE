namespace BestHTTP.SignalR.Messages;

public interface IServerMessage
{
	MessageTypes Type { get; }

	void Parse(object data);
}
